using Microsoft.AspNetCore.Mvc;

namespace ProjeStaj.ViewComponents
{
    public class _FooterUserLayoutComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
