using Microsoft.AspNetCore.Mvc;
using ProjeStaj.Context;

namespace ProjeStaj.Controllers
{
    public class ActivationController : Controller
    {

        private readonly EmailContext _emailContext;

        public ActivationController(EmailContext emailContext)
        {
            _emailContext = emailContext;
        }

        [HttpGet]
        public IActionResult UserActivation()
        {
            var email = TempData["EmailMove"];
            TempData["mail"] = email;
            return View();
        }


        [HttpPost]
        public IActionResult UserActivation(int userCode)
        {
            string email = TempData["mail"].ToString();

            var code=_emailContext.Users.Where(x=>x.Email== email).Select(x=>x.ActivationCode).FirstOrDefault();

            if (userCode == code)
            {
                var value=_emailContext.Users.Where(x=>x.Email==email).FirstOrDefault();
                value.EmailConfirmed = true;
                _emailContext.SaveChanges();
                return RedirectToAction("UserLogin", "Login");
            }


            return View();
        }
    }
}
/*xrru pgxc qkmi rmmf*/