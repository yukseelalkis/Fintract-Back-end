using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Extensions;
using api.Interfaces;
using api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    /// <summary>
    /// Kullanıcının portföyünü yöneten API controller'dır.
    /// Kullanıcının hisse senedi portföyünü listeleme, ekleme ve silme işlemlerini içerir.
    /// </summary>
    [Route("api/Portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUsers> _userManager;
        private readonly IPortfolioRepository _portfolioRepo;
        private readonly IFMPService _fmpService;

        public PortfolioController(
            UserManager<AppUsers> userManager,
            IStockRepository stockRepo,
            IPortfolioRepository portfolioRepo,
            IFMPService fmpService)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
            _fmpService = fmpService;
        }

        /// <summary>
        /// Giriş yapmış kullanıcının portföyünü getirir.
        /// </summary>
        /// <returns>Kullanıcının portföyündeki hisselerin listesi.</returns>
        /// <response code="200">Portföy başarıyla döndürüldü.</response>
        /// <response code="404">Kullanıcı veya portföy bulunamadı.</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUserName();
            if (string.IsNullOrEmpty(username))
                return BadRequest("Username is null or empty.");

            var appuser = await _userManager.FindByNameAsync(username);
            if (appuser == null)
                return NotFound("User not found.");

            if (appuser.Id == null)
                return BadRequest("User ID is null.");

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appuser);
            if (userPortfolio == null || !userPortfolio.Any())
                return NotFound("User portfolio is empty or null.");

            return Ok(userPortfolio);
        }

        /// <summary>
        /// Kullanıcının portföyüne yeni bir hisse senedi ekler.
        /// </summary>
        /// <param name="symbol">Eklenmek istenen hisse senedi sembolü.</param>
        /// <returns>Eklenen portföy verisi.</returns>
        /// <response code="201">Portföy kaydı başarıyla oluşturuldu.</response>
        /// <response code="400">Hisse zaten mevcut ya da bulunamadı.</response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetBySymbol(symbol);

            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                    return BadRequest("Stock does not exist.");
                else
                    await _stockRepo.CreateAsync(stock);
            }

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest("Cannot add the same stock to portfolio.");

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUsersId = appUser.Id
            };

            await _portfolioRepo.CreatedAsync(portfolioModel);

            if (portfolioModel == null)
                return StatusCode(500, "Could not create portfolio.");

            return Created();
        }

        /// <summary>
        /// Kullanıcının portföyünden belirtilen hisseyi siler.
        /// </summary>
        /// <param name="symbol">Silinmek istenen hisse senedi sembolü.</param>
        /// <returns>Başarılı işlem sonucu.</returns>
        /// <response code="200">Silme işlemi başarılı.</response>
        /// <response code="400">Hisse portföyde bulunamadı.</response>
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var userName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(userName);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            var filteredStock = userPortfolio
                .Where(s => s.Symbol.ToLower() == symbol.ToLower())
                .ToList();

            if (filteredStock.Count() == 1)
            {
                await _portfolioRepo.DeletePortfolio(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock not in your portfolio.");
            }

            return Ok();
        }
    }
}
