using HCF.BDO;
using HCF.Web.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class EpDescriptionViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int epDetailId, int maxLength, EPDetails epdetails)
        {
            if (epdetails == null)
                epdetails = new EPDetails();

            var model = new EpDescriptionViewModel
            {
                EpDetailId = epDetailId,
                MaxLength = maxLength,
                Epdetails = epdetails
            };
            return await Task.FromResult(View(model));
        }
    }
}
