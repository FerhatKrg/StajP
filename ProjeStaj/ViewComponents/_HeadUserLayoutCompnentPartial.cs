using Microsoft.AspNetCore.Mvc;

namespace ProjeStaj.ViewComponents
{
    public class _HeadUserLayoutCompnentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
