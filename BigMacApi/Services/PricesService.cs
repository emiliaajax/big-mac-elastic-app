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

        public async Task<List<PricePerYear>> GetAsync() {
            var response = await elasticClient.SearchAsync<PriceData>(s => s
                .Index("bigmacpricesdata")
                .Size(0)
                .Aggregations(a => a
                    .DateHistogram("pricesOverTime", dh => dh
                        .Field(f => f.TimeStamp)
                        .CalendarInterval(DateInterval.Month)
                        .Aggregations(aa => aa
                            .Average("avg_price", avg => avg
                                .Field(f => f.dollar_price)
                            )
                        )
                    )
                )
            );

            var pricesOverTime = response.Aggregations.DateHistogram("pricesOverTime");
            var pricesList = new List<PricePerYear>();

            foreach (var bucket in pricesOverTime.Buckets) {
                if (bucket.Average("avg_price").Value != null) {
                    pricesList.Add(new PricePerYear {
                        TimeStamp = bucket.Date,
                        price = bucket.Average("avg_price").Value.GetValueOrDefault()
                    });
                }
            }

            return pricesList;
        }

    // PER COUNTRY
    public async Task<List<CountryPricePerYear>> GetCountryAsync(string name)
    {
        var response = await elasticClient.SearchAsync<PriceData>(s => s
            .Index("bigmacpricesdata")
            .Size(1)
            .Query(q => q
                .Match(m => m
                    .Field(f => f.name)
                    .Query(name)
                )
            )
            .Aggregations(a => a
                .DateHistogram("pricesOverTime", dh => dh
                    .Field(f => f.TimeStamp)
                    .CalendarInterval(DateInterval.Month)
                            .Aggregations(aa => aa
                                .Average("avg_price", avg => avg
                                    .Field(f => f.local_price)
                                )
                            )
                )
            )
            .Source(src => src
                .Includes(i => i
                    .Fields(
                        f => f.currency_code
                    )
                )
            )
        );

        var pricesOverTime = response.Aggregations.DateHistogram("pricesOverTime");
        var pricesList = new List<CountryPricePerYear>();

        foreach (var bucket in pricesOverTime.Buckets) {
            if (bucket.Average("avg_price").Value != null) {
                pricesList.Add(new CountryPricePerYear {
                    currencyCode = response.Documents.FirstOrDefault()?.currency_code,
                    name = name,
                    price = bucket.Average("avg_price").Value.GetValueOrDefault(),
                    TimeStamp = bucket.Date
                });
            }
        }

        return pricesList;
    }

        public async Task<List<Country>> GetMostExpensiveCountries(int limit, string startYear, string endYear) {
            var response = await elasticClient.SearchAsync<PriceData>(s => s
                .Index("bigmacpricesdata")
                .Size(0)
                .Query(query => query
                    .DateRange(range => range
                        .Field(field => field.TimeStamp)
                        .GreaterThanOrEquals($"{startYear}-01-01||/d")
                        .LessThanOrEquals($"{endYear}-12-31||/d")
                    )
                )
                .Aggregations(ag => ag
                    .Terms("countries", t => t
                        .Field("name.keyword")
                        .Size(limit)
                        .Order(order => order
                            .Descending("avg_price")
                        )
                        .Aggregations(ag1 => ag1
                            .Average("avg_price", price => price
                                .Field(f2 => f2.dollar_price)
                            )
                        )
                    )
                )
            );

            var countries = response.Aggregations.Terms("countries");
            var mostExpensiveCountries = new List<Country>();

            if (response.Aggregations == null) {
                return mostExpensiveCountries;
            }

            foreach (var bucket in countries.Buckets) {
                mostExpensiveCountries.Add(new Country {
                    name = bucket.Key,
                    price = bucket.Average("avg_price").Value.GetValueOrDefault()
                });
            }

            return mostExpensiveCountries;
        }

        public async Task<List<Country>> GetCheapestCountries(int limit, string startYear, string endYear) {
            var response = await elasticClient.SearchAsync<PriceData>(s => s
                .Index("bigmacpricesdata")
                .Size(0)
                .Query(query => query
                    .DateRange(range => range
                        .Field(field => field.TimeStamp)
                        .GreaterThanOrEquals($"{startYear}-01-01||/d")
                        .LessThanOrEquals($"{endYear}-12-31||/d")
                    )
                )
                .Aggregations(ag => ag
                    .Terms("countries", t => t
                        .Field("name.keyword")
                        .Size(limit)
                        .Order(order => order
                            .Ascending("avg_price")
                        )
                        .Aggregations(ag1 => ag1
                            .Average("avg_price", price => price
                                .Field(f2 => f2.dollar_price)
                            )
                        )
                    )
                )
            );

            var countries = response.Aggregations.Terms("countries");
            var cheapestCountries = new List<Country>();

            if (response.Aggregations == null) {
                return cheapestCountries;
            }

            foreach (var bucket in countries.Buckets) {
                cheapestCountries.Add(new Country {
                    name = bucket.Key,
                    price = bucket.Average("avg_price").Value.GetValueOrDefault()
                });
            }

            return cheapestCountries;
        }
    }
}
