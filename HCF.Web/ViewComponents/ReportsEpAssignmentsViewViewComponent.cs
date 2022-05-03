using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class ReportsEpAssignmentsViewViewComponent : ViewComponent
    {
        private readonly IEpService _epService;
        public ReportsEpAssignmentsViewViewComponent(IEpService epService)
        {
            _epService = epService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var epdetails = _epService.GetEpAssignment().ToList();
            return await Task.FromResult(View(epdetails));
        }
    }
}
