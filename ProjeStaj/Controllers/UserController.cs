using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjeStaj.Context;
using ProjeStaj.Entities;

namespace ProjeStaj.Controllers
{
    public class UserController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager, EmailContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> UserList()
        {
            var values = await _userManager.Users.ToListAsync();
            return View(values);

        }

        public async Task<IActionResult> DeActivateUserAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.IsActive = false;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("UserList");

        }

        public async Task<IActionResult> ActivateUserAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.IsActive = true;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("UserList");

        }
    }
}
