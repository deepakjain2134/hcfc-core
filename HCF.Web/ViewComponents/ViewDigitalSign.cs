using HCF.BAL;
using HCF.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class ViewDigitalSign : ViewComponent
    {
        public ViewDigitalSign(IDeviceTestingService deviceTestingService)
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(DigitalSignatureViewModel model)
        {
            if (model.signs==null||model.signs.IsDeleted)
            {
                model.signs = new BDO.DigitalSignature();
               
            }

            return await Task.FromResult(View(model));
        }
    }
}
