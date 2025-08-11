using Microsoft.AspNetCore.Mvc;

namespace ProjeStaj.Controllers
{
    public class UserLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
