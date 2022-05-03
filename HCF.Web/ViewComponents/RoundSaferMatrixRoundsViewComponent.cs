using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HCF.BAL;

namespace HCF.Web.ViewComponents
{
    public class RoundSaferMatrixRoundsViewComponent : ViewComponent
    {
        private static IRoundsService _roundService;
        public RoundSaferMatrixRoundsViewComponent(IRoundsService roundService)
        {
            _roundService = roundService;
        }
        public async Task<IViewComponentResult> InvokeAsync(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
                startDate = DateTime.Now.AddMonths(-3);
            if (!endDate.HasValue)
                endDate = DateTime.Now.AddHours(1);
            var data = _roundService.GetMatrixData(startDate.Value, endDate.Value);
            return await Task.FromResult(View(data));
        }
    }
}
