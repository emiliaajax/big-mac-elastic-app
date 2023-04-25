using System.Text.Json.Serialization;

namespace BigMacDataScript
{
    /// <summary>
    /// Represents a data point for the price of a Big Mac.
    /// </summary>
    public class Price
    {
        /// <summary>
        /// The date the data point was collected.
        /// </summary>
        [JsonPropertyName("date")]
        public string? date { get; set; }

        /// <summary>
        /// The currency code used for the country where the data was collected.
        /// </summary>
        [JsonPropertyName("currency_code")]
        public string? currency_code { get; set; }

        /// <summary>
        /// The name of the country where the data was collected.
        /// </summary>
        [JsonPropertyName("name")]
        public string? name { get; set; }

        /// <summary>
        /// The price of a Big Mac in the local currency of the country.
        /// </summary>
        [JsonPropertyName("local_price")]
        public double local_price { get; set; }

        /// <summary>
        /// The exchange rate between the local currency and the US dollar.
        /// </summary>
        [JsonPropertyName("dollar_ex")]
        public int dollar_ex { get; set; }

        /// <summary>
        /// The price of a Big Mac in US dollars.
        /// </summary>
        [JsonPropertyName("dollar_price")]
        public double dollar_price { get; set; }

        /// <summary>
        /// Gets the timestamp when the data point was collected, based on the value of the date property.
        /// </summary>
        public DateTime TimeStamp
        {
            get
            {
                return GetTimeStamp();
            }
        }

        // This code was implemented with the help of ChatGPT.
        /// <summary>
        /// Parses the date property and returns a DateTime object representing the timestamp when the data point was collected.
        /// </summary>
        /// <returns>A DateTime object representing the timestamp when the data point was collected.</returns>
        private DateTime GetTimeStamp()
        {
            DateTime timeStamp;

            if (!DateTime.TryParse(date, out timeStamp))
            {
                throw new FormatException("Invalid date format");
            }

            return timeStamp;
        }
    }
}
