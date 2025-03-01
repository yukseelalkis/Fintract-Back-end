using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stocks;
using api.Interfaces;
using api.models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContex _context;
        public StockRepository(ApplicationDBContex contex )
        {
            _context = contex;
        }

        public async  Task<Stock> CreateAsync(Stock stockModel)
        {
           await _context.Stocks.AddAsync(stockModel);
           await _context.SaveChangesAsync();
           return stockModel;
        }

        public async Task<Stock> DeleteAsync(int id)
        {
           var stockModel =  await _context.Stocks.FirstOrDefaultAsync(x=>x.Id == id);
             if (stockModel == null)
            {
                return null;
            }
           _context.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;

        }

        public  async Task<List<Stock>> GetAllAsync()
        {
           return await  _context.Stocks.ToListAsync();
        }

       
        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await   _context.Stocks.FindAsync(id);
        }

        public async  Task<Stock> UpdateAsync(int id, UpdateStockRequestDto updateDto)
        {
            var exitingStock =  await _context.Stocks.FirstOrDefaultAsync(x=>x.Id == id);
              if (exitingStock == null)
            {
                return null;
            }
            exitingStock.Symbol = updateDto.Symbol;
            exitingStock.CompanyName = updateDto.CompanyName;
            exitingStock.Purchase = updateDto.Purchase;
            exitingStock.LastDiv = updateDto.LastDiv;
            exitingStock.Industry = updateDto.Industry;
            exitingStock.MarketCap = updateDto.MarketCap;
            await  _context.SaveChangesAsync();
            
            return exitingStock;
        }

        
    }
}