using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stocks;
using api.Helpers;
using api.models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id ); 
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock> UpdateAsync(int id , UpdateStockRequestDto updateStockRequestDto);
        Task<Stock> DeleteAsync(int id );
        Task<bool> StockExists(int id );
        Task<Stock?> GetBySymbol(string symbol);
        Task<List<Stock>> GetPopularStocksAsync(int count = 5);

    }
}