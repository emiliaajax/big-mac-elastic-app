using BigMacApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BigMacApi.Controllers
{
  /// <summary>
  /// Controller for handling Big Mac price data.
  /// </summary>
  [ApiController]
  [Route("api/prices")]
  public class PricesController : ControllerBase
  {
    private readonly IPricesService service;

    /// <summary>
    /// Initializes a new instance of the PricesController class with the specified service.
    /// </summary>
    /// <param name="service">The service to be used by the controller.</param>
    public PricesController(IPricesService service)
    {
      this.service = service;
    }

    /// <summary>
    /// Returns a list of all Big Mac prices with date.
    /// </summary>
    /// <returns>List of Big Mac prices with date.</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var prices = await service.GetAsync();

      var results = prices.Select(price => new
      {
        DollarPrice = price.dollar_price,
        TimeStamp = price.TimeStamp
      });

      return Ok(results);
    }

    /// <summary>
    /// Returns a list of Big Mac prices with dates for a specific country.
    /// </summary>
    /// <param name="country">The name of the country to get prices for.</param>
    /// <returns>List of Big Mac prices for the specified country.</returns>
    [HttpGet("countries/{country}")]
    public async Task<IActionResult> GetCountry(string country)
    {
      var prices = await service.GetCountryAsync(country);

      if (!prices.Any())
      {
        return NotFound("No prices found.");
      }

      var results = prices.Select(price => new
      {
        Name = price.name,
        CurrencyCode = price.currency_code,
        LocalPrice = price.local_price,
        TimeStamp = price.TimeStamp
      });

      return Ok(results);
    }

    /// <summary>
    /// Returns a list of country names that have Big Mac price data.
    /// </summary>
    /// <returns>List of country names.</returns>
    [HttpGet("countries")]
    public async Task<IActionResult> GetCountryNames()
    {
      var response = await service.GetUniqueCountryNamesAsync();

      var result = response.Select(country => new
      {
        Name = country,
        Endpoint = country.ToLower().Replace(" ", "-")
      });

      return Ok(result);
    }

    /// <summary>
    /// Returns the top most expensive countries with prices for Big Macs within a given year range.
    /// </summary>
    /// <param name="limit">The number of countries to return.</param>
    /// <param name="startYear">The starting year for the price range.</param>
    /// <param name="endYear">The ending year for the price range.</param>
    /// <returns>List of the top most expensive countries for Big Macs within the given year range.</returns>
    [HttpGet("top-expensive")]
    public async Task<IActionResult> GetMostExpensiveCountries(
      int limit = 10,
      [FromQuery(Name = "start-year")] string startYear = "2000",
      [FromQuery(Name = "end-year")] string endYear = "2022")
    {
      var prices = await service.GetMostExpensiveCountriesAsync(limit, startYear, endYear);

      var results = prices.Select(price => new
      {
        Name = price.name,
        DollarPrice = price.dollar_price
      });

      return Ok(results);
    }

    /// <summary>
    /// Returns the top cheapest countries with prices for Big Macs within a given year range.
    /// </summary>
    /// <param name="limit">The number of countries to return.</param>
    /// <param name="startYear">The starting year for the price range.</param>
    /// <param name="endYear">The ending year for the price range.</param>
    /// <returns>The top cheapest countries for Big Macs within the given year range.</returns>
    [HttpGet("top-cheapest")]
    public async Task<IActionResult> GetCheapestCountries(
      int limit = 10,
      [FromQuery(Name = "start-year")] string startYear = "2000",
      [FromQuery(Name = "end-year")] string endYear = "2022")
    {
      var prices = await service.GetCheapestCountriesAsync(limit, startYear, endYear);

      var results = prices.Select(price => new
      {
        Name = price.name,
        DollarPrice = price.dollar_price
      });

      return Ok(results);
    }
  }
}
