using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjeStaj.Context;
using ProjeStaj.Entities;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ProjeStaj.Controllers
{
    public class CommentController : Controller
    {
        private readonly EmailContext _emailContext;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(EmailContext emailContext, UserManager<AppUser> userManager)
        {
            _emailContext = emailContext;
            _userManager = userManager;
        }

        public IActionResult UserComments()
        {
            var values = _emailContext.Comments.Include(x => x.AppUser).ToList();
            return View(values);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult UserCommentList()
        {
            var values = _emailContext.Comments.Include(x => x.AppUser).ToList();
            return View(values);
        }
        [HttpGet]
        public PartialViewResult CreateComment()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(Comment comment)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            comment.AppUserId = user.Id;
            comment.CommentDate = DateTime.Now;



            // Toxic Bert Analizi
            using (var client = new HttpClient())
            {
                var apikey = "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";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apikey);

                try
                {
                    var translateRequestBody = new
                    {
                        inputs = comment.CommentDetail
                    };
                    var translateJson = System.Text.Json.JsonSerializer.Serialize(translateRequestBody);
                    var translateContent = new StringContent(translateJson, Encoding.UTF8, "application/json");

                    var translateResponse = await client.PostAsync("https://api-inference.huggingface.co/models/Helsinki-NLP/opus-mt-tr-en", translateContent);
                    var translateResponseString = await translateResponse.Content.ReadAsStringAsync();

                    string englishText = comment.CommentDetail;
                    if (translateResponseString.TrimStart().StartsWith("["))
                    {
                        var translateDoc = JsonDocument.Parse(translateResponseString);
                        englishText = translateDoc.RootElement[0].GetProperty("translation_text").GetString();
                    }

                    var toxicRequestBody = new
                    {
                        inputs = englishText
                    };


                    var toxicJson = System.Text.Json.JsonSerializer.Serialize(toxicRequestBody);
                    var toxicContent = new StringContent(toxicJson, Encoding.UTF8, "application/json");

                    var toxicResponse = await client.PostAsync("https://api-inference.huggingface.co/models/unitary/toxic-bert", toxicContent);
                    var tocixResponseString = await toxicResponse.Content.ReadAsStringAsync();

                    if (tocixResponseString.TrimStart().StartsWith("["))
                    {
                        var toxicDoc = JsonDocument.Parse(tocixResponseString);
                        foreach (var item in toxicDoc.RootElement[0].EnumerateArray())
                        {
                            string label = item.GetProperty("label").GetString();
                            double score = item.GetProperty("score").GetDouble();
                            if (score > 0.5)
                            {
                                comment.CommentStatus = "Toksik Yorum";
                                break;
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(comment.CommentStatus))
                    {
                        comment.CommentStatus = "Yorum Onaylandı";
                    }

                }
                catch (Exception ex)

                {
                    comment.CommentStatus = "Onay Bekliyor";
                }


                _emailContext.Comments.Add(comment);
                _emailContext.SaveChanges();
                return RedirectToAction("UserCommentList");
            }
        }

        public IActionResult DeleteComment(int id)
        {
            var values = _emailContext.Comments.Find(id);
            _emailContext.Comments.Remove(values);
            _emailContext.SaveChanges();
            return RedirectToAction("UserCommentList");
        }

        public IActionResult CommentStatusChangeToToxic(int id)
        {
            var values = _emailContext.Comments.Find(id);
            values.CommentStatus = "Toksik Yorum";
           
            _emailContext.SaveChanges();
            return RedirectToAction("UserCommentList");
        }

        public IActionResult CommentStatusChangeToPassive(int id)
        {
            var values = _emailContext.Comments.Find(id);
            values.CommentStatus = "Yorum Kaldırıldı";

            _emailContext.SaveChanges();
            return RedirectToAction("UserCommentList");
        }

        public IActionResult CommentStatusChangeToActive(int id)
        {
            var values = _emailContext.Comments.Find(id);
            values.CommentStatus = "Yorum Onaylandı";

            _emailContext.SaveChanges();
            return RedirectToAction("UserCommentList");
        }
    }
}

//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
