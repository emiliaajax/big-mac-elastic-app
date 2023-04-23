using Nest;

namespace BigMacDataScript
{
    /// <summary>
    /// Class for adding Big Mac price data to Elasticsearch.
    /// </summary>
    public class AddToElastic
    {
        private readonly IElasticClient elasticClient;

        /// <summary>
        /// Initializes a new instance of the AddToElastic class with the specified Elasticsearch client.
        /// </summary>
        /// <param name="elasticClient">The Elasticsearch client to be used by the class.</param>
        public AddToElastic(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        /// <summary>
        /// Adds the specified Big Mac price data to Elasticsearch.
        /// </summary>
        /// <param name="data">The Big Mac price data to add to Elasticsearch.</param>
        public async Task AddData(IEnumerable<Price> data)
        {
            var index = "bigmacpricesdata";
            var batchSize = 200;
            var shipped = 0;

            while (data.Skip(shipped).Take(batchSize).Any())
            {
                var batch = data.Skip(shipped).Take(batchSize);
                await elasticClient.BulkAsync(b => b.CreateMany(batch).Index(index));
                shipped += batchSize;
            }
        }
    }
}
