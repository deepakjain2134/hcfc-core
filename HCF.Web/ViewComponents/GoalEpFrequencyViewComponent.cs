using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.BDO;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class GoalEpFrequencyViewComponent : ViewComponent
    {
        private readonly IEpService _epService;
       

        public GoalEpFrequencyViewComponent(IEpService epService)
        {
                      _epService = epService;
           
        }
        public async Task<IViewComponentResult> InvokeAsync(string pagemode)
        {
            var epDetails = _epService.OngoingEpDetails().Where(m => m.IsActive = true).ToList();
            ViewBag.Pagemode = pagemode;
            return await Task.FromResult(View(epDetails));
        }

    
    }
}