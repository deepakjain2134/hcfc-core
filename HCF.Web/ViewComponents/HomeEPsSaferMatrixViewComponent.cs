using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class HomeEPsSaferMatrixViewComponent : ViewComponent
    {
        private readonly IInsActivityService _inspectionActivityService;
        public HomeEPsSaferMatrixViewComponent(IInsActivityService inspectionActivityService)
        {
            _inspectionActivityService = inspectionActivityService;
        }

        public async Task<IViewComponentResult> InvokeAsync(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
                startDate = DateTime.Now.AddYears(-3);
            if (!endDate.HasValue)
                endDate = DateTime.Now.AddHours(1);
            var data = _inspectionActivityService.GetMatrixData(startDate.Value, endDate.Value);
            return await Task.FromResult(View(data));
        }
    }
}
