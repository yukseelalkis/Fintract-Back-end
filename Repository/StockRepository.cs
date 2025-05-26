using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stocks;
using api.Helpers;
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
        /////////////////// Get ALLL //////////////////////

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stocks.Include(c => c.Comments).ThenInclude(a=>a.Appuser).AsQueryable();
           // boş, null veya sadece boşluklardan oluşup oluşmadığını kontrol etmek için kullanılır.
           if(!string.IsNullOrWhiteSpace(query.CompanyName)){
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
           }
           //
              if(!string.IsNullOrWhiteSpace(query.Symbol)){
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
           }
           if(!string.IsNullOrWhiteSpace(query.SortBy)){
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase)){
                stocks = query.IsDecsending ? stocks.OrderByDescending(s => s.Symbol): stocks.OrderBy(s=> s.Symbol);
            }
           }
           var skipNumber  = (query.PageNumber - 1) * query.PageSize;
           return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
         
        }

        public async Task<List<Stock>> GetPopularStocksAsync(int count = 5)
        {
            return await _context.Stocks
                .OrderByDescending(s => s.Comments.Count) // En çok yorum alanlar popüler sayılır
                .Take(count) // Belirtilen sayıda getir
                .ToListAsync();
        }


    


        public async Task<Stock?> GetByIdAsync(int id)
        {
            //include eklendi ve bunun sayesinde biz commentleri getirebiliyoruz
            return await   _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i=>i.Id == id);
        }

        public async Task<Stock?> GetBySymbol(string symbol)
        {
            return  await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol );
        }

        public  Task<bool> StockExists(int id)
        {
            return _context.Stocks.AnyAsync(s=>s.Id == id);
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


