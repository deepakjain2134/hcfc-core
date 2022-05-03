using HCF.BAL;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class BinderMenusViewComponent : ViewComponent
    {
        private readonly IDocumentsService _documentsService;
        public BinderMenusViewComponent(IDocumentsService documentsService)
        {
            _documentsService = documentsService;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserSession.CurrentUser.UserId;
            var binders = _documentsService.GetBindersList().Where(x=>x.IsActive).ToList();          
            return await Task.FromResult(View(binders));
        }
    }
}
