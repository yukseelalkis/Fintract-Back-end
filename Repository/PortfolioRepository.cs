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
        private readonly ApplicationDBContex _contex;
        public PortfolioRepository(ApplicationDBContex contex)
        {
            _contex= contex;
        }

        public async Task<Portfolio> CreatedAsync(Portfolio portfolio)
        {
            await _contex.Portfolios.AddAsync(portfolio);
            await _contex.SaveChangesAsync();
            return portfolio;
        }


        //////////  GPT ////////// 
        public async Task<List<Stock>> GetUserPortfolio(AppUsers user)
        {
            if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User is null in GetUserPortfolio.");
                }

            if (_contex.Portfolios == null)
                {
                    throw new InvalidOperationException("Database context is null.");
                }

                return await _contex.Portfolios
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