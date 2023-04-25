using BigMacApi.Models;
using Nest;

// The methods in this service have been partly developed by consulting ChatGPT.
namespace BigMacApi.Services
{
  /// <summary>
  /// Service for interacting with Big Mac price data in Elasticsearch.
  /// </summary>
  public class PricesService : IPricesService
  {
    private readonly IElasticClient elasticClient;

    /// <summary>
    /// Initializes a new instance of the PricesService class with the specified Elasticsearch client.
    /// </summary>
    /// <param name="elasticClient">The Elasticsearch client to be used by the service.</param>
    public PricesService(IElasticClient elasticClient)
    {
      this.elasticClient = elasticClient;
    }

    /// <summary>
    /// Returns a list of average Big Mac prices.
    /// </summary>
    /// <returns>List of PriceData objects.</returns>
    public async Task<List<PriceData>> GetAsync()
    {
      var response = await elasticClient.SearchAsync<PriceData>(s => s
          .Index("bigmacpricesdata")
          .Size(0)
          .Aggregations(a => a
              .DateHistogram("pricesOverTime", dh => dh
                  .Field(f => f.TimeStamp)
                  .CalendarInterval(DateInterval.Month)
                  .Aggregations(aa => aa
                      .Average("averagePrice", avg => avg
                          .Field(f => f.dollar_price)
                      )
                  )
              )
          )
      );

      var pricesOverTime = response.Aggregations.DateHistogram("pricesOverTime");
      var pricesList = new List<PriceData>();

      foreach (var bucket in pricesOverTime.Buckets)
      {
        if (bucket.Average("averagePrice").Value != null)
        {
          pricesList.Add(new PriceData
          {
            TimeStamp = bucket.Date,
            dollar_price = bucket.Average("averagePrice").Value.GetValueOrDefault()
          });
        }
      }

      return pricesList;
    }

    /// <summary>
    /// Returns a list of Big Mac prices for a specific country.
    /// </summary>
    /// <param name="name">The name of the country.</param>
    /// <returns>List of PriceData objects.</returns>
    public async Task<List<PriceData>> GetCountryAsync(string countryName)
    {
      var response = await elasticClient.SearchAsync<PriceData>(s => s
          .Index("bigmacpricesdata")
          .Size(1)
          .Query(q => q
              .MatchPhrase(m => m
                  .Field(f => f.name)
                  .Query(countryName)
                  .Analyzer("standard")
              )
          )
          .Aggregations(a => a
              .DateHistogram("pricesOverTime", dh => dh
                  .Field(f => f.TimeStamp)
                  .CalendarInterval(DateInterval.Month)
                          .Aggregations(aa => aa
                              .Average("averagePrice", avg => avg
                                  .Field(f => f.local_price)
                              )
                          )
              )
          )
          .Source(src => src
              .Includes(i => i
                  .Fields(
                      f => f.name,
                      f => f.currency_code
                  )
              )
          )
      );

      var pricesOverTime = response.Aggregations.DateHistogram("pricesOverTime");
      var pricesList = new List<PriceData>();

      foreach (var bucket in pricesOverTime.Buckets)
      {
        if (bucket.Average("averagePrice").Value != null)
        {
          pricesList.Add(new PriceData
          {
            currency_code = response.Documents.FirstOrDefault()?.currency_code,
            name = response.Documents.FirstOrDefault()?.name,
            local_price = bucket.Average("averagePrice").Value.GetValueOrDefault(),
            TimeStamp = bucket.Date
          });
        }
      }

      return pricesList;
    }

    /// <summary>
    /// Gets a list of unique country names that have Big Mac prices.
    /// </summary>
    /// <returns>List of country names.</returns>
    public async Task<List<string>> GetUniqueCountryNamesAsync()
    {
      var response = await elasticClient.SearchAsync<PriceData>(s => s
          .Index("bigmacpricesdata")
          .Size(0)
          .Aggregations(a => a
              .Terms("countries", t => t
                  .Field(f => f.name.Suffix("keyword"))
                  .Size(int.MaxValue)
              )
          )
      );

      var countries = response.Aggregations.Terms("countries");
      var uniqueCountryNames = countries.Buckets.Select(bucket => bucket.Key).ToList();

      return uniqueCountryNames;
    }

    /// <summary>
    /// Returns a list of the top most expensive countries and there prices within a specific date range.
    /// </summary>
    /// <param name="limit">The number of countries to return.</param>
    /// <param name="startYear">The start year of the date range.</param>
    /// <param name="endYear">The end year of the date range.</param>
    /// <returns>List of PriceData objects.</returns>
    public async Task<List<PriceData>> GetMostExpensiveCountriesAsync(int limit, string startYear, string endYear)
    {
      var response = await elasticClient.SearchAsync<PriceData>(s => s
          .Index("bigmacpricesdata")
          .Size(0)
          .Query(q => q
              .DateRange(r => r
                  .Field(f => f.TimeStamp)
                  .GreaterThanOrEquals($"{startYear}-01-01||/d")
                  .LessThanOrEquals($"{endYear}-12-31||/d")
              )
          )
          .Aggregations(a => a
              .Terms("countries", t => t
                  .Field("name.keyword")
                  .Size(limit)
                  .Order(o => o
                      .Descending("averagePrice")
                  )
                  .Aggregations(aa => aa
                      .Average("averagePrice", price => price
                          .Field(f => f.dollar_price)
                      )
                  )
              )
          )
      );

      var countries = response.Aggregations.Terms("countries");
      var mostExpensiveCountries = new List<PriceData>();

      if (response.Aggregations == null)
      {
        return mostExpensiveCountries;
      }

      foreach (var bucket in countries.Buckets)
      {
        mostExpensiveCountries.Add(new PriceData
        {
          name = bucket.Key,
          dollar_price = bucket.Average("averagePrice").Value.GetValueOrDefault()
        });
      }

      return mostExpensiveCountries;
    }

    /// <summary>
    /// Returns a list of the top cheapest countries with Big Mac prices within a specific date range.
    /// </summary>
    /// <param name="limit">The maximum number of countries to return.</param>
    /// <param name="startYear">The start year of the date range.</param>
    /// <param name="endYear">The end year of the date range.</param>
    /// <returns>List of PriceData objects.</returns>
    public async Task<List<PriceData>> GetCheapestCountriesAsync(int limit, string startYear, string endYear)
    {
      var response = await elasticClient.SearchAsync<PriceData>(s => s
        .Index("bigmacpricesdata")
        .Size(0)
        .Query(q => q
            .DateRange(r => r
                .Field(f => f.TimeStamp)
                .GreaterThanOrEquals($"{startYear}-01-01||/d")
                .LessThanOrEquals($"{endYear}-12-31||/d")
            )
        )
        .Aggregations(a => a
            .Terms("countries", t => t
                .Field("name.keyword")
                .Size(limit)
                .Order(o => o
                    .Ascending("avg_price")
                )
                .Aggregations(aa => aa
                    .Average("avg_price", price => price
                        .Field(f => f.dollar_price)
                    )
                )
            )
        )
      );

      var countries = response.Aggregations.Terms("countries");
      var cheapestCountries = new List<PriceData>();

      if (response.Aggregations == null)
      {
        return cheapestCountries;
      }

      foreach (var bucket in countries.Buckets)
      {
        cheapestCountries.Add(new PriceData
        {
          name = bucket.Key,
          dollar_price = bucket.Average("avg_price").Value.GetValueOrDefault()
        });
      }

      return cheapestCountries;
    }
  }
}
