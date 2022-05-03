using HCF.BDO;
using HCF.Web.Base;
using HCF.Web.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HCF.BAL;
using HCF.Utility;
using HCF.Web.Models;
using HCF.BAL.Interfaces.Services;
using HCF.Web.ViewModels.Round;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HCF.BDO.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hcf.Api.Application;
using System.Threading.Tasks;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class RoundController : BaseController
    {
        private readonly IActivityService _activityService;
        private readonly IRoundsService _roundService;
        private readonly IRoundInspectionsService _roundInspectionService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IBuildingsService _buildingsService;
        private readonly IRoundsService _roundsService;
        private readonly IHttpPostFactory _httpService;
        private readonly IHCFSession _session;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IWorkOrderService _workOrderService;
        private readonly IApiCommon _apiCommon;

        public RoundController(
             IApiCommon apiCommon,
                 ICommonModelFactory commonModelFactory,
                 IHCFSession session,
                 IRoundInspectionsService roundInspectionsService,
                 IActivityService activityService,
                 IRoundsService roundService,
                 IEmailService emailService,
                 IUserService userService,
                 IBuildingsService buildingsService,
                 IRoundsService roundsService,
                 IHttpPostFactory httpService,
                 IWorkOrderService workOrderService)
        {
            _commonModelFactory = commonModelFactory;
            _session = session;
            _activityService = activityService;
            _roundService = roundService;
            _roundInspectionService = roundInspectionsService;
            _emailService = emailService;
            _userService = userService;
            _buildingsService = buildingsService;
            _roundsService = roundsService;
            _httpService = httpService;
            _workOrderService = workOrderService;
            _apiCommon = apiCommon;
        }

        #region Round Inspection
        //[CrxAuthorize(Roles = "Round_Index")]
        public ActionResult Index()
        {
            UISession.AddCurrentPage("Round_Index", 0, "Round Inspection");
            var tRounds = new List<TRounds>(); //_roundService.GetRoundDetails();
            return View(tRounds);
        }
        public ActionResult SchedulerPageFlow()
        {
            UISession.AddCurrentPage("Rounds_SchedulerPageFlow", 0, "Round Scheduler Page Flow");
            var tRounds = _roundService.GetRoundDetails();
            return View(tRounds);
        }
        public ActionResult NewRoundInsp(int? roundId, int? rgid, int? roundGroupId, int? rscheduleDateId, int roundType, string roundInspDate)
        {           
            if (roundType == 2)
                return RedirectToAction("GroupRoundInsp", new { id = roundGroupId, rgid = rgid, RoundDate = roundInspDate, rscheduleDateId = rscheduleDateId });

            UISession.AddCurrentPage("Round_NewRoundInsp", 0, "New Round");
            var rounds = new TRounds();
            if (roundGroupId.HasValue && rgid.HasValue)
            {
                rounds.RoundScheduleId = rgid;
                var data = _roundInspectionService.GetRoundUserGroup(roundGroupId.Value, "", 1);
                var roundSchedules = data?.RoundSchedules.FirstOrDefault(x => x.TRoundGroupId == rgid.Value);
                var roundCatIds = roundSchedules.Categories?.Split(',');
                if (roundSchedules != null)
                {
                    var roundInspectionDate =
                        roundSchedules.RoundScheduleDates.FirstOrDefault(x => x.Id == rscheduleDateId);

                    if (roundInspectionDate != null)
                        rounds.RoundDate = roundInspectionDate.RoundDate;
                    rounds.RoundCatId = roundSchedules.RoundCatId;
                    rounds.Locations = _roundInspectionService.GetRoundGroupLocations(roundGroupId.Value,0);
                }
                rounds.RoundName = data.Name;
                var roundGroupUsersForThis = data.RoundGroupUsers.Where(x => x.RoundCategories.Split(',').Any(p => roundCatIds.Any(fp => fp == p)));
                var roundsUers = new List<TRoundUsers>();
                foreach (var roundsuer in roundGroupUsersForThis)
                {
                    var user = new TRoundUsers { User= roundsuer.userProfile, IsActive = true, RoundUserId = roundsuer.UserId };
                    if (!roundsUers.Any(x => x.RoundUserId == user.RoundUserId))
                        roundsUers.Add(user);
                }
                rounds.TRoundUsers = roundsUers;
            }
            return View(rounds);
        }

        public ActionResult RoundIssues(int rId)
        {
            UISession.AddCurrentPage("Round_RoundIssues", 0, "Round Issues");
            var rounds = _roundService.GetRoundWODetails(rId);
            ViewBag.BuildingTRoundId = rId;
            return View(rounds);
        }
        public ActionResult GroupRoundIssues(string rId, string groupid, string userids, DateTime? rounddate)
        {
            UISession.AddCurrentPage("Round_GroupRoundIssues", 0, "Group Round Issues");
            List<TRounds> objtround = new List<TRounds>();
            foreach (string roundid in rId.Split(","))
            {
                if (!string.IsNullOrEmpty(roundid))
                {
                    var rounds = _roundService.GetRoundWODetails(Convert.ToInt32(roundid));
                    objtround.Add(rounds);
                }
            }

            ViewBag.BuildingTRoundId = rId;
            ViewBag.groupid = groupid;
            ViewBag.userids = userids;
            ViewBag.rounddate = rounddate;
            return View(objtround);
        }

        public ActionResult GetPrtialRoundDetails(int roundId)
        {
            var rounds = _roundService.GetRoundDetails(roundId);
            return PartialView("_RoundDetails", rounds);
        }

        public ActionResult FloorRoundInspction(int rId, int floorId)
        {
            UISession.AddCurrentPage("Round_FloorRoundInspction", 0, "Round Inspection History");
            var rounds = _roundService.GetRoundDetails(rId);
            ViewBag.BuildingTRoundId = rId;
            ViewBag.FloorTRoundId = floorId;
            return View(rounds);
        }

        public ActionResult ShowCombinedInspection(int floorId, string roundid)
        {
            UISession.AddCurrentPage("Round_FloorRoundInspction", 0, "Round Inspection History");
            List<TRounds> lstround = new List<TRounds>();
            foreach (string id in roundid.TrimEnd(',').Split(','))
            {
                var rounds = _roundService.GetRoundDetails(Convert.ToInt32(id));
                lstround.Add(rounds);
            }
            ViewBag.FloorTRoundId = floorId;
            return View(lstround);
        }

        public ActionResult GetBuildingRounds(int? buildingId, int? roundCatId)
        {
            var tRounds = buildingId.HasValue ? _roundService.GetBuildingRound(buildingId) : _roundService.GetRoundDetails();
            if (roundCatId.HasValue)
                tRounds = tRounds.Where(x => x.RoundCatId == roundCatId).ToList();

            return PartialView("_roundLists", tRounds);
        }

        public ActionResult RoundsQuestonaries(int floorId, int rId)
        {
            return ViewComponent("GroupRoundsQuestonaries", new { floorId = floorId, rId = rId, seq = "1", troundId = rId });
        }

        [HttpPost]
        public ActionResult SaveRound(TRounds rounds, string floorIds, string users)
        {
            rounds.CreatedBy = UserSession.CurrentUser.UserId;
            rounds.RoundCategories = Convert.ToString(rounds.RoundCatId);
            rounds.TRoundId = _roundService.AddRoundInspection(rounds, floorIds, users);
            var result = _roundService.SaveTRoundUserCategories(rounds.TRoundId, rounds.CreatedBy, rounds.RoundCategories);
            return RedirectToAction("RoundInspection", new { rid = rounds.TRoundId });
        }

        public ActionResult InspectGroupRound(int roundId, string roundCatIds)
        {
            var userId = UserSession.CurrentUser.UserId;
            if (string.IsNullOrEmpty(roundCatIds))
                return Json(false);

            var result = _roundService.SaveTRoundUserCategories(roundId, userId, roundCatIds);
            return Json(result);
            //var trounds = new TRounds();
            //trounds.CreatedBy = UserSession.CurrentUser.UserId;
            //trounds.RoundScheduleId = rgid;
            //trounds.RoundCatId = 0;
            //trounds.RoundDate = !string.IsNullOrEmpty(rounddate) ? Convert.ToDateTime(rounddate, CultureInfo.InvariantCulture) : DateTime.Now;
            //trounds.RoundCategories = roundCatIds;
            //trounds.TRoundId = _roundService.AddRoundInspection(trounds, floorIds, users);
            //return Json(trounds);
        }

        public ActionResult GetGroups(int id)
        {
            var troundGroup = _roundInspectionService.GetTGroupRoundGroup(id, "GroupName");
            return PartialView("_getGroups", troundGroup);
        }

        public ActionResult GetGroupUserDetails(int id)
        {
            var troundGroup = _roundInspectionService.GetTGroupRoundGroup(id, "Groupuser");
            var rmodel = new RoundViewModel
            {
                userList = _userService.GetUsers().Where(x => x.IsActive).ToList(),
            };
            rmodel.rounds.TroundGroups = troundGroup;
            return PartialView("_getGroupUserDetails", rmodel);
        }

        public ActionResult RoundLocations(int roundId)
        {
            var buildings = _roundService.GetRoundLocations(roundId);
            return PartialView("_RoundLocations", buildings);
        }

        public ActionResult SaveLocation(int roundId, int floorId, bool status)
        {
            var locations = new TRoundLocations
            {
                AddedBy = UserSession.CurrentUser.UserId,
                IsActive = status,
                FloorId = floorId,
                TRoundId = roundId
            };
            int result = _roundService.AddRoundLocation(locations);
            return Json(new { result });
        }

        public ActionResult SaveInspector(int roundId, int userId, bool status)
        {
            var user = new TRoundUsers
            {
                AddedBy = UserSession.CurrentUser.UserId,
                IsActive = status,
                RoundUserId = userId,
                TRoundId = roundId
            };
            var result = _roundService.AddRoundInspector(user);
            return Json(new { result });
        }

        public ActionResult RoundInspectors(int roundId)
        {
            var users = _roundService.GetRoundUsers(roundId);
            return PartialView("_RoundInspectors", users);
        }

        [HttpPost]
        public ActionResult SaveRoundInspection(TRoundInspections inspection)
        {
            var currentUser = UserSession.CurrentUser;
            bool result = false;
            if (inspection != null)
            {
                inspection.UserId = currentUser.UserId;
                if (inspection.Questionnaires.Count > 0)
                {
                    var currentQuestions = inspection.Questionnaires.FirstOrDefault();

                    if (!string.IsNullOrEmpty(currentQuestions.FileContent))
                        currentQuestions.FilePath = _apiCommon.SaveFile(currentQuestions.FileContent, currentQuestions.FileName, "round_insp_attach", Convert.ToString(inspection.TRoundId)).FilePath;

                    result = _roundService.SaveRoundInspectionSteps(inspection.FloorId,
                        inspection.UserId, inspection.Status,
                        inspection.Comment, currentQuestions, inspection.RoundCatId);

                    if (inspection.Status == 0 && inspection.TRoundId > 0)
                    {
                        //var failResult = _roundService.SaveRoundFailStatus(inspection.Status, inspection.TRoundId);
                        ///*commented code for deficiency*/
                        //var rounds = _roundService.GetRoundWODetails(inspection.TRoundId);
                        //var TRoundWorkOrders = new List<TRoundWorkOrders>();
                        //int? issueid = 0;
                        //foreach (var roundInspection in rounds.Inspections.Where(x => x.FloorId == inspection.FloorId))
                        //{
                        //    foreach (var item in roundInspection.Questionnaires.ToList())
                        //    {
                        //        var objTRoundWorkOrders = new TRoundWorkOrders
                        //        {
                        //            TRoundId = rounds.TRoundId,
                        //            RoundName = rounds.RoundName,
                        //            questionIdText = item.RoundsQuestionnaires.RoundStep.Trim(),
                        //            FloorId = roundInspection.Floor.FloorId,
                        //            StepsId = item.RouQuesId.ToString()
                        //        };
                        //        if (!item.IssueId.HasValue)
                        //            item.IssueId = 0;
                        //        else
                        //            issueid = item.IssueId;

                        //        objTRoundWorkOrders.IssueId = issueid.Value;

                        //        if (objTRoundWorkOrders != null)
                        //            TRoundWorkOrders.Add(objTRoundWorkOrders);
                        //    }
                        //}
                        //if (TRoundWorkOrders != null && TRoundWorkOrders.Count > 0)
                        //{
                        //    var objWo = new WorkOrder();
                        //    var troundDescriptions = string.Empty;
                        //    if (!string.IsNullOrEmpty(TRoundWorkOrders.FirstOrDefault().questionIdText)) { troundDescriptions = string.Join(",", TRoundWorkOrders.Select(x => x.questionIdText)); }
                        //    objWo.TRoundWorkOrders = TRoundWorkOrders;
                        //    objWo.Description = TRoundWorkOrders.FirstOrDefault().RoundName + " : " + troundDescriptions;
                        //    var user = _userService.GetUser(currentUser.UserId);
                        //    objWo.RequesterEmail = user.Email;
                        //    objWo.RequesterName = user.FullName;
                        //    objWo.BuildingCode = rounds.Inspections.FirstOrDefault().Floor.BuildingCode;
                        //    objWo.SiteCode = rounds.Inspections.FirstOrDefault().Floor.Building.SiteCode;
                        //    objWo.RequesterPhone = user.PhoneNumber;
                        //    objWo.AccountCode = "8060";
                        //    objWo.SkillCode = "GM";
                        //    objWo.StatusCode = "ACTIV";
                        //    objWo.SubStatusCode = "READY";
                        //    objWo.TypeCode = "RQ";
                        //    objWo.PriorityCode = "2";
                        //    objWo.IssueId = TRoundWorkOrders.Any(x => x.IssueId > 0) ? TRoundWorkOrders.Where(x => x.IssueId > 0).FirstOrDefault().IssueId : 0;
                        //    if (objWo.TRoundWorkOrders != null && objWo.TRoundWorkOrders.Count > 0)
                        //    {

                        //        objWo.FloorId = objWo.TRoundWorkOrders.FirstOrDefault().FloorId;
                        //        objWo.DeficiencyCode = "RD";
                        //    }
                        //    objWo.CreatedBy = currentUser.UserId;
                        //    if (objWo != null && objWo.TRoundWorkOrders != null && objWo.TRoundWorkOrders.Count > 0)
                        //    {
                        //        var postData = _commonModelFactory.JsonSerialize<WorkOrder>(objWo);
                        //        var statusCode = Convert.ToInt32(HttpReponseStatusCode.Success);
                        //        var uri = APIUrls.Rounds_FloorRoundInspection;
                        //        var results = _httpService.CallPostMethod(postData, uri, ref statusCode);
                        //        if (statusCode == Convert.ToInt32(HttpReponseStatusCode.Success))
                        //        {
                        //            var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(results);
                        //            var newWo = httpResponse.Result.WorkOrder;
                        //            foreach (TRoundWorkOrders item in objWo.TRoundWorkOrders)
                        //            {
                        //                item.IssueId = newWo.IssueId;
                        //                _workOrderService.SaveTRounWorkOrder(item);
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
                else
                    result = _roundService.SaveRoundInspection(inspection);
            }
            return Json(new { result });
        }

        public ActionResult FloorRoundQuestonaries(int rId, int floorId)
        {
            var rounds = _roundService.GetFloorRoundsQuestionnaires(floorId, rId);
            return PartialView("_FloorRoundQuestonaries", rounds);
        }

        public ActionResult SaveRoundStatus(int roundId, int status)
        {
            var rounds = new TRounds { TRoundId = roundId, Status = status };
            var userid = HCF.Web.Base.UserSession.CurrentUser.UserId;
            var result = _roundService.SaveRoundStatus(rounds, userid);
            return Json(new { result });
        }

        public ActionResult SaveRoundInspStatus(int roundId, int floorId, int status)
        {
            var roundInspection = new TRoundInspections
            {
                UserId = UserSession.CurrentUser.UserId,
                Status = status,
                FloorId = floorId,
                TRoundId = roundId
            };
            var result = _roundService.SaveRoundInspStatus(roundInspection);
            if (roundId > 0 && status == 3)
            {
                var rounds = _roundService.GetRoundDetails(roundId);
                if (rounds != null)
                {
                    var inspections = rounds.Inspections;
                    foreach (var building in rounds.Locations.Where(x => x.IsActive).OrderBy(x => x.BuildingName))
                    {
                        foreach (var floor in building.Floor.Where(x => !inspections.Select(z => z.FloorId).Contains(x.FloorId)).OrderBy(x => x.Sequence))
                        {
                            roundInspection = new TRoundInspections
                            {
                                UserId = UserSession.CurrentUser.UserId,
                                Status = status,
                                FloorId = floor.FloorId,
                                TRoundId = roundId
                            };
                            result = _roundsService.SaveRoundInspection(roundInspection);
                        }
                    }
                }
            }
            return Json(new { result });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteRoundInspectionImage(string fileName, string filePath)
        {
            bool result;
            result = await _apiCommon.DeleteFile(fileName, filePath);
            return Json(new { result });
        }

        #endregion

        #region 

        // [CrxAuthorize(Roles = "Round_RoundCategories")]
        public ActionResult RoundCategories()
        {
            UISession.AddCurrentPage("Round_RoundCategories", 0, "Round Categories");
            var roundsCateg = _roundService.GetRoundCategories().ToList();
            return View(roundsCateg);
        }

        // [CrxAuthorize(Roles = "Round_RoundQuestionnaire")]
        public ActionResult RoundQuestionnaire(int? roundCatId)
        {
            UISession.AddCurrentPage("Round_RoundQuestionnaire", 0, "Round Questionnaire");
            var roundsCateg = _roundService.GetRoundCategories_Data().ToList();
            ViewBag.RoundCatId = roundCatId;
            return View(roundsCateg);
        }

        public ActionResult AddRoundsQuestonaries(int roundCatId, int? roundQuestionId, string isTempQuestion)
        {
            ViewBag.RoundCatId = roundCatId;
            var roundsQuestionnair = new RoundsQuestionnaires();
            var roundQues = _roundService.GetRoundCategories_Data().ToList();
            ViewBag.RoundCategories = roundQues;

            if (roundCatId > 0)
                roundsQuestionnair = roundQues.FirstOrDefault(x => x.RoundCatId == roundCatId).RoundItems.FirstOrDefault(x => x.RouQuesId == roundQuestionId);
            else
                ViewBag.RoundCategories = roundQues.Where(x => x.IsActive).ToList();

            if (roundsQuestionnair == null)
            {
                roundsQuestionnair = new RoundsQuestionnaires();
                roundsQuestionnair.IsCommonRoundQues = false;
                roundsQuestionnair.IsActive = true;
            }

            if (!string.IsNullOrEmpty(isTempQuestion) && isTempQuestion == "1")
                roundsQuestionnair.IsOneTimeStep = true;
            else
                roundsQuestionnair.IsOneTimeStep = false;

            return PartialView("~/Views/Shared/PopUp/_AddRoundsQuestonaries.cshtml", roundsQuestionnair);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTempRoundsQuestionnaires(RoundsQuestionnaires roundQues, int roundId, int floorId)
        {
            var roundsQuestionnaires = new TRoundsQuestionnaires
            {
                RoundsQuestionnaires = roundQues,
                TRoundId = roundId,
                Status = -1
            };
            roundQues.IsShared = false;
            roundQues.IsActive = true;
            roundQues.RiskType = RiskScore.Low;
            roundQues.RiskStepCode = "";
            roundQues.CreatedBy = UserSession.CurrentUser.UserId;
            roundQues.IsOneTimeStep = true;
            roundQues.RouQuesId = _roundsService.AddRoundsQuestionnaires(roundQues);
            roundsQuestionnaires.RouQuesId = roundQues.RouQuesId;
            roundsQuestionnaires.RoundsQuestionnaires = roundQues;
            ViewBag.troundId = roundId;

            _roundService.SaveRoundInspectionSteps(floorId,
                              roundQues.CreatedBy, -1,
                              "", roundsQuestionnaires, roundQues.RoundCatId);

            return PartialView("_RoundCategoryStepInspection", roundsQuestionnaires);
        }

        public ActionResult AddTempRoundsQuestionnaires(int roundId, int roundCatId, int floorId)
        {
            RoundsQuestionnaires roundsQuestionnaires = new RoundsQuestionnaires();
            return PartialView(roundsQuestionnaires);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoundsQuestonaries(RoundsQuestionnaires roundQues)
        {
            bool isNew = roundQues.RouQuesId <= 0;
            roundQues.IsShared = true;
            roundQues.CreatedBy = UserSession.CurrentUser.UserId;
            string postData = _commonModelFactory.JsonSerialize<RoundsQuestionnaires>(roundQues);
            int statusCode = Convert.ToInt32(HttpReponseStatusCode.Success);
            string result = _httpService.CallPostMethod(postData, APIUrls.Rounds_AddRoundsQuestonaries, ref statusCode);
            if (statusCode == Convert.ToInt32(HttpReponseStatusCode.Success))
            {
                var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                if (httpresponse.Success)
                {
                    SuccessMessage = (isNew) ? "Round question " + httpresponse.Message : "Round question " + ConstMessage.Success_Updated;
                    return RedirectToRoute("roundQuest");
                }
                else
                {
                    ErrorMessage = httpresponse.Message;
                    return RedirectToRoute("roundQuest");
                }
            }
            else
            {
                ErrorMessage = ConstMessage.Internal_Server_Error;
                return RedirectToRoute("roundQuest");
            }
        }

        public ActionResult AddRoundCategory(int? roundCatId)
        {
            var roundscate = new RoundCategory();
            if (roundCatId.HasValue)
            {
                roundscate = _roundService.GetRoundCategory(roundCatId.Value).FirstOrDefault();
            }
            return PartialView("_AddRoundCategory", roundscate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoundCategory(RoundCategory roundQues)
        {
            roundQues.CreatedBy = UserSession.CurrentUser.UserId;
            _roundService.AddRoundCategory(roundQues);
            _roundService.Update_RoundSchduleDatesOnRoundCat(roundQues);
            return RedirectToAction("RoundCategories");
        }

        public ActionResult AddQuesttoCustomRound(int roundCatId = 0)
        {
            var roundsCateg = _roundService.GetRoundCategories().ToList();
            foreach (var item in roundsCateg)
            {
                item.RoundItems.RemoveAll(x => x.RoundCatId == roundCatId);
            }
            ViewBag.RoundCategoryId = roundCatId;
            return PartialView(roundsCateg);
        }

        public JsonResult AddQuestionToCustomRound(int roundQuestionnaireId = 0, int roundCatId = 0, bool status = true)
        {
            bool result = _roundService.AddQuestionToCustomRound(roundQuestionnaireId, roundCatId, status);
            string message = result ? "Added successfully" : "Questionnaire already added";
            return Json(new { status = result, message = message });
        }

        public ActionResult ManageAddRoundsQuestonaries(int roundcatId)
        {
            var roundsCateg = _roundService.GetRoundCategories().ToList();
            var res = _commonModelFactory.JsonSerialize<RoundCategory>(roundsCateg);
            ViewBag.ModelData = res;
            return View(roundsCateg);
        }

        public ActionResult GetRoundQuestionExcept(int roundCatId)
        {
            List<RoundCategory> lists;
            if (roundCatId > 0)
                lists = _roundService.GetRoundCategories().Where(x => x.RoundCatId != roundCatId).ToList();
            else
                lists = _roundService.GetRoundCategories().ToList();

            ViewBag.RoundItems = _roundService.GetRoundCategories().FirstOrDefault(x => x.RoundCatId == roundCatId);
            ViewBag.RoundCategoryId = lists;
            return View("_GetQuestCustomRound", lists);
        }

        #endregion

        #region Round Sceduler

        [CrxAuthorize(Roles = "round_roundScheduler")]
        public ActionResult RoundScheduler()
        {
            UISession.AddCurrentPage("Rounds_RoundScheduler", 0, "Round Scheduler");
            return View();
        }

        [CrxAuthorize(Roles = "round_roundScheduler")]
        [HttpGet]
        public ActionResult AddRoundGroup(int? roundUserGroupId, string mode, int? IsRoundlistRequest = 0)
        {
            UISession.AddCurrentPage("Rounds_addRoundUserGroup", 0, "Add RoundUserGroup");
            ViewBag.mode = mode;
            ViewBag.IsRoundlistRequest = IsRoundlistRequest;

            var newRoundUserGroup = new RoundGroup();
            if (roundUserGroupId == null)
            {
                newRoundUserGroup.CreatedBy = UserSession.CurrentUser.UserId;
                newRoundUserGroup.IsActive = 1;
                return PartialView("AddRoundGroup", newRoundUserGroup);
            }
            newRoundUserGroup = _roundInspectionService.GetRoundUserGroup().FirstOrDefault(x => x.RoundGroupId == roundUserGroupId);

            return PartialView("AddRoundGroup", newRoundUserGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoundGroup(RoundGroup newRoundUserGroup, string IsRoundlistRequest)
        {
            if (ModelState.IsValid)
            {
                var lstgroup = _roundInspectionService.GetRoundUserGroup().Where(x => x.Name.ToLower() == newRoundUserGroup.Name.ToLower()).ToList();
                if (newRoundUserGroup.RoundGroupId > 0 && lstgroup != null && lstgroup.Count > 0)
                {
                    lstgroup = lstgroup.Where(x => x.RoundGroupId != newRoundUserGroup.RoundGroupId).ToList();
                }
                if (lstgroup.Any())
                {
                    var result = new
                    {
                        IsExists = 1,
                        Result = newRoundUserGroup.RoundGroupId,

                    };
                    return Json(result);
                }
                else
                {
                    newRoundUserGroup.RoundGroupId = _roundInspectionService.Save(newRoundUserGroup);
                    newRoundUserGroup.CreatedBy = UserSession.CurrentUser.UserId;
                    //if (newRoundUserGroup.RoundGroupId > 0)
                    //    SuccessMessage = Utility.ConstMessage.Saved_Successfully;

                    if (!string.IsNullOrEmpty(IsRoundlistRequest) && IsRoundlistRequest != "0")
                    {
                        var result = new
                        {
                            IsExist = 0,
                            Result = newRoundUserGroup.RoundGroupId,
                        };
                        return Json(result);
                    }
                }
                return RedirectToAction("RoundScheduler");
            }
            return View(newRoundUserGroup);
        }

        public ActionResult RoundGroup()
        {
            var roundUserGroup = _roundInspectionService.GetRoundUserGroup();
            return PartialView("_roundGroup", roundUserGroup);
        }

        public ActionResult UserGroup(int RoundType, int? ispopup = null, int? grproundid = null, int? caninspect = null)
        {
            ViewBag.RoundType = RoundType;
            ViewBag.grproundid = grproundid;
            var model = new RoundViewModel
            {
                userList = _userService.GetUsers().Where(x => x.IsActive).ToList(),
                roundLocationGroup = _roundInspectionService.GetRoundGroupList(),
                buildings = _buildingsService.GetBuildingFloors().Where(x => x.IsActive).ToList(),
            };
            model.Roundgrouplist = model.roundLocationGroup;
            if (ispopup.HasValue)
            {
                ViewBag.ispopup = 1;
            }
            if (caninspect.HasValue)
            {
                ViewBag.caninspect = 1;
            }
            return PartialView("_userGroup", model);
        }

        public ActionResult GetData(string mode, int id)
        {
            if (mode == "UserGroup")
            {
                var users = _roundInspectionService.GetUsers(id).ToList();
                return PartialView("_roundUsers", users);
            }
            else
            {
                var rgroup = _roundInspectionService.GetRoundGroup(id);
                return PartialView("_roundGroupData", rgroup);
            }
        }

        public ActionResult UpdateandSaveGroup(int userid, int groupid, bool isActive)
        {
            var roundGroupuser = new RoundGroupUsers
            {
                UserId = userid,
                RoundGroupId = groupid,
                IsActive = isActive,
                RoundCategories = null,
            };
            _roundInspectionService.InsertOrUpdateGroup(roundGroupuser);
            if (!isActive)
            {
                GetRoundgroupUsersDetails(groupid, UserActivityType.Removed.ToString(), userid, 0, null, null);
            }
            return Json("");
        }

        public ActionResult ManageTRoundUsers(int userid, int troundId, bool isActive)
        {
            var roundGroupuser = new TRoundUsers
            {
                TRoundId= troundId,
                 RoundUserId= userid, IsActive=isActive, AddedBy=UserSession.CurrentUser.UserId
            };
            _roundInspectionService.ManageTroundUsers(roundGroupuser);           
            return Json("");
        }


        public ActionResult UpdateGroupUserType(int roundcatId, int roundGroupId, int frequency, string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                var tRoundGroup = new RoundSchedules
                {
                    RoundCatId = roundcatId,
                    RoundGroupId = roundGroupId,
                    FrequencyId = (Frequency)frequency,
                    StartInitialDate = Convert.ToDateTime(date)
                };
                var dates = "";
                var roundDates = GetRoundScheduleDates(tRoundGroup.FrequencyId, tRoundGroup.StartInitialDate, string.Empty, null);
                if (roundDates != null && roundDates.Count > 0)
                    dates = string.Join(",", roundDates.Select(x => x.RoundDate).Distinct());
                _roundInspectionService.InsertOrUpdateGroupUserType(tRoundGroup, dates);
                GetRoundgroupUsersDetails(roundGroupId, UserActivityType.Added.ToString(), 0, roundcatId, null, null);
            }
            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateIndividualRecurencePattern(RoundGroup newRoundUserGroup, string roundTime)
        {
            if (!string.IsNullOrEmpty(newRoundUserGroup.StartDate.ToString()))
            {
                var tRoundGroup = new RoundSchedules
                {
                    RoundCatId = Convert.ToInt32(newRoundUserGroup.Roundcategories),
                    RoundGroupId = newRoundUserGroup.RoundGroupId,
                    FrequencyId = (Frequency)newRoundUserGroup.Frequency,
                    StartInitialDate = Convert.ToDateTime(newRoundUserGroup.StartDate),
                    EndDate = newRoundUserGroup.EndDate,
                    Occurence = newRoundUserGroup.Ocurrence,
                    Dayno = newRoundUserGroup.Dayno,
                    Monthno = newRoundUserGroup.Monthno,
                    RecurFor = newRoundUserGroup.RecurFor,
                    ReoccurEvery = newRoundUserGroup.ReoccurEvery,
                    Weekno = newRoundUserGroup.Weekno,
                    The = newRoundUserGroup.The,
                    Yearno = newRoundUserGroup.The,
                    Categories = newRoundUserGroup.Roundcategories
                };

                if (!string.IsNullOrEmpty(roundTime))
                    tRoundGroup.Time = DateTime.Parse(roundTime).TimeOfDay;

                var dates = "";
                var roundDates = GetRoundScheduleDates(tRoundGroup.FrequencyId, tRoundGroup.StartInitialDate, tRoundGroup.Occurence, tRoundGroup.EndDate, tRoundGroup.ReoccurEvery, tRoundGroup.RecurFor, tRoundGroup.Monthno, tRoundGroup.Yearno, tRoundGroup.Weekno, tRoundGroup.Dayno, tRoundGroup.The);
                if (roundDates != null && roundDates.Count > 0)
                    dates = string.Join(",", roundDates.Select(x => x.RoundDate).Distinct());
                _roundInspectionService.InsertOrUpdateGroupUserType(tRoundGroup, dates);
                //if (tRoundGroup.RoundGroupId > 0 && UserActivityType.Added > 0 && tRoundGroup.RoundCatId > 0)
                //    GetRoundgroupUsersDetails(tRoundGroup.RoundGroupId, UserActivityType.Added.ToString(), 0, tRoundGroup.RoundCatId, null, null);
            }
            return Json("");
        }

        public void GetRoundgroupUsersDetails(int roundgroupid, string Type, int userid, int roundcatid, string startDate, string endDate)
        {
            var getRoundDetails = _roundInspectionService.GetRoundGroupDetails(roundgroupid);
            List<ActivityData> activityDatas = _roundInspectionService.GetNotifyDetails_RoundsDetails().ToList();
            if (Type == UserActivityType.Removed.ToString())
            {
                var roundgrps = _roundInspectionService.GetNotifyDetails_RoundsDetails().Where(x => x.user.UserId == userid).ToList();
                var user = getRoundDetails.GroupUsers.FirstOrDefault(x => x.UserId == userid);
                if (roundgrps.Count > 0)
                {
                    foreach (var item in roundgrps)
                    {
                        _activityService.RoundUserRemoved(user, item.Data, item.EffectiveDate.ToString(), UserSession.CurrentUser != null && UserSession.CurrentUser.UserId > 0 ? UserSession.CurrentUser.UserId : 0);
                    }
                }
                foreach (var item in activityDatas.Where(x => x.user.UserId == userid && x.roundGroupId == roundgroupid))
                {
                    _roundInspectionService.IsmailNotified(item.Id, UserActivityType.Removed.ToString());
                }

            }
            else if (Type == UserActivityType.Added.ToString())
            {
                var schdules = getRoundDetails.RoundSchedules.Where(x => x.RoundCatId == roundcatid).ToList();
                foreach (var user in getRoundDetails.RoundGroupUsers)
                {
                    if (!string.IsNullOrEmpty(user.RoundCategories))
                    {
                        foreach (var schdule in schdules)
                        {
                            if (user.RoundCategories.Contains(roundcatid.ToString()))
                            {

                                _activityService.RoundUserAdded(user.userProfile, roundgroupid, roundcatid, schdule.StartInitialDate.ToString(), UserSession.CurrentUser != null && UserSession.CurrentUser.UserId > 0 ? UserSession.CurrentUser.UserId : 0);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var user in getRoundDetails.roundVolunteers)
                {
                    if (user.RoundCategories.Contains(roundcatid.ToString()) && user.RoundGroupId == roundgroupid && user.Userid == userid)
                    {
                        if (Type == UserActivityType.TemporaryReplaced.ToString())
                        {
                            _activityService.RoundUserTemporarilyReplaced(user.userProfile, roundgroupid, roundcatid.ToString(), user.StartDate.ToString(), UserSession.CurrentUser != null && UserSession.CurrentUser.UserId > 0 ? UserSession.CurrentUser.UserId : 0, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
                        }
                        else
                        {
                            _activityService.RoundUserPermanentlyReplaced(user.userProfile, roundgroupid, roundcatid.ToString(), user.StartDate.ToString(), UserSession.CurrentUser != null && UserSession.CurrentUser.UserId > 0 ? UserSession.CurrentUser.UserId : 0);
                        }
                    }
                }
            }
        }

        public ActionResult Group_RoundType()
        {
            var rmodel = new RoundViewModel
            {
                Roundgrouplist = _roundInspectionService.GetRoundUserGroup().Where(x => x.IsActive == 1).ToList(),
                roundCategory = _roundsService.GetRoundCategories().Where(x => x.IsActive).OrderBy(x => x.CategoryName).ToList(),
            };
            return PartialView("_groupRoundType", rmodel);
        }
        public ActionResult UserRoundType(string id)
        {
            ViewBag.RoundType = 1;
            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.SelectedID = id;
                var rmodel = new RoundViewModel
                {
                    roundGroup = _roundInspectionService.GetRoundUserGroup(Convert.ToInt32(id), null, 1),
                };
                return PartialView("_getgroupData", rmodel);
            }
            var model = new RoundViewModel
            {
                Roundgrouplist = _roundInspectionService.GetRoundUserGroup().ToList(),
            };
            return PartialView("_userRoundType", model);
        }

        public ActionResult SaveUserCategories(string roundcatId, int roundGroupId, bool IsActive, int UserID)
        {
            List<int> roundcategories = new List<int>();
            var roundgroupusers = _roundInspectionService.roundUserGroups(UserID).FirstOrDefault(x => x.UserId == UserID && x.RoundGroupId == roundGroupId);
            if (roundgroupusers != null && !string.IsNullOrEmpty(roundgroupusers.RoundCategories))
                roundcategories = roundgroupusers.RoundCategories.Split(',').Select(x => int.Parse(x)).ToList();

            if (IsActive)
                roundcategories.Add(Convert.ToInt32(roundcatId));
            else
                roundcategories.Remove(Convert.ToInt32(roundcatId));

            var tRoundgroup = new RoundGroupUsers
            {
                RoundGroupId = roundGroupId,
                IsActive = IsActive,
                UserId = UserID,
                RoundCategories = string.Join(",", roundcategories.Distinct())
            };
            _roundInspectionService.InsertOrUpdateGroup(tRoundgroup);
            return Json("");
        }


        public ActionResult RoundSchedulerLocation(int RoundType, int? caninspect = null)
        {
            ViewBag.RoundType = RoundType;
            var model = new RoundViewModel
            {
                buildings = _buildingsService.GetBuildingFloors().Where(x => x.IsActive).ToList(),
                Roundgrouplist = _roundInspectionService.GetRoundGroupList().Where(x => x.IsActive == 1).ToList(),
            };
            if (caninspect.HasValue)
            {
                ViewBag.caninspect = 1;
            }
            return PartialView("_roundSchedulerLocation", model);
        }


        public ActionResult RoundInspectionSelection(int RoundType, int caninspect, int roundGroupId)
        {
            ViewBag.RoundType = 2;
            var model = new RoundViewModel
            {
                buildings = _buildingsService.GetBuildingFloors().Where(x => x.IsActive).ToList(),
                Roundgrouplist = _roundInspectionService.GetRoundGroupList().Where(x => x.IsActive == 1).ToList(),
                userList = _userService.GetUsers().Where(x => x.IsActive).ToList(),
                //roundLocationGroup = _roundInspectionService.GetRoundGroupList(),
            };

            var roundGrouDetails = _roundInspectionService.GetRoundGroupDetails(roundGroupId);
            model.roundGroup = roundGrouDetails;
            return PartialView("RoundInspectionSelection", model);
        }

        public ActionResult RoundBuilding(int roundGroupId)
        {
            var model = new RoundViewModel
            {
                Roundgrouplist = _roundInspectionService.GetRoundUserGroup().Where(x => x.IsActive == 1).ToList(),
                buildings = _buildingsService.GetBuildingFloors().Where(x => x.IsActive).ToList(),
            };
            return PartialView("_roundsBuildings", model);
        }

        public JsonResult SaveRoundGroupLocations(string roundGroupId, string buildingId, string floorId, bool IsActive)
        {
            RoundGroupLocations objroundGroupLocations = new RoundGroupLocations();
            objroundGroupLocations.RoundGroupId = Convert.ToInt32(roundGroupId);
            objroundGroupLocations.BuildingId = Convert.ToInt32(buildingId);
            objroundGroupLocations.FloorId = Convert.ToInt32(floorId);
            objroundGroupLocations.IsActive = IsActive;
            objroundGroupLocations.CreatedBy = UserSession.CurrentUser.UserId;
            var roundGroupLocationId = _roundInspectionService.SaveRoundGroupLocations(objroundGroupLocations);
            return Json(roundGroupLocationId);
        }

        public JsonResult SaveTRoundLocations(int troundId, string buildingId, string floorId, bool IsActive)
        {
            TRoundLocations objTRoundLocations = new TRoundLocations();
            objTRoundLocations.TRoundId = Convert.ToInt32(troundId);            
            objTRoundLocations.FloorId = Convert.ToInt32(floorId);
            objTRoundLocations.IsActive = IsActive;
            objTRoundLocations.AddedBy = Base.UserSession.CurrentUser.UserId;
            var roundGroupLocationId = _roundInspectionService.SaveTRoundLocations(objTRoundLocations);
            return Json(roundGroupLocationId);
        }

        public JsonResult GetRoundGroupLocations(string roundGroupId, int troundId)
        {
            var groupLocation = _roundInspectionService.GetRoundGroupLocations(Convert.ToInt32(roundGroupId), troundId);
            return Json(new { Result = groupLocation });
        }

        public JsonResult GetRoundGroupUsers(string roundGroupId, int RoundType, int troundId)
        {
            RoundGroup groupUsers = new RoundGroup();
            if (troundId > 0)
            {
                groupUsers = _roundInspectionService.GetTRoundUserGroup(troundId);
                return Json(new { Result = groupUsers.RoundGroupUsers.Where(x=>x.IsActive).ToList() });
            }

            groupUsers = _roundInspectionService.GetRoundUserGroup(Convert.ToInt32(roundGroupId), null, RoundType);
            return Json(new { Result = groupUsers });
        }

        public ActionResult ViewAssignment()
        {
            return ViewComponent("RoundViewAssignment");
        }

        public ActionResult ViewAssignmentData(int? userId, int? locationGroupId)
        {
            return ViewComponent("RoundViewAssignmentData", new { userId = userId, locationGroupId = locationGroupId });
        }

        private List<RoundScheduleDates> GetRoundScheduleDates(Frequency frequency, DateTime startDate, string Occurence, DateTime? endtime = null, int? recurecery = 1, int? recurfor = 1, int? month = null, int? year = null, int? week = null, int? day = null, int? The = null)
        {
            var roundScheduleDates = new List<RoundScheduleDates>();

            if (endtime != null && endtime.Value == DateTime.MinValue)
            {
                endtime = null;
            }

            var dates = ScheduleDateTime.GetDates((int)frequency, startDate, month.HasValue ? month.Value : 0, week.HasValue ? week.Value : 0, day.HasValue ? day.Value : 0, Occurence, endtime, recurecery.HasValue ? recurecery.Value : 1, The.HasValue ? The.Value : 0, recurfor.HasValue ? recurfor.Value : 1).Distinct().OrderBy(x => x.Date);
            foreach (var date in dates)
            {
                if (date.Date >= DateTime.Now.Date)
                {
                    var roundScheduleDate = new RoundScheduleDates();
                    if (date.DayOfWeek == DayOfWeek.Saturday && frequency != Frequency.Daily) { roundScheduleDate.RoundDate = date.AddDays(2); }
                    else if (date.DayOfWeek == DayOfWeek.Sunday && frequency != Frequency.Daily) { roundScheduleDate.RoundDate = date.AddDays(1); }
                    else if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) && (frequency == BDO.Enums.Frequency.Daily && (recurfor.HasValue && recurfor == 2)))
                    { }
                    else
                        roundScheduleDate.RoundDate = date;
                    roundScheduleDates.Add(roundScheduleDate);
                }
            }

            return roundScheduleDates;
        }
        public JsonResult GetRoundDate(DateTime start, DateTime end, int? locationGroupId, int? userId)
        {
            var events = new List<EventViewModel>();
            //userId = userId.HasValue && userId.Value > 0 ? userId : HCF.Web.Base.UserSession.CurrentUser.UserId;
            List<RoundScheduleDates> roundScheduleDates = new List<RoundScheduleDates>();
            roundScheduleDates = _roundInspectionService.GetRoundScheduleDates(locationGroupId, userId, start, end);
            var roundInMonth = roundScheduleDates.Where(x =>
            x.RoundDate.DayOfWeek != DayOfWeek.Sunday &&
            x.RoundDate.DayOfWeek != DayOfWeek.Saturday
            ).GroupBy(i => i.RoundDate.Date).Select(g => new
            {
                Date = g.Key,
                Id = g.FirstOrDefault().RoundScheduleId,
                start = g.FirstOrDefault().RoundDate,
                count = g.Count()
            });
            foreach (var item in roundInMonth)
            {
                events.Add(new EventViewModel()
                {
                    id = item.Id,
                    title = item.count.ToString(),
                    start = item.Date.ToString(CultureInfo.InvariantCulture),
                    allDay = true,
                    eventDate = item.Date.ToString(CultureInfo.InvariantCulture),
                });
            }
            return Json(events.ToArray());
        }

        private static string SetRoundStatus(int roundStatus)
        {
            var color = "#7DB45A";
            switch (roundStatus)
            {
                case 1:
                    color = "#7DB45A";
                    break;
                case 2:
                    color = "#808000";
                    break;
                default:
                    color = "#5A5BB4";
                    break;
            }
            return color;
        }

        public ActionResult GetRoundByDate(string start, int? locationGroupId, int? userId)
        {
            return ViewComponent("RoundGetRoundByDate", new { start = start, locationGroupId = locationGroupId, userId = userId });
        }

        public ActionResult GetRoundRecentChangesByDate(string start, int? locationGroupId, int? userId)
        {
            var roundGroups = _roundInspectionService.GetRoundRecentChangesByDate(start, locationGroupId, userId);
            if (locationGroupId != 0)
                roundGroups = roundGroups.Where(x => x.roundGroupId == locationGroupId).ToList();
            if (userId != 0)
                roundGroups = roundGroups.Where(x => x.user.UserId == userId).ToList();
            return PartialView("_RoundRecentChangesByDate", roundGroups);
        }

        public ActionResult OpenReplacePopup(int roundGroupId, int userid, string username)
        {
            var roundVolunteer = new RoundVolunteer();
            roundVolunteer.Userid = userid;
            roundVolunteer.RoundGroupId = roundGroupId;
            roundVolunteer.ReplacingUserName = username;
            roundVolunteer.userList = _userService.GetUsers().Where(x => x.IsActive && x.IsUserRole(HCF.BDO.Enums.UserRole.RoundVolunteer) && x.FullName != username).ToList();
            roundVolunteer.roundGroups = _roundInspectionService.GetUserRoundGroup(userid);
            return PartialView("_VolunteerReplacement", roundVolunteer);
        }

        public ActionResult ReplaceVolunteer(RoundVolunteer roundVolunteer)
        {
            List<string> grouplist = new List<string>();
            var status = false;
            if (roundVolunteer.ReplacementType == 1)
            {
                DateTime nextday = DateTime.Now + TimeSpan.FromDays(1);
                DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(Convert.ToDateTime(nextday));
                if (day == DayOfWeek.Saturday)
                {
                    roundVolunteer.StartDate = nextday + TimeSpan.FromDays(2);
                    roundVolunteer.EndDate = nextday + TimeSpan.FromDays(2);
                }
                else if (day == DayOfWeek.Sunday)
                {
                    roundVolunteer.StartDate = nextday + TimeSpan.FromDays(1);
                    roundVolunteer.EndDate = nextday + TimeSpan.FromDays(1);
                }
                else
                {
                    roundVolunteer.StartDate = DateTime.Now + TimeSpan.FromDays(1);
                    roundVolunteer.EndDate = DateTime.Now + TimeSpan.FromDays(1);
                }
            }
            if (roundVolunteer.ReplacementType == 3)
            {
                roundVolunteer.StartDate = roundVolunteer.EffectiveDate;
                roundVolunteer.EndDate = null;
            }

            if (!string.IsNullOrEmpty(roundVolunteer.SelectedChecks))
            {
                foreach (var i in roundVolunteer.SelectedChecks.Split(','))
                {
                    if (!grouplist.Contains(i.Split('_')[1]))
                        grouplist.Add(i.Split('_')[1]);
                }

                foreach (var value in grouplist)
                {
                    List<string> roundcategories = new List<string>();
                    roundVolunteer.RoundGroupId = Convert.ToInt32(value);
                    foreach (var i in roundVolunteer.SelectedChecks.Split(','))
                    {
                        if (i.Contains(value))
                        {
                            roundcategories.Add(i.Split('_')[0]);
                        }
                    }
                    roundVolunteer.RoundCategories = string.Join(",", roundcategories.Distinct());
                    var roundschdules = _roundInspectionService.GetRoundScheduleDatesIDS(roundVolunteer.RoundGroupId, roundVolunteer.Userid);
                    if (roundVolunteer.ReplacementType == 1)
                        roundschdules = roundschdules.Where(x => x.RoundDate.ToFormatDate() == roundVolunteer.StartDate.ToFormatDate()).ToList();
                    if (roundVolunteer.ReplacementType == 2)
                        roundschdules = roundschdules.Where(x => x.RoundDate >= roundVolunteer.StartDate && x.RoundDate <= roundVolunteer.EndDate).ToList();
                    if (roundVolunteer.ReplacementType == 3)
                        roundschdules = roundschdules.Where(x => x.RoundDate >= roundVolunteer.StartDate).ToList();
                    var roundSchedulesDates = new List<RoundScheduleDates>();
                    foreach (var cate in roundcategories)
                    {
                        var _roundSchedulesDates = roundschdules.Where(x => x.RoundCatId == Convert.ToInt32(cate)).ToList();
                        roundSchedulesDates.AddRange(_roundSchedulesDates);
                    }
                    if (roundSchedulesDates.Count == 0)
                    {
                        return Json("NoSchedules");
                    }

                    List<string> RsdIds_list = new List<string>();
                    foreach (var category in roundVolunteer.RoundCategories.Split(','))
                    {
                        var item = roundSchedulesDates.Where(x => x.RoundCatId == Convert.ToInt32(category)).ToList();
                        if (item != null)
                        {
                            foreach (var RGvalue in item)
                            {
                                RsdIds_list.Add(RGvalue.Id.ToString());
                            }
                        }
                    }
                    roundVolunteer.RoundSchedulesIds = string.Join(",", RsdIds_list.Distinct());
                    status = _roundInspectionService.UserReplacevolunteer(roundVolunteer);
                    foreach (var category in roundcategories)
                    {
                        if (roundVolunteer.ReplacementType == 1 || roundVolunteer.ReplacementType == 2)
                        {
                            GetRoundgroupUsersDetails(roundVolunteer.RoundGroupId, UserActivityType.TemporaryReplaced.ToString(), roundVolunteer.RoundVolunteerID, Convert.ToInt32(category), roundVolunteer.StartDate.ToString(), roundVolunteer.EndDate.ToString());
                        }
                        else
                        {
                            GetRoundgroupUsersDetails(roundVolunteer.RoundGroupId, UserActivityType.PermanentlyReplaced.ToString(), roundVolunteer.RoundVolunteerID, Convert.ToInt32(category), null, null);
                        }
                    }
                }
            }
            return Json(status);
        }

        public ActionResult GetRoundVolunteerDetails(int groupid, int userid)
        {
            var roundGroup = _roundInspectionService.GetUser_Volunteer_RoundGroup(groupid, userid);
            return PartialView("_VolunteerDetails", roundGroup);
        }

        public ActionResult RemoveReplaceVolunteer(int userid, int groupid, bool isActive)
        {
            var roundGroupuser = new RoundVolunteer
            {
                Userid = userid,
                RoundGroupId = groupid,
                IsActive = isActive
            };
            _roundInspectionService.RemoveReplaceVolunteer(roundGroupuser);
            return Json("");
        }
        #endregion

        #region GroupRounds Scheduler

        [CrxAuthorize(Roles = "round_roundScheduler")]
        public ActionResult GroupRoundScheduler()
        {
            UISession.AddCurrentPage("Rounds_GroupRoundScheduler", 0, "Group Round Scheduler");
            return View();
        }
        public ActionResult ScheduleGroupRound(int roundGroupId)
        {
            var newRoundUserGroup = new RoundGroup();
            var enumData = from SequenceOcuurence e in Enum.GetValues(typeof(SequenceOcuurence))
                           select new
                           {
                               Value = (int)e,
                               Text = e.ToString()
                           };

            var lstgroup = _roundInspectionService.GetRoundGroupList().Where(x => x.IsActive == 1 && x.RoundType == 2).ToList();
            if (lstgroup != null && lstgroup.Count > 0)
            {

                ViewBag.RoundGroups = new SelectList(lstgroup, "RoundGroupId", "Name");
            }

            if (roundGroupId > 0)
            {
                newRoundUserGroup = _roundInspectionService.GetRoundUserGroup(Convert.ToInt32(roundGroupId), null, 2);
                newRoundUserGroup.Roundcategories = newRoundUserGroup.RoundGroupUsers != null && newRoundUserGroup.RoundGroupUsers.Count > 0 ? newRoundUserGroup.RoundGroupUsers.FirstOrDefault().RoundCategories : string.Empty;
            }
            else
            {
                newRoundUserGroup.RoundGroupId = 0;
            }
            newRoundUserGroup.SequenceOcuurence = GetOtherSequence();
            newRoundUserGroup.DayName = GetDaysOfWeek();
            newRoundUserGroup.Months = Enumerable.Range(1, 12).Select(i => new KeyValuePair<int, string>(i, DateTimeFormatInfo.CurrentInfo.GetMonthName(i))).ToDictionary(x => x.Key, x => x.Value);
            return PartialView("_scheduleGroupRound", newRoundUserGroup);
        }
        public ActionResult RoundRecurence(int roundcatId, int roundGroupId, int frequency, string date)
        {
            UISession.AddCurrentPage("Rounds_RoundRecurence", 0, "Round Recurence");
            var newRoundUserGroup = new RoundGroup();
            var Roundgroup = _roundInspectionService.GetRoundUserGroup().Where(x => x.IsActive == 1 && x.RoundGroupId == roundGroupId).ToList();
            if (Roundgroup != null && Roundgroup.Count > 0)
            {
                var tgroup = Roundgroup.FirstOrDefault().RoundSchedules.FirstOrDefault(x => x.RoundCatId == roundcatId);
                if (tgroup != null)
                {
                    newRoundUserGroup = new RoundGroup()
                    {
                        RoundGroupId = roundGroupId,
                        StartDate = Convert.ToDateTime(tgroup.StartInitialDate),
                        Frequency = frequency,
                        Ocurrence = tgroup.Occurence,
                        Dayno = tgroup.Dayno,
                        Monthno = tgroup.Monthno,
                        RecurFor = tgroup.RecurFor,
                        ReoccurEvery = tgroup.ReoccurEvery,
                        Yearno = tgroup.ReoccurEvery,
                        EndDate = Convert.ToDateTime(tgroup.EndDate),
                        Weekno = tgroup.Weekno,
                        The = tgroup.The,
                        Roundcategories = roundcatId.ToString(),
                        IsRecurringRound = true,
                        Time = tgroup.Time
                    };
                }
                else
                {
                    newRoundUserGroup = new RoundGroup()
                    {
                        RoundGroupId = roundGroupId,
                        StartDate = Convert.ToDateTime(DateTime.Now),
                        Ocurrence = string.Empty,
                        Dayno = 0,
                        Monthno = 0,
                        RecurFor = 0,
                        Yearno = 0,
                        Weekno = 0,
                        The = 0,
                        Roundcategories = roundcatId.ToString()
                    };
                }

                var enumData = from SequenceOcuurence e in Enum.GetValues(typeof(SequenceOcuurence))
                               select new
                               {
                                   Value = (int)e,
                                   Text = e.ToString()
                               };

                newRoundUserGroup.SequenceOcuurence = GetOtherSequence();
                newRoundUserGroup.DayName = GetDaysOfWeek();
                newRoundUserGroup.Months = Enumerable.Range(1, 12).Select(i => new KeyValuePair<int, string>(i, DateTimeFormatInfo.CurrentInfo.GetMonthName(i))).ToDictionary(x => x.Key, x => x.Value);
            }
            return PartialView("RoundRecurence", newRoundUserGroup);
        }
        public ActionResult RoundGroupLists(int roundType)
        {
            var newRoundUserGroup = new List<RoundGroup>();
            var lstgroup = _roundInspectionService.GetRoundUserGroup().Where(x => x.RoundType == roundType).ToList();
            ViewBag.RoundType = roundType;
            return PartialView("_roundgrouplists", lstgroup);
        }
        public ActionResult BinRecurencePattern(int frequency)
        {
            var newRoundUserGroup = new RoundGroup();
            ViewData["frequency"] = frequency;
            newRoundUserGroup.Months = Enumerable.Range(1, 12).Select(i => new KeyValuePair<int, string>(i, DateTimeFormatInfo.CurrentInfo.GetMonthName(i))).ToDictionary(x => x.Key, x => x.Value);

            newRoundUserGroup.DayName = GetDaysOfWeek();
            var enumData = from SequenceOcuurence e in Enum.GetValues(typeof(SequenceOcuurence))
                           select new
                           {
                               Value = (int)e,
                               Text = e.ToString()
                           };
            newRoundUserGroup.SequenceOcuurence = GetOtherSequence();
            return PartialView("_recurencepattern", newRoundUserGroup);
        }
        public static Dictionary<int, string> GetDaysOfWeek()
        {
            Dictionary<int, string> daysOfWeek = new Dictionary<int, string>();
            for (int i = 0; i < 7; i++)
            {
                daysOfWeek.Add(i, Enum.GetName(typeof(DayOfWeek), i));
            }
            return daysOfWeek;
        }
        public static Dictionary<int, string> GetOtherSequence()
        {
            Dictionary<int, string> otherocuurence = new Dictionary<int, string>();
            foreach (var value in Enum.GetValues(typeof(SequenceOcuurence)))
            {
                otherocuurence.Add(Convert.ToInt32(value), value.GetDescription());
            }
            return otherocuurence;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScheduleGroupRound(RoundGroup newRoundUserGroup, string roundTime)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(roundTime))
                    newRoundUserGroup.Time = DateTime.Parse(roundTime).TimeOfDay;

                var lstgroup = _roundInspectionService.GetRoundGroupList().Where(x => x.Name.ToLower() == newRoundUserGroup.Name.ToLower()).ToList();
                if (newRoundUserGroup.RoundGroupId > 0 && lstgroup != null && lstgroup.Count > 0)
                {
                    lstgroup = lstgroup.Where(x => x.RoundGroupId != newRoundUserGroup.RoundGroupId).ToList();
                }
                if (lstgroup.Any())
                {
                    //ErrorMessage = ConstMessage.RoundExists;
                    var result = new
                    {
                        Result = newRoundUserGroup.RoundGroupId,
                        Active = newRoundUserGroup.IsActive,
                        RoundGroupId = newRoundUserGroup.RoundGroupId,
                        IsExists = 1,
                        ErrorMsg = ConstMessage.RoundExists
                    };
                    return Json(result);
                }
                else
                {
                    if (newRoundUserGroup != null && !string.IsNullOrEmpty(newRoundUserGroup.Ocurrence))
                    {
                        newRoundUserGroup.Ocurrence = newRoundUserGroup.Ocurrence.TrimEnd(',');
                        if (newRoundUserGroup.Frequency != 8)
                        {
                            newRoundUserGroup.Ocurrence = "";
                        }
                    }
                    newRoundUserGroup.CreatedBy = UserSession.CurrentUser.UserId;
                    newRoundUserGroup.RoundGroupId = _roundInspectionService.Save(newRoundUserGroup);
                    newRoundUserGroup.CreatedBy = UserSession.CurrentUser.UserId;
                    //SuccessMessage = ConstMessage.Saved_Successfully;
                    if (newRoundUserGroup.RoundGroupId > 0)
                    {
                        List<int> roundcategories = new List<int>();
                        if (newRoundUserGroup.RoundGroupId == 0 || string.IsNullOrEmpty(newRoundUserGroup.Roundcategories))
                        {
                            var tRoundgroup = new RoundGroupUsers
                            {
                                RoundGroupId = newRoundUserGroup.RoundGroupId,
                                IsActive = true,
                                UserId = 0,
                                RoundCategories = string.Join(",", roundcategories.Distinct())
                            };

                            var tRoundGroup = new RoundSchedules
                            {
                                RoundCatId = Convert.ToInt32(0),
                                RoundGroupId = newRoundUserGroup.RoundGroupId,
                                FrequencyId = (Frequency)newRoundUserGroup.Frequency,
                                StartInitialDate = Convert.ToDateTime(newRoundUserGroup.StartDate),
                                Time = newRoundUserGroup.Time,
                                Categories = string.Join(",", roundcategories.Distinct())
                            };
                            var dates = "";
                            var roundDates = GetRoundScheduleDates(tRoundGroup.FrequencyId, tRoundGroup.StartInitialDate, newRoundUserGroup.Ocurrence, Convert.ToDateTime(newRoundUserGroup.EndDate), newRoundUserGroup.ReoccurEvery, newRoundUserGroup.RecurFor, newRoundUserGroup.Monthno, newRoundUserGroup.Yearno, newRoundUserGroup.Weekno, newRoundUserGroup.Dayno, newRoundUserGroup.The);
                            if (roundDates != null && roundDates.Count > 0)
                                dates = string.Join(",", roundDates.Select(x => x.RoundDate).Distinct());
                            _roundInspectionService.InsertOrUpdateGroupUserType(tRoundGroup, dates);
                            _roundInspectionService.InsertOrUpdateGroup(tRoundgroup);
                        }
                        else
                        {
                            var tRoundgroup = new RoundGroupUsers
                            {
                                RoundGroupId = newRoundUserGroup.RoundGroupId,
                                IsActive = true,
                                UserId = 0,
                                RoundCategories = newRoundUserGroup.RoundGroupId > 0 ? newRoundUserGroup.Roundcategories : string.Join(",", roundcategories.Distinct())
                            };
                            var dates = "";

                            var freq = (Frequency)newRoundUserGroup.Frequency;
                            var roundDates = GetRoundScheduleDates(freq, newRoundUserGroup.StartDate.Value, newRoundUserGroup.Ocurrence, Convert.ToDateTime(newRoundUserGroup.EndDate), newRoundUserGroup.ReoccurEvery, newRoundUserGroup.RecurFor, newRoundUserGroup.Monthno, newRoundUserGroup.Yearno, newRoundUserGroup.Weekno, newRoundUserGroup.Dayno, newRoundUserGroup.The);
                            if (roundDates != null && roundDates.Count > 0)
                                dates = string.Join(",", roundDates.Select(x => x.RoundDate).Distinct());
                            if (!string.IsNullOrEmpty(newRoundUserGroup.Roundcategories))
                            {
                                //foreach (string cat in newRoundUserGroup.Roundcategories.Split(","))
                                //{
                                var tRoundGroup = new RoundSchedules
                                {
                                    RoundCatId = Convert.ToInt32(0),
                                    RoundGroupId = newRoundUserGroup.RoundGroupId,
                                    FrequencyId = (BDO.Enums.Frequency)newRoundUserGroup.Frequency,
                                    StartInitialDate = Convert.ToDateTime(newRoundUserGroup.StartDate),
                                    Categories = newRoundUserGroup.Roundcategories,
                                    Time = newRoundUserGroup.Time
                                };
                                _roundInspectionService.InsertOrUpdateGroupUserType(tRoundGroup, dates);
                                _roundInspectionService.InsertOrUpdateGroup(tRoundgroup);
                                // }
                            }
                        }
                    }
                    var result = new
                    {
                        Result = newRoundUserGroup.RoundGroupId,
                        Active = newRoundUserGroup.IsActive,
                        RoundGroupId = newRoundUserGroup.RoundGroupId,
                        IsExists = 0
                    };
                    return Json(result);
                }
            }
            return View(newRoundUserGroup);
        }
        public ActionResult RoundType(string id, int? isfirsttime)
        {
            var rmodel = new RoundViewModel();
            ViewBag.RoundGroupId = !string.IsNullOrEmpty(id) ? id : "0";
            ViewBag.RoundType = 2;
            if (isfirsttime.HasValue)
                id = "";

            if (!string.IsNullOrEmpty(id))
            {
                rmodel = new RoundViewModel
                {
                    roundGroup = _roundInspectionService.GetRoundUserGroup(Convert.ToInt32(id), null, 2),

                };
                return PartialView("_getgrouproundTypes", rmodel);
            }

            rmodel = new RoundViewModel
            {
                Roundgrouplist = _roundInspectionService.GetRoundGroupList().Where(x => x.IsActive == 1).ToList(),
                roundCategory = _roundsService.GetRoundCategories().Where(x => x.IsActive).OrderBy(x => x.CategoryName).ToList(),
            };
            if (!isfirsttime.HasValue)
            {
                ViewBag.RoundGroupId = "0";
            }
            return PartialView("_roundtype", rmodel);
        }
        public ActionResult groupRoundInsp(string id, int roundId, int? rgid, int? troundGroupId, int? rscheduleDateId, string users, string RoundDate)
        {
            var currentUserId = UserSession.CurrentUser.UserId;
            UISession.AddCurrentPage("Rounds_SchedulerPageFlow", 0, "Round Scheduler Page Flow");
            TRounds trounds = new TRounds();
            if (roundId == 0)
            {
                trounds = _roundService.GetRoundDetailsbyScheduleDateId(rscheduleDateId);
                if (trounds.TRoundId == 0)
                {
                    trounds.CreatedBy = UserSession.CurrentUser.UserId;
                    trounds.RoundScheduleId = rgid;
                    trounds.RoundCatId = 0;
                    trounds.RoundName = trounds.RoundGroup.Name;
                    trounds.RoundDate = !string.IsNullOrEmpty(trounds.RoundDate.ToString()) ? Convert.ToDateTime(trounds.RoundDate, CultureInfo.InvariantCulture) : DateTime.Now;
                    trounds.RoundCategories = trounds.RoundGroup.RoundSchedules.FirstOrDefault().Categories;
                    var floorIds = string.Join(",", trounds.RoundGroup.RoundGroupLocations.Select(x => x.FloorId));
                    users = string.Join(",", trounds.RoundGroup.RoundGroupUsers.Where(x=>x.UserId>0).Select(x => x.UserId));
                    trounds.TRoundId = _roundService.AddRoundInspection(trounds, floorIds, users);
                }
                else
                {

                    trounds = _roundsService.GetRoundDetails(trounds.TRoundId);
                }
            }
            else
            {
                trounds = _roundsService.GetRoundDetails(roundId);
                if (trounds != null && trounds.TRoundUsers.Any(x => x.RoundUserId == currentUserId))
                {
                    var roundUser = trounds.TRoundUsers.FirstOrDefault(x => x.RoundUserId == currentUserId);
                    if (!string.IsNullOrEmpty(roundUser.RoundCatIds))
                    {
                        return RedirectToAction("GroupRoundInspection", new { troundId = roundId });
                    }
                }
                else
                    return RedirectToAction("Index");

                if (trounds != null && trounds.TroundGroups.Any())
                {
                    id = Convert.ToString(trounds.TroundGroups.FirstOrDefault().RoundGroupId);
                }
            }
            ViewBag.roundgroupid = id;
            //ViewBag.roundscheduleId = rgid;
            //ViewBag.rscheduleDateId = rscheduleDateId;
            //ViewBag.roundId = roundId;
            //ViewBag.users = users;
            //ViewBag.RoundDate = RoundDate;
            return View(trounds);
        }

        public ActionResult RoundInspection(int rid)
        {
            return RedirectToAction("GroupRoundInspection", new { troundId = rid });
        }

        public ActionResult GroupRoundInspection(int troundId)
        {
            if (troundId == 0)
                return RedirectToRoute("newrounds");

            var currentUserId = UserSession.CurrentUser.UserId;

            ViewBag.tRoundId = troundId;
            var existingRound = _roundService.GetRoundDetails(troundId);
            if (existingRound == null || existingRound.TRoundId == 0)
                return RedirectToRoute("newrounds");

            var lstRounds = new List<TRounds>();
            lstRounds.Add(existingRound);


            var isRoundUser = existingRound.TRoundUsers.FirstOrDefault(x => x.RoundUserId == currentUserId);
            if (isRoundUser != null && string.IsNullOrEmpty(isRoundUser.RoundCatIds) && existingRound.RoundType==2)
                return RedirectToAction("GroupRoundInsp", new { roundId = troundId });

            ViewBag.FloorId = existingRound.DefaultFloorId;
            ViewBag.CanEdit = false;
            if (existingRound != null && existingRound.TRoundUsers != null && existingRound.TRoundUsers.Any(x => x.RoundUserId == currentUserId))
            {
                ViewBag.CanEdit = true;
            }

            return View(lstRounds);
        }

        //public ActionResult GroupRoundInspectionPartial(int troundId)
        //{
        //    var existingRound = _roundService.GetRoundDetails(troundId);
        //    if (existingRound == null || existingRound.TRoundId == 0)
        //        return RedirectToRoute("newrounds");

        //    var lstRounds = new List<TRounds>();
        //    lstRounds.Add(existingRound);
        //    ViewBag.FloorId = existingRound.DefaultFloorId;
        //    ViewBag.CanEdit = false;
        //    if (existingRound != null && existingRound.TRoundUsers != null && existingRound.TRoundUsers.Any(x => x.RoundUserId == UserSession.CurrentUser.UserId))
        //    {
        //        ViewBag.CanEdit = true;
        //    }
        //    return PartialView("_GroupRoundInspection", lstRounds);
        //}

        public ActionResult GroupRoundsQuestonaries(int floorId, int rId, int seq)
        {
            ViewBag.sequennce = seq;
            var rounds = _roundService.GetRoundsQuestionnaires(floorId, rId, UserSession.CurrentUser.UserId);
            return PartialView("_grouproundQuestions", rounds);
        }

        public ActionResult GetGroupRoundsQuestonariesView(int floorId, int rId, string seq, int rounCatId)
        {
            TRoundInspections roundInspections = new TRoundInspections();
            roundInspections.RoundCatId = rounCatId;
            roundInspections.TRoundId = rId;
            roundInspections.FloorId = floorId;
            roundInspections.UserId = UserSession.CurrentUser.UserId;
            roundInspections.IsAdditional = true;
            roundInspections.IsOpen = true;
            var status = _roundService.SaveRoundInspection(roundInspections);
            return ViewComponent("GroupRoundsQuestonaries", new { floorId = floorId, rId = rId, seq = seq });
        }

        public ActionResult SaveGroupRoundCategories(string roundcatId, int roundGroupId, bool IsActive, string listroundcatId)
        {
            var tRoundgroup = new RoundGroupUsers
            {
                RoundGroupId = roundGroupId,
                IsActive = IsActive,
                UserId = 0,
                RoundCategories = listroundcatId
            };
            //var Roundgroup = _roundInspectionService.GetRoundUserGroup().FirstOrDefault(x => x.IsActive == 1 && x.RoundGroupId == roundGroupId);
            //if (Roundgroup != null)
            //{
            //    var tRoundGroup = new RoundSchedules
            //    {
            //        RoundCatId = Convert.ToInt32(roundcatId),
            //        RoundGroupId = roundGroupId,
            //        FrequencyId = (Frequency)Roundgroup.Frequency,
            //        StartInitialDate = Convert.ToDateTime(Roundgroup.StartDate),

            //    };
            //    var dates = "";
            //    var roundDates = GetRoundScheduleDates(tRoundGroup.FrequencyId, tRoundGroup.StartInitialDate, Roundgroup.Ocurrence, Convert.ToDateTime(Roundgroup.EndDate), Roundgroup.ReoccurEvery);
            //    if (roundDates != null && roundDates.Count > 0)
            //        dates = string.Join(",", roundDates.Select(x => x.RoundDate).Distinct());
            //    _roundInspectionService.InsertOrUpdateGroupUserType(tRoundGroup, dates);
            //}

            //_roundService.UpdateRoundCategories(roundGroupId, roundCategories);

            _roundInspectionService.InsertOrUpdateGroup(tRoundgroup);
            return Json("");
        }

        #endregion

        #region Safer Matrix

        public ActionResult SaferMatrixRounds(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
                startDate = DateTime.Now.AddMonths(-3);
            if (!endDate.HasValue)
                endDate = DateTime.Now.AddHours(1);
            var data = _roundService.GetMatrixData(startDate.Value, endDate.Value);
            return PartialView("_saferMatrixRounds", data);
        }

        #endregion

        #region Rounds Common Questionnaries
        public ActionResult RoundCommonQuestionnaire(int? roundCatId)
        {
            UISession.AddCurrentPage("Round_Questionnaire", 0, "Round Questionnaire");
            var roundsQues = _roundService.RoundCommonQuestionnaries().ToList();
            ViewBag.CatId = roundCatId;
            return View(roundsQues);
        }

        public ActionResult AddRoundsCommQuestonaries(int roundCatId, int roundquesid)
        {
            var clientNo = _session.ClientNo;
            var roundsQuestionnaires = new RoundsQuestionnaires();
            ViewBag.CatId = roundCatId;
            var categories = _roundService.GetCommonRoundCategory().ToList();
            if (roundCatId > 0)
            {
                ViewBag.RoundCategory = categories.ToList();
                if (roundquesid > 0)
                {
                    roundsQuestionnaires = categories.Where(x => x.RoundCatId == roundCatId).FirstOrDefault().RoundItems.FirstOrDefault(x => x.RouQuesId == roundquesid);
                    if (roundsQuestionnaires.Applicable.Contains(clientNo))
                        roundsQuestionnaires.IsActive = true;
                }
            }
            else { ViewBag.RoundCategory = categories.Where(x => x.IsActive).ToList(); }
            return PartialView("~/Views/Round/_AddRoundsCommQuestonaries.cshtml", roundsQuestionnaires);
        }

        public ActionResult RoundCmnQuesActiveStatus(int id, bool status, int CatId)
        {
            var clientNo = Convert.ToInt32(_session.ClientNo);
            List<int> clientIds = new List<int>();
            if (id > 0)
            {
                var Ques = _roundService.RoundCommonQuestionnaries().Where(x => x.RoundCatId == CatId).FirstOrDefault().RoundItems.FirstOrDefault(x => x.RouQuesId == id);
                Ques.IsActive = status;
                Ques.IsCommonRoundQues = Ques.IsShared = true;
                if (!string.IsNullOrEmpty(Ques.Applicable))
                    clientIds = Ques.Applicable.Split(',').Select(x => int.Parse(x)).ToList();
                if (status)
                    clientIds.Add(clientNo);
                else
                    clientIds.Remove(clientNo);

                Ques.Applicable = string.Join(",", clientIds.Distinct());
                string postData = _commonModelFactory.JsonSerialize<RoundsQuestionnaires>(Ques);
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                string result = _httpService.CallPostMethod(postData, APIUrls.Rounds_AddRoundsCommQuestonaries, ref statusCode);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                {
                    var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    if (httpresponse.Success)
                    {
                        SuccessMessage = ConstMessage.Success_Updated;
                    }
                    else
                    {
                        ErrorMessage = httpresponse.Message;
                    }
                }
            }
            return RedirectToRoute("roundcommonques");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoundsCommQuestonaries(RoundsQuestionnaires roundQues)
        {
            var clientNo = Convert.ToInt32(_session.ClientNo);
            List<int> clientIds = new List<int>();
            bool isNew = roundQues.RouQuesId <= 0;
            roundQues.IsShared = roundQues.IsCommonRoundQues = true;
            roundQues.CreatedBy = UserSession.CurrentUser.UserId;

            if (roundQues.RoundCatId > 0 && roundQues.RouQuesId > 0)
            {
                var roundsCateg = _roundService.GetCommonRoundCategory().FirstOrDefault(x => x.RoundCatId == roundQues.RoundCatId);
                var roundques = roundsCateg.RoundItems.FirstOrDefault(y => y.RouQuesId == roundQues.RouQuesId);

                if (!string.IsNullOrEmpty(roundques.Applicable))
                    clientIds = roundques.Applicable.Split(',').Select(x => int.Parse(x)).ToList();
                if (roundQues.IsActive)
                    clientIds.Add(clientNo);
                else
                    clientIds.Remove(clientNo);

                roundQues.Applicable = string.Join(",", clientIds.Distinct());
            }
            else
            {
                roundQues.Applicable = _session.ClientNo;
            }

            string postData = _commonModelFactory.JsonSerialize<RoundsQuestionnaires>(roundQues);
            int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            string result = _httpService.CallPostMethod(postData, APIUrls.Rounds_AddRoundsCommQuestonaries, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                if (httpresponse.Success)
                {
                    SuccessMessage = (isNew) ? "Round question " + httpresponse.Message : "Round question " + ConstMessage.Success_Updated;
                }
                else
                {
                    ErrorMessage = httpresponse.Message;
                }
            }
            else
            {
                ErrorMessage = ConstMessage.Internal_Server_Error;
            }
            return RedirectToRoute("roundcommonques");
        }

        public ActionResult RoundCommonCategories()
        {
            UISession.AddCurrentPage("rounds_RoundCommonCategories", 0, "Round Category");
            var roundsCateg = _roundService.GetCommonRoundCategory().ToList();
            return View(roundsCateg);
        }

        public ActionResult AddRoundCommonCategory(int roundCatId)
        {
            var clientNo = Convert.ToInt32(_session.ClientNo);
            var roundscat = new RoundCategory();
            if (roundCatId > 0)
            {
                roundscat = _roundService.GetCommonRoundCategory().FirstOrDefault(x => x.RoundCatId == roundCatId);
                if (roundscat.Applicable.Contains(clientNo.ToString()))
                    roundscat.IsActive = true;
            }
            return PartialView("~/Views/Round/_AddRoundCommonCategory.cshtml", roundscat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoundCommonCategory(RoundCategory roundCat)
        {
            var clientNo = Convert.ToInt32(_session.ClientNo);
            List<int> clientIds = new List<int>();
            roundCat.CreatedBy = UserSession.CurrentUser.UserId;
            roundCat.IsCommonCat = true;

            if (roundCat.RoundCatId > 0)
            {
                var roundsCateg = _roundService.GetCommonRoundCategory().FirstOrDefault(x => x.RoundCatId == roundCat.RoundCatId);
                if (!string.IsNullOrEmpty(roundsCateg.Applicable))
                    clientIds = roundsCateg.Applicable.Split(',').Select(x => int.Parse(x)).ToList();
                if (roundCat.IsActive)
                    clientIds.Add(clientNo);
                else
                    clientIds.Remove(clientNo);

                roundCat.Applicable = string.Join(",", clientIds.Distinct());
            }
            else
            {
                roundCat.Applicable = _session.ClientNo;
            }
            _roundService.AddRoundCategory(roundCat);
            return RedirectToAction("RoundCommonCategories");
        }

        public ActionResult RoundCategoriesActiveStatus(int CatId, bool status)
        {
            var clientNo = Convert.ToInt32(_session.ClientNo);
            List<int> clientIds = new List<int>();
            if (CatId > 0)
            {
                var roundsCateg = _roundService.GetCommonRoundCategory().FirstOrDefault(x => x.RoundCatId == CatId);
                roundsCateg.IsCommonCat = true;
                roundsCateg.IsActive = status;
                if (!string.IsNullOrEmpty(roundsCateg.Applicable))
                    clientIds = roundsCateg.Applicable.Split(',').Select(x => int.Parse(x)).ToList();
                if (status)
                    clientIds.Add(clientNo);
                else
                    clientIds.Remove(clientNo);

                roundsCateg.Applicable = string.Join(",", clientIds.Distinct());
                _roundService.AddRoundCategory(roundsCateg);
                var Status = _roundService.Update_RoundSchduleDatesOnRoundCat(roundsCateg);
            }
            return RedirectToRoute("RoundCommonCategories");
        }

        #endregion

        #region Notify Volunteers

        public ActionResult NotifyVolunteer()
        {
            var roundgrps = _roundInspectionService.GetNotifyDetails_RoundsDetails().OrderByDescending(x => x.EffectiveDate).ToList();
            return PartialView("_NotifyVolunteer", roundgrps);
        }

        public ActionResult SendVolunteermails(string id)
        {
            var status = false;
            var activityList = new ActivityData();
            var list = new List<ActivityData>();
            string[] arr = id.Split(',').ToArray();
            var roundgrps = _roundInspectionService.GetNotifyDetails_RoundsDetails().ToList();
            foreach (var item in arr)
            {
                activityList = roundgrps.FirstOrDefault(x => x.Id == Guid.Parse(item));
                list.Add(activityList);
            }
            for (int i = 0; i < list.Count; i++)
            {
                var mailList = new List<ActivityData>();
                mailList = list.Where(x => x.user.UserId == list[i].user.UserId).ToList();
                _emailService.SendNotifyVolunteerMail(mailList, UserSession.CurrentUser.FullName);
                foreach (var uniqueid in mailList)
                {
                    status = _roundInspectionService.IsmailNotified(uniqueid.Id, UserActivityType.Added.ToString());
                }
                list.RemoveAll(x => x.user.UserId == list[i].user.UserId);
            }
            return Json(status);
        }

        public ActionResult RecentChangeLog()
        {
            var roundgrps = _roundInspectionService.GetNotifyDetails_RoundsDetails().Where(x => x.CreatedDate > DateTime.Now.Date.AddDays(-30)).OrderByDescending(x => x.EffectiveDate).ToList();
            return PartialView("_RecentChangeLog", roundgrps);
        }
        #endregion

        #region Round Deficiencies Binder

        public ActionResult RoundDeficienciesBinder()
        {
            var model = new RoundViewModel
            {
                userList = _userService.GetUsers().Where(x => x.IsActive && x.IsUserRole(HCF.BDO.Enums.UserRole.RoundVolunteer)).ToList(),
                Roundgrouplist = _roundInspectionService.GetRoundUserGroup().Where(x => x.IsActive == 1).ToList(),
            };
            ViewBag.buildingIds = "-1";
            return View(model);
        }

        public ActionResult PartialRoundDeficienciesBinder(string start, string enddate, string lstBuildingDetails, int? roundType)
        {

            var roundDate = (!string.IsNullOrEmpty(start)) ? Convert.ToDateTime(start) : default(DateTime);
            var roundendDate = (!string.IsNullOrEmpty(enddate)) ? Convert.ToDateTime(enddate) : default(DateTime);
            var reportfor = (roundendDate - roundDate).TotalDays + 1;
            string locationGroupId = "";
            if (!string.IsNullOrEmpty(lstBuildingDetails))
            {
                //var buildings = JsonConvert.DeserializeObject<List<Buildings>>(lstBuildingDetails);
                //locationGroupId = string.Join(",", buildings.Select(x => x.BuildingId));
                locationGroupId = lstBuildingDetails;
            }
            List<RoundGroup> roundGroups = new List<RoundGroup>();

            var roundGroup = _roundInspectionService.GetRoundByDateApp(roundDate, roundType.HasValue && roundType.Value > 0 ? roundType.Value : 0, UserSession.CurrentUser.UserId, true, Convert.ToInt32(reportfor), roundendDate).ToList();
            //roundGroup = roundGroup.FindAll(x => x.RoundSchedules.Any(x => x.RoundScheduleDates.Any(x => x.RoundStatus == "status_fail")) || x.Inspections.Any(x => x.Questionnaires.Any(x => x.Status == 0))).ToList();
            if (!string.IsNullOrEmpty(locationGroupId) && roundGroup != null && roundGroup.Count > 0 && locationGroupId != "0")
            {
                List<RoundGroup> lstPersons = new List<RoundGroup>();

                foreach (string building in locationGroupId.Split(','))
                {
                    if (!string.IsNullOrEmpty(building))
                    {
                        List<RoundGroup> lstround = new List<RoundGroup>();
                        lstround = roundGroup.FindAll(x => x.Locations.Any(y => y.BuildingId == Convert.ToInt32(building))).ToList();

                        foreach (var objround in lstround)
                        {
                            if (!roundGroups.Any(x => x.RoundGroupId == objround.RoundGroupId && x.RoundDate == objround.RoundDate && x.Locations.Any(y => y.BuildingId == Convert.ToInt32(building))))
                                roundGroups.Add(objround);
                        }
                    }
                }
                return PartialView("_roundDeficienciesBinder", roundGroups);
            }
            else
            {
                return PartialView("_roundDeficienciesBinder", roundGroup);
            }
            //var lists = _workOrderService.GetRoundWorkOrders();
        }

        public ActionResult RoundReportBinder()
        {
            var model = new RoundViewModel
            {
                userList = _userService.GetUsers().Where(x => x.IsActive && x.IsUserRole(HCF.BDO.Enums.UserRole.RoundVolunteer)).ToList(),
                Roundgrouplist = _roundInspectionService.GetRoundUserGroup().Where(x => x.IsActive == 1).ToList()
            };
            ViewBag.buildingIds = "-1";
            return View(model);
        }

        public JsonResult GetRoundType()
        {
            var RoundCategory = _roundService.GetRoundCategories_Data().ToList();
            return Json(RoundCategory);
        }

        //public ActionResult PartialRoundReportBinder(string start, string enddate, string lstBuildingDetails, string roundType)
        //{

        //    var roundDate = (!string.IsNullOrEmpty(start)) ? Convert.ToDateTime(start) : default(DateTime);
        //    var roundendDate = (!string.IsNullOrEmpty(enddate)) ? Convert.ToDateTime(enddate) : default(DateTime);
        //    var reportfor = (roundendDate - roundDate).TotalDays + 1;
        //    string locationGroupId = "";
        //    if (!string.IsNullOrEmpty(lstBuildingDetails))
        //    {
        //        //var buildings = JsonConvert.DeserializeObject<List<Buildings>>(lstBuildingDetails);
        //        //locationGroupId = string.Join(",", buildings.Select(x => x.BuildingId));
        //        locationGroupId = lstBuildingDetails;
        //    }
        //    List<RoundGroup> roundGroups = new List<RoundGroup>();

        //    var roundGroup = _roundInspectionService.GetRoundByDateApp(roundDate, 0, UserSession.CurrentUser.UserId, true, Convert.ToInt32(reportfor), roundendDate).ToList();
        //    roundGroup = roundGroup.Where(x => x.RoundSchedules.Any(y => y.RoundScheduleDates.Any(z => z.Status == 1 && z.TRoundId != null))).ToList();
        //    // roundGroup = roundGroup.FindAll(x => x.RoundSchedules.Any(x => x.RoundScheduleDates.Any(x => x.RoundStatus == "status_fail")) || x.Inspections.Any(x => x.Questionnaires.Any(x => x.Status == 0))).ToList();
        //    if (!string.IsNullOrEmpty(locationGroupId) && roundGroup != null && roundGroup.Count > 0 && (locationGroupId != "0" && !locationGroupId.Contains("-1")))
        //    {
        //        List<RoundGroup> lstPersons = new List<RoundGroup>();

        //        foreach (string building in locationGroupId.Split(','))
        //        {
        //            if (!string.IsNullOrEmpty(building))
        //            {
        //                List<RoundGroup> lstround = new List<RoundGroup>();
        //                lstround = roundGroup.FindAll(x => x.Locations.Any(y => y.BuildingId == Convert.ToInt32(building))).ToList();

        //                foreach (var objround in lstround)
        //                {
        //                    if (!roundGroups.Any(x => x.RoundGroupId == objround.RoundGroupId && x.RoundDate == objround.RoundDate && x.Locations.Any(y => y.BuildingId == Convert.ToInt32(building))))
        //                        roundGroups.Add(objround);
        //                }
        //            }
        //        }




        //    }
        //    else
        //    {
        //        roundGroups = roundGroup;

        //    }

        //    if (roundType != "0" && roundGroups != null)
        //    {
        //        List<RoundGroup> copyround = new List<RoundGroup>();
        //        copyround = roundGroups;
        //        roundGroups = new List<RoundGroup>();
        //        foreach (string type in roundType.Split(','))
        //        {
        //            if (!string.IsNullOrEmpty(type) && type != "-1")
        //            {
        //                List<RoundGroup> lstround = new List<RoundGroup>();
        //                lstround = copyround.FindAll(x => x.RoundCategory.Any(y => y.RoundCatId == Convert.ToInt32(type))).ToList();

        //                foreach (var objround in lstround)
        //                {
        //                    if (!roundGroups.Any(x => x.RoundGroupId == objround.RoundGroupId && x.RoundDate.Date == objround.RoundDate.Date && x.RoundCategory.Any(y => y.RoundCatId == Convert.ToInt32(type))))
        //                        roundGroups.Add(objround);

        //                }
        //            }
        //        }
        //    }
        //    return PartialView("_roundReportBinder", roundGroups);
        //}

        //
        //
        #endregion

        #region RoundActivity
        public ActionResult RoundActivity()
        {
            UISession.AddCurrentPage("RoundActivity", 0, "Round Activity");
            return View();
        }
        public ActionResult PartialRoundActivityReport(string start, string enddate, string lstBuildingDetails, string roundType, string pageMode)
        {
            var roundDate = (!string.IsNullOrEmpty(start)) ? Convert.ToDateTime(start) : default(DateTime);
            var roundendDate = (!string.IsNullOrEmpty(enddate)) ? Convert.ToDateTime(enddate) : default(DateTime);
            var reportfor = (roundendDate - roundDate).TotalDays + 1;
            string locationGroupId = "";
            if (!string.IsNullOrEmpty(lstBuildingDetails))
            {
                locationGroupId = lstBuildingDetails;
            }
            List<RoundGroup> roundGroups = new List<RoundGroup>();

            var roundGroup = _roundInspectionService.GetRoundByDateApp(roundDate, 0, UserSession.CurrentUser.UserId, true, Convert.ToInt32(reportfor), roundendDate).ToList();
            if (!string.IsNullOrEmpty(locationGroupId) && roundGroup != null && roundGroup.Count > 0 && (locationGroupId != "0" && !locationGroupId.Contains("-1")))
            {
                List<RoundGroup> lstPersons = new List<RoundGroup>();
                foreach (string building in locationGroupId.Split(','))
                {
                    if (!string.IsNullOrEmpty(building))
                    {
                        List<RoundGroup> lstround = new List<RoundGroup>();
                        lstround = roundGroup.FindAll(x => x.Locations.Any(y => y.BuildingId == Convert.ToInt32(building))).ToList();

                        foreach (var objround in lstround)
                        {
                            if (!roundGroups.Any(x => x.RoundGroupId == objround.RoundGroupId && x.RoundDate == objround.RoundDate && x.Locations.Any(y => y.BuildingId == Convert.ToInt32(building))))
                                roundGroups.Add(objround);
                        }
                    }
                }
            }
            else
                roundGroups = roundGroup;

            if (roundType != "0" && roundGroups != null)
            {
                List<RoundGroup> copyround = new List<RoundGroup>();
                copyround = roundGroups;
                roundGroups = new List<RoundGroup>();
                foreach (string type in roundType.Split(','))
                {
                    if (!string.IsNullOrEmpty(type) && type != "-1")
                    {
                        List<RoundGroup> lstround = new List<RoundGroup>();
                        lstround = copyround.FindAll(x => x.RoundCategory.Any(y => y.RoundCatId == Convert.ToInt32(type))).ToList();

                        foreach (var objround in lstround)
                        {
                            if (!roundGroups.Any(x => x.RoundGroupId == objround.RoundGroupId && x.RoundDate.Date == objround.RoundDate.Date && x.RoundCategory.Any(y => y.RoundCatId == Convert.ToInt32(type))))
                                roundGroups.Add(objround);
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(pageMode))
                pageMode = "activity";
            ViewBag.pageMode = pageMode;
            
            //var rounds  = roundGroups.SelectMany(x=>x.RoundSchedules).SelectMany(x=>x.RoundScheduleDates).Where(x=>x.TRoundId.HasValue && x.Tround.IsClosed);

           

            return PartialView("_roundActivityReportBinder", roundGroups);
        }


        public ActionResult LoadFloorUserRoundCat(int floorId, int troundId, int userId)
        {
            return ViewComponent("RoundCategoriesAddRemove", new { floorId, troundId, userId });
        }

        public ActionResult RoundGroupUserDropDownList(int roundGroupId)
        {
            var users = new List<UserProfile>();
            if (roundGroupId > 0)
                users = _roundInspectionService.GetRoundUserGroups(roundGroupId).Where(x => x.IsActive).Select(x => x.userProfile).ToList();
            else
            {
                users = _userService.GetUsers().Where(x => x.IsActive && x.IsUserRole(UserRole.RoundVolunteer)).ToList();

            }
            var model = new ViewModels.Common.CustomSelectPicker("ddlusers", "All Volunteers", "0", Convert.ToString(UserSession.CurrentUser.UserId))
            {
                Items = users.Select(x =>
                                        new SelectListItem()
                                        {
                                            Text = x.FullName,
                                            Value = Convert.ToString(x.UserId)
                                        }).ToList()
            };
            return PartialView("RoundGroupUserDropDownList", model);
        }

        #endregion
    }
}
//2197