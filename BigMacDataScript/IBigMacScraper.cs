namespace BigMacDataScript
{
    /// <summary>
    /// Interface for scraping Big Mac price data from a local JSON file.
    /// </summary>
    public interface IBigMacScraper
    {
        /// <summary>
        /// Gets Big Mac price data from a local JSON file and returns it as a sequence of Price objects.
        /// </summary>
        /// <returns>The Big Mac price data as a sequence of Price objects.</returns>
        public Task<IEnumerable<Price>> GetData();
    }
}
