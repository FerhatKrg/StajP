using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using ProjeStaj.Entities;
using ProjeStaj.Models.IdentityModels;

namespace ProjeStaj.Controllers
{
    public class RegisterController : Controller
    {

        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterUserViewModel model)
        {
            Random rn = new Random();
            int code = rn.Next(100000, 1000000);
            AppUser appUser = new AppUser()
            {
                Name = model.Name,
                Email = model.Email,
                Surname = model.Surname,
                UserName = model.UserName,
                ActivationCode = code,
            };
            var result = await _userManager.CreateAsync(appUser, model.Password);

            if (result.Succeeded)

            {

                MimeMessage mimeMessage = new MimeMessage();
                MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin","stajproje8@gmail.com");

                mimeMessage.From.Add(mailboxAddressFrom);

                MailboxAddress mailboxAddressTo=new MailboxAddress("User",model.Email);
                mimeMessage.To.Add(mailboxAddressTo);

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "Hesabınızı Doğrulamak İçin Gerekli Olan Aktivasyon Kodu : " + code;
                mimeMessage.Body = bodyBuilder.ToMessageBody();

                mimeMessage.Subject = "Staj Aktivasyon Kodu";

               SmtpClient client=new SmtpClient();
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("stajproje8@gmail.com", "xrru pgxc qkmi rmmf");

                client.Send(mimeMessage);
                client.Disconnect(true);

                TempData["EmailMove"]=model.Email;

                return RedirectToAction("UserActivation", "Activation");
            }

            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
        }
    }
}
