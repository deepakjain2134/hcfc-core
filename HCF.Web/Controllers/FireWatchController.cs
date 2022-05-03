using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using HCF.Web.Models;
using HCF.Utility;
using HCF.BDO;
using HCF.BAL;
using HCF.Web.Base;
using HCF.BAL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class FireWatchController : BaseController
    {

        public readonly IFireWatchService _fireWatchService;
        private readonly IPermitService _permitService;
        private readonly IRoundsService _roundsService;
        public FireWatchController(IFireWatchService fireWatchService, IPermitService permitService, IRoundsService roundsService)
        {
            _fireWatchService = fireWatchService;
            _permitService = permitService;
            _roundsService = roundsService;
        }
        #region Fire Watch

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>        
        public ActionResult FireWatch()
        {
            UISession.AddCurrentPage("firewatch_FireWatch", 0, "Fire Watch");
            ViewBag.Mode = "Edit";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult AddFireWatchlog(List<TimeSlots> objTimeSlots)
        {

            foreach (var item in objTimeSlots)
            {
                foreach (var fireWatch in item.FireWatchLog)
                {
                    if (!string.IsNullOrEmpty(fireWatch.CompleteTime))
                    {
                        var slot = DateTime.ParseExact(fireWatch.CompleteTime, "hh:mm tt", CultureInfo.InvariantCulture);
                        if (fireWatch.LogDate != null)
                            fireWatch.RoundInspDate =
                                fireWatch.LogDate.Value.Add(slot.TimeOfDay);
                        fireWatch.FinishTime = slot.TimeOfDay;
                        int id = _fireWatchService.SaveFireWatchLog(fireWatch);
                        if (id > 0)
                            SuccessMessage = ConstMessage.Saved_Successfully;
                    }
                }
            }
            return RedirectToAction("FireWatch", "FireWatch");
        }


        public ActionResult LogFireWatch(DateTime selecteddate, string buildingId)
        {
            var timeSlotPeriod = UserSession.CurrentOrg.FireWatchTimeSlot > 0 ? UserSession.CurrentOrg.FireWatchTimeSlot : 4;
            var date = Convert.ToDateTime(selecteddate).ToUTCTime();
            var timeSlots = _roundsService.GetTimeSlots(date, timeSlotPeriod);

            var sclogs = _fireWatchService.GetScheduledLogs(null).ToList();//.Where(x => x.IsClosed == false).ToList();
            if (!string.IsNullOrEmpty(buildingId))
            {
                sclogs = sclogs.Where(x => x.BuildingId == Convert.ToInt32(buildingId)).ToList();
            }
            foreach (var item in timeSlots)
            {
                var logs = _fireWatchService.GetFireWatchLog(item.Start);
                var firelog = logs.Where(x => x.RoundInspDate > item.Start && x.RoundInspDate < item.End).ToList();
                var sclog = sclogs.Where(x => x.IsClosed == false).Where(x => x.StartDate != null && ((x.StartDate.Value >= item.Start && x.StartDate.Value <= item.End) || x.StartDate.Value <= item.Start && item.End >= x.StartDate.Value))
                     .Select(x => x.AreaComment);


                if (firelog.Count > 0)
                    item.FireWatchLog = firelog;
                else
                {
                    item.FireWatchLog = new List<FireWatchLog>();
                    var fireWatch = new FireWatchLog
                    {
                        FinishTime = new TimeSpan(),
                        InspectorName = string.Empty,
                        Comment = string.Empty,
                        Area = string.Join(" ,\n ", sclog),
                        LogDate = date
                    };
                    fireWatch.LogDateText = fireWatch.LogDate.Value.ToString("MMM dd ,yyyy");
                    item.FireWatchLog.Add(fireWatch);
                }
            }
            ViewBag.WireWatchLogs = sclogs;
            return PartialView("_FireWatchLog", timeSlots);
        }

        public ActionResult FireWatchSchedules(string mode)
        {
            TempData["SchedulesMode"] = mode;
            var logs = _fireWatchService.GetScheduledLogs(null);
            return PartialView("_FireWatchSchedules", logs);
        }

        public ActionResult FireWatchHistory()
        {
            UISession.AddCurrentPage("firewatch_FireWatchHistory", 0, "Fire Watch History");
            return View();

        }



        /// <summary>
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public JsonResult GetEvents(DateTime start, DateTime end)
        {
            var fireWatchLogs = _fireWatchService.GetFireWatchLog().Where(x => x.LogDate.Value >= start && x.LogDate.Value < end).ToList();

            var data = fireWatchLogs.GroupBy(i => i.RoundInspDate.Value.ToClientTime().Date).Select(g => new
            {
                Month = g.Key,
                Id = g.FirstOrDefault().FWLogId,
                g.FirstOrDefault().Comment,
                start = g.FirstOrDefault().RoundInspDate.Value.ToClientTime()
            });

            return Json(data.Select(item => new EventViewModel()
            {
                id = item.Id,
                title = "Rounds",
                start = item.start.ToString(CultureInfo.InvariantCulture),
                end = item.start.ToString(CultureInfo.InvariantCulture),
                allDay = false
            }).ToArray());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public ActionResult FireWatchHistoryDetails(string timeSpan)
        {
            var timeSlotPeriod = UserSession.CurrentOrg.FireWatchTimeSlot > 0 ? UserSession.CurrentOrg.FireWatchTimeSlot : 4;
            var selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToUTCTime();
            if (!string.IsNullOrEmpty(timeSpan))
                selectedDate = Conversion.ConvertToDateTime(double.Parse(timeSpan.Substring(0, timeSpan.Length - 3))).ToUTCTime();

            var timeSlots = _fireWatchService.Get_FirewatchlogbyTimeSlot(selectedDate, timeSlotPeriod);
            return PartialView(timeSlots);
        }

        #endregion

        #region ScheduledLogs

        public ActionResult ManageFireWatchSchedules(int? id, int? permit)
        {
            ScheduledLogs objScheduledLogs;
            objScheduledLogs = _fireWatchService.GetFirewatchNotificationType();
            UISession.AddCurrentPage("firewatch_ManageFireWatchSchedules", 0, "Manage Fire Watch Schedules");
            var logs = (permit > 0) ? _fireWatchService.GetScheduledLogs(null).FirstOrDefault(x => x.PermitNo == permit) : null;
            if (logs != null)
            {
                id = logs.Id;
            }
            if (id.HasValue)
            {
                objScheduledLogs = _fireWatchService.GetScheduledLogsDetail(id).FirstOrDefault();
                if (objScheduledLogs != null)
                {
                    objScheduledLogs.StartDate = objScheduledLogs.StartDate.HasValue
                        ? objScheduledLogs.StartDate.Value.ToClientTime()
                        : objScheduledLogs.StartDate;
                    objScheduledLogs.Enddate = objScheduledLogs.Enddate.HasValue
                        ? objScheduledLogs.Enddate.Value.ToClientTime()
                        : objScheduledLogs.Enddate;
                }
            }
            if (id == null && permit > 0)
            {
                var permitDetails = _permitService.GetAllPermit(0).FirstOrDefault(x => x.PermitId == permit.ToString() && x.PermitType == (int)BDO.Enums.PermitType.FireSystemBypassPermit);
                if (permitDetails != null && permitDetails.PermitType == (int)BDO.Enums.PermitType.FireSystemBypassPermit)
                {
                    TFireSystemByPassPermit objTFireSystemByPassPermit = _permitService.GetFSBPermit(permit);
                    objScheduledLogs.StartDate = objTFireSystemByPassPermit.StartDate;
                    if (objTFireSystemByPassPermit.TFSBPBuildingDetails.Count != 0)
                    {
                        objScheduledLogs.BuildingId = objTFireSystemByPassPermit.TFSBPBuildingDetails.FirstOrDefault()?.BuildingId;
                    }
                    if (objTFireSystemByPassPermit.EndDate != null)
                    {
                        objScheduledLogs.Enddate = objTFireSystemByPassPermit.EndDate;
                    }
                    //if (objTFireSystemByPassPermit.StartTime.HasValue)
                    objScheduledLogs.EffectiveTime = objTFireSystemByPassPermit.StartTime;
                    // if (objTFireSystemByPassPermit.EndTime.HasValue)
                    objScheduledLogs.UntilTime = objTFireSystemByPassPermit.EndTime;

                    objScheduledLogs.Area = objTFireSystemByPassPermit.DepartmentZonesAffected;
                    objScheduledLogs.PermitNo = permit;
                    objScheduledLogs.PermitType = permitDetails.PermitType;
                    objScheduledLogs.InitiatedBy = permitDetails.PermitNo;
                    //objScheduledLogs.Comment = "FSBP# Incident " + permit;
                    ViewBag.mode = "Permit";
                }
            }
            return View(objScheduledLogs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageFireWatchSchedules(ScheduledLogs objScheduledLogs)
        {
            if (objScheduledLogs.Enddate.HasValue && !string.IsNullOrEmpty(objScheduledLogs.UtTime))
            {
                var dateTime = DateTime.ParseExact(objScheduledLogs.UtTime, "hh:mm tt", CultureInfo.InvariantCulture);
                objScheduledLogs.UntilTime = dateTime.TimeOfDay;
                objScheduledLogs.Enddate = objScheduledLogs.Enddate.Value.Add(dateTime.TimeOfDay).ToUTCTime();
            }
            if (objScheduledLogs.StartDate.HasValue && !string.IsNullOrEmpty(objScheduledLogs.EfTime))
            {
                var dateTime = DateTime.ParseExact(objScheduledLogs.EfTime, "hh:mm tt", CultureInfo.InvariantCulture);
                objScheduledLogs.EffectiveTime = dateTime.TimeOfDay;
                objScheduledLogs.StartDate = objScheduledLogs.StartDate.Value.Add(dateTime.TimeOfDay).ToUTCTime();
            }

            if (objScheduledLogs.Enddate != null)
            {
                if (objScheduledLogs.Enddate >= objScheduledLogs.StartDate)
                {

                }
                else
                {
                    return View(objScheduledLogs);
                }
            }

            objScheduledLogs.CreatedBy = UserSession.CurrentUser.UserId;

            if (objScheduledLogs.Id > 0)
            {
                _fireWatchService.Update(objScheduledLogs);
                SuccessMessage = ConstMessage.Success_Updated;
            }
            else
            {
                objScheduledLogs.Id = _fireWatchService.Save(objScheduledLogs);
                SuccessMessage = ConstMessage.Saved_Successfully;
            }

            // var firewatchid = objScheduledLogs.FirewatchNotificationType.Where(x => x.IsActive == true).Select(x => x.ID).ToList();

            foreach (var firenoticationtype in objScheduledLogs.TFirewatchNotificationType)
            {
                var objnotificationtype = new TFirewatchNotificationType();
                if (!string.IsNullOrEmpty(firenoticationtype.FirewatchNotificationName) ||
                     (firenoticationtype.InitNotificationDate != null)
                    || firenoticationtype.EndNotificationDate != null)
                {

                    objnotificationtype.FirewatchNotificationTypeId = firenoticationtype.ID;
                    objnotificationtype.ScheduledLogID = objScheduledLogs.Id;
                    objnotificationtype.Name = firenoticationtype.Name;
                    objnotificationtype.InitNotificationDate = firenoticationtype.InitNotificationDate;
                    objnotificationtype.EndNotificationDate = firenoticationtype.EndNotificationDate;
                    objnotificationtype.InitNotificationTime = firenoticationtype.InitNotificationTime;
                    objnotificationtype.EndNotificationTime = firenoticationtype.EndNotificationTime;
                    objnotificationtype.CreatedBy = UserSession.CurrentUser.UserId;
                    _fireWatchService.SaveFirewatchNotification(objnotificationtype);
                }
            }


            return RedirectToAction("FireWatch", "FireWatch");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult CloseSchedulelog(int Id)
        {
            var closedBy = UserSession.CurrentUser.UserId;
            var status = _fireWatchService.CloseSchedulelog(Id, closedBy);
            return Json(status);
        }
        #endregion
    }
}