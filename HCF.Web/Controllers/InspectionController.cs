using HCF.BDO;
using HCF.BAL;
using HCF.Web.Base;
using HCF.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HCF.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HCF.Utility;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class InspectionController : BaseController
    {
        #region ctor

        private readonly ITransactionService _transactioneervice;
        private readonly IOrganizationService _orgService;

        public InspectionController(ITransactionService transactionservice, IOrganizationService orgService)
        {
            _transactioneervice = transactionservice;
            _orgService = orgService;
        }

        #endregion

        #region Inspection Group

        public ActionResult InspectionGroups()
        {
            UISession.AddCurrentPage("Inspection_InspectionGroups", 0, "Inspection Schedules");
            var groups = _transactioneervice.GetInspectionGroup();
            return View(groups);
        }


        public JsonResult getGroups(int? inspectionGroupId)
        {
            if (inspectionGroupId.HasValue)
            {
                var inspectionDate = string.Empty;
                var inspectiongroup = _transactioneervice.GetInspectionGroup().FirstOrDefault(x => x.InspectionGroupId == inspectionGroupId.Value);
                if (inspectiongroup != null)
                {
                    inspectionDate = inspectiongroup.InspectionDate.Value.ToLocalTime().ToShortDateString();
                }
                var res = new { Result = true, InspectionDate = inspectionDate };
                return Json(res);
            }
            return Json(string.Empty);
        }


        [HttpGet]
        public ActionResult AddGroup()
        {
            UISession.AddCurrentPage("Inspection_addgroup", 0, "Add Inspection Schedule");
            var group = new InspectionGroup
            {
                IsActive = true,
                InspectionDate = DateTime.UtcNow.ToLocalTime()
            };
            return View(group);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGroup(InspectionGroup newInspectionGroup)
        {
            newInspectionGroup.CreatedBy = UserSession.CurrentUser.UserId;
            if (ModelState.IsValid)
            {
                if (newInspectionGroup.InspectionGroupId > 0)
                {
                    bool status = _transactioneervice.UpdateInspectionGroup(newInspectionGroup);
                    if (status)
                        SuccessMessage = ConstMessage.Update_Schedule_Success;
                    else
                        ErrorMessage = ConstMessage.Schedule_Already_Exist;
                }
                else
                {
                    int newid = _transactioneervice.Save(newInspectionGroup);
                    if (newid > 0)
                        SuccessMessage = ConstMessage.Insert_Schedule_Success;
                    else
                        ErrorMessage = ConstMessage.Schedule_Already_Exist;

                }
                return RedirectToAction("InspectionGroups", "Inspection");
            }
            return View();
        }

        [HttpGet]
        public ActionResult editGroup(int? gid)
        {
            UISession.AddCurrentPage("Inspection_editGroup", 0, "Edit Inspection Schedule");
            var newinspectionGroup = new InspectionGroup();
            if (gid != null)
            {
                newinspectionGroup = _transactioneervice.GetInspectionGroup().FirstOrDefault(x => x.InspectionGroupId == gid);
                if (newinspectionGroup.InspectionDate.HasValue)
                {
                    newinspectionGroup.InspectionDate = newinspectionGroup.InspectionDate.Value.ToLocalTime();
                }
            }
            return View(newinspectionGroup);
        }


        #endregion

        #region Inspection calendar 

        [CrxAuthorize(Roles = "inspection_calendar")]
        public ActionResult InspCalendar()
        {
            UISession.AddCurrentPage("Inspection_InspCalendar", 0, "Inspection Calendar");
            return View();
        }

        public ActionResult PartialInspCalendar()
        {
            return PartialView("_InspCalendar");
        }


        public JsonResult GetInspections(DateTime start, DateTime end, int userId)
        {
            var inspections = _transactioneervice.GetInspectionsForCalendar(userId, start, end);
            var events = new List<EventViewModel>();


            var dueInspection = inspections.Where(x => x.DueDate.Value.ToClientTime().Date >= start.Date && x.DueDate.Value.ToClientTime().Date <= end.Date).ToList();

            var data = dueInspection.Where(x => x.DueDate.Value.ToClientTime().Date >= DateTime.Now.ToClientTime().Date).GroupBy(i => i.DueDate.Value.ToClientTime().Date).Select(g => new
            {
                Date = g.Key,
                Id = g.FirstOrDefault().InspectionId,
                Comment = g.FirstOrDefault().EPDetails.StandardEp,
                start = g.FirstOrDefault().DueDate.Value.ToClientTime(),
                count = g.Count()
            });

            var pastdueInspection = dueInspection.Where(x => x.DueDate.Value.ToClientTime().Date <= DateTime.Now.AddDays(-1).ToClientTime().Date).ToList();
            var pastduedata = pastdueInspection.GroupBy(i => i.DueDate.Value.ToClientTime().Date).Select(g => new
            {
                Date = g.Key,
                Id = g.FirstOrDefault().InspectionId,
                Comment = g.FirstOrDefault().EPDetails.StandardEp,
                start = g.FirstOrDefault().DueDate.Value.ToClientTime(),
                count = g.Count()
            });


            var cuurentInspection = inspections.Where(x => x.LastUpdatedDate.ToClientTime().Date >= start.Date && x.LastUpdatedDate.ToClientTime().Date <= end.Date).ToList();
            var inspectionDoneInMonth = cuurentInspection.GroupBy(i => i.LastUpdatedDate.ToClientTime().Date).Select(g => new
            {
                Date = g.Key,
                Id = g.FirstOrDefault().InspectionId,
                Comment = g.FirstOrDefault().EPDetails.StandardEp,
                start = g.FirstOrDefault().LastUpdatedDate.ToClientTime(),
                count = g.Count()
            });


            foreach (var item in data)
            {
                events.Add(new EventViewModel()
                {
                    id = item.Id,
                    title = item.count.ToString(),
                    //title = "Due Ins (" + item.count + ")",
                    start = item.Date.ToString(CultureInfo.InvariantCulture),
                    end = item.Date.ToString(CultureInfo.InvariantCulture),
                    allDay = false,
                    color = "#FFFF00",
                    textColor = "#000000"
                });
            }

            foreach (var item in pastduedata)
            {
                events.Add(new EventViewModel()
                {
                    id = item.Id,
                    title = item.count.ToString(),
                    //title = "Past Due Ins (" + item.count + ")",
                    start = item.Date.ToString(CultureInfo.InvariantCulture),
                    end = item.Date.ToString(CultureInfo.InvariantCulture),
                    allDay = false,
                    color = "#ff0000",
                    textColor = "#ffffff"
                });
            }

            foreach (var item in inspectionDoneInMonth)
            {
                events.Add(new EventViewModel()
                {
                    id = item.Id,
                    title = item.count.ToString(),
                    //title = "Ins Done (" + item.count + ")",
                    start = item.Date.ToString(),
                    end = item.Date.ToString(),
                    allDay = false,
                    color = "#808080",
                    textColor = "#ffffff"
                });
            }


            return Json(events.ToArray());
        }

        public ActionResult GetInspectionCalDetails(int userId, string timeSpan)
        {
            var inspections = new List<Inspection>();
            var selecteddate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if (!string.IsNullOrEmpty(timeSpan))
            {
                if (double.TryParse(timeSpan.Substring(0, timeSpan.Length - 3), out double span))
                {
                    selecteddate = Utility.Conversion.ConvertToDateTime(double.Parse(timeSpan.Substring(0, timeSpan.Length - 3)));
                }
                ViewBag.timespan = timeSpan;
                inspections = _transactioneervice.GetInspectionsForCalendar(userId, selecteddate, selecteddate).Where(x => x.DueDate.Value.ToClientTime().Date == selecteddate.Date.Date || x.LastUpdatedDate.ToClientTime().Date == selecteddate.Date).ToList();
            }
            return PartialView(inspections);
        }


        #endregion

        #region Inspection Question

        public ActionResult InspectionQuestion()
        {
            UISession.AddCurrentPage("Inspection_InspectionQuestion", 0, "Inspection", true);
            var services = _orgService.GetUserDashBoard(UserSession.CurrentUser.UserId, 0).Services.ToList();
            return View(services);
        }

        #endregion

        #region Previous Inspection

        public ActionResult InspectionDateSelect(int epId, DateTime StartDate, DateTime EndDate)
        {
            EPDetails ep = new EPDetails();
            ep.EPDetailId = epId;
            ep.Inspection.StartDate = StartDate;
            ep.Inspection.EndDate = EndDate;
            return View(ep);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPreviousInspection(int epId, string datetime)
        {
            Inspection inspection = new Inspection();
            if (!string.IsNullOrEmpty(datetime))
            {
                inspection.EPDetailId = epId;
                inspection.StartDate = Convert.ToDateTime(datetime);
                inspection.LastUpdatedBy = UserSession.CurrentUser.UserId;
                inspection.InspectionId = _transactioneervice.AddPreviousInspection(inspection);
            }
            return Json(inspection);
        }

        #endregion

        #region Undo Review/Inspection
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UndoInspection(int epId, int inspectionId, string activityId)
        {
            int Status = -1;
            HttpResponseMessage result = new HttpResponseMessage();
            TInspectionActivity activity = new TInspectionActivity();
            Inspection inspection = new Inspection();
            if (!string.IsNullOrEmpty(activityId) && epId >0 && inspectionId > 0)
            {
                inspection.EPDetailId = epId;
                inspection.InspectionId = inspectionId;               
                activity.ActivityId = Guid.Parse(activityId);
                inspection.TInspectionActivity.Add(activity);
                Status = _transactioneervice.UndoInspection(inspection);
                if (Status > 0)
                {
                    result.Success = true;
                    result.Message = "Review Cancelled Successfully";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Review Cancellation Failed";
                }
            }
            return Json(result);
        }
        #endregion
    }
}