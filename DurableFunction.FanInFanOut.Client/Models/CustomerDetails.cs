namespace DurableFunction.FanInFanOut.Client.Models
{
    /// <summary>
    /// CustomerDetails
    /// </summary>
    public class CustomerDetails
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the salary.
        /// </summary>
        /// <value>
        /// The salary.
        /// </value>
        public long Salary { get; set; }

        /// <summary>
        /// Gets or sets the loan amount.
        /// </summary>
        /// <value>
        /// The loan amount.
        /// </value>
        public decimal LoanAmount { get; set; }

        /// <summary>
        /// Gets or sets the loan tenure.
        /// </summary>
        /// <value>
        /// The loan tenure.
        /// </value>
        public int LoanTenure { get; set; }

        /// <summary>
        /// Gets or sets the interest rate.
        /// </summary>
        /// <value>
        /// The interest rate.
        /// </value>
        public decimal InterestRate { get; set; }
    }
}
