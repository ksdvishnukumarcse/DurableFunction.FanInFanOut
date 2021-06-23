namespace DurableFunction.FanInFanOut.Client.Models
{
    /// <summary>
    /// LoanCharges
    /// </summary>
    public class LoanCharges
    {
        /// <summary>
        /// Gets or sets the processing charge.
        /// </summary>
        /// <value>
        /// The processing charge.
        /// </value>
        public decimal? ProcessingCharge { get; set; }

        /// <summary>
        /// Gets or sets the document charge.
        /// </summary>
        /// <value>
        /// The document charge.
        /// </value>
        public decimal? DocumentCharge { get; set; }

        /// <summary>
        /// Gets or sets the interest amount.
        /// </summary>
        /// <value>
        /// The interest amount.
        /// </value>
        public decimal InterestAmount { get; set; }

        /// <summary>
        /// Gets or sets the prinipal amount.
        /// </summary>
        /// <value>
        /// The prinipal amount.
        /// </value>
        public decimal PrinipalAmount { get; set; }
    }
}
