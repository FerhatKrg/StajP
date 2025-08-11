using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjeStaj.Context;
using ProjeStaj.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace ProjeStaj.Controllers
{
    public class CategoryController : Controller
    {
        private readonly EmailContext _emailContext;

        public CategoryController(EmailContext emailContext)
        {
            _emailContext = emailContext;
        }

        [Authorize]
        public IActionResult CategoryList()
        {
            var token = Request.Cookies["JwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                TempData["error"] = "Giriş Yapmalısınız ";
                return RedirectToAction("UserLogin", "Login");
            }
            JwtSecurityToken jwt;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                jwt = handler.ReadJwtToken(token);
            }
            catch 
            
            {
                TempData["Error"] = "Token Geçersiz";
                return RedirectToAction("UserLogin", "Login");
            }

            var city = jwt.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

            if(city!= "İstanbul")
            {
                return Forbid();
            }

            var values = _emailContext.Categories.ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CrreateCategory()
        {
         
            return View();
        }

        [HttpPost]
        public IActionResult CrreateCategory(Category category) 
        {
            category.CategoryStatus = true;
            _emailContext.Categories.Add(category);
            _emailContext.SaveChanges();
            return RedirectToAction("CategoryList");
        }

        public IActionResult DeleteCategory(int id) 
        {
            var value = _emailContext.Categories.Find(id);
            _emailContext.Categories.Remove(value);
            _emailContext.SaveChanges();
            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            var value = _emailContext.Categories.Find(id);
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            _emailContext.Categories.Update(category);
            _emailContext.SaveChanges();
            return RedirectToAction("CategoryList");
        }
    }
}
