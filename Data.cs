using System;
using System.Text.Json.Serialization;

namespace assignment_wt2_oauth
{
    public class Data
    {
        [JsonPropertyName("date")]
        public string date { get; set; }

        [JsonPropertyName("currency_code")]
        public string currency_code { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("local_price")]
        public double local_price { get; set; }

        [JsonPropertyName("dollar_ex")]
        public int dollar_ex { get; set; }

        [JsonPropertyName("dollar_price")]
        public double dollar_price { get; set; }

        public DateTime TimeStamp 
        {
            get
            {
                return DateTime.Parse(date);
            }
        }
    }
}
