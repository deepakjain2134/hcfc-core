using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class HomeDeviceDeficiencyViewComponent :ViewComponent
    {
        private readonly IDeviceTestingService _deviceTestingService;
        public HomeDeviceDeficiencyViewComponent(IDeviceTestingService deviceTestingService)
        {
            _deviceTestingService = deviceTestingService;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data =await _deviceTestingService.Get();
            data = data.Where(x => x.TestResult.ToLower() == "failed").ToList();
            return await Task.FromResult(View(data));
        }
    }
}
