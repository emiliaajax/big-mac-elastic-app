using System;
using Newtonsoft.Json;

namespace assignment_wt2_oauth
{
    public class BigMacScraper : IBigMacScraper
    {
        public async Task<IEnumerable<Data>> GetData()
        {
            using (StreamReader r = new StreamReader("./data/BigMacPrice.json"))
                {
                    string json =  await r.ReadToEndAsync();
                    IEnumerable<Data> data = JsonConvert.DeserializeObject<IEnumerable<Data>>(json);
                    return data;
                }
        }
    }
}
