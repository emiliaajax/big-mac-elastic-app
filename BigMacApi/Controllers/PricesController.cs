using System;
using BigMacApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BigMacApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricesController {
      private readonly IPricesService service;

      public PricesController(IPricesService service)
      {
        this.service = service;
      }

      [HttpGet]
      public async Task<IEnumerable<Price>> GetCountry() => 
        await service.GetCountryAsync("Norway");           
    }
}
