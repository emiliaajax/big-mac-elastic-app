using BigMacApi.Models;
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
    public async Task<IEnumerable<PricePerYear>> Get() =>
      await service.GetAsync();

    [HttpGet("countries/{country}")]
    public async Task<IEnumerable<CountryPricePerYear>> GetCountry(string country) =>
      await service.GetCountryAsync(country);

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
    public async Task<IEnumerable<Country>> GetMostExpensiveCountries(
      int limit = 10,
      [FromQuery(Name = "start-year")] string startYear = "2000",
      [FromQuery(Name = "end-year")] string endYear = "2022") =>
      await service.GetMostExpensiveCountries(limit, startYear, endYear);

    [HttpGet("top-cheapest")]
    public async Task<IEnumerable<Country>> GetCheapestCountries(
      int limit = 10,
      [FromQuery(Name = "start-year")] string startYear = "2000",
      [FromQuery(Name = "end-year")] string endYear = "2022") =>
      await service.GetCheapestCountries(limit, startYear, endYear);
  }
}
