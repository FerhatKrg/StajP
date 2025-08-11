using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Org.BouncyCastle.Bcpg;
using ProjeStaj.Entities;
using ProjeStaj.Models.ForgetPasswordModel;

namespace ProjeStaj.Controllers
{
    public class PasswordChangeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public PasswordChangeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordresetTokenLink = Url.Action("ResetPassword", "PasswordChange", new
            {
                UserId = user.Id,
                token = passwordResetToken,
            }, HttpContext.Request.Scheme);


            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxAddressFrom = new MailboxAddress("Notika Admin", "stajproje8@gmail.com");
            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo=new MailboxAddress("User",forgetPasswordViewModel.Email);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder=new BodyBuilder();
            bodyBuilder.TextBody = passwordresetTokenLink;
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            mimeMessage.Subject = "Şifre Değişiklik Talebi";

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("stajproje8@gmail.com", "whut jmre gudk ltgh");

            client.Send(mimeMessage);
            client.Disconnect(true);

            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword(string userId,string token)
        {
            TempData["userId"]=userId; 
            TempData["token"]=token;
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];
            if (userId == null || token == null)
            {
                ViewBag.v = "Hata Oluştu!!";
            }
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result=await _userManager.ResetPasswordAsync(user,token.ToString(),resetPasswordViewModel.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("UserLogin", "Login");
            }

            return View();
        }
    }
}
