namespace BigMacDataScript
{
    public interface IBigMacScraper
    {
        public Task<IEnumerable<Price>> GetData();
    }
}
