using Newtonsoft.Json;

namespace BigMacDataScript
{
    /// <summary>
    /// Scrapes Big Mac price data from a local JSON file and returns it as a sequence of Price objects.
    /// </summary>
    public class BigMacScraper : IBigMacScraper
    {
         /// <summary>
        /// Gets Big Mac price data from a local JSON file and returns it as a sequence of Price objects.
        /// </summary>
        /// <returns>The Big Mac price data as a sequence of Price objects.</returns>
        public async Task<IEnumerable<Price>> GetData()
        {
            using (StreamReader r = new StreamReader("./data/BigMacPrice.json"))
                {
                    string json = await r.ReadToEndAsync();

                    IEnumerable<Price>? data = JsonConvert.DeserializeObject<IEnumerable<Price>>(json);

                    if (data == null)
                    {
                        return Enumerable.Empty<Price>();
                    }

                    return data;
                }
        }
    }
}
