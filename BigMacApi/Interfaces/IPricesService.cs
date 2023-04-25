using BigMacApi.Models;

namespace BigMacApi.Services
{
    /// <summary>
    /// Interface for retrieving Big Mac prices data from a data source.
    /// </summary>
    public interface IPricesService
    {
        /// <summary>
        /// Returns a list of average Big Mac prices.
        /// </summary>
        /// <returns>List of PriceData objects.</returns>
        public Task<List<PriceData>> GetAsync();

        /// <summary>
        /// Returns a list of Big Mac prices for a specific country.
        /// </summary>
        /// <param name="name">The name of the country.</param>
        /// <returns>List of PriceData objects.</returns>
        public Task<List<PriceData>> GetCountryAsync(string countryName);

        /// <summary>
        /// Gets a list of unique country names that have Big Mac prices.
        /// </summary>
        /// <returns>List of country names.</returns>
        public Task<List<string>> GetUniqueCountryNamesAsync();

        /// <summary>
        /// Returns a list of the top most expensive countries and there prices within a specific date range.
        /// </summary>
        /// <param name="limit">The number of countries to return.</param>
        /// <param name="startYear">The start year of the date range.</param>
        /// <param name="endYear">The end year of the date range.</param>
        /// <returns>List of PriceData objects.</returns>
        public Task<List<PriceData>> GetMostExpensiveCountriesAsync(int limit, string startYear, string endYear);

        /// <summary>
        /// Returns a list of the top cheapest countries with Big Mac prices within a specific date range.
        /// </summary>
        /// <param name="limit">The maximum number of countries to return.</param>
        /// <param name="startYear">The start year of the date range.</param>
        /// <param name="endYear">The end year of the date range.</param>
        /// <returns>List of PriceData objects.</returns>
        public Task<List<PriceData>> GetCheapestCountriesAsync(int limit, string startYear, string endYear);
    }
}
