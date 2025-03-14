using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUsers user);
        Task<Portfolio> CreatedAsync(Portfolio portfolio);
        Task<Portfolio> DeletePortfolio(AppUsers user , string symbol); 
    }
}