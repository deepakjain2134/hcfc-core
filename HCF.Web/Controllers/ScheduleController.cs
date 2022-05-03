//using HCF.BAL;
//using HCF.BAL.Interfaces.Services;
//using HCF.BDO;
//using HCF.Web.Base;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;


//namespace HCF.Web.Controllers
//{
//    public class ScheduleController : BaseController
//    {
//        protected readonly ILoggingService _loggingService;
//        private readonly IScheduleService _scheduleService;
//        private readonly IFrequencyService _frequencyService;
//        private readonly IAssetsService _assetsService;
//        public ScheduleController(ILoggingService loggingService, IAssetsService assetsService, IScheduleService scheduleService, IFrequencyService frequencyService) :base(loggingService)
//        {
//            _loggingService = loggingService;
//            _scheduleService = scheduleService;
//            _frequencyService = frequencyService;
//            _assetsService = assetsService;

//        }
//        #region Save Schedule
//        public ActionResult Schedules()
//        {
//            UISession.AddCurrentPage("Schedule_Index", 0, "Schedule");
//            var schedules = _scheduleService.GetSchedule();
//            return View(schedules);
//        }


//        public JsonResult GetSchedules(int scheduleId)
//        {
//            var schedules = _scheduleService.GetSchedule(scheduleId);
//            return Json(new { result = schedules });
//        }

//        public ActionResult ManageSchedule(int? scheduleId)
//        {
//            var schedules = new Schedules();
//            var frequencies = _frequencyService.GetFrequency().Where(x => x.FrequencyId != 27).ToList();

//            var sflist = new List<ScheduleFrequency>();
//            foreach (var item in frequencies)
//            {
//                var sf = new ScheduleFrequency
//                {
//                    FrequencyId = item.FrequencyId,
//                    Frequency = item
//                };
//                sflist.Add(sf);
//            }
//            schedules.ScheduleFrequency = sflist;
//            return PartialView("_manageSchedule", schedules);
//        }

//        [HttpPost]
//        public ActionResult SaveSchedule(Schedules newSchedule)
//        {
//            if (newSchedule.ScheduleId == 0)
//            {
//                newSchedule.CreatedBy = UserSession.CurrentUser.UserId;
//                newSchedule.ScheduleId = _scheduleService.AddSchedule(newSchedule);
//                if (newSchedule.ScheduleId > 0)
//                {
//                    foreach (var item in newSchedule.ScheduleFrequency.Where(x => x.IsActive))
//                    {
//                        item.ScheduleId = newSchedule.ScheduleId;
//                        _scheduleService.AddScheduleFrequency(item);
//                    }
//                }
//            }
//            return Json(new { result = newSchedule });
//        }



//        public JsonResult UpdateSchedule(int scheduleId, bool isActive)
//        {
//            var status = _scheduleService.UpdateSchedule(scheduleId, isActive);
//            return Json(status);
//        }

//        #endregion

//        #region Assign Schedule

//        public ActionResult AssetsSchedule(int? scheduleId)
//        {
//            return View();
//        }

//        [HttpPost]
//        public JsonResult SaveAssetsSchedule(List<ScheduleEPAssets> data)
//        {
//            if (data.Count > 0)
//            {
//                var floorAssetIds = string.Join(",", data.Select(x => x.FloorAssetId));
//                var epDetailId = data.FirstOrDefault().EPDetailId;
//                if (epDetailId != null)
//                {
//                    var epId = epDetailId.Value;
//                    _scheduleService.SaveAssetsSchedule(data.FirstOrDefault().ScheduleId, floorAssetIds, epId, 2);
//                }
//            }
//            return Json(data);
//        }


//        public ActionResult EPbyAssets(int assetId)
//        {
//            var eps = _assetsService.GetAssetEp(assetId);
//            return PartialView("_asseteps", eps);
//        }


//        public ActionResult GetFloorAssetsForIns(string buildingId, string floorId, string assetId)
//        {
//            var assets = new List<Assets>(); //_assetsService.InspectByFloor(assetId, "", Convert.ToInt32(buildingId), Convert.ToInt32(floorId));
//            return PartialView("_schedulesAssets", assets);
//        }



//        public ActionResult EPSchedule(int? scheduleId)
//        {
//            return View();
//        }


//        public ActionResult EPsSchedulePartial()
//        {
//            var epDetails = EpRepository.GetNonAssetEPsSchedule();
//            var epDetails = EpRepository.GetEpDetails();
//            return PartialView("_EPsSchedulePartial", epDetails);

//        }

//        #endregion
//    }
//}