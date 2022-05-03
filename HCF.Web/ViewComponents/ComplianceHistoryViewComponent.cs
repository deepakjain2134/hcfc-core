using HCF.BAL;
using HCF.BDO;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class ComplianceHistoryViewComponent : ViewComponent
    {
        private readonly IReportsService _reportService;
        public ComplianceHistoryViewComponent(IReportsService reportService)
        {
            _reportService = reportService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int epdetailid, int? buildingid, int? frequencyid, int year, int month)
        {
            var InspectionReport = _reportService.GetInspectionDetail(epdetailid, frequencyid, year, month).FirstOrDefault();
            //ViewBag.Year = year;
            ViewBag.FrequencyId = frequencyid.HasValue && frequencyid.Value > 0 ? frequencyid.Value : 0;
            return await Task.FromResult(View(InspectionReport));
        }
    }
}
