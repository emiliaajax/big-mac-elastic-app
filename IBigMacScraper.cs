using System;

namespace assignment_wt2_oauth
{
    public interface IBigMacScraper
    {
        public Task<IEnumerable<object>> GetData();
    }
}
