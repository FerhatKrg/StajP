using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjeStaj.Entities;
using ProjeStaj.Models.IdentityModels;

namespace ProjeStaj.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userMenager;

        public ProfileController(UserManager<AppUser> userMenager)
        {
            _userMenager = userMenager;
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var values = await _userMenager.FindByNameAsync(User.Identity.Name);
            UserEditViewModel userEditViewModel = new UserEditViewModel();
            userEditViewModel.Name = values.Name;
            userEditViewModel.Surname = values.Surname;
            userEditViewModel.PhoneNumber = values.PhoneNumber;
            userEditViewModel.ImageUrl = values.ImageUrl;
            userEditViewModel.City = values.City;
            userEditViewModel.UserName = values.UserName;
            userEditViewModel.Email = values.Email;
            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditViewModel model)
        {
            if (model.Password == model.PasswordConfirm)
            {
                var user = await _userMenager.FindByNameAsync(User.Identity.Name);
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.PhoneNumber = model.PhoneNumber;
                user.City = model.City;
                user.Email = model.Email;
                user.ImageUrl = model.ImageUrl;
                user.UserName = model.UserName;
                user.PasswordHash=_userMenager.PasswordHasher.HashPassword(user,model.Password);
                await _userMenager.UpdateAsync(user);
            }
            return View();
        }
    }
}
