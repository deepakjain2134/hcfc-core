using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Utility.AppConfig;
using HCF.Utility.Extensions;
using HCF.Web.Base;
using HCF.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class WorkOrderController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWorkOrderService _workOrderService;
        private readonly IInsActivityService _inspectionActivityService;
        private readonly ITransactionService _transactionService;
        private readonly IStatusService _statusService;
        private readonly IIlsmService _ilsmService;
        private readonly ICommonService _commonService;
        private readonly IOrganizationService _orgService;
        private readonly IDeviceTestingService _deviceTestingService;
        private readonly IEpService _epService;
        private readonly IBuildingsService _buildingsService;
        private readonly IHCFSession _session;
        private readonly IHttpPostFactory _httpService;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IAppSetting _appSettings;
        private readonly IFloorService _floorService;

        public WorkOrderController(
            IFloorService floorService,
            IEpService epService,
            IBuildingsService buildingsService,
            ICommonService commonService,
            IInsActivityService inspectionActivityService,
            IUserService userService,
            ICommonModelFactory commonModelFactory,
            ITransactionService transactionService,
            IIlsmService ilsmService,
            IWorkOrderService workOrderService,
            IStatusService statusService,
            IOrganizationService orgService,
            IHttpPostFactory httpService,
            IAppSetting appSettings,
            IDeviceTestingService deviceTestingService,
            IHCFSession session)
        {
            _floorService = floorService;
            _appSettings = appSettings;
            _commonModelFactory = commonModelFactory;
            _deviceTestingService = deviceTestingService;
            _buildingsService = buildingsService;
            _epService = epService;
            _commonService = commonService;
            _ilsmService = ilsmService;
            _transactionService = transactionService;
            _inspectionActivityService = inspectionActivityService;
            _userService = userService;
            _workOrderService = workOrderService;
            _statusService = statusService;
            _orgService = orgService;
            _deviceTestingService = deviceTestingService;
            _session = session;
            _httpService = httpService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="floorAssetId"></param>
        /// <param name="ePdetailId"></param>
        /// <param name="sortOrder"></param>
        /// <param name="orderbySort"></param>
        /// <param name="searchparam"></param>
        /// <returns></returns>
        //[CrxAuthorize(Roles = "workorder_index")]
        public ActionResult Index(int? page, int? floorAssetId, int? ePdetailId, string sortOrder = "", string orderbySort = "", string searchparam = "", string activityid = "")
        {
            ViewBag.FlootAssetId = floorAssetId ?? 0;
            ViewBag.EPdetailId = ePdetailId ?? 0;
            ViewBag.Activityid = activityid ?? "";
            UISession.AddCurrentPage("WorkOrder_Index", 0, "Work Orders", true);
            return View();
        }

        [ActionValidate(isRequired: false)]
        public ActionResult SyncWorkOrders(string searchParam)
        {
            ViewBag.SyncMode = "1";
            var objRequest = new Request();
            var list = new List<WorkOrder>();
            objRequest.SearchcBy = string.IsNullOrEmpty(searchParam) ? null : searchParam;
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var postData = _commonModelFactory.JsonSerialize<Request>(objRequest);
            var uri = APIUrls.WorkOrder_SyncWorkOrders;
            var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                if (httpResponse !=null && httpResponse.Result !=null && httpResponse.Result.WorkOrders != null)
                    list = httpResponse.Result.WorkOrders;

                ViewBag.NewWorkOrders = list.Count;
                TempData.Put("PageIndex", TempData.Get<string>("PageIndex"));
                TempData.Put("lastIndex", TempData.Get<string>("lastIndex"));


            }
            return PartialView("_workOrders", list);
        }


        /// <summary>
        /// /
        /// </summary>
        /// <param name="page"></param>
        /// <param name="floorAssetId"></param>
        /// <param name="sortOrder"></param>
        /// <param name="OrderbySort"></param>
        /// <param name="searchparam"></param>
        /// <returns></returns>

        [ActionValidate(isRequired: false)]
        public ActionResult WorkOrders(int? page, int? floorAssetId, int? EPdetailId, string sortOrder = "", string OrderbySort = "", string searchparam = "", bool isScroll = false, bool isPopUp = false, bool json = false, string activityid = "", string fltrstatuscode = "")
        {
            ViewBag.WoFloorAssets = floorAssetId;
            var userId = UserSession.CurrentUser.UserId;
            var objRequest = new Request();
            if (UserSession.CurrentUser.IsVendorUser)
            {
                objRequest.UserId = userId;
            }
            var list = new List<WorkOrder>();
            var fixedPageSize = Convert.ToInt32(_appSettings.PageSize);
            var fixedpageIndex = 1;
            var pageIndex = 1;
            var pageSize = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : pageIndex;
            var pageCount = 0;
            if (isScroll)
            {
                fixedpageIndex = (pageIndex - 1) * fixedPageSize;
                pageSize = fixedPageSize;
            }
            else
            {
                pageSize = pageIndex * fixedPageSize;
                fixedpageIndex = 1;
            }
            sortOrder = string.IsNullOrEmpty(sortOrder) ? "DateCreated" : sortOrder;
            OrderbySort = string.IsNullOrEmpty(OrderbySort) ? "DESC" : OrderbySort;

            objRequest.PageIndex = fixedpageIndex;
            objRequest.PageSize = pageSize;
            objRequest.SortOrder = sortOrder;
            objRequest.OrderbySort = OrderbySort;
            objRequest.WhereCondition = fltrstatuscode;
            if (floorAssetId.HasValue)
                objRequest.FloorAssetId = floorAssetId.Value;
            if (EPdetailId.HasValue)
                objRequest.EpDetailId = EPdetailId.Value;
            if (activityid != null)
                objRequest.ActivityId = activityid;

            objRequest.SearchcBy = string.IsNullOrEmpty(searchparam) ? null : searchparam;
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);

            var postData = _commonModelFactory.JsonSerialize<Request>(objRequest);

            var uri = APIUrls.WorkOrder_WorkOrders;
            var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                //  var wo = new List<WorkOrder>();
                if (httpResponse.Result.WorkOrders != null)
                {

                    list = httpResponse.Result.WorkOrders;
                    pageCount = httpResponse.PageCount;
                    var results = pageCount % pageSize;
                    var index = results > 0 ? (pageCount / pageSize) + 1 : pageCount / pageSize;
                    var lastIndex = index;
                    TempData.Put("PageIndex", pageIndex.ToString());
                    TempData.Put("lastIndex", lastIndex.ToString());

                    //TempData.Keep();
                    //ViewBag.PageIndex = pageIndex;
                    //ViewBag.lastIndex = lastIndex;


                }
            }
            if (json)
                return Json(list);

            //if (floorAssetId.HasValue && floorAssetId.Value > 0)
            //    list = list.Where(y => y.WorkOrderFloorAssets.Any(x => x.FloorAssetId == floorAssetId.Value)).ToList();

            return PartialView(isPopUp ? "_workOrdersPopUp" : "_workOrders", list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public ActionResult IssueHistory(int issueId)
        {
            var list = _workOrderService.GetWorkOrders().SingleOrDefault(x => x.IssueId == issueId);
            return View(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="workOrderId"></param>
        /// <param name="commnet"></param>
        /// <param name="issueId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveStatus(IFormFile file, string workOrderId, string commnet, int issueId)
        {
            var newIssue = new WorkOrder
            {
                IssueId = issueId,
                WorkOrderId = Convert.ToInt32(workOrderId),
                CompletionComments = commnet,

                CreatedDate = DateTime.UtcNow,

            };
            var postData = _commonModelFactory.JsonSerialize<WorkOrder>(newIssue);
            var baseUri = _appSettings.ApiPath;
            var uri = baseUri + APIUrls.WorkOrder_SaveStatus;
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            _httpService.CallPostMethod(postData, uri, ref statusCode);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="tilsmId"></param>
        /// <param name="activityType"></param>
        /// <returns></returns>
        public ActionResult WorkOrderFailSteps(string activityId, string tilsmId, string activityType)
        {
            UISession.AddCurrentPage("workorder_CreateWorkOrder", 0, "Work Order Fail Steps");
            if (!string.IsNullOrEmpty(activityType))
                TempData.Put("activityType", activityType);
            //ViewBag.activityType = activityType;



            var inspections = new List<Inspection>();
            inspections = _transactionService.GetInspectionsforWorkOrder(activityId);

            if (activityType == "1" && inspections.Any())
            {

                TempData.Put("epdetailId", inspections.FirstOrDefault().EPDetailId.ToString());
                TempData.Put("activityId", activityId);
                //TempData["epdetailId"] = inspections.FirstOrDefault().EPDetailId;
                //["activityId"] = activityId;
                //ViewBag.epdetailId = inspections.FirstOrDefault().EPDetailId;
                //ViewBag.activityId = activityId;
            }

            if (inspections.Any(x => x.TInspectionActivity.Any(y => y.TInspectionDetail.Any(z => z.InspectionSteps.Any(t => t.IssueId == null && t.Status == 0)))))
            {
                if (!string.IsNullOrEmpty(tilsmId))
                {
                    TempData.Put("TilsmId", tilsmId);
                    //ViewBag.TilsmId = tilsmId;
                }
                return View(inspections);
            }
            else
            {
                if (TempData.Get<string>("TilsmId") != null)
                {
                    if (Convert.ToInt32(TempData.Get<string>("TilsmId")) > 0)
                    {
                        return RedirectToAction("GetIlsmInspection", "goal", new { tilsmId = Convert.ToInt32(TempData.Get<string>("TilsmId")) });
                    }
                }
                var url = UISession.GetBackPageInspection("assets_GetTfloorAssets");
                if (url == "/dashboard")
                {
                    return RedirectToRoute("workorder");
                }
                return Redirect(url);
            }
        }


        public ActionResult CloseTaggedDeficiency(string activityId, bool isClosed)
        {
            WorkOrder wo = new WorkOrder();
            if (isClosed == true)
            {
                var item = _inspectionActivityService.GetDeficiencyAssets(4, activityId, null);
                wo = item.FirstOrDefault().TInspectionDetail.FirstOrDefault().InspectionSteps.FirstOrDefault().WorkOrders.FirstOrDefault();
            }

            ViewBag.ActivityId = activityId;
            //var inspections = new List<Inspection>();
            //if (!string.IsNullOrEmpty(activityId))
            //{
            //    foreach (var id in activityId.Split(','))
            //    {
            //        var data = BAL.Transaction.GetDeficiencies(id);
            //        inspections.AddRange(data);
            //    }
            //}
            return PartialView("_CloseTaggedDeficiency", wo);
        }

        public ActionResult DeficiencyCloseWO(string activityId)
        {
            ViewBag.ActivityId = activityId;
            var inspections = new List<Inspection>();
            if (!string.IsNullOrEmpty(activityId))
            {
                foreach (var id in activityId.Split(','))
                {
                    var data = _transactionService.GetDeficiencies(id);
                    inspections.AddRange(data);
                }
            }
            return View(inspections);
        }

        public ActionResult DeficiencySubmit(WorkOrder workOrder)
        {
            LoadCreateData();
            //int? floorAssetId = null;
            var inspections = new List<Inspection>();
            foreach (var id in workOrder.ActivitiesIds.Split(','))
            {
                var data = _transactionService.GetInspectionsforWorkOrder(id);
                var inspectionActivity = _inspectionActivityService.GetDeficiencyAssets(4, id, null).FirstOrDefault();
                var groupedWOList = inspectionActivity.TInspectionDetail.SelectMany(foo => foo.InspectionSteps.Where(x => x.Status == 0 && x.IssueId.HasValue))
                          .GroupBy(u => u.IssueId)
                          .Select(grp => grp.ToList())
                          .ToList();
                inspections.AddRange(data);
            }
            var user = UserSession.CurrentUser;
            workOrder.RequesterEmail = user.Email;
            workOrder.RequesterName = user.FullName;
            workOrder.RequesterPhone = user.PhoneNumber;
            if (inspections != null && inspections.Count > 0)
            {
                var comment = string.Empty;
                var requesterComment = string.Empty;
                var tfloorAssets = new List<TFloorAssets>();
                var woFloorAssets = new List<WorkOrderFloorAssets>();
                var inspectionSteps = new List<InspectionSteps>();
                workOrder.DeficiencyCode = BDO.Enums.WODeficiencyCode.ACD.ToString();
                workOrder.IsDeleted = true;
                foreach (var item in inspections)
                {
                    var epdtails = _epService.GetEpDescription(item.EPDetailId);
                    TempData.Put("epdetails", epdtails);
                    //ViewBag.epdetails = epdtails;
                    item.TInspectionActivity = item.TInspectionActivity.GroupBy(test => test.ActivityId).Select(grp => grp.First()).ToList();
                    foreach (var activity in item.TInspectionActivity)
                    {
                        var obj = new WorkOrderFloorAssets
                        {
                            ActivityId = activity.ActivityId
                        };
                        if (activity.TFloorAssets != null)
                            tfloorAssets.Add(activity.TFloorAssets);

                        foreach (var details in activity.TInspectionDetail)
                        {
                            foreach (var inspstep in details.InspectionSteps)
                            {
                                comment = comment + inspstep.Steps.Step + ", ";
                                if (!string.IsNullOrEmpty(inspstep.Comments))
                                    requesterComment = requesterComment + inspstep.Comments + ", ";
                                inspectionSteps.Add(inspstep);
                            }
                        }
                        obj.TinspectionActivity = activity;
                        woFloorAssets.Add(obj);
                    }
                }
                var buildIngFloor = string.Empty;
                foreach (var item in tfloorAssets)
                    buildIngFloor = buildIngFloor + $"{item.Floor.Building.BuildingCode}-{item.Floor.FloorCode}-{item.AssetNo} " + ", ";

                workOrder.Description = buildIngFloor + " " + comment;
                if (tfloorAssets.Count > 0)
                {
                    //ViewBag.TfloorAssets = tfloorAssets;
                    TempData.Put("TfloorAssets", tfloorAssets);
                    workOrder.BuildingCode = tfloorAssets.FirstOrDefault().Floor.Building.BuildingCode;
                    workOrder.SiteCode = tfloorAssets.FirstOrDefault().Floor.Building.SiteCode;
                }

                if (woFloorAssets.Count > 0)
                    TempData.Put("WorkOrderFloorAssets", woFloorAssets);
                //ViewBag.WorkOrderFloorAssets = woFloorAssets;

                workOrder.StatusCode = "CMPLT";
                workOrder.SubStatusCode = "CHANG";
                //if (inspectionSteps.Any(x => x.Status == 0 && x.DeficiencyCode == "RA" && x.IsRA))
                //{
                //    workOrder.TypeCode = "LS";
                //    workOrder.PriorityCode = "1";
                //}
                if (inspectionSteps.Any(x => x.Status == 0))
                {
                    workOrder.TypeCode = "LS";
                    workOrder.PriorityCode = "1";
                }
                else
                {
                    workOrder.TypeCode = "RQ";
                    workOrder.PriorityCode = "1";
                }
                workOrder.InspectionSteps = inspectionSteps;
                TempData.Put("WOInspectionSteps", inspectionSteps);
                //TempData.Keep();
                //ViewBag.WOInspectionSteps = inspectionSteps;

            }

            workOrder.CreatedBy = UserSession.CurrentUser.UserId;
            var postData = _commonModelFactory.JsonSerialize<WorkOrder>(workOrder);
            var uri = $"{Utility.APIUrls.WorkOrder_Create}/{null}";
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var resp = _httpService.CallPostMethod(postData, uri, ref statusCode);
            dynamic response = Newtonsoft.Json.Linq.JObject.Parse(resp);


            var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(resp);
            int issueid = httpResponse.Result.WorkOrder.IssueId;
            List<Int32> tfileIds = new List<int>();
            if (!string.IsNullOrEmpty(workOrder.AttachFiles))
            {
                foreach (var item in workOrder.AttachFiles.Split(','))
                {
                    int fileId = Convert.ToInt32(item.Split('_')[0]);
                    tfileIds.Add(fileId);
                }
                foreach (var ids in tfileIds)
                {
                    WorkOrderfiles workOrderfiles = new WorkOrderfiles();
                    var tfile = _commonService.GetFile(Convert.ToInt32(ids), "");
                    workOrderfiles.IssueId = issueid;
                    workOrderfiles.FileName = tfile.FileName;
                    workOrderfiles.FilePath = tfile.FilePath;
                    workOrderfiles.WorkOrderFileId = _workOrderService.SaveWorkOrderFiles(workOrderfiles.IssueId, workOrderfiles);
                }

            }

            bool status = response.Success;
            var orgId = _session.Get<string>(SessionKey.OrganizationId);
            var taggedid = _session.Get<string>(SessionKey.TaggedId);
            if (!string.IsNullOrEmpty(orgId) && !string.IsNullOrEmpty(taggedid))
            {
                return RedirectToAction("Deficiencies", "Home", new
                {
                    taggedId = taggedid,
                    orgId
                });
            }
            return RedirectToAction("Deficiencies", "Home");
        }


        public ActionResult WoStatusMultiSelect()
        {
            List<HCF.BDO.WoStatus> woStatuses = new List<HCF.BDO.WoStatus>();
            var orgData = _orgService.GetWOMastersData();
            woStatuses = orgData.Result.WoStatus;
            return PartialView("~/Views/Shared/_woStatusMultiselect.cshtml", woStatuses);
        }

        #region create WorkOrder               

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tilsmId"></param>
        /// <returns></returns>
        public ActionResult ILSMWorkOrder(int tilsmId)
        {
            UISession.AddCurrentPage("workorder_CreateWorkOrder", 0, "Create new Work Order");
            LoadCreateData();
            var wo = new WorkOrder();
            var user = UserSession.CurrentUser;
            wo.RequesterEmail = user.Email;
            wo.RequesterName = user.FullName;
            wo.RequesterPhone = user.PhoneNumber;
            wo.DeficiencyCode = BDO.Enums.WODeficiencyCode.OT.ToString();
            wo.Description = _ilsmService.GetIlsmwoDescription(tilsmId);
            wo.TilsmId = tilsmId;
            return View("Create", wo);
        }


        [HttpPost]
        public ActionResult SaveTRoundWorkOrder([FromBody] List<TRoundWorkOrders> TRoundWorkOrders)
        {
            if (TRoundWorkOrders != null && TRoundWorkOrders.Count > 0)
            {
                var objWo = new WorkOrder();
                var floor = new Floor();

                var floorId = TRoundWorkOrders.Where(x => x.FloorId > 0).FirstOrDefault()?.FloorId;
                if (floorId != null && floorId > 0)
                {
                    floor = _floorService.GetFloor(floorId.Value);
                    if (floor != null && floor.FloorId > 0)
                    {
                        objWo.FloorId = floor.FloorId;
                        objWo.SiteCode = floor.Building?.SiteCode;
                        objWo.BuildingCode = floor.Building?.BuildingCode;
                    }
                }

                var troundDescriptions = string.Empty;
                troundDescriptions =  string.Join(",", TRoundWorkOrders.Select(x => x.questionIdText).Distinct());
                var tRequesterComments = string.Empty;
                tRequesterComments = string.Join(",", TRoundWorkOrders.Select(x => x.questionResponseComment).Distinct());
                objWo.TRoundWorkOrders = TRoundWorkOrders;
                objWo.Description = !string.IsNullOrEmpty(troundDescriptions) ? TRoundWorkOrders.FirstOrDefault().RoundName + " : " + troundDescriptions : TRoundWorkOrders.FirstOrDefault().RoundName;
                objWo.RequesterComments= !string.IsNullOrEmpty(tRequesterComments) ? tRequesterComments : string.Empty;
                TempData.Put("TRoundWorkOrders", TRoundWorkOrders);
                TempData.Put("RoundWO", objWo);
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }


        public ActionResult CreateTRoundWorkOrder()
        {

            UISession.AddCurrentPage("workorder_CreateWorkOrder", 0, "Create Round WorkOrder");
            var wo = new WorkOrder();
            LoadCreateData();
            var user = UserSession.CurrentUser;
            if (TempData["RoundWO"] != null)
            {
                wo = TempData.Get<WorkOrder>("RoundWO");
                wo.RequesterEmail = user.Email;
                wo.RequesterName = user.FullName;
                if (!string.IsNullOrEmpty(user.PhoneNumber))
                    wo.RequesterPhone = user.PhoneNumber;
                else if (!string.IsNullOrEmpty(user.CellNo))
                    wo.RequesterPhone = user.CellNo;

                if (wo.TRoundWorkOrders != null && wo.TRoundWorkOrders.Count > 0)
                {
                    TempData.Put("TRoundWorkOrders", wo.TRoundWorkOrders);
                    wo.FloorId = wo.TRoundWorkOrders.FirstOrDefault().FloorId;
                    wo.DeficiencyCode = "RD";
                }
            }
            return View("Create", wo);
        }

        public ActionResult Create(List<Inspection> inspection, string Tinsids, string TilsmId, string activityType, string epdetailId, string activityId, string mode = "")
        {
            UISession.AddCurrentPage("workorder_CreateWorkOrder", 0, "Create new Work Order");
            LoadCreateData();

            if (mode != null && mode == "WOFailStep")
                inspection = TempData.Get<List<Inspection>>("WOFailStepInspection");

            //ViewBag.TilsmId = TilsmId;
            TempData.Put("TilsmId", TilsmId);
            var WO = new WorkOrder();
            var user = UserSession.CurrentUser;
            WO.RequesterEmail = user.Email;
            WO.RequesterName = user.FullName;
            if (!string.IsNullOrEmpty(user.PhoneNumber))
                WO.RequesterPhone = user.PhoneNumber;
            else if (!string.IsNullOrEmpty(user.CellNo))
                WO.RequesterPhone = user.CellNo;

            if (!string.IsNullOrEmpty(activityType))
                WO.ActivityType = Convert.ToInt32(activityType);

            if (WO.ActivityType == 1)
            {
                WO.EpDetailId = Convert.ToInt32(epdetailId);
                WO.ActivityId = Guid.Parse(activityId);
            }

            WO.DeficiencyCode = BDO.Enums.WODeficiencyCode.OT.ToString();
            if (inspection != null)
            {
                if (inspection.Count > 0)
                {
                    var Ids = Tinsids.Split(',');
                    var tinsId = new List<int>();
                    for (var i = 0; i < Ids.Count() - 1; i++)
                    {
                        tinsId.Add(Convert.ToInt32(Ids[i]));
                    }
                    var comment = string.Empty;
                    var requesterComment = string.Empty;
                    var tfloorAssets = new List<TFloorAssets>();
                    var woFloorAssets = new List<WorkOrderFloorAssets>();
                    var inspectionSteps = new List<InspectionSteps>();
                    //var epdetails = new EPDetails();
                    WO.DeficiencyCode = BDO.Enums.WODeficiencyCode.AD.ToString();
                    foreach (var item in inspection)
                    {
                        var epdtails = _epService.GetEpDescription(item.EPDetailId);
                        TempData.Put("epdetails", epdtails);
                        ViewBag.epdetails = epdtails;

                        item.TInspectionActivity = item.TInspectionActivity.GroupBy(test => test.ActivityId).Select(grp => grp.First()).ToList();
                        foreach (var activity in item.TInspectionActivity)
                        {
                            var obj = new WorkOrderFloorAssets
                            {
                                ActivityId = activity.ActivityId
                            };
                            if (activity.TFloorAssets != null)
                                tfloorAssets.Add(activity.TFloorAssets);

                            foreach (var details in activity.TInspectionDetail)
                            {
                                foreach (var inspstep in details.InspectionSteps.Where(m => tinsId.Contains(m.TInsStepsId)))
                                {
                                    comment = comment + inspstep.Steps.Step + ", ";
                                    if (!string.IsNullOrEmpty(inspstep.Comments))
                                        requesterComment = requesterComment + inspstep.Comments + ", ";
                                    inspectionSteps.Add(inspstep);
                                }
                            }
                            obj.TinspectionActivity = activity;
                            woFloorAssets.Add(obj);
                        }
                    }
                    var buildIngFloor = string.Empty;
                    foreach (var item in tfloorAssets)
                        buildIngFloor = buildIngFloor + $"{item.Floor.Building.BuildingCode}-{item.Floor.FloorCode}-{item.AssetNo} " + ", ";

                    WO.Description = buildIngFloor + " " + comment;
                    WO.RequesterComments = requesterComment;

                    if (tfloorAssets.Count > 0)
                    {
                        //ViewBag.TfloorAssets = tfloorAssets;
                        TempData.Put("TfloorAssets", tfloorAssets);
                        WO.BuildingCode = tfloorAssets.FirstOrDefault().Floor.Building.BuildingCode;
                        WO.SiteCode = tfloorAssets.FirstOrDefault().Floor.Building.SiteCode;
                    }

                    if (woFloorAssets.Count > 0)
                        TempData.Put("WorkOrderFloorAssets", woFloorAssets);
                    // ViewBag.WorkOrderFloorAssets = woFloorAssets;


                    WO.WorkOrderFloorAssets = woFloorAssets;
                    if (inspectionSteps.Any(x => x.Status == 0 && x.DeficiencyCode == "RA" && x.IsRA))
                    {
                        WO.TypeCode = "LS";
                        WO.PriorityCode = "1";
                    }
                    WO.InspectionSteps = inspectionSteps;
                    TempData.Put("WOInspectionSteps", inspectionSteps);
                    //TempData.Keep();
                    //ViewBag.WOInspectionSteps = inspectionSteps;

                }
            }



            var tIlsmId = string.IsNullOrEmpty(TilsmId) ? 0 : Convert.ToInt32(TilsmId);
            if (tIlsmId > 0)
            {
                WO.IsIlsm = true;
                ViewBag.RedirectToPage = "ilsm";
            }
            return View("Create", WO);
        }

        public ActionResult CreateWOFailSteps(List<Inspection> inspection, string Tinsids, string TilsmId, string activityType, string epdetailId, string activityId)
        {
            TempData.Put("WOFailStepInspection", inspection);
            return RedirectToAction("Create", new { Tinsids, TilsmId, activityType, epdetailId, activityId, mode = "WOFailStep" });
        }

        public ActionResult CreateDeviceWorkOrder(int id, string description, string comment)
        {
            UISession.AddCurrentPage("workorder_CreateWorkOrder", 0, "Create new Work Order");
            LoadCreateData();

            var WO = new WorkOrder();
            var user = UserSession.CurrentUser;
            WO.RequesterEmail = user.Email;
            WO.RequesterName = user.FullName;
            if (!string.IsNullOrEmpty(user.PhoneNumber))
                WO.RequesterPhone = user.PhoneNumber;
            else if (!string.IsNullOrEmpty(user.CellNo))
                WO.RequesterPhone = user.CellNo;

            WO.DeficiencyCode = BDO.Enums.WODeficiencyCode.OT.ToString();

            WO.Description = description;
            WO.RequesterComments = comment;

            return View("Create", WO);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateWO(int? issueId)
        {
            UISession.AddCurrentPage("workorder_CreateWorkOrder", 0, "Create WorkOrder");
            LoadCreateData();
            var user = UserSession.CurrentUser;
            var wo = new WorkOrder();
            if (TempData.Get<WorkOrder>("WO") != null)
            {
                wo = TempData.Get<WorkOrder>("WO");//(WorkOrder)ViewBag.WO;
                wo.RequesterEmail = user.Email;
                wo.RequesterName = user.FullName;
                if (!string.IsNullOrEmpty(user.PhoneNumber))
                    wo.RequesterPhone = user.PhoneNumber;
                else if (!string.IsNullOrEmpty(user.CellNo))
                    wo.RequesterPhone = user.CellNo;

                if (wo.IsIlsm)
                    ViewBag.RedirectToPage = "ilsm";
            }
            if (issueId.HasValue)
            {
                var newWo = _workOrderService.GetWorkOrder(issueId.Value);
                wo.Description = newWo.Description;
                wo.ParentIssueId = issueId.Value;
                wo.FloorId = newWo.FloorId;
                wo.Xcoordinate = newWo.Xcoordinate;
                wo.Ycoordinate = newWo.Ycoordinate;
                wo.ZoomLevel = newWo.ZoomLevel;
                wo.RequesterEmail = (!string.IsNullOrEmpty(newWo.RequesterEmail) ? newWo.RequesterEmail : user.Email);
                wo.RequesterName = (!string.IsNullOrEmpty(newWo.RequesterName) ? newWo.RequesterName : user.FullName);
                wo.RequesterPhone = (!string.IsNullOrEmpty(newWo.RequesterPhone) ? newWo.RequesterPhone : user.PhoneNumber);

               
            }
            return View("Create", wo);
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadCreateData(WorkOrder wo = null)     
        {

            var orgData = _orgService.GetWOMastersData();
            ViewBag.IsWOSubStatusEnabled = orgData.Result.SubStatus.Any();
            if (wo != null) { ViewBag.Status = new SelectList(orgData.Result.WoStatus, "Code", "Name", wo.StatusCode); }
            else { ViewBag.Status = new SelectList(orgData.Result.WoStatus, "Code", "Name", orgData.Result.WoStatus.FirstOrDefault(x => x.IsDefaultSelected)?.Code); }
            ViewBag.Priority = new SelectList(orgData.Result.Priorities, "Code", "Name", wo != null ? wo.PriorityCode : string.Empty);
            ViewBag.SubStatusCode = new SelectList(orgData.Result.SubStatus, "Code", "Name", wo != null ? wo.SubStatusCode : string.Empty);
            ViewBag.Type = new SelectList(orgData.Result.Types, "Code", "Name", wo != null ? wo.TypeCode : string.Empty);
            ViewBag.AccountCode = new SelectList(orgData.Result.Departments, "Code", "Name", wo != null ? wo.AccountCode : string.Empty);
            ViewBag.SkillCode = new SelectList(orgData.Result.Skills, "Code", "Name", wo != null ? wo.SkillCode : string.Empty);
            ViewBag.Problem = new SelectList(orgData.Result.Problem, "Code", "Name", wo != null ? wo.ProblemCode : string.Empty);
            ViewBag.Item = new SelectList(orgData.Result.Item, "Code", "Name", wo != null ? wo.ItemCode : string.Empty);
            ViewBag.Cause = new SelectList(orgData.Result.Cause, "Code", "Name", wo != null ? wo.CauseCode : string.Empty);
            ViewBag.Action = new SelectList(orgData.Result.Actions, "Code", "Name", wo != null ? wo.ActionCode : string.Empty);
            ViewBag.SiteViewBag = new SelectList(orgData.Result.Sites, "Code", "Name", wo != null ? wo.SiteCode : string.Empty);
            var buildings = _buildingsService.GetBuildings();
            ViewBag.Buildings = new SelectList(buildings, "BuildingCode", "BuildingName", wo != null ? wo.BuildingCode : string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="floorAssetId"></param>
        /// <param name="lstResourcesDetails"></param>
        /// <param name="redirectToPage"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(WorkOrder workOrder, int? floorAssetId, string lstResourcesDetails, string redirectToPage)
        {
            string tilsmId = "0";
            List<UserProfile> allUsers = _userService.GetUserList().ToList();
            if (TempData.Get<string>("TilsmId") != null)
            {
                tilsmId = TempData.Get<string>("TilsmId");
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (!string.IsNullOrEmpty(workOrder.RequesterPhone))
                workOrder.RequesterPhone = new string(workOrder.RequesterPhone.Where(char.IsDigit).ToArray());

            if (!string.IsNullOrEmpty(lstResourcesDetails))
            {
                var lstresources = JsonConvert.DeserializeObject<List<UserProfile>>(lstResourcesDetails);
                if (lstresources.Count > 0)
                {
                    workOrder.Resources = new List<UserProfile>();
                    foreach (var item in lstresources)
                    {
                        var obj = new UserProfile();
                        obj = allUsers.FirstOrDefault(x => x.UserId == item.UserId);
                        workOrder.Resources.Add(obj);
                    }
                }
            }
            //if (workOrder.Resources.Any())
            //{
            //    if (workOrder.Resources[0].UserId == 0)
            //    {
            //        workOrder.Resources[0].UserId = -1;
            //    }
            //}
            if (workOrder.Description.StartsWith("--"))
            {
                workOrder.Description = workOrder.Description.Replace("--", "");
            }

            if (TempData["TRoundWorkOrders"] != null)
            {
                workOrder.TRoundWorkOrders = TempData.Get<List<TRoundWorkOrders>>("TRoundWorkOrders");//(List<TRoundWorkOrders>)TempData["TRoundWorkOrders"];
            }
            if (TempData["WOInspectionSteps"] != null)
            {
                workOrder.InspectionSteps = TempData.Get<List<InspectionSteps>>("WOInspectionSteps");//(List<InspectionSteps>)TempData["WOInspectionSteps"];

            }
            if (TempData["WorkOrderFloorAssets"] != null)
            {
                workOrder.WorkOrderFloorAssets = TempData.Get<List<WorkOrderFloorAssets>>("WorkOrderFloorAssets");//(List<WorkOrderFloorAssets>)TempData["WorkOrderFloorAssets"];
            }
            if (TempData["TfloorAssets"] != null)
            {
                var tfloorAssets = TempData.Get<List<TFloorAssets>>("TfloorAssets");//(List<TFloorAssets>)TempData["TfloorAssets"];
                workOrder.FloorAssets = tfloorAssets;
            }

            //if (workOrder.Resources.Any())
            //{
            //    workOrder.Resources = Users.GetUsers().Where(x => x.UserId == workOrder.Resources.FirstOrDefault().UserId).ToList();
            //}

            workOrder.CreatedBy = UserSession.CurrentUser.UserId;
            var postData = JsonConvert.SerializeObject(workOrder); //_commonModelFactory.JsonSerialize<WorkOrder>(workOrder);
            var uri = $"{APIUrls.WorkOrder_Create}/{floorAssetId}";
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var resp = _httpService.CallPostMethod(postData, uri, ref statusCode);
            dynamic response = Newtonsoft.Json.Linq.JObject.Parse(resp);

            bool status = response.Success;
            if (status)
            {
                var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(resp);
                if (httpResponse.Success)
                {
                    if (httpResponse.Result.WorkOrder.WorkOrderId == -2)
                        SuccessMessage = string.Format(httpResponse.Message, httpResponse.Result.WorkOrder.WorkOrderNumber);
                    else
                    {
                        if (!string.IsNullOrEmpty(httpResponse.Result.WorkOrder.WorkOrderNumber))
                        {
                            SuccessMessage = $"Work Order #{httpResponse.Result.WorkOrder.WorkOrderNumber} has been created successfully.";
                            TempData["TfloorAssets"] = null;
                            TempData["epdetails"] = null;
                            TempData["WorkOrderFloorAssets"] = null;
                            TempData["WOInspectionSteps"] = null;
                            TempData["TRoundWorkOrders"] = null;
                        }
                    }
                }
                if (httpResponse.Result.WorkOrder.IssueId != 0)
                {
                    var result = _deviceTestingService.UpdateIssueID(httpResponse.Result.WorkOrder).ToString();
                }

                var orgId = _session.Get<string>(Utility.SessionKey.OrganizationId);
                var taggedid = _session.Get<string>(Utility.SessionKey.TaggedId);
                if (!string.IsNullOrEmpty(orgId) && !string.IsNullOrEmpty(taggedid))
                {
                    return RedirectToAction("Deficiencies", "Home", new
                    {
                        taggedId = taggedid,
                        orgId
                    });
                }
                var url = UISession.GetBackPage("workorder_CreateWorkOrder");
                if (workOrder.status == "CloseDeficiences")
                {
                    var urls = UISession.GetPreviousPage("home_Deficiencies");
                    return Json(urls);
                }
                TempData.Peek("TilsmId");

                if (!string.IsNullOrEmpty(redirectToPage))
                {
                    if (redirectToPage.ToLower() == "ilsm")
                        return RedirectToAction("AddIlsmShow", "ILSM", new { tilsmId = tilsmId });
                    //return RedirectToAction("AddILSM", "ILSM", new { issueId = httpResponse.Result.WorkOrder.IssueId });
                }

                return Redirect(url);
            }
            return View(workOrder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public ActionResult GetSubStatus(string statusCode)
        {
            var woStatus = _statusService.GetStatus().FirstOrDefault(x => x.Code == statusCode);
            var subStatus = _workOrderService.GetSubStatus().Where(x => woStatus != null && x.WoStatusId == woStatus.WoStatusId).ToList();
            return Json(subStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public ActionResult UpdateWorkOrder(int? issueId)
        {
            if (!issueId.HasValue)
                return RedirectToRoute("workorder");

            UISession.AddCurrentPage("workorder_UpdateWorkOrder", 0, "Update Work Order");
            //LoadCreateData();
            //LoadWorkOrderData();
            var wo = new WorkOrder();
            if (issueId > 0)
            {
                wo = _workOrderService.GetWorkOrder(Convert.ToInt32(issueId));
                var woStatus = _statusService.GetStatus().FirstOrDefault(x => x.Code == wo.StatusCode);
                LoadCreateData(wo);
                if (woStatus != null)
                {
                    ViewBag.SubStatus = _workOrderService.GetSubStatus().Where(x => x.WoStatusId == woStatus.WoStatusId).ToList();
                }
            }
            return View(wo);
        }

        /// <summary>
        /// 
        /// </summary>
        //private void LoadWorkOrderData()
        //{
        //    var orgData = OrganizationRepository.GetWOMastersData();
        //    ViewBag.IsWOSubStatusEnabled = orgData.Result.SubStatus.Any();
        //    ViewBag.Priority = orgData.Result.Priorities;
        //    ViewBag.Status = orgData.Result.WoStatus;
        //    ViewBag.Type = orgData.Result.Types;
        //    ViewBag.Account = orgData.Result.Departments;
        //    ViewBag.Skill = orgData.Result.Skills;
        //    ViewBag.Problem = orgData.Result.Problem;
        //    ViewBag.Item = orgData.Result.Item;
        //    ViewBag.Cause = orgData.Result.Cause;
        //    ViewBag.Action = orgData.Result.Actions;
        //    ViewBag.SiteViewBag = orgData.Result.Sites;
        //    var buildings = BuildingsRepository.GetBuildings();
        //    ViewBag.Buildings = buildings;
        //    var statusCode = Convert.ToInt32(Utility.Enums.HttpReponseStatusCode.Success);
        //    var result = _httpService.CallGetMethod(Utility.APIUrls.WorkOrder_LoadWorkOrderData, ref statusCode);
        //    var users = new List<UserProfile>();
        //    if (statusCode == Convert.ToInt32(Utility.Enums.HttpReponseStatusCode.Success))
        //    {
        //        var httpResponse = Common.JsonDeserialize<HttpResponseMessage>(result);
        //        if (httpResponse.Result != null)
        //            users = httpResponse.Result.Users.Where(x => x.IsActive && x.IsInternalUser).OrderBy(x => x.FullName).ToList();
        //    }
        //    ViewBag.Resources = new SelectList(users, "UserId", "FullName", string.Empty);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateWorkOrder(WorkOrder wo, string lstResourcesDetails)
        {
            List<UserProfile> allUsers = _userService.GetUserList().ToList();
            if (!string.IsNullOrEmpty(wo.RequesterPhone))
                wo.RequesterPhone = new string(wo.RequesterPhone.Where(char.IsDigit).ToArray());
            wo.UpdatedBy = Convert.ToInt32(UserSession.CurrentUser.UserId);
            if (!string.IsNullOrEmpty(lstResourcesDetails))
            {
                var lstresources = JsonConvert.DeserializeObject<List<UserProfile>>(lstResourcesDetails);
                if (lstresources.Count > 0)
                {
                    wo.Resources = new List<UserProfile>();
                    foreach (var item in lstresources)
                    {
                        var obj = new UserProfile();
                        obj = allUsers.FirstOrDefault(x => x.UserId == item.UserId);
                        wo.Resources.Add(obj);
                    }
                }
            }
            //wo.Resources = Users.GetUsers().Where(x => x.UserId == wo.Resources.FirstOrDefault().UserId).ToList();
            wo.CreatedBy = UserSession.CurrentUser.UserId;
            var postData = _commonModelFactory.JsonSerialize<WorkOrder>(wo);
            var uri = Utility.APIUrls.WorkOrder_UpdateWorkOrder; //"tms/WorkOrderSave";
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var resp = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                dynamic response = Newtonsoft.Json.Linq.JObject.Parse(resp);
                bool status = response.Success;
                if (status)
                {
                    var url = UISession.GetBackPage("workorder_UpdateWorkOrder");
                    SuccessMessage = Utility.ConstMessage.Success_Updated;
                    return Redirect(url);
                }
                else
                    return RedirectToAction("UpdateWorkOrder", "WorkOrder", new { issueId = wo.IssueId });
            }
            else
            {
                var url = UISession.GetBackPage("workorder_UpdateWorkOrder");
                return Redirect(url);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IssueId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetWorkOrdertfloorassets(string IssueId)
        {
            var floorAssets = new List<TFloorAssets>();
            if (!string.IsNullOrEmpty(IssueId))
                floorAssets = _workOrderService.GetIssuesFloorAssets(Convert.ToInt32(IssueId));

            return PartialView("_GetWorkOrdertfloorassets", floorAssets);
        }

        #endregion

        #region Search WorkOrder PopUp

        public ActionResult WorkOrderSeachPopUp(string type, int id)
        {
            ViewBag.WOPopUpType = type;
            ViewBag.WOPopUpId = id;

            return PartialView("_WorkOrderSeachPopUp");

        }

        #endregion


    }
}