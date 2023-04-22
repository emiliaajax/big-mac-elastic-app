using BigMacApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BigMacApi.Controllers
{
  [ApiController]
  [Route("api/prices")]
  public class PricesController : ControllerBase
  {
    private readonly IPricesService service;

    public PricesController(IPricesService service)
    {
      this.service = service;
    }

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

    [HttpGet("countries/{country}")]
    public async Task<IActionResult> GetCountry(string country)
    {
      var prices = await service.GetCountryAsync(country);

      var results = prices.Select(price => new
      {
        Name = price.name,
        CurrencyCode = price.currency_code,
        LocalPrice = price.local_price,
        TimeStamp = price.TimeStamp
      });

      return Ok(results);
    }

    [HttpGet("countries")]
    public async Task<IActionResult> GetCountryNames()
    {
      var response = await service.GetUniqueCountryNamesAsync();

      var result = response.Select(c => new
      {
        Name = c,
        Link = Url.Action("GetCountry", "Prices", new { country = c.ToLower().Replace(" ", "-") }, Request.Scheme, HttpContext.Request.Host.Value)
      });

      return Ok(result);
    }

    [HttpGet("top-expensive")]
    public async Task<IActionResult> GetMostExpensiveCountries(
      int limit = 10,
      [FromQuery(Name = "start-year")] string startYear = "2000",
      [FromQuery(Name = "end-year")] string endYear = "2022")
    {
      var prices = await service.GetMostExpensiveCountries(limit, startYear, endYear);

      var results = prices.Select(price => new
      {
        Name = price.name,
        DollarPrice = price.dollar_price
      });

      return Ok(results);
    }

    [HttpGet("top-cheapest")]
    public async Task<IActionResult> GetCheapestCountries(
      int limit = 10,
      [FromQuery(Name = "start-year")] string startYear = "2000",
      [FromQuery(Name = "end-year")] string endYear = "2022")
    {
      var prices = await service.GetCheapestCountries(limit, startYear, endYear);

      var results = prices.Select(price => new
      {
        Name = price.name,
        DollarPrice = price.dollar_price
      });

      return Ok(results);
    }
  }
}
