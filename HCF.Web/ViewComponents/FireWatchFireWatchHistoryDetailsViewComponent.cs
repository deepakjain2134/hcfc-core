using HCF.BAL;
using HCF.Utility;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class FireWatchFireWatchHistoryDetailsViewComponent :ViewComponent
    {
        private readonly IFireWatchService _fireWatchService;
        public FireWatchFireWatchHistoryDetailsViewComponent(IFireWatchService fireWatchService)
        {
            _fireWatchService = fireWatchService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string timeSpan)
        {
            var timeSlotPeriod = UserSession.CurrentOrg.FireWatchTimeSlot > 0 ? UserSession.CurrentOrg.FireWatchTimeSlot : 4;
            var selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToUTCTime();
            if (!string.IsNullOrEmpty(timeSpan))
                selectedDate = Conversion.ConvertToDateTime(double.Parse(timeSpan.Substring(0, timeSpan.Length - 3))).ToUTCTime();

            var timeSlots = _fireWatchService.Get_FirewatchlogbyTimeSlot(selectedDate, timeSlotPeriod);
            return await Task.FromResult(View(timeSlots));
        }
    }
}
