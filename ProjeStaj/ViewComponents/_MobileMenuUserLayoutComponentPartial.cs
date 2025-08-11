using Microsoft.AspNetCore.Mvc;

namespace ProjeStaj.ViewComponents
{
    public class _MobileMenuUserLayoutComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
