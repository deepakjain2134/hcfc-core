using HCF.Web.Base;
using HCF.Web.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class LogoViewComponent : ViewComponent
    {
        //private readonly ICommonModelFactory _commonModelFactory;
        public LogoViewComponent(
            //ICommonModelFactory commonModelFactory
            )
        {
            // _commonModelFactory = commonModelFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(LogoModel model)
        {
            //var model = _commonModelFactory.PrepareLogoModel();
            return View(model);
        }
    }
}
