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
    [Route("api/Portfolio")]
    [ApiController]

    public class PortfolioController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUsers> _userManager;
        private readonly IPortfolioRepository _portfolioRepo;
        // portfolyo tamamen appuser altinda 
        // app user in icersiidne portfolye 
        //portfolyede ise stocklarla baglantisi var 
        // stockunda comment ile baglantisi var 
        // app useri cekerken protfolye cekiyoruz stock cekioruz ve comment cekiyoruz baglasntili olacak sekilde

        public PortfolioController(UserManager<AppUsers> userManager , IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
        {
            
            _userManager = userManager;
            _stockRepo=stockRepo;
            _portfolioRepo= portfolioRepo;
        }

        // [HttpGet]
        // [Authorize]
        // public async  Task<IActionResult> GetUserPortfolio()
        // {
        //     //Bu satırda, JWT token veya kimlik doğrulama bilgileri içinden kullanıcının adını (username) almak için ClaimsPrincipal nesnesi kullanılıyor.
        //     var username = User.GetUserName();
        //     //Bu satırda, veritabanındaki kullanıcıyı (AppUser) getiriyoruz.
        //     var appuser = await _userManager.FindByNameAsync(username);
        //     var userPortfolio = await _portfolioRepo.GetUserPortfolio(appuser);
        //     return Ok(userPortfolio);
        // }
        


        ///////////GPT//////
        ///
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
        
        var username = User.GetUserName();
         if (string.IsNullOrEmpty(username))
        {
            return BadRequest("Username is null or empty.");
        }

         var appuser = await _userManager.FindByNameAsync(username);
    
        if (appuser == null)
        {
             return NotFound("User not found.");
        }

         if (appuser.Id == null)
        {
             return BadRequest("User ID is null.");
        }

         var userPortfolio = await _portfolioRepo.GetUserPortfolio(appuser);
         if (userPortfolio == null || !userPortfolio.Any())
         {
            return NotFound("User portfolio is empty or null.");
         }

            return Ok(userPortfolio);
        }
    
    [HttpPost]
    [Authorize]
    public  async Task<IActionResult> AddPortfolio(string symbol){
        var username = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(username);
        // amacimiz sadece stockun symbollerini alip kurdurmak istiyoruz
        var stock = await _stockRepo.GetBySymbol(symbol);
        if(stock == null ) return  BadRequest("Stock is not found!!!");
        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol)) return BadRequest("Cannot add same stock to portfolio");
        
        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUsersId = appUser.Id
        };
        await _portfolioRepo.CreatedAsync(portfolioModel);   

        if (portfolioModel == null )
        {
            return StatusCode(500, "Could not create");
        }
        else {
            return Created();
        }
       
    }
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePortfolio (string symbol){
        // user name lazim cunklu Authorize bu  user name lazim
        // extenmsion uzerinden cekecegiz
        var userName = User.GetUserName();
        // burasi ekli olan paketten gelen bir durum 
        var appUser  = await _userManager.FindByNameAsync(userName);

        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

        var filteredStock = userPortfolio.Where(S=> S.Symbol.ToLower() == symbol.ToLower()).ToList();

        if(filteredStock.Count() == 1){
            await _portfolioRepo.DeletePortfolio(appUser,symbol);
        }
        else{
            return BadRequest("Stock not in your portfolio");
        }
        return Ok();

    }
  }
}
