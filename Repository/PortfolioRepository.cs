using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContex _context;
        public PortfolioRepository(ApplicationDBContex contex)
        {
            _context= contex;
        }

        public async Task<Portfolio> CreatedAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUsers user, string symbol)
        {
            // ilk once arama yapacak 
            var portfolioModel =  await _context.Portfolios.FirstOrDefaultAsync(x => x.AppUsersId == user.Id  && x.Stock.Symbol.ToLower() == symbol.ToLower());
            // ARAMA SONUCUNU KONTROL
            if(portfolioModel == null)
            {
                return null;
            }
            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
            
        }


        //////////  GPT ////////// 
        public async Task<List<Stock>> GetUserPortfolio(AppUsers user)
        {
            if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User is null in GetUserPortfolio.");
                }

            if (_context.Portfolios == null)
                {
                    throw new InvalidOperationException("Database context is null.");
                }

                return await _context.Portfolios
                            .Where(u => u.AppUsersId == user.Id)
                            .Select(stock => new Stock
                            {
                             Id = stock.StockId,
                            Symbol = stock.Stock.Symbol,
                            CompanyName = stock.Stock.CompanyName,
                            Purchase = stock.Stock.Purchase,
                            LastDiv = stock.Stock.LastDiv,
                            Industry = stock.Stock.Industry,
                            MarketCap = stock.Stock.MarketCap
                            }).ToListAsync();
        }

    }
}