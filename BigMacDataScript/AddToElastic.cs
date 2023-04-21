using System;
using Nest;

namespace BigMacDataScript
{
    public class AddToElastic
    {
        private readonly IElasticClient elasticClient;

        public AddToElastic(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task AddData(IEnumerable<Price> data)
        {
            var index = "bigmacpricesdata";
            var batchSize = 200;
            var shipped = 0;

            while(data.Skip(shipped).Take(batchSize).Any()) {
                var batch = data.Skip(shipped).Take(batchSize);
                var response = await elasticClient.BulkAsync(b => b.CreateMany(batch).Index(index));
                shipped += batchSize;
            }
        }
    }
}
