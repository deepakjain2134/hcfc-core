using HCF.BDO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class AssetsActivityViewComponent : ViewComponent
    {
        public AssetsActivityViewComponent()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync(TInspectionActivity activity, int? status)
        {
            if (activity != null)
            {
                if (status.HasValue && status != null)
                    activity.TInspectionDetail = activity.TInspectionDetail;
            }
            else
                activity = new TInspectionActivity();

            return await Task.FromResult(View(activity));
        }
    }
}
