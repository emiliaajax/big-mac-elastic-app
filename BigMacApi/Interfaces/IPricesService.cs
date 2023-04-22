using BigMacApi.Models;

namespace BigMacApi.Services
{
    public interface IPricesService
    {
        public Task<List<PriceData>> GetAsync();
        public Task<List<PriceData>> GetCountryAsync(string name);
        public Task<List<string>> GetUniqueCountryNamesAsync();
        public Task<List<PriceData>> GetMostExpensiveCountries(int limit, string startYear, string endYear);
        public Task<List<PriceData>> GetCheapestCountries(int limit, string startYear, string endYear);
    }
}
