using System;

namespace assignment_wt2_oauth
{
    public class AddToElastic
    {
        private readonly IElasticClient elasticClient;

        public AddToElastic(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task AddData(IEnumerable<Data> data)
        {

        }
    }
}
