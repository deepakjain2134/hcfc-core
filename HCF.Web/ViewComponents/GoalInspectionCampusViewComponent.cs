using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.BDO;
using HCF.Web.Base;
using HCF.Web.ViewModels;
using HCF.Web.ViewModels.Goal;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class GoalInspectionCampusViewComponent : ViewComponent
    {
        public GoalInspectionCampusViewComponent(IDeviceTestingService deviceTestingService)
        {
        }
        public async Task<IViewComponentResult> InvokeAsync(InspectionCampusViewModel model)
        {
            if (model.Campus == null|| model.Campus.Count==0)
            {
                model.Campus = new List<BDO.Buildings>();

            }

            return await Task.FromResult(View(model));
        }

    }
}

