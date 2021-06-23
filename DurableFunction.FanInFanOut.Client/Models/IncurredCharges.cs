namespace DurableFunction.FanInFanOut.Client.Models
{
    /// <summary>
    /// IncurredCharges
    /// </summary>
    public class IncurredCharges
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is processing charge available.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is processing charge available; otherwise, <c>false</c>.
        /// </value>
        public bool IsProcessingChargeAvailable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is document charge available.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is document charge available; otherwise, <c>false</c>.
        /// </value>
        public bool IsDocumentChargeAvailable { get; set; }    }
}
