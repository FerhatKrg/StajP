using Microsoft.AspNetCore.Mvc;

namespace ProjeStaj.ViewComponents
{
    public class _NavbarUserLayoutComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
