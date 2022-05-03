using HCF.Web.Base;
using HCF.Web.Filters;
using HCF.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;   
using HCF.BDO;   
using HCF.Utility;
using Microsoft.AspNetCore.Mvc;
using HCF.Utility.AppConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Services;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IInsActivityService _inspectionActivityService;
        private readonly IUserService _userService;
        private readonly IEpService _epService;
        private readonly ITransactionService _transactionService;
        private readonly ITypeService _typeService;
        private readonly IOrganizationService _orgService;
        private readonly IDeviceTestingService _deviceTestingService;
        private readonly IInsStepsService _insStepsService;
        private readonly IGoalMasterService _goalMasterService;
        private readonly IHCFSession _session;
        private readonly IAppSetting _appSetting;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityServerInteractionService _identity;
        private readonly ICommonModelFactory _commonModelFactory;

        public HomeController(
            IHttpContextAccessor httpContextAccessor,
            IAppSetting appSetting,
            IHCFSession session,
            IEpService epService,
            IInsActivityService inspectionActivityService,
            IUserService userService,
            ITransactionService transactionService,
            ITypeService typeService,
            IOrganizationService orgService,
            IInsStepsService insStepsService,
            IDeviceTestingService devicetestingService,
            IGoalMasterService goalMasterService,
            IIdentityServerInteractionService identity,
            ICommonModelFactory commonModelFactory
            )
        {
            _appSetting = appSetting;
            _session = session;
            _deviceTestingService = devicetestingService;
            _inspectionActivityService = inspectionActivityService;
            _userService = userService;
            _epService = epService;
            _transactionService = transactionService;
            _typeService = typeService;
            _orgService = orgService;
            _deviceTestingService = devicetestingService;
            _insStepsService = insStepsService;
            _goalMasterService = goalMasterService;
            _httpContextAccessor = httpContextAccessor;
            _identity = identity;
            _commonModelFactory = commonModelFactory;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Error(string errorId)
        {
            var errormessage = await _identity.GetErrorContextAsync(errorId);

            return Json(errormessage);
        }

        [ActionValidate(isRequired: false)]
        public IActionResult Index()
        {
            var clientDBName = _session.ClientDbName; //_httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == SessionKey.ClientDBName);
            if (clientDBName == null || string.IsNullOrEmpty(clientDBName))
                return RedirectToAction(nameof(AuthController.LogOff), "Auth");

            UISession.ClearBreadCrumb();
            var userid = UserSession.CurrentUser.UserId;
            var objRisk = _goalMasterService.GetRiskCount(userid);
            return View(objRisk);
        }

        public ActionResult VendorDashboard()
        {
            return ViewComponent("HomeVendorDashboard", new { maxPriority = 3, isDone = false });
        }

        public ActionResult ChildPage(int? id, int? isback, string menuName)
        {
            UISession.ClearBreadCrumb();
            UISession.AddCurrentPage($"Home_ChildPage_{id.Value}", 0, menuName);
            if (id != null)
            {
                var services = _orgService.GetUserDashBoard(UserSession.CurrentUser.UserId, id.Value)
                    .Services.ToList();
                if (services.Count > 0)
                    return View(services.Where(x => x.ParentId == id.Value).ToList());
            }
            return RedirectToAction("Index");
        }

        public ActionResult ActivityDashboard(string isBack)
        {
            UISession.AddCurrentPage("home_ActivityDashboard", 0, "Compliance Dashboard");
            var users = _userService.GetUserList().Where(x => (x.IsActive && !string.IsNullOrEmpty(x.Email) && x.EpsCount > 0)
            || x.UserId == UserSession.CurrentUser.UserId).OrderBy(x => x.FullName).ToList();
            return View(users);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="userid"></param>
        /// <param name="sortOrder"></param>
        /// <param name="orderbySort"></param>
        /// <param name="categoryId"></param>
        /// <param name="status"></param>
        /// <param name="noofdays"></param>
        /// <param name="duemonth"></param>
        /// <param name="dueyear"></param>
        /// <param name="searchtext"></param>
        /// <param name="isScroll"></param>
        /// <returns></returns>
        [ActionValidate(isRequired: false)]
        public IActionResult EpsDashboard(int? page, int? userid, string sortOrder = "", string orderbySort = "", int? categoryId = null, int? status = null, int? noofdays = null, int? duemonth = null, int? dueyear = null, string searchtext = "", bool isScroll = false)
        {
            var objrequest = new Request();
            var epDetails = new List<EPDetails>();
            var fixedPageSize = Convert.ToInt32(_appSetting.PageSize);
            int fixedPageIndex;
            var pageIndex = 0;
            int pageSize;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : pageIndex;
            if (isScroll)
            {
                fixedPageIndex = (pageIndex - 1) * fixedPageSize;
                pageSize = fixedPageSize;
            }
            else
            {
                pageSize = pageIndex * fixedPageSize;
                fixedPageIndex = 0;
            }
            sortOrder = string.IsNullOrEmpty(sortOrder) ? "Status" : sortOrder;
            orderbySort = string.IsNullOrEmpty(orderbySort) ? "ASC" : orderbySort;
            objrequest.PageIndex = fixedPageIndex;
            objrequest.PageSize = pageSize > 0 ? pageSize : fixedPageSize;
            objrequest.SortOrder = sortOrder;
            objrequest.OrderbySort = orderbySort;
            objrequest.SearchcBy = string.IsNullOrEmpty(searchtext) ? null : searchtext;
            objrequest.FilterByUserUserId = userid;
            var httpResponse = _epService.GetDashBoardEp(objrequest, UserSession.CurrentUser.UserId, categoryId, status, noofdays, duemonth, dueyear);
            if (httpResponse != null && httpResponse.Result != null && httpResponse.Result.EPDetails != null)
            {
                epDetails = httpResponse.Result.EPDetails;
                var recordCounts = httpResponse.Result.UserRecordsCount;
                var pageCount = httpResponse.PageCount;
                var results = pageCount % fixedPageSize;
                var lastIndex = results > 0 ? (pageCount / fixedPageSize) + 1 : pageCount / fixedPageSize;

                ViewBag.isShowMoreRecords = epDetails.Count > 0 && pageCount != epDetails.Count;
                TempData["PageIndex"] = pageIndex;
                TempData["lastIndex"] = lastIndex;

                ViewBag.RecordCounts = recordCounts;
            }
            return PartialView("_activityDashboard", epDetails);
        }

        public ActionResult RiskAssessMent(string pageName, int id)
        {
            UISession.AddCurrentPage("home_RiskAssessMent", 0, "RiskAssessMent");
            var userid = UserSession.CurrentUser.UserId;
            TempData["PageName"] = pageName;
            List<DashboardDetails> dashboadData;
            switch (pageName)
            {
                case "RA":
                    dashboadData = _epService.GetDashBoardData(userid).Where(x => x.IsRAFail && x.IsDeficiency && !x.IslmFail).ToList();
                    break;
                case "ILSM":
                    dashboadData = _epService.GetDashBoardData(userid).Where(x => x.SubStatus == BDO.Enums.InspectionSubStatus.IL.ToString()).ToList();
                    break;
                default:
                    dashboadData = _epService.GetDashBoardData(userid).Where(x => x.IsDeficiency && x.ScoreId == id).ToList();
                    break;
            }
            return View(dashboadData);
        }

        public ActionResult Deficiency()
        {
            var trans = _transactionService.GetInspection(BDO.Enums.InspectionSubStatus.DE.ToString());
            return View(trans);
        }

        public ActionResult FailSteps(Guid activityId)
        {
            var activity = _insStepsService.GetTransactionSteps(activityId);
            return PartialView("_FailSteps", activity);
        }

        public ActionResult Deficiencies(string taggedId, string orgId, string activityId)
        {
            UISession.AddCurrentPage("home_Deficiencies", 0, "Deficiencies Dashboard");
            ViewBag.DeficiencyTaggedid = taggedId;
            ViewBag.OrganizationID = orgId;


            _session.Set(SessionKey.TaggedId, (taggedId != "0") ? taggedId : string.Empty);
            _session.Set(SessionKey.ActIdFrmDshb, !string.IsNullOrEmpty(activityId) ? activityId : string.Empty);
            if (!string.IsNullOrEmpty(orgId) && string.IsNullOrEmpty(activityId))
            {
                ViewBag.ShowOnlyAssetDeficiences = true;
                _session.Set(SessionKey.OrganizationId, orgId);
            }
            else
            {
                ViewBag.ShowOnlyAssetDeficiences = false;
                _session.Set(SessionKey.OrganizationId, string.Empty);
            }
            return View();
        }

        [ActionValidate(isRequired: false)]
        public ActionResult Assetsdeficiency(string Taggedid, string orgId)
        {
            //List<TaggedMaster> taggedMaster = new();
            List<TInspectionActivity> inspectionActivity = new();
            var userid = UserSession.CurrentUser.UserId;

            Guid? taggedGuid = null;
            if (Taggedid != "0" && Taggedid != null)
            {
                ViewBag.TaggedID = Taggedid;
                taggedGuid = Guid.Parse(Taggedid);
            }
            var taggedMaster = _typeService.GetTaggedList(userid, taggedGuid, Convert.ToInt32(BDO.Enums.TaggedType.Deficiency));

            _session.Set(SessionKey.IsExistTagDeficiency, false);
            var actIdFrmDshb = _session.Get<string>(SessionKey.ActIdFrmDshb);
            ViewBag.actIdFrmDshb = actIdFrmDshb;
            ViewBag.IsGuestUser = _session.Get<string>(SessionKey.IsGuestUser);
            var activityIdFrmdshbrd = (string.IsNullOrEmpty(actIdFrmDshb)) ? null : actIdFrmDshb;
            if (!string.IsNullOrEmpty(activityIdFrmdshbrd))
            {
                inspectionActivity = _inspectionActivityService.GetDeficiencyAssets(0, activityIdFrmdshbrd, null);
            }
            else
            {
                if (!string.IsNullOrEmpty(Taggedid) && Taggedid != "0")
                {
                    taggedMaster = taggedMaster.Where(x => x.TaggedId == Guid.Parse(Taggedid)).ToList();
                    var result = taggedMaster.FirstOrDefault();
                    if (result == null)
                    {
                        ViewBag.IsExistTagDeficiency = "TagNOTExists";
                    }
                    else
                    {
                        if (result.TaggedId.ToString() == Taggedid.ToLower())
                        {
                            double days = (DateTime.Now - result.CreatedDate).TotalDays;
                            if (days > 14)
                            {
                                ViewBag.IsExistTagDeficiency = "TagExpired";
                            }
                            else
                            {
                                ViewBag.IsExistTagDeficiency = "true";
                                _session.Set(SessionKey.IsExistTagDeficiency, true);
                            }
                        }
                    }

                    if (taggedMaster.Count > 0)
                    {
                        List<string> activityIds = new List<string>();
                        foreach (var id in taggedMaster)
                        {
                            if (!string.IsNullOrEmpty(id.ActivityId.ToString()))
                                activityIds.Add(id.ActivityId.ToString());
                        }
                        foreach (var id in activityIds)
                        {
                            var item = _inspectionActivityService.GetDeficiencyAssets(0, id, null);
                            if (item.Count > 0)
                            {
                                inspectionActivity.Add(item.FirstOrDefault());
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.IsExistTagDeficiency = "false";
                    inspectionActivity = _inspectionActivityService.GetDeficiencyAssets(userid, null, null);
                }
            }
            if (inspectionActivity.Count > 0)
            {
                inspectionActivity = inspectionActivity.GroupBy(test => test.ActivityId)
                      .Select(grp => grp.First())
                      .ToList();
            }
            foreach (var item in inspectionActivity)
            {
                item.TaggedMaster = taggedMaster.Where(x => x.ActivityId == item.ActivityId).ToList();
            }
            return PartialView("_Assetsdeficiency", inspectionActivity);
        }

        public ActionResult Epdeficiency(int scoreId, string mode)
        {
            var epdetails = _epService.GetDeficientEPs(UserSession.CurrentUser.UserId);
            if (scoreId > 0)
                epdetails = epdetails.Where(x => x.ScoreId == scoreId).ToList();
            return PartialView("_Epdeficiency", epdetails);
        }

        public ActionResult DocumentDeficiency()
        {
            return PartialView("_DocumentDeficiency");
        }

        [AjaxOnly]
        public ActionResult RoundDeficiency()
        {
            return ViewComponent("HomeRoundDeficiency");
        }

        //#region Decrypt Password

        //[HttpGet]
        //public ActionResult GetPassword()
        //{
        //    if (UserSession.CurrentUser.IsSystemUser)
        //        return View();
        //    else
        //        return RedirectToRoute("dashboard");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult GetPassword(string email)
        //{
        //    string passwordDec = "";
        //    if (UserSession.CurrentUser.IsSystemUser)
        //    {
        //        var user = _userService.GetUserEmailPassword(email);
        //        passwordDec = !string.IsNullOrEmpty(user.Password) ? Conversion.Decrypt(user.Password, Convert.ToString(user.Salt)) : "no user found";
        //    }
        //    return Json(passwordDec);
        //}

        //#endregion       

        [AjaxOnly]
        public ActionResult UserOptions()
        {
            return PartialView("_UserOptions");
        }

        #region Device Deficiency Siemens

        // [ChildActionOnly]
        [AjaxOnly]
        public ActionResult DeviceDeficiency()
        {
            List<TDeviceTesting> data = _deviceTestingService.Get().Result;
            data = data.Where(x => x.TestResult.ToLower() == "failed").ToList();
            return PartialView("_deviceDeficiency", data);
        }

        #endregion

        #region Safer Matrix

        public ActionResult EPsSaferMatrix(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
                startDate = DateTime.Now.AddYears(-3);
            if (!endDate.HasValue)
                endDate = DateTime.Now.AddHours(1);
            List<TInspectionActivity> data = _inspectionActivityService.GetMatrixData(startDate.Value, endDate.Value);
            return PartialView("_saferMatrix", data);
        }
        #endregion

        #region CMS Dashboard 

        public ActionResult CmsDashboard()
        {
            var userid = UserSession.CurrentUser.UserId;
            var objCopDetails = _epService.GetCMSDashboard(userid);
            UISession.AddCurrentPage("home_CMSDashboard", 0, "CMS Dashboard");
            return View(objCopDetails);
        }

        #endregion CMS Dashboard

        #region Dashboard Calender View

        [ActionValidate(false)]
        public JsonResult CalenderView(DateTime start, DateTime end, string filterType)
        {
            var events = new List<EventViewModel>();

            var calenderView = _transactionService.GetDashboardCalendar(UserSession.CurrentUser.UserId);
            var dueInspection = calenderView.inspections.Where(x => x.DueDate.Value.ToClientTime().Date >= start.Date && x.DueDate.Value.ToClientTime().Date <= end.Date).ToList();
            var data = dueInspection.Where(x => x.DueDate.Value.ToClientTime().Date >= DateTime.Now.ToClientTime().Date).GroupBy(i => i.DueDate.Value.ToClientTime().Date).Select(g => new
            {
                Date = g.Key,
                Id = g.FirstOrDefault().InspectionId,
                start = g.FirstOrDefault().DueDate.Value.ToClientTime(),
                count = g.Count(),
                type = 1,
                color = "#ffff00",
                textColor = "#000000",
            });

            var pastdueInspection = dueInspection.Where(x => x.DueDate.Value.ToClientTime().Date <= DateTime.Now.AddDays(-1).ToClientTime().Date).ToList();
            var pastduedata = pastdueInspection.GroupBy(i => i.DueDate.Value.ToClientTime().Date).Select(g => new
            {
                Date = g.Key,
                Id = g.FirstOrDefault().InspectionId,
                start = g.FirstOrDefault().DueDate.Value.ToClientTime(),
                count = g.Count(),
                type = 0,
                color = "#ff0000",
                textColor = "#000000"
            });


            var cuurentInspection = calenderView.inspections.Where(x => x.LastUpdatedDate.ToClientTime().Date >= start.Date && x.LastUpdatedDate.ToClientTime().Date <= end.Date).ToList();
            var inspectionDoneInMonth = cuurentInspection.GroupBy(i => i.LastUpdatedDate.ToClientTime().Date).Select(g => new
            {
                Date = g.Key,
                Id = g.FirstOrDefault().InspectionId,
                start = g.FirstOrDefault().LastUpdatedDate.ToClientTime(),
                count = g.Count(),
                type = 2,
                color = "#808080",
                textColor = "#000000"
            });

            var exercises = calenderView.Exercises.Where(x => x.Date >= start.Date && x.Date <= end.Date).ToList();
            var exercisesdetails = exercises.GroupBy(i => i.Date).Select(g => new
            {
                Date = g.Key,
                Id = g.FirstOrDefault().TExerciseId,
                count = g.Count()
            });

            var roundScheduleDates = calenderView.RoundSchedules.Where(x => x.RoundDate >= start.Date && x.RoundDate <= end.Date).ToList();
            var roundInMonth = roundScheduleDates.GroupBy(i => i.RoundDate.Date).Select(g => new
            {
                Date = g.Key,
                Id = g.FirstOrDefault().RoundScheduleId,
                start = g.FirstOrDefault().RoundDate,
                count = g.Count()
            });


            var epCompleteList = data.Concat(pastduedata).Concat(inspectionDoneInMonth).ToList();

            if (filterType == "All" || filterType == "EP")
            {
                var groupData = epCompleteList.GroupBy(x => x.Date).ToList();
                foreach (var item in groupData)
                {
                    var eventOfDates = item.Key;
                    var eventsData = epCompleteList.Where(x => x.Date == eventOfDates).ToList();
                    events.Add(new EventViewModel()
                    {
                        id = 0,
                        text = "due",
                        start = eventOfDates.ToString(CultureInfo.InvariantCulture),
                        end = eventOfDates.ToString(CultureInfo.InvariantCulture),
                        allDay = false,
                        title = _commonModelFactory.JsonSerialize<calenderViewDashBoard>(eventsData)
                    });
                }
            }

            if (filterType == "All" || filterType == "Rounds")
            {
                foreach (var item in roundInMonth)
                {
                    events.Add(new EventViewModel()
                    {
                        id = item.Id,
                        text = "Rounds ",
                        title = "<b>" + item.count + "</b>",
                        start = item.Date.ToString(),
                        color = "#3399ff",
                        textColor = "#000000"
                    });
                }
            }

            if (filterType == "All" || filterType == "Firedrill")
            {


                foreach (var item in exercisesdetails)
                {
                    events.Add(new EventViewModel()
                    {
                        id = item.Id,
                        text = "Fire Drills ",
                        title = "<b>" + item.count + "</b>",
                        start = item.Date.ToString(),
                        color = "#ffcc00",
                        textColor = "#000000"
                    });
                }
            }

            return Json(events.ToArray());
        }

        #endregion

        #region TaggedUsers 

        public ActionResult DraftEmailPopup(string ActivityIds, string AssetName, string assetIds)
        {
            var taggedMaster = new TaggedMaster
            {
                allUsers = _userService.Get().ToList(),
                TaggedType = (int)BDO.Enums.TaggingType.Deficiency
            };

            ViewBag.AssetName = AssetName.Split(',')[0];
            ViewBag.AssetIds = assetIds;
            //ViewBag.Path = _appSetting.WebUrlPath + "deficiencies/" + id + "/" + UserSession.CurrentOrg.Orgkey.ToString();
            ViewBag.ActivityIDs = ActivityIds;
            return PartialView("~/Views/Home/_draftEmailPopup.cshtml", taggedMaster);
        }

        [HttpPost]
        public ActionResult TaggedUsersPopUp(TaggedMaster taggedMaster)
        {
            if (taggedMaster == null)
                taggedMaster = new TaggedMaster();

            taggedMaster.allUsers = _userService.GetUserList().ToList();
            //ViewBag.Path = _appSetting.WebUrlPath + "deficiencies/" + id + "/" + UserSession.CurrentOrg.Orgkey.ToString();          
            return PartialView("~/Views/Home/_draftEmailPopup.cshtml", taggedMaster);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DraftEmail(TaggedMaster taggedMaster)
        {
            taggedMaster.TaggedId = Guid.NewGuid();
            taggedMaster.Createdby = UserSession.CurrentUser.UserId;

            if (taggedMaster.TaggedType == (int)BDO.Enums.TaggingType.RoundDeficiency && taggedMaster.Resource.Any(x => x.IssueId.HasValue && x.IssueId.Value > 0))
                taggedMaster.selectedDeficiencies = string.Join(',', taggedMaster.Resource.Select(x => x.IssueId));

            if (!string.IsNullOrEmpty(taggedMaster.selectedDeficiencies))
            {
                _typeService.SaveTaggedUser(taggedMaster);
            }

            return Json(new { Result = taggedMaster });
        }


        public ActionResult RemoveTagID(string TagId, string activityID, int UserId, int Tagtype, string PermitNo)
        {
            _typeService.RemoveTagID(TagId, activityID, UserId, Tagtype, PermitNo);
            return Json(true);
        }


        public ActionResult GetUserdetails(string email)
        {
            var allUser = _userService.GetUsers().ToList();
            var userdetails = allUser.FirstOrDefault(x => x.Email == email);
            return Json(userdetails);
        }

        #endregion        
    }
}