using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    /// <summary>
    /// Kullanıcı hesap işlemleri için API denetleyicisidir.
    /// Kayıt ve giriş işlemleri bu controller üzerinden gerçekleştirilir.
    /// </summary>
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUsers> _signInManager;

        public AccountController(UserManager<AppUsers> usermanager, ITokenService tokenService, SignInManager<AppUsers> signInManager)
        {
            _userManager = usermanager;
            _tokenService = tokenService;
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        /// <summary>
        /// Yeni bir kullanıcı kaydı oluşturur.
        /// </summary>
        /// <param name="registerDto">Kullanıcının kayıt bilgilerini içeren DTO.</param>
        /// <returns>Yeni oluşturulan kullanıcının bilgileri ve JWT token.</returns>
        /// <response code="200">Kayıt başarılı, kullanıcı oluşturuldu.</response>
        /// <response code="400">ModelState geçersiz.</response>
        /// <response code="500">Kullanıcı oluşturulurken bir hata oluştu.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var appUser = new AppUsers
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };

                var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(new NewUserDto
                        {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            Tokens = _tokenService.CreateToken(appUser)
                        });
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Kullanıcı girişini gerçekleştirir.
        /// </summary>
        /// <param name="loginDto">Kullanıcının giriş bilgileri (kullanıcı adı ve şifre).</param>
        /// <returns>Başarılı giriş sonrası kullanıcı bilgileri ve JWT token.</returns>
        /// <response code="200">Giriş başarılı.</response>
        /// <response code="400">ModelState geçersiz.</response>
        /// <response code="401">Kullanıcı adı veya şifre hatalı.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("Invalid UserName");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized("Username not found or password incorrect");

            return Ok(new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Tokens = _tokenService.CreateToken(user)
            });
        }
    }
}
