using DurableFunction.FanInFanOut.Client.Constants;
using DurableFunction.FanInFanOut.Client.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DurableFunction.FanInFanOut.Client
{
    /// <summary>
    /// FanInFanOut
    /// </summary>
    public static class FanInFanOut
    {
        /// <summary>
        /// Runs the orchestrator.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>LoanCharges</returns>
        [FunctionName(AppConstants.FanInFanOutOrchestrator)]
        public static async Task<LoanCharges> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<Task<LoanCharges>>();
            var payload = context.GetInput<CustomerDetails>();

            var getLoanDetails = await context.CallActivityAsync<IncurredCharges>(AppConstants.FanInFanOutGetLoanChargesType, payload);

            outputs.Add(context.CallActivityAsync<LoanCharges>(AppConstants.FanInFanOutGetLoanCharge, getLoanDetails));
            outputs.Add(context.CallActivityAsync<LoanCharges>(AppConstants.FanInFanOutGetLoanCharge, getLoanDetails));
            var parallelTask = await Task.WhenAll(outputs);

            var model = new CompositeModel
            {
                CustomerDetails = payload,
                LoanCharges = parallelTask.ToList()
            };
            var totalCharge = await context.CallActivityAsync<LoanCharges>(AppConstants.FanInFanOutTotalCharge, model);

            return totalCharge;
        }

        /// <summary>
        /// Fans the in fan out get loan charges.
        /// </summary>
        /// <param name="customerDetails">The customer details.</param>
        /// <param name="log">The log.</param>
        /// <returns>IncurredCharges</returns>
        [FunctionName(AppConstants.FanInFanOutGetLoanChargesType)]
        public static IncurredCharges FanInFanOutGetLoanCharges([ActivityTrigger] CustomerDetails customerDetails, ILogger log)
        {
            var incurredCharges = new IncurredCharges();
            if (customerDetails.InterestRate > 5 && customerDetails.LoanAmount > 5000000)
            {
                incurredCharges.IsDocumentChargeAvailable = true;
                incurredCharges.IsProcessingChargeAvailable = true;
            }
            else if (customerDetails.InterestRate <= 5 && customerDetails.LoanAmount > 5000000)
            {
                incurredCharges.IsDocumentChargeAvailable = false;
                incurredCharges.IsProcessingChargeAvailable = true;
            }
            else if (customerDetails.InterestRate <= 5 && customerDetails.LoanAmount <= 5000000)
            {
                incurredCharges.IsDocumentChargeAvailable = false;
                incurredCharges.IsProcessingChargeAvailable = false;
            }
            else
            {
                incurredCharges.IsDocumentChargeAvailable = true;
                incurredCharges.IsProcessingChargeAvailable = false;
            }
            return incurredCharges;
        }

        /// <summary>
        /// Fans the in fan out get loan charge.
        /// </summary>
        /// <param name="incurredCharges">The incurred charges.</param>
        /// <param name="log">The log.</param>
        /// <returns>LoanCharges</returns>
        [FunctionName(AppConstants.FanInFanOutGetLoanCharge)]
        public static LoanCharges FanInFanOutGetLoanCharge([ActivityTrigger] IncurredCharges incurredCharges, ILogger log)
        {
            var loanCharges = new LoanCharges();
            if (incurredCharges.IsDocumentChargeAvailable)
            {
                loanCharges.DocumentCharge = 5000;
            }
            else if (incurredCharges.IsProcessingChargeAvailable)
            {
                loanCharges.ProcessingCharge = 12500;
            }

            return loanCharges;
        }

        /// <summary>
        /// Fans the in fan out total charge.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="log">The log.</param>
        /// <returns>LoanCharges</returns>
        [FunctionName(AppConstants.FanInFanOutTotalCharge)]
        public static LoanCharges FanInFanOutTotalCharge([ActivityTrigger] CompositeModel model, ILogger log)
        {
            var loanCharges = new LoanCharges
            {
                PrinipalAmount = model.CustomerDetails.LoanAmount,
                InterestAmount = model.CustomerDetails.InterestRate,
                ProcessingCharge = model.LoanCharges.Where(s => s.ProcessingCharge != null)?.FirstOrDefault()?.ProcessingCharge,
                DocumentCharge = model.LoanCharges.Where(s => s.DocumentCharge != null)?.FirstOrDefault()?.DocumentCharge
            };

            return loanCharges;
        }

        /// <summary>
        /// HTTPs the start.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="starter">The starter.</param>
        /// <param name="log">The log.</param>
        /// <returns>HttpResponseMessage</returns>
        [FunctionName(AppConstants.FanInFanOutClient)]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage httpRequest,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var payload = await httpRequest.Content.ReadAsAsync<CustomerDetails>();
            string instanceId = await starter.StartNewAsync(AppConstants.FanInFanOutOrchestrator, payload);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(httpRequest, instanceId);
        }
    }
}