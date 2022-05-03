using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.Web.ViewComponents
{
    public class RoundViewAssignmentDataViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int? userId, int? locationGroupId)
        {
            var roundGroup = new List<RoundGroup>();
            ViewBag.UserId = userId??-1;
            ViewBag.locationGroupId = locationGroupId??-1;
            return await Task.FromResult(View(roundGroup));
        }
    }
}
