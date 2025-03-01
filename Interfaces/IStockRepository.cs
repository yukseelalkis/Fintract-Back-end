using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stocks;
using api.models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();
        Task<Stock?> GetByIdAsync(int id ); 
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock> UpdateAsync(int id , UpdateStockRequestDto updateStockRequestDto);
        Task<Stock> DeleteAsync(int id );


    }
}