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

    }
}