using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjeStaj.Models.JwtModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjeStaj.Controllers
{
  
    public class TokenController : Controller
    {
        private readonly JwtSettingsModel _settings;

        public TokenController(IOptions<JwtSettingsModel> settings)
        {
            _settings = settings.Value;
        }

        [HttpGet]
        public IActionResult Genarate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Genarate(SimpleUserViewModel simpleUserViewModel)
        {
            var claim = new[]
            {
                new Claim("name",simpleUserViewModel.Name),
                new Claim("surname",simpleUserViewModel.Surname),
                new Claim("city",simpleUserViewModel.City),
                new Claim("username",simpleUserViewModel.Username),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
             issuer: _settings.Issuer,
             audience: _settings.Audience,
             claims: claim,
             expires: DateTime.UtcNow.AddMinutes(_settings.ExpireMinutes),
             signingCredentials: creds);

            simpleUserViewModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return View(simpleUserViewModel);
        }
    }
}
