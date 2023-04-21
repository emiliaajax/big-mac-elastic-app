using System;
using Nest;

namespace BigMacApi.Services
{
    public class PricesService : IPricesService
    {
    private readonly IElasticClient elasticClient;

        public PricesService(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task<List<Price>> GetCountryAsync(string name) {
            var response = await elasticClient.SearchAsync<Price>(s => s
                .Index("bigmacpricesdata")
                .Size(0)
                .Query(q => q
                    .Match(m => m
                        .Field("name.keyword")
                        .Query("Norway")
                    )
                )
                // .Aggregations(a => a
                //     .DateHistogram("pricesOverTime", dh => dh
                //         .Field(f => f.TimeStamp)
                //         .CalendarInterval(DateInterval.Year)
                //         .Aggregations(aaa => aaa
                //             .Terms("CountryPrices", t => t
                //                 .Field(f => f.name)
                //                 .Aggregations(aa => aa
                //                     .Average("avgPrice", avg => avg
                //                         .Field(f => f.local_price)
                //                     )
                //                 )
                //             )
                //         )
                //     )
                // )
            );

            // var response = await elasticClient.SearchAsync<Price>(s => s
            //     .Index("bigmacpricesdata")
            //     .Size(0)
            //     .Query(q => q
            //         .Match(m => m
            //             .Field(f => f.name)
            //             .Query(name)
            //         )
            //     )
            //     .Aggregations(a => a
            //         .Terms("CountryPrices", t => t
            //             .Field(f => f.name)
            //             .Aggregations(aa => aa
            //                 .Average("avgLocalPrice", avg => avg
            //                     .Field(f => f.local_price)
            //                 )
            //                 .DateHistogram("pricesOverTime", dh => dh
            //                     .Field(f => f.TimeStamp)
            //                     .CalendarInterval(DateInterval.Year)
            //                     .Aggregations(aaa => aaa
            //                         .Average("avgPrice", avg => avg
            //                             .Field(f => f.local_price)
            //                         )
            //                     )
            //                 )
            //             )
            //         )
            //     )
            // );
            
            
            // var countryBucket = countryPrices?.Buckets.FirstOrDefault(); // Get the first bucket, assuming there's only one country in the result
            // if (countryBucket == null) return new List<Price>(); // Return empty list if no bucket is found

            // var countryLocalPrice = countryBucket.Average("avgLocalPrice").Value;
            // var countryPricesOverTime = countryBucket.DateHistogram("pricesOverTime");

            var norwayPricesList = new List<Price>();
            // foreach (var dateHistogramBucket in countryPricesOverTime.Buckets)
            // {
            //     var date = dateHistogramBucket.Date;
            //     var avgPrice = dateHistogramBucket.Average("avgPrice").Value;
            //     var norwayPrice = new Price
            //     {
            //         date = date.ToString("yyyy-MM"),
            //         name = name,
            //         local_price = avgPrice.GetValueOrDefault(),
            //     };
            //     norwayPricesList.Add(norwayPrice);
            // }

            return norwayPricesList;
        }
    }
}
