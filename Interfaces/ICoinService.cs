using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stocks;
using api.models;

namespace api.Interfaces
{
    public interface ICoinService
    {   
        Task<List<CoinModelDto>> GetPopularCoinAsync();

        Task<List<CoinModelDto>> GetAllAsync();

        Task<CoinMarketChartDto> Get7DaysStatistics(string id);
        Task<CoinMarketChartDto> Get15DaysStatistics(string id);
        Task<CoinMarketChartDto> Get30DaysStatistics(string id);
        }
}