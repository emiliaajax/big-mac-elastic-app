using BigMacApi.Models;
using Nest;

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
    /// Gets a list of average Big Mac prices per year based on monthly data.
    /// </summary>
    /// <returns>List of PricePerYear objects.</returns>
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
                      .Average("avg_price", avg => avg
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
        if (bucket.Average("avg_price").Value != null)
        {
          pricesList.Add(new PriceData
          {
            TimeStamp = bucket.Date,
            dollar_price = bucket.Average("avg_price").Value.GetValueOrDefault()
          });
        }
      }

      return pricesList;
    }

    /// <summary>
    /// Gets a list of average Big Mac prices per year for a specific country.
    /// </summary>
    /// <param name="name">The name of the country.</param>
    /// <returns>List of CountryPricePerYear objects.</returns>
    public async Task<List<PriceData>> GetCountryAsync(string name)
    {
      var response = await elasticClient.SearchAsync<PriceData>(s => s
          .Index("bigmacpricesdata")
          .Size(1)
          .Query(q => q
              .MatchPhrase(m => m
                  .Field(f => f.name)
                  .Query(name)
                  .Analyzer("standard")
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
        if (bucket.Average("avg_price").Value != null)
        {
          pricesList.Add(new PriceData
          {
            currency_code = response.Documents.FirstOrDefault()?.currency_code,
            name = response.Documents.FirstOrDefault()?.name,
            local_price = bucket.Average("avg_price").Value.GetValueOrDefault(),
            TimeStamp = bucket.Date
          });
        }
      }

      return pricesList;
    }

    /// <summary>
    /// Gets a list of unique country names that have Big Mac price
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
      var uniqueCountryNames = countries.Buckets.Select(b => b.Key).ToList();

      return uniqueCountryNames;
    }

    /// <summary>
    /// Gets a list of the top N most expensive countries based on their average Big Mac price within a specific date range.
    /// </summary>
    /// <param name="limit">The maximum number of countries to return.</param>
    /// <param name="startYear">The start year of the date range.</param>
    /// <param name="endYear">The end year of the date range.</param>
    /// <returns>List of Country objects.</returns>
    public async Task<List<PriceData>> GetMostExpensiveCountries(int limit, string startYear, string endYear)
    {
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
          dollar_price = bucket.Average("avg_price").Value.GetValueOrDefault()
        });
      }

      return mostExpensiveCountries;
    }

    /// <summary>
    /// Gets a list of the top N cheapest countries based on their average Big Mac price within a specific date range.
    /// </summary>
    /// <param name="limit">The maximum number of countries to return.</param>
    /// <param name="startYear">The start year of the date range.</param>
    /// <param name="endYear">The end year of the date range.</param>
    /// <returns>List of Country objects.</returns>
    public async Task<List<PriceData>> GetCheapestCountries(int limit, string startYear, string endYear)
    {
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
