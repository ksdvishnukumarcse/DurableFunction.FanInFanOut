using System.Collections.Generic;

namespace DurableFunction.FanInFanOut.Client.Models
{
    /// <summary>
    /// CompositeModel
    /// </summary>
    public class CompositeModel
    {
        /// <summary>
        /// Gets or sets the customer details.
        /// </summary>
        /// <value>
        /// The customer details.
        /// </value>
        public CustomerDetails CustomerDetails { get; set; }

        /// <summary>
        /// Gets or sets the loan charges.
        /// </summary>
        /// <value>
        /// The loan charges.
        /// </value>
        public List<LoanCharges> LoanCharges { get; set; }
    }
}
