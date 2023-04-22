using System;
using Newtonsoft.Json;

namespace BigMacDataScript
{
    public class BigMacScraper : IBigMacScraper
    {
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
