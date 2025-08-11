using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjeStaj.Context;
using ProjeStaj.Entities;

namespace ProjeStaj.Controllers
{
    public class StaticController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _context;

        public StaticController(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.countRmessage=_context.Messages.Where(x=>x.ReceiverEmail==user.Email).Count();
            ViewBag.countSmessage=_context.Messages.Where(x=>x.SenderEmail==user.Email).Count();
            ViewBag.countSeyahatmessage=_context.Messages.Where(x=>x.ReceiverEmail == user.Email && x.CategoryId==1).Count();
            ViewBag.countEgitimmessage=_context.Messages.Where(x=>x.ReceiverEmail == user.Email && x.CategoryId==2).Count();
            ViewBag.countSosyalmessage=_context.Messages.Where(x=>x.ReceiverEmail == user.Email && x.CategoryId==3).Count();
            ViewBag.countFinansmessage=_context.Messages.Where(x=>x.ReceiverEmail == user.Email && x.CategoryId==4).Count();
            ViewBag.countKampanyamessage=_context.Messages.Where(x=>x.ReceiverEmail == user.Email && x.CategoryId==5).Count();
            ViewBag.countComment=_context.Comments.Count();
   


            return View();
        }
    }
}
