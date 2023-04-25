namespace BigMacApi.Models
{
    /// <summary>
    /// Represents a data point for the price of a Big Mac.
    /// </summary>
    public class PriceData
    {
        /// <summary>
        /// The date the data point was collected.
        /// </summary>
        public string? date { get; set; }

        /// <summary>
        /// The currency code used for the country where the data was collected.
        /// </summary>
        public string? currency_code { get; set; }

        /// <summary>
        /// The name of the country where the data was collected.
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// The price of a Big Mac in the local currency of the country.
        /// </summary>
        public double local_price { get; set; }

        /// <summary>
        /// The exchange rate between the local currency and the US dollar.
        /// </summary>
        public int dollar_ex { get; set; }

        /// <summary>
        /// The price of a Big Mac in US dollars.
        /// </summary>
        public double dollar_price { get; set; }

        /// <summary>
        /// The timestamp when the data point was collected.
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
