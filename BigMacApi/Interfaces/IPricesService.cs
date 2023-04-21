using System;

namespace BigMacApi.Services
{
    public interface IPricesService
    {
        public Task<List<Price>> GetCountryAsync(string name);
    }
}
