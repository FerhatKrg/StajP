using Microsoft.AspNetCore.Mvc;

namespace ProjeStaj.ViewComponents
{
    public class _BreadCombUserLayoutComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
