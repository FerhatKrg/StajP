using Microsoft.AspNetCore.Mvc;
using ProjeStaj.Context;

namespace ProjeStaj.ViewComponents.MessageViewComponents
{
    public class _MessageCategoryListSideBarComponentPartial:ViewComponent
    {
        private readonly EmailContext _emailContext;

        public _MessageCategoryListSideBarComponentPartial(EmailContext emailContext)
        {
            _emailContext = emailContext;
        }

        public IViewComponentResult Invoke()
        {
            var values=_emailContext.Categories.ToList();
            return View(values);
        }
    }
}
