using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Web.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;


namespace HCF.Web.Controllers
{
    [Authorize]
    public class PlantOpsController : BaseController
    {
        #region ctor

        public static IRoundsService _roundService;
        private readonly ICommonModelFactory _commonModelFactory;
        public PlantOpsController(IRoundsService roundService, ICommonModelFactory commonModelFactory)
        {
            _roundService = roundService;
            _commonModelFactory = commonModelFactory;
        }

        #endregion

        #region

        public ActionResult DailyLogs(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate == null && fromDate == null)
            {
                toDate = DateTime.Today;
                fromDate = DateTime.Today.AddMonths(-1);
            }
            ViewBag.FromDate = $"{fromDate:MMM d, yyyy}";
            ViewBag.ToDate = $"{toDate:MMM d, yyyy}"; ;
            UISession.AddCurrentPage("rounds_dailylogs", 0, "PM Logs");
            var pmLogs = _roundService.PMDailyLogs(fromDate, toDate);
            return View(pmLogs);
        }


        public ActionResult PmDailyLogs(int? pmlogId)
        {
            UISession.AddCurrentPage("rounds_pmdailylogs", 0, "Add PM Logs");
            var pmLogs = _roundService.PMDailyLog(pmlogId);
            if (pmLogs.PMLogId == 0)
            {
                pmLogs.Date = DateTime.Now.ToClientTime();
                pmLogs.POTime = _commonModelFactory.ConvertToAMPM(DateTime.UtcNow.ToClientTime());
                pmLogs.Name = UserSession.CurrentUser.FullName;
            }

            return View(pmLogs);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PmDailyLogs(TPMLogs logs)
        {
            if (logs.PMLogId > 0)
                logs.ParentLogId = logs.PMLogId;

            if (!string.IsNullOrEmpty(logs.POTime))
            {
                var dateTime = DateTime.ParseExact(logs.POTime, "hh:mm tt", CultureInfo.InvariantCulture);
                logs.Time = dateTime.TimeOfDay;
            }
            logs.CreatedBy = UserSession.CurrentUser.UserId;
            var pmlogId = _roundService.SavePMDailyLog(logs);
            logs.PMLogId = pmlogId;
            if (pmlogId > 0)
            {
                SuccessMessage = Utility.ConstMessage.Saved_Successfully;
                return RedirectToAction("DailyLogs");
            }

            return View(logs);
        }



        #endregion
    }
}
