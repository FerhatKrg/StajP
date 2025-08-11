using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjeStaj.Context;
using ProjeStaj.Entities;

namespace ProjeStaj.ViewComponents
{
    public class _HeaderUserLayoutComponentPartial:ViewComponent
    {
        private readonly EmailContext _emailContext;
        private readonly UserManager<AppUser> _userManager;

        public _HeaderUserLayoutComponentPartial(EmailContext emailContext, UserManager<AppUser> userManager)
        {
            _emailContext = emailContext;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userValue = await _userManager.FindByNameAsync(User.Identity.Name);
            var userEmail = userValue.Email;
            var userEmailCount=_emailContext.Messages.Where(x=>x.ReceiverEmail==userEmail).Count();
            ViewBag.userEmailCount=userEmailCount;
            return View();
        }
    }
}
