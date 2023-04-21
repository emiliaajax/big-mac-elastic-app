using System;

namespace big_mac_api.Services
{
    public interface IPricesService
    {
        public Task<List<Price>> GetAsync();
    }
}
