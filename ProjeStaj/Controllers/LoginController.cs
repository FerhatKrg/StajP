using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjeStaj.Context;
using ProjeStaj.Entities;
using ProjeStaj.Models.IdentityModels;
using ProjeStaj.Models.JwtModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjeStaj.Controllers
{

    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailContext _emailContext;
        private readonly JwtSettingsModel _jwtSettings;
        public LoginController(SignInManager<AppUser> signInManager, EmailContext emailContext, IOptions<JwtSettingsModel> jwtSettings)
        {
            _signInManager = signInManager;
            _emailContext = emailContext;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UserLogin(UserLoginViewModel model)
        {

            var value = _emailContext.Users.Where(x => x.UserName == model.Username).FirstOrDefault();
            SimpleUserViewModel simpleUserViewModel = new SimpleUserViewModel()
            {
                City = value.City,
                Email = value.Email,
                Id = value.Id,
                Name = value.Name,
                Surname = value.Surname,
                Username=value.UserName
            };


            if (value == null)

            {
                ModelState.AddModelError(string.Empty, "Kullanıcı Bulunamadı");
                return View(model);
            }

            if (!value.EmailConfirmed)

            {
                ModelState.AddModelError(string.Empty, "Email Adresinizi Henüz Onaylanmamış");
                return View(model);
            }

            if (value.IsActive == false)

            {
                ModelState.AddModelError(string.Empty, "Kullanıcı Pasif Durumda , Giriş Yapamaz ");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);
            if (result.Succeeded)
            {
               
                var token = GenarateJwtToken(simpleUserViewModel);
                 Response.Cookies.Append("JwtToken",token,new CookieOptions
                 {
                     HttpOnly = true,
                     Secure = true,
                     SameSite=SameSiteMode.Strict,
                     Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes)
                 });
                return RedirectToAction("EditProfile", "Profile");
            }
            ModelState.AddModelError(string.Empty, "Kullanıcı Veya Şifre Yanlış");
            return View(model);

        }
        public string GenarateJwtToken(SimpleUserViewModel simpleUserViewModel)
        {
            var claim = new[]
            {
                new Claim("name",simpleUserViewModel.Name),
                new Claim("surname",simpleUserViewModel.Surname),
                new Claim("city",simpleUserViewModel.City),
                new Claim("username",simpleUserViewModel.Username),
                new Claim(ClaimTypes.NameIdentifier,simpleUserViewModel.Id),
                new Claim(ClaimTypes.Email,simpleUserViewModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
             issuer: _jwtSettings.Issuer,
             audience: _jwtSettings.Audience,
             claims: claim,
             expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
             signingCredentials: creds);

           // simpleUserViewModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
            //return View(simpleUserViewModel);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
