using System;

namespace BigMacApi.Services
{
    public interface IPricesService
    {
        public Task<List<PricePerYear>> GetAsync();
        public Task<List<CountryPricePerYear>> GetCountryAsync(string name);
        public Task<List<Country>> GetMostExpensiveCountries(int limit, string startYear, string endYear);
        public Task<List<Country>> GetCheapestCountries(int limit, string startYear, string endYear);
    }
}
