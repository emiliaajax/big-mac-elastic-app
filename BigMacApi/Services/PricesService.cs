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
    /// Returns a list of average Big Mac prices.
    /// </summary>
    /// <returns>List of PriceData objects.</returns>
    public async Task<List<PriceData>> GetAsync()
    {
      var response = await elasticClient.SearchAsync<PriceData>(search => search
          .Index("bigmacpricesdata")
          .Size(0)
          .Aggregations(aggregation => aggregation
              .DateHistogram("pricesOverTime", dateHistogram => dateHistogram
                  .Field(field => field.TimeStamp)
                  .CalendarInterval(DateInterval.Month)
                  .Aggregations(aggregation2 => aggregation2
                      .Average("averagePrice", average => average
                          .Field(field => field.dollar_price)
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
      var response = await elasticClient.SearchAsync<PriceData>(search => search
          .Index("bigmacpricesdata")
          .Size(1)
          .Query(query => query
              .MatchPhrase(match => match
                  .Field(field => field.name)
                  .Query(countryName)
                  .Analyzer("standard")
              )
          )
          .Aggregations(aggregation => aggregation
              .DateHistogram("pricesOverTime", dateHistogram => dateHistogram
                  .Field(field => field.TimeStamp)
                  .CalendarInterval(DateInterval.Month)
                          .Aggregations(aggregation2 => aggregation2
                              .Average("averagePrice", average => average
                                  .Field(field => field.local_price)
                              )
                          )
              )
          )
          .Source(source => source
              .Includes(include => include
                  .Fields(
                      field => field.name,
                      field => field.currency_code
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
      var response = await elasticClient.SearchAsync<PriceData>(search => search
          .Index("bigmacpricesdata")
          .Size(0)
          .Aggregations(aggregation => aggregation
              .Terms("countries", term => term
                  .Field(field => field.name.Suffix("keyword"))
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
      var response = await elasticClient.SearchAsync<PriceData>(search => search
          .Index("bigmacpricesdata")
          .Size(0)
          .Query(query => query
              .DateRange(range => range
                  .Field(field => field.TimeStamp)
                  .GreaterThanOrEquals($"{startYear}-01-01||/d")
                  .LessThanOrEquals($"{endYear}-12-31||/d")
              )
          )
          .Aggregations(aggregation => aggregation
              .Terms("countries", term => term
                  .Field("name.keyword")
                  .Size(limit)
                  .Order(order => order
                      .Descending("averagePrice")
                  )
                  .Aggregations(aggregation2 => aggregation2
                      .Average("averagePrice", price => price
                          .Field(field => field.dollar_price)
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
