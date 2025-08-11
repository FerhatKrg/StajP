using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjeStaj.Context;
using ProjeStaj.Entities;
using ProjeStaj.Models;
using ProjeStaj.Models.MessageViewModels;

namespace ProjeStaj.Controllers
{
    public class MessageController : Controller
    {
        private readonly EmailContext
            _emailContext;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(EmailContext emailContext, UserManager<AppUser> userManager)
        {
            _emailContext = emailContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Inbox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //var results = _emailContext.Messages.Where(x => x.ReceiverEmail ==user.Email).ToList();
            var values = (from m in _emailContext.Messages
                          join u in _emailContext.Users
                        on m.SenderEmail equals u.Email
                     into userGroup
                          from sender in userGroup.DefaultIfEmpty()

                          join c in _emailContext.Categories
                          on m.CategoryId equals c.CategoryId into categoryGroup
                          from category in categoryGroup.DefaultIfEmpty()

                          where m.ReceiverEmail == user.Email
                          select new MessageWithSenderİnfoModel
                          {
                              MessageId = m.MessageId,
                              MessageDetail = m.MessageDetail,
                              Subject = m.Subject,
                              SendDate = m.SendDate,
                              SenderEmail = m.SenderEmail,
                              SenderName = sender != null ? sender.Name : "Bilinmeyen",
                              SenderSurname = sender != null ? sender.Surname : "Kullanıcı",
                              CategoryName = category != null ? category.CategoryName : "Kategori Yok"
                          }).ToList();
            return View(values);
        }

        public async Task<IActionResult> Sendbox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //var results = _emailContext.Messages.Where(x => x.ReceiverEmail ==user.Email).ToList();
            var values = (from m in _emailContext.Messages
                          join u in _emailContext.Users
                        on m.ReceiverEmail equals u.Email
                     into userGroup
                          from receiver in userGroup.DefaultIfEmpty()

                          join c in _emailContext.Categories
                          on m.CategoryId equals c.CategoryId into categoryGroup
                          from category in categoryGroup.DefaultIfEmpty()

                          where m.SenderEmail == user.Email
                          select new MessageWithReceiverInfoViewModel
                          {
                              MessageId = m.MessageId,
                              MessageDetail = m.MessageDetail,
                              Subject = m.Subject,
                              SendDate = m.SendDate,
                              ReceiverEmail = m.ReceiverEmail,
                              ReceiverName = receiver != null ? receiver.Name : "Bilinmeyen",
                              ReceiverSurname = receiver != null ? receiver.Surname : "Kullanıcı",
                              CategoryName = category != null ? category.CategoryName : "Kategori Yok"
                          }).ToList();
            return View(values);
        }

        public IActionResult MessageDetail(int id)
        {
            var results = _emailContext.Messages.Where(x => x.MessageId == id).FirstOrDefault();
            return View(results);
        }

        [HttpGet]
        public IActionResult ComposeMessage()
        {
            var categories= _emailContext.Categories.ToList();
          ViewBag.v=  categories.Select(c=> new SelectListItem
            {
                Text=c.CategoryName,
                Value=c.CategoryId.ToString()
            }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ComposeMessage(Message message)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            message.SenderEmail = user.Email;
            message.SendDate = DateTime.Now;
            message.IsRead = false;
            _emailContext.Messages.Add(message);
            _emailContext.SaveChanges();
            return RedirectToAction("SendBox");
        }

        public async Task<IActionResult> GetMessageListByCategory(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //var results = _emailContext.Messages.Where(x => x.ReceiverEmail ==user.Email).ToList();
            var values = (from m in _emailContext.Messages
                          join u in _emailContext.Users
                        on m.SenderEmail equals u.Email
                     into userGroup
                          from sender in userGroup.DefaultIfEmpty()

                          join c in _emailContext.Categories
                          on m.CategoryId equals c.CategoryId into categoryGroup
                          from category in categoryGroup.DefaultIfEmpty()

                          where m.ReceiverEmail == user.Email &&  m.CategoryId==id
                          select new MessageWithSenderİnfoModel
                          {
                              MessageId = m.MessageId,
                              MessageDetail = m.MessageDetail,
                              Subject = m.Subject,
                              SendDate = m.SendDate,
                              SenderEmail = m.SenderEmail,
                              SenderName = sender != null ? sender.Name : "Bilinmeyen",
                              SenderSurname = sender != null ? sender.Surname : "Kullanıcı",
                              CategoryName = category != null ? category.CategoryName : "Kategori Yok"
                          }).ToList();
            return View(values);
        }
    }
}
