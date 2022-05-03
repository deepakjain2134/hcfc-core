using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Utility.AppConfig;
using HCF.Web.Base;
using HCF.Web.Filters;
using HCF.Web.ViewModels;
using Microsoft.AspNetCore.Http;  
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;    
using Microsoft.AspNetCore.Mvc.Rendering;      
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HCF.Web.Controllers
{
    public class CommonController : Controller
    {
        private readonly ISiteService _siteService;
        private readonly IManufactureService _manufactureService;
        private readonly IDocumentsService _documentsService;
        private readonly IDocumentTypeService _documentTypeService;
        private readonly ICommonService _commonService;
        private readonly IFloorTypeService _floorTypeService;
        private readonly IEpService _epService;
        private readonly IDigitalSignService _signService;
        private readonly IFloorService _floorService;
        private readonly IWorkOrderService _workOrderService;
        private readonly ILocationService _locationService;
        private readonly IAssetsService _assetService;
        private readonly IStatusService _statusService;
        private readonly IStepsService _stepService;
        private readonly IBuildingsService _buildingsService;
        private readonly IConstructionService _constructionService;
        private readonly ITIcraProjectService _IcraProjectService;
        private readonly IUserService _userService;
        private readonly IInsStepsService _insStepsService;
        private readonly IOrganizationService _organizationService;
        private readonly IFloorAssetService _floorAssetService;
        private readonly ILoggingService _logger;
        private readonly IAppSetting _appSettings;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IFileUpload _fileUpload;
        private readonly UserManager<UserProfile> _userManager;

        public CommonController(ILocationService locationService,
            IFileUpload fileUpload, 
            UserManager<UserProfile> userManager,
            IDocumentTypeService documentTypeService,
            ICommonModelFactory commonModelFactory,
            IAssetsService assetService,
            IStepsService stepService,
            IStatusService statusService,
            IWorkOrderService workOrderService,
            IBuildingsService buildingsService,
            IEpService epService, IDigitalSignService signService, IFloorService floorService,
            IFloorTypeService floorTypeService, ICommonService commonService, ISiteService siteService, IManufactureService manufactureService, IDocumentsService documentsService, IConstructionService constructionService, ITIcraProjectService tIcraProjectService,
            IUserService userService, IInsStepsService insStepsService, IOrganizationService organizationService, IFloorAssetService floorAssetService,
            ILoggingService logger, IAppSetting appSettings, IHttpContextAccessor contextAccessor)
        {
            _commonModelFactory = commonModelFactory;
            _buildingsService = buildingsService;
            _stepService = stepService;
            _statusService = statusService;
            _assetService = assetService;
            _locationService = locationService;
            _documentTypeService = documentTypeService;
            _workOrderService = workOrderService;
            _floorTypeService = floorTypeService;
            _commonService = commonService;
            _siteService = siteService; 
            _manufactureService = manufactureService;
            _documentsService = documentsService;
            _epService = epService;
            _signService = signService; _floorService = floorService;
            _constructionService = constructionService;
            _IcraProjectService = tIcraProjectService;
            _userService = userService;
            _insStepsService = insStepsService;
            _organizationService = organizationService;
            _floorAssetService = floorAssetService;
            _logger = logger;
            _appSettings = appSettings;
            _contextAccessor = contextAccessor;
            _fileUpload = fileUpload;
            _userManager = userManager;
        }

        #region Common Methods 

        [ActionValidate(isRequired: false)]
        public async Task<IActionResult> RedirectRoute(string routeName, string menuName, int? menuId)
        {
            UISession.ClearBreadCrumb();
            return RedirectToRoute(routeName);
        }

        [ActionValidate(isRequired: false)]
        public async Task<IActionResult> RedirectToPage(int pageIndex, string screenName)
        {
            var url = UISession.GetBackBreadCrumb(pageIndex, screenName);
            return RedirecttoLocalUrl(url);
        }

        [ActionValidate(isRequired: false)]
        public async Task<IActionResult> RedirectTobackpage(string pageUrl, string screenName)
        {
            var url = UISession.GetBackPagebyPageUrl(pageUrl, screenName);
            return RedirecttoLocalUrl(url);
        }

        private IActionResult RedirecttoLocalUrl(string url)
        {
            Uri obj = new Uri(url);
            if (Url.IsLocalUrl(obj.AbsolutePath))
            {
                return Redirect(obj.PathAndQuery);
            }
            else
            {
                return RedirectToRoute("logout");
            }
        }

        [ActionValidate(isRequired: false)]
        public IActionResult GetBackPageInspection(string screenName)
        {
            var url = UISession.GetBackPageInspection(screenName);
            if (Url.IsLocalUrl(url))
            {
                return Redirect(url);
            }
            else
            {
                return RedirectToRoute("logout");
            }
        }

        public IActionResult ShowCmsInfo(int epdetailid)
        {
            var epdetail = _epService.GetEpDescription(epdetailid);
            return PartialView("~/Views/Shared/PopUp/_EPDescription.cshtml", epdetail);
        }

        public IActionResult ForwardDocument(int ePdetailId)
        {
            var documents = new Documents();
            TempData["epDetailId"] = ePdetailId;
            IEnumerable<DocumentType> attch = _documentTypeService.GetDocumentTypes(ePdetailId);
            documents.Attachments = new List<FilesRepository>();
            foreach (var item in attch)
            {
                var attachment = new FilesRepository
                {
                    Id = item.DocTypeId,
                    FilePath = item.Path
                };
                documents.Attachments.Add(attachment);
            }
            TempData.Keep();
            return PartialView("~/Views/Shared/PopUp/_ForwardDocument.cshtml", documents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForwardDocument(Documents newDocuments, int ePdetailId)
        {
            var documents = newDocuments;
            var postData = _commonModelFactory.JsonSerialize<Documents>(documents);
            var uri = $"{APIUrls.Common_ForwardDocument}/{ePdetailId}";
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = CallPostMethod(postData, uri, ref statusCode);
            return Json(new { Result = result });
        }

        public IActionResult ReplyDocument(int? documentRepoId)
        {
            var documents = new Documents();
            if (documentRepoId.HasValue)
            {
                documents = _documentsService.GetDocument(documentRepoId.Value);
            }
            TempData.Keep();
            return PartialView("~/Views/Shared/PopUp/_ReplyDocument.cshtml", documents);
        }

        [HttpPost]
        public IActionResult ReplyDocument(Documents documents)
        {
            var postData = _commonModelFactory.JsonSerialize<Documents>(documents);
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = CallPostMethod(postData, APIUrls.Common_ReplyDocument, ref statusCode);
            return Json(result);
        }

        public IActionResult GetDocuments()
        {
            var docs = _documentsService.GetInboxMails(UserSession.CurrentOrg.ClientNo);
            return PartialView("_Inbox", docs);
        }

        [NonAction]
        public string CallPostMethod(string postData, string apiUrl, ref int statusCode, bool isCompleteUrl = false)
        {
            string url;
            var loginToken = UserSession.LogOnToken;
            if (isCompleteUrl)
                url = apiUrl;
            else
            {
                var baseUrl = _appSettings.CommonApiUrl;
                url = baseUrl + apiUrl;
            }

            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.Timeout = Timeout.Infinite;
            webRequest.KeepAlive = true;            
            webRequest.ContentType = "application/json";
            webRequest.Headers.Add(APIkeys.LogOnToken, loginToken);
            webRequest.Headers.Add(APIkeys.DBName, UserSession.CurrentOrg.DbName);
            webRequest.Headers.Add(APIkeys.ClientNo, UserSession.CurrentOrg.ClientNo.ToString());
            try
            {
                using (var requestWriter2 = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter2.Write(postData);
                }
                using (var responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    postData = responseReader.ReadToEnd();
                }
            }
            catch (WebException exe)
            {
                if (exe.Status == WebExceptionStatus.ProtocolError && exe.Response is HttpWebResponse response)
                {
                    statusCode = (int)(response.StatusCode);
                    _logger.Error("Post Data Url " + " " + exe.Message);
                    _logger.Error("Post Data Url " + " " + url);
                    _logger.Error("Login Token " + " " + loginToken);
                    _logger.Error("Post Data" + " " + postData);
                }
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
            return postData;
        }

        [NonAction]
        public string CallGetMethod(string url, ref int statusCode)
        {
            var baseUrl = _appSettings.CommonApiUrl;
            url = baseUrl + HttpUtility.HtmlEncode(url);
            var result = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = Timeout.Infinite;
            request.KeepAlive = true;
            request.Headers.Add(APIkeys.LogOnToken, UserSession.LogOnToken);
            request.Headers.Add(APIkeys.DBName, UserSession.CurrentOrg.DbName);
            request.Headers.Add(APIkeys.ClientNo, UserSession.CurrentOrg.ClientNo.ToString());
            request.AutomaticDecompression = DecompressionMethods.GZip;
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (WebException exe)
            {
                if (exe.Status == WebExceptionStatus.ProtocolError)
                {
                    if (exe.Response is HttpWebResponse response)
                    {
                        statusCode = (int)(response.StatusCode);
                        _logger.Error(exe);
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///  use this method to call normal HttpMethod Outside  of CRx
        /// </summary>
        /// <param name="url"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public string GetHttpWebMethod(string url, ref int statusCode)
        {
            var result = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = Timeout.Infinite;
            request.KeepAlive = true;
            request.AutomaticDecompression = DecompressionMethods.GZip;
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                    if (stream != null)
                        using (var reader = new StreamReader(stream))
                        {
                            result = reader.ReadToEnd();
                        }
            }
            catch (WebException exe)
            {
                if (exe.Status == WebExceptionStatus.ProtocolError)
                {
                    if (exe.Response is HttpWebResponse response)
                    {
                        statusCode = (int)(response.StatusCode);
                        _logger.Error(exe);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public IActionResult CallGetDashboardinfoMethod(string baseUrl, string dbName)
        {
            var url = baseUrl + "goal/getRiskManagementCount/4";
            var result = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = Timeout.Infinite;
            request.KeepAlive = true;
            request.Headers.Add("dbname", dbName);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                    if (stream != null)
                        using (var reader = new StreamReader(stream))
                        {
                            result = reader.ReadToEnd();
                        }
            }
            catch (WebException exe)
            {
                if (exe.Status == WebExceptionStatus.ProtocolError)
                {
                    if (exe.Response is HttpWebResponse)
                    {
                        _logger.Error(exe);
                    }
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public IActionResult CommentPopUp(string title, string description)
        {
            var commnet = new Models.Comment
            {
                Title = title,
                Description = description
            };
            return PartialView("~/Views/Shared/PopUp/_Comment.cshtml", commnet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imgSrc"></param>
        /// <param name="title"></param>
        /// <returns></returns>        
        public IActionResult FilePreview(string imgSrc, string title)
        {
            var imageDetail = new Models.ImageDetail
            {
                Title = title,
                Imgsrc = imgSrc.ToLower()
            };
            return PartialView("~/Views/Shared/FilePreview.cshtml", imageDetail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imgSrc"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public IActionResult ImagePreview(string imgSrc, string title)
        {
            var imageDetail = new Models.ImageDetail
            {
                Title = title,
                Imgsrc = (!string.IsNullOrEmpty(imgSrc))?imgSrc.ToLower(): string.Empty
            };
            return PartialView("~/Views/Shared/PopUp/_ImagePreview.cshtml", imageDetail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="floorPlanId"></param>
        /// <param name="permitId"></param>
        /// <param name="ispopup"></param>
        /// <returns></returns>
        public IActionResult DrawingPreview(string mode, Guid? floorPlanId, string permitId, int? ispopup)
        {
            if (string.IsNullOrEmpty(mode))
                mode = "drawing";
            DrawingViewer viewer = new DrawingViewer
            {
                ViewerMode = !string.IsNullOrEmpty(permitId) && permitId == "0" ? 2 : 3
            };
            if (floorPlanId.HasValue)
                viewer.FloorPlanId = floorPlanId.Value;

            if (ispopup.HasValue)
                ViewBag.ispopup = ispopup.Value;

            if (!string.IsNullOrEmpty(permitId))
                ViewBag.permitId = permitId;
            else
                ViewBag.permitId = string.Empty;
            return PartialView("~/Views/Shared/PopUp/_DrawingPreview.cshtml", viewer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tfloorAssets"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MovetfloorAsset(TFloorAssets tfloorAssets)
        {
            var postData = _commonModelFactory.JsonSerialize<TFloorAssets>(tfloorAssets);
            var uri = $"{APIUrls.Common_MovetfloorAsset}?Mode=Add";
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            CallPostMethod(postData, uri, ref statusCode);
            return RedirectToAction("GetTfloorAssets", "Assets", tfloorAssets.FloorId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tfloorAssetId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult TfloorAssetdelete(int tfloorAssetId)
        {
            var location = new TFloorAssetsLocations
            {
                FloorAssetsId = tfloorAssetId
            };
            bool res = _locationService.UpdateFloorAssetsLocation(location);
            var result = new { Result = res };
            return Json(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult AssetsMultiSelectDropDown()
        {
            var assets = _assetService.GetAssets(UserSession.CurrentUser.UserId);
            return PartialView("_AssetsMultiSelectDropDown", assets);
        }

        #endregion

        #region Binder

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binderId"></param>
        /// <returns></returns>
        public IActionResult AddSubBinder(int? binderId)
        {
            var binder = _documentsService.GetBinder(Convert.ToInt32(binderId));
            var newSubBinder = new Binders();
            if (binderId.HasValue)
                newSubBinder.ParentBinderId = binderId.Value;

            ViewBag.Binder = binder;
            return PartialView("~/Views/Shared/PopUp/_AddSubBinder.cshtml", newSubBinder);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSubBinder(Binders model)
        {
            if (ModelState.IsValid)
            {
                model.BinderId = 0;
                model.CreatedBy = UserSession.CurrentUser.UserId;
                string postData = _commonModelFactory.JsonSerialize<Binders>(model);
                string uri = APIUrls.Common_AddSubBinder;
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                CallPostMethod(postData, uri, ref statusCode);
            }
            return RedirectToAction("Binders", "Repository");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <param name="xcoordinate"></param>
        /// <param name="ycoordinate"></param>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public IActionResult FloorInspection(int floorId, string xcoordinate, string ycoordinate, int? issueId)
        {
            var wo = new WorkOrder();
            ViewBag.WOStatus = _statusService.GetStatus().Where(x => x.IsActive == true);
            if (issueId > 0)
            {
                wo = _workOrderService.GetWorkOrder(Convert.ToInt32(issueId));
                return PartialView("~/Views/Shared/PopUp/_UpdFloorInspection.cshtml", wo);
            }
            else
            {
                wo.FloorId = floorId;
                wo.Floor = _floorService.GetFloor(floorId);
                wo.IssueId = Convert.ToInt32(issueId);
                wo.Xcoordinate = xcoordinate;
                wo.Ycoordinate = ycoordinate;
                if (wo.Floor.Building != null)
                {
                    wo.SiteCode = wo.Floor.Building.SiteCode;
                    wo.BuildingCode = wo.Floor.Building.BuildingCode;
                }
                wo.WorkOrderFiles = new List<WorkOrderfiles>();
                return PartialView("~/Views/Shared/PopUp/_InsFloorInspection.cshtml", wo);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="insStepsId"></param>
        /// <returns></returns>
        public IActionResult UpdateStepDeficeincyStatus(int insStepsId)
        {
            InspectionSteps inspectionSteps = _insStepsService.GetInspectionSteps(insStepsId);
            inspectionSteps.Steps = _stepService.GetStep(inspectionSteps.StepsId);
            ViewBag.WOStatus = _statusService.GetStatus().Where(x => x.IsActive == true);
            return PartialView("~/Views/Shared/PopUp/_UpdateStepDeficiency.cshtml", inspectionSteps);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdateMultipleFloorPlan()
        {
            var buildings = _buildingsService.GetBuildingFloors();
            return PartialView("~/Views/Shared/PopUp/_UpdateMultipleFloorPlan.cshtml", buildings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionSteps"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStepDeficeincyStatus(InspectionSteps inspectionSteps)
        {
            var postData = _commonModelFactory.JsonSerialize<InspectionSteps>(inspectionSteps);
            var uri = $"{APIUrls.Common_UpdateStepDeficeincyStatus}/{UserSession.CurrentUser.UserId}";
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            CallPostMethod(postData, uri, ref statusCode);
            return RedirectToAction("ActivityDashboard", "Home");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CrxAuthorize(Roles = "Common_TMSRefresh")]
        public IActionResult TmsRefresh()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult TmsRefresh(string typeCode, string days)
        {
            var result = string.Empty;
            if (typeCode != null)
            {
                string uri;
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);

                switch (typeCode)
                {
                    case "workorder":
                        uri = $"tms/GetWorkOrdersByCauseCode/{days}";
                        result = CallPostMethod(string.Empty, uri, ref statusCode);
                        break;
                    case "PriorityMaster":
                        uri = "tms/GetTmsPriority/" + "WO";
                        result = CallGetMethod(uri, ref statusCode);
                        break;
                    case "BuildingMaster":
                        uri = "tms/GetTmsBuildings";
                        result = CallGetMethod(uri, ref statusCode);
                        break;
                    case "StatusMaster":
                        uri = "tms/GetTmsStatus/" + "WO";
                        result = CallGetMethod(uri, ref statusCode);
                        break;
                    case "AssetMaster":
                        uri = $"tms/GetTmsAssets/{days}";
                        result = CallGetMethod(uri, ref statusCode);
                        break;
                    case "ResourecMaster":
                        uri = "tms/GetTmsResource/";
                        result = CallGetMethod(uri, ref statusCode);
                        break;
                    case "SkillMaster":
                        uri = "tms/GetTmsSkill/";
                        result = CallGetMethod(uri, ref statusCode);
                        break;
                    case "TypeMaster":
                        uri = "tms/GetTmsType/" + "WO";
                        result = CallGetMethod(uri, ref statusCode);
                        break;
                }
            }
            return Json(new { Result = result });
        }

        #endregion

        #region Json Results Actions

        [HttpGet]
        public JsonResult GetBuildingsBySite(string siteCode)
        {
            var buildings = _buildingsService.GetBuildings().Where(x => x.SiteCode == siteCode && x.IsActive).ToList();
            return Json(buildings);
        }

        #endregion

        #region Header menu  

        //[ChildActionOnly]
        //[OutputCache(Duration = 60, VaryByParam = "none")]
        public PartialViewResult HeaderMenu()
        {
            var org = new Organization();
            if (UserSession.CurrentOrg.Orgkey != Guid.Empty)
                org = _organizationService.GetUserDashBoard(UserSession.CurrentUser.UserId, 0);

            var lists = from BDO.Enums.MenuPermitAliasType e in Enum.GetValues(typeof(BDO.Enums.MenuPermitAliasType))
                        select new
                        {
                            Value = (int)e,
                            Text = e.GetDescription()
                        };

            if (!UserSession.CurrentUser.IsPowerUser)
            {
                int permitparentid = Convert.ToInt32(org.Services.Where(x => x.Alias.Trim().ToLower() == "inspection_icra" && x.IsActive == true).Select(x => x.Id).FirstOrDefault());
                if (permitparentid > 0)
                {
                    foreach (var permit in org.Services)
                    {
                        if (permit.ParentId == permitparentid && permit.Alias.Trim().ToLower() != "icraproject_index" && permit.Alias.Trim().ToLower() != "permit_paperpermit" && permit.Alias.Trim().ToLower() != "icra_icraprojectpermit" && permit.Alias.Trim().ToLower() != "permit_getallpermits")
                        {
                            int permittype = Convert.ToInt32(lists.Where(x => x.Text.ToLower() == permit.Name.ToLower()).Select(x => x.Value).FirstOrDefault());
                            bool Isfalse = _organizationService.IsPermitActiveByUserID(UserSession.CurrentUser.UserId, permittype);
                            if (!Isfalse)
                            {
                                org.Services = org.Services.Where(x => x.Id != permit.Id).ToList();
                            }
                        }

                    }
                }
            }


            return PartialView("_header", org);
        }

        public JsonResult GetActiveClass(string url)
        {
            var menuName = string.Empty; //_commonModelFactory.GetActiveClass(url);
            return Json(menuName);
        }

        [HttpGet]
        public string B64EncodeImage(string imgFile)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var ext = Path.GetExtension(imgFile)?.Replace(".", string.Empty);
            var webClient = new WebClient();
            if (!string.IsNullOrEmpty(imgFile))
            {
                var imageBytes = webClient.DownloadData(imgFile.ToLower());
                return "data:image/" + ext + ";base64," + Convert.ToBase64String(imageBytes);
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region Footer

        //[ChildActionOnly]
        //[OutputCache(Duration = 3600)]
        public PartialViewResult FooterPartialView()
        {
            return PartialView("_footer");
        }

        #endregion

        #region Import Excel

        private DataTable SaveManufactures(DataTable dtRecords)
        {
            var view = new DataView(dtRecords);
            var distinctValues = view.ToTable(true, "ManufactureName");
            foreach (DataRow row in distinctValues.Rows)
            {
                var manufactures = new Manufactures
                {
                    ManufactureName = row["ManufactureName"].ToString(),
                    IsActive = true,
                    CreatedBy = UserSession.CurrentUser.UserId
                };
                var newManId = _manufactureService.Save(manufactures);
                if (newManId > 0)
                {
                    foreach (DataRow row2 in dtRecords.Rows)
                    {
                        if (row2["ManufactureName"].ToString() == manufactures.ManufactureName)
                            row2.SetField("ManufactureId", newManId);
                    }
                }
            }
            return dtRecords;
        }

        private DataTable SaveRoute(DataTable dtRecords)
        {
            var view = new DataView(dtRecords);
            var distinctValues = view.ToTable(true, "RouteName");
            foreach (DataRow row in distinctValues.Rows)
            {
                var route = new RouteMaster
                {
                    RouteNo = row["RouteName"].ToString(),
                    IsActive = true,
                    CreatedBy = UserSession.CurrentUser.UserId
                };
                int routeId = _locationService.SaveRoute(route, string.Empty);
                if (routeId > 0)
                {
                    foreach (DataRow row2 in dtRecords.Rows)
                    {
                        if (row2["RouteName"].ToString() == route.RouteNo)
                            row2.SetField("RouteId", routeId);
                    }
                }
            }
            return dtRecords;
        }

        private void ConvertToFloorAssets(DataTable dtRecords)
        {
            dtRecords = SaveManufactures(dtRecords);
            dtRecords = SaveRoute(dtRecords);

            var floorAssets = new List<TFloorAssets>();
            foreach (DataRow item in dtRecords.Rows)
            {
                var floorAsset = new TFloorAssets
                {
                    AssetId = Convert.ToInt32(item["AssetId"].ToString()),
                    AscId = Convert.ToInt32(item["AscId"].ToString()),
                    SerialNo = item["TagNo"].ToString(),
                    DeviceNo = item["TagNo"].ToString(),
                    NearBy = item["StopName"].ToString(),
                    Name = item["Model"].ToString(),
                    Stop = new StopMaster()
                };
                floorAsset.Stop.StopCode = item["StopCode"].ToString();
                floorAsset.Stop.StopName = item["StopName"].ToString();
                floorAsset.FloorId = Convert.ToInt32(item["FloorId"].ToString());
                floorAsset.Model = item["Model"].ToString();
                floorAsset.Source = (int)BDO.Enums.Source.Internal;
                if (!string.IsNullOrEmpty(item["ManufactureId"].ToString()))
                    floorAsset.ManufactureId = Convert.ToInt32(item["ManufactureId"].ToString());
                floorAsset.CreatedBy = 4;
                floorAssets.Add(floorAsset);
            }

            var tfloorAssets = new List<TFloorAssets>();
            foreach (var item in floorAssets)
            {
                try
                {
                    var floorAsset = _floorAssetService.AddFloorAssets(item);
                    tfloorAssets.Add(floorAsset);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
            SaveRouteMapping(dtRecords, tfloorAssets);


            //JavaScriptSerializer js = new JavaScriptSerializer();
            //js.MaxJsonLength = Int32.MaxValue;

            //string Uri = string.Empty;
            //Uri = "assets/addfloorassets";
            //foreach (var floorAsset in floorAssets)
            //{
            //    string postData = js.Serialize(floorAsset);
            //    int StatusCode = Convert.ToInt32(HCF.Utility.Enums.HttpReponseStatusCode.Success);
            //    string result = CommonController.CallPostMethod(postData, Uri, ref StatusCode);
            //}
        }

        private void SaveRouteMapping(DataTable dtRecords, IReadOnlyCollection<TFloorAssets> tfloorAssets)
        {
            foreach (DataRow row2 in dtRecords.Rows)
            {
                foreach (var item in tfloorAssets)
                {
                    if (row2["StopName"].ToString() == item.Stop.StopName && row2["StopCode"].ToString() == item.Stop.StopCode)
                        row2.SetField("StopId", item.StopId);
                }
            }
            var view = new DataView(dtRecords);
            var distinctValues = view.ToTable(true, "RouteId");
            foreach (DataRow row in distinctValues.Rows)
            {
                var selectedValues = dtRecords.AsEnumerable().Where(s => s.Field<string>("RouteId") == row["RouteId"].ToString()).Select(s => s.Field<string>("StopId")).ToArray();
                string commaSeperatedValues = string.Join(",", selectedValues);
                string locationId = commaSeperatedValues;
                var objStopsRouteMapping = new StopsRouteMapping
                {
                    RouteId = Convert.ToInt32(row["RouteId"].ToString())
                };
                _locationService.SaveLocationRouteMapping(objStopsRouteMapping, locationId + ",");
            }
        }

        #endregion

        #region FileUpload

        public IActionResult FileUpload()
        {
            return View();
        }

        #endregion

        #region DigitalSignature

        public IActionResult DigitalSignature(string fileName, string fileContent, string imgPreviewClass = "")
        {
            var stringArray = new List<string>
            {
                fileName, //0
                fileContent //1           
                ,
                !string.IsNullOrEmpty(fileName)
                    ? fileName.Split('.')[0]
                    : UserSession.CurrentUser.FullName,
                imgPreviewClass
            };
            var digitalSignatureViewModel = new DigitalSignatureViewModel
            {

                signs = _signService.GetUserSign(UserSession.CurrentUser.UserId).Where(x => x.IsSignOverride == true).OrderByDescending(x => x.CreatedDate).FirstOrDefault(),
                stringarray = stringArray

            };
            return PartialView("_DigitalSignature", digitalSignatureViewModel);
        }

        public IActionResult AddDigitalSignature(string fileName, string fileContent, string imgPreviewClass = "", string signctrlId = "")
        {
            var stringArray = new List<string>
            {
                fileName, //0
                fileContent //1           
                ,
                !string.IsNullOrEmpty(fileName)
                    ? fileName.Split('.')[0]
                    : UserSession.CurrentUser.FullName,
                imgPreviewClass,signctrlId
            };
            var digitalSignatureViewModel = new DigitalSignatureViewModel
            {

                signs = _signService.GetUserSign(UserSession.CurrentUser.UserId).Where(x => x.IsSignOverride == true).OrderByDescending(x => x.CreatedDate).FirstOrDefault(),
                stringarray = stringArray

            };

            return PartialView("_AddDigitalSignature", digitalSignatureViewModel);

        }

        [HttpPost]
        public IActionResult SaveDigitalSign(DigitalSignature digitalSignature)
        {
            if (!string.IsNullOrEmpty(digitalSignature.FileName) && !string.IsNullOrEmpty(digitalSignature.FileContent))
            {
                digitalSignature.CreatedBy = UserSession.CurrentUser.UserId;
                digitalSignature.UserId = digitalSignature.CreatedBy;
                digitalSignature = _signService.SaveSign(digitalSignature);
                digitalSignature = _signService.GetDigitalSign().Where(x => x.DigSignatureId == Convert.ToInt32(digitalSignature.DigSignatureId)).FirstOrDefault();
            }
            var result = new { Result = digitalSignature, status = true };
            return Json(result);
        }

        public IActionResult GetDigitalSignture(int digSignatureId)
        {
            var digitalSignature = _signService.GetDigitalSign().Where(x => x.DigSignatureId == digSignatureId).ToList();
            var result = new { Result = digitalSignature, status = true };
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDigitalSign(int digSignatureId, string fileNameCtr)
        {
             _signService.DeleteDigitalSign(digSignatureId);           
            return ViewComponent("ViewDigitalSign", new DigitalSignatureViewModel() { signs = null, HiddenFileControl = fileNameCtr });
        }

         [HttpPost]
        public IActionResult SaveDigitalSignfsb(DigitalSignature digitalSignature, string fileNameCtr, string signctrlId)
        {
            int? signindex = 0;
            if (!string.IsNullOrEmpty(digitalSignature.FileName) && !string.IsNullOrEmpty(digitalSignature.FileContent))
            {
                digitalSignature.CreatedBy = UserSession.CurrentUser.UserId;
                digitalSignature.UserId = digitalSignature.CreatedBy;
                digitalSignature = _signService.SaveSign(digitalSignature);
                digitalSignature = _signService.GetDigitalSign().FirstOrDefault(x => x.DigSignatureId == Convert.ToInt32(digitalSignature.DigSignatureId));
                string[] splitstr = fileNameCtr.Split('_');
                if (splitstr!=null && splitstr.Length == 3)
                {
                    if (splitstr[2].Length > 0)
                        signindex = Convert.ToInt32(splitstr[2]);
                }
            }                      
            return ViewComponent("ViewDigitalSign", new DigitalSignatureViewModel()
            {
                signs = digitalSignature,
                HiddenFileControl = fileNameCtr,
                MainSignatureClass = fileNameCtr,
                SignatureControlId = signctrlId,
                SignIndex = signindex
            });
       
        }

        [HttpPost]
        public IActionResult GetMySignature(int DigSignatureId, string fileNameCtr, string signctrlId)
        {
            var digitalSignature = new DigitalSignature();
            int? signindex = 0;
            if (DigSignatureId > 0)
            {
                digitalSignature.CreatedBy = UserSession.CurrentUser.UserId;
                digitalSignature.UserId = UserSession.CurrentUser.UserId;
                //digitalSignature = _signService.SaveSign(digitalSignature);
                digitalSignature = _signService.GetDigitalSign().FirstOrDefault(x => x.DigSignatureId == Convert.ToInt32(DigSignatureId));
                if (!string.IsNullOrEmpty(fileNameCtr))
                {
                    string[] splitstr = fileNameCtr.Split('_');
                    if (splitstr != null && splitstr.Length == 3)
                    {
                        if(splitstr[2].Length > 0)
                        signindex = Convert.ToInt32(splitstr[2]);
                    }
                }
            }           
            return ViewComponent("ViewDigitalSign", new DigitalSignatureViewModel()
            {
                signs = digitalSignature,
                HiddenFileControl = fileNameCtr,
                MainSignatureClass = fileNameCtr,
                SignatureControlId = signctrlId,
                SignIndex = signindex
            });
        
        }

        #endregion

        #region Common

        public IActionResult ComingSoon()
        {
            return View();
        }

        #endregion

        #region Scan PopUp

        public IActionResult ScanPopUp()
        {
            return PartialView("_ScanPopUp");
        }


        #endregion

        #region EPUser PopUps

        public IActionResult EpUserView(int epDetailId, string standardEp, int? docTypeId)
        {
            ViewBag.Title = standardEp;
            var epUsers = _epService.GetEPUsers(epDetailId, docTypeId);
            return PartialView("~/Views/Shared/PopUp/_EPUsersView.cshtml", epUsers);
        }

        #endregion

        #region Common Controls

        public IActionResult FLoorDDL(int buildingId, int selectedValue, string firstValue, string firstText)
        {
            var values = new List<SelectListItem>();
            List<Floor> floors;
            if (!string.IsNullOrEmpty(firstText))
            {
                var firstItem = new SelectListItem { Value = firstValue, Text = firstText };
                values.Add(firstItem);
            }
            if (buildingId > 0)
            {
                floors = _floorService.GetBuildingFloors(buildingId);
                foreach (var item in floors)
                {
                    var selItem = new SelectListItem
                    {
                        Text = item.FloorName,
                        Value = Convert.ToString(item.FloorId)
                    };
                    if (selectedValue > 0 && item.FloorId == selectedValue)
                        selItem.Selected = true;
                    values.Add(selItem);
                }
            }
            return View("~/Views/Shared/Control/_FloorDDL.cshtml", values);
        }

        #endregion

        #region Image Upload

        public IActionResult ImageUpload(string fileName, string fileContent, string imgPreviewCtr)
        {
            Dictionary<string, string> dataList = new Dictionary<string, string>
            {
                { "fileName", fileName },
                { "fileContent", fileContent },
                { "imgPreviewCtr", imgPreviewCtr },
                { "type", "camera" }
            };
            return PartialView(dataList);

        }

        #endregion

        #region CRxUrls

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 60 * 60 * 24)]     
        public IActionResult CRxUrls()
        {
            return PartialView("_CRxUrls");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 60 * 60 * 24)]
        public IActionResult JsScripts()
        {
            return PartialView("_jsscripts");
        }

        #endregion

        #region DymoPrinterBarcodeLabel

        //public IActionResult GenrateBarcodeLabel(string barcode,string description)
        //{
        //    bool isLabelPrinted;
        //    if (Utility.Common.IsPrinterAvialable(DymoPrinterConstants.PRINTER_NAME))
        //    {
        //        isLabelPrinted = Utility.Common.PrintLabel( string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/DymoLabel/")+DymoPrinterConstants.FORMAT_PATH, DymoPrinterConstants.PRINTER_NAME, barcode, description, DateTime.UtcNow.ToString("MMM dd, yyyy"));
        //    }
        //    else
        //    {
        //        return Json(new { success = false , message="Printer Not Found!!" });
        //    }

        //    return Json(new { success = isLabelPrinted , message="Barcode Printed Successfully." });
        //}

        #endregion

        #region Non Track Ep CSV
        private string AddEscapeSequenceInCsvField(string valueToEscape)
        {

            if (valueToEscape.Contains("\""))
            {
                valueToEscape = valueToEscape.Replace("\"", "\"\"");
            }

            if (valueToEscape.Contains(","))
            {
                valueToEscape = valueToEscape.Replace(",", ";");
            }

            if (valueToEscape.Contains(System.Environment.NewLine))
            {
                valueToEscape = $"\"{valueToEscape}\"";
            }
            if (valueToEscape.Contains("\r\n"))
            {
                valueToEscape = valueToEscape.Replace("\r\n", "");
            }
            return valueToEscape;
        }
        public IActionResult PrintCSVNonTrack(string CategoryId)
        {
            var eps = _epService.GetStandardEPs().Where(x => x.IsApplicable == false).ToList();
            if (!string.IsNullOrEmpty(CategoryId) && Convert.ToInt32(CategoryId) != 0)
                eps = eps.Where(x => x.CategoryId == Convert.ToInt32(CategoryId)).ToList();

            if (eps.Count > 0)
            {
                var categories = eps.GroupBy(x => x.CategoryId).Select(x => new Category { CategoryId = x.FirstOrDefault().CategoryId, Name = x.FirstOrDefault().CategoryName }).ToList();
                string csv = string.Empty;

                foreach (var category in categories)
                {

                    csv += category.Name;
                    csv += "\r\n";
                    foreach (var item in eps.Where(x => x.CategoryId == category.CategoryId).GroupBy(x => x.StDescID).Select(x => x.FirstOrDefault()).ToList())
                    {
                        csv += "EP#" + ',';
                        csv += "Description" + ',';
                        csv += "\r\n";

                        foreach (var ep in eps.Where(x => x.StDescID == item.StDescID).ToList())
                        {
                            //csv += ep.TJCStandard+ ',';

                            //csv += ep.Description + ',';
                            csv += AddEscapeSequenceInCsvField(ep.TJCStandard) + ',';
                            csv += AddEscapeSequenceInCsvField(ep.Description) + ',';
                            csv += "\r\n";
                        }

                    }
                }
                string fileName = "NonTrackEp_" + DateTime.Now.ToShortDateString() + ".csv";
                //string fileName = "NonTrackEp" + ".csv";
                var response = _contextAccessor.HttpContext.Response;
                //response.BufferOutput = true;
                response.Clear();
                //response.ClearHeaders();
                //response.ContentEncoding = Encoding.Unicode;
                response.Headers.Add("content-disposition", "attachment;filename=" + fileName);
                response.ContentType = "text/plain";
                response.WriteAsync(csv);               
            }
            return View();
        }
        #endregion

        #region State/City/Master

        //[CrxAuthorize(Roles = "Common_GetStates")]
        public IActionResult GetStates()
        {
            UISession.AddCurrentPage("Common_GetStates", 0, "State");
            var data = _siteService.GetStates();
            return View("States", data);
        }
        public IActionResult AddStateName(string stateId)
        {
            var state = new StateMaster();
            return PartialView("_AddStateName", state);
        }

        //[CrxAuthorize(Roles = "Common_GetCitiesForView")]
        public IActionResult GetCitiesForView()
        {
            UISession.AddCurrentPage("Common_GetCitiesForView", 0, "City");
            return View("Cities");
        }
        public IActionResult CityList(int stateId = 0, String state = "")
        {
            var data = _siteService.GetCities(stateId);
            ViewBag.StateName = state;
            return PartialView("_CityList", data);

        }
        public IActionResult GetCities(int stateId)
        {
            var data = _siteService.GetCities(stateId);
            return Json(data);
        }
        public IActionResult AddCity(string cityId)
        {
            var city = new CityMaster();
            return PartialView("_AddCity", city);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCity(CityMaster cityMaster)
        {
            if (ModelState.IsValid)
            {
                cityMaster.CreatedBy = UserSession.CurrentUser.UserId;
                cityMaster.CityId = _siteService.AddCity(cityMaster);
                if (cityMaster.CityId > 0)
                {
                }

                return RedirectToAction("GetCitiesForView");
            }
            else
            {
                return View("Cities", cityMaster);
            }

        }

        #endregion

        #region File History
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> RecentFiles()
        {
            var files = _commonService.GetRecentFiles(UserSession.CurrentUser.UserId);           
            return PartialView("_RecentFilesHistory", files);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public JsonResult GetUploadedFile(string attachment)
        {
            var attachments = new List<TFiles>();
            if (!string.IsNullOrEmpty(attachment))
                attachments = _commonService.GetTFiles(attachment);

            return Json(attachments);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadFiles(List<TFiles> files)
        {
            files = SaveAHJFiles(files);
            var result = new { Result = files, status = true };
            return Json(result);
        }

        private List<TFiles> SaveAHJFiles(List<TFiles> files)
        {
            files = _commonService.SaveTFiles(files, UserSession.CurrentUser.UserId);
            foreach (var item in files)
            {
                item.FilePath = _commonModelFactory.FilePath(item.FilePath);
            }
            return files;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="filesData"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveUploadedFile(IFormCollection files, string filesData)
        {
            var currentUserId = UserSession.CurrentUser.UserId;
            var tfiles = new List<TFiles>();
            List<FileObject> result;
            bool savedSuccessfully = true;
            bool foundDuplicateFiles = false;
            try
            {
                result = _commonModelFactory.JsonDeserialize<List<FileObject>>(filesData);
                if (files.Files.Count > 0)
                {
                    var filesdata = HttpContext.Request.Form.Files;
                    foreach (var file in filesdata)
                    {
                        var tFile = new TFiles();
                        var fileExtension = CommonUtility.GetFileExtension(file.FileName);
                        var newFileName = result.FirstOrDefault(x => x.OrgFileName == file.FileName);
                        if (newFileName != null)
                        {
                            byte[] thePictureAsBytes = ConvertToBytes(file);
                            tFile.FileSize = file.Length;
                            tFile.FileContent = Convert.ToBase64String(thePictureAsBytes);
                            tFile.FileType = BDO.Enums.FileType.MiscDocuments;
                            tFile.FileName = $"{newFileName.FileName}.{fileExtension}";
                            tFile.MD5Digest = _fileUpload.ComputeHashOfFile(tFile.FileContent);
                            tFile.UploadedBy = currentUserId;
                        }
                        tfiles.Add(tFile);
                    }
                }
            }
            catch (Exception)
            {
                savedSuccessfully = false;
            }

            string mdfDigest = string.Join(',', tfiles.Select(x => x.MD5Digest));
            string fileNames = string.Join(',', tfiles.Select(x => x.FileName));
            var duplicateFiles = _commonService.FindUploadeFiles(mdfDigest, fileNames, currentUserId).GroupBy(l => l.TFileId)
                .Select(g => g.OrderByDescending(c => c.TFileId).FirstOrDefault())
                .ToList();
            if (duplicateFiles.Any())
            {
                foundDuplicateFiles = true;
                foreach (var item in tfiles)
                {
                    item.ReleatedFiles = duplicateFiles.Where(x => x.MD5Digest == item.MD5Digest || x.FileName.Contains(item.FileName));
                }
            }
            if (!foundDuplicateFiles)
                tfiles = SaveAHJFiles(tfiles);

            var data = new { Result = tfiles, status = savedSuccessfully, foundDuplicateFiles, duplicateFiles };
            return Json(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private static byte[] ConvertToBytes(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.OpenReadStream().CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public IActionResult UploadFile(List<TFiles> files, string fileIds)
        {
            var tfiles = SaveAHJFiles(files.Where(x => x.TFileId == 0 && !string.IsNullOrEmpty(x.FileContent)).ToList());
            var data = new { Result = tfiles, status = true };
            return Json(data);
        }

        #endregion

        #region DrawingPathway

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorTypeId"></param>
        /// <returns></returns>
        public IActionResult DrawingPathway(int? floorTypeId = 1)
        {
            UISession.AddCurrentPage("Repository_FloorPlans", 0, "Drawings");
            ViewBag.FloorTypeId = floorTypeId;
            var userId = UserSession.CurrentUser.UserId;
            if (UserSession.CurrentUser.IsPowerUser)
                userId = 0;
            var floorTpes = _floorTypeService.GetFloorTypes(userId).ToList();

            return PartialView("_DrawingPathway", floorTpes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorTypeId"></param>
        /// <returns></returns>
        public IActionResult DrawingpathwayFloorPlans(int? floorTypeId = 1)
        {
            List<FloorTypes> floorTpes = new List<FloorTypes>();
            FloorTypes floorType = _floorTypeService.GetFloorTypeFloors(floorTypeId.Value, 1);
            ViewBag.floorTypID = floorTypeId;
            if (floorType != null)
                floorTpes.Add(floorType);
            return PartialView("_DrawingpathwayFloorPlans", floorTpes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public JsonResult GetUploadedDrawings(string attachment)
        {
            var attachments = new List<DrawingpathwayFiles>();
            if (!string.IsNullOrEmpty(attachment))
                attachments = _commonService.GetUploadedDrawings(attachment);

            return Json(attachments);

        }

        #endregion

        #region Shared Partial View Action Method

        public IActionResult buildingCustomddl(string type)
        {
            var buildings = _buildingsService.GetBuildings().Where(x => x.IsActive).ToList();
            ViewBag.type = type;
            return PartialView("~/Views/Shared/_buildingCustomddl.cshtml", buildings);
        }

        public IActionResult ProjectCustomDDL(string type, string ProjectId)
        {
            int parentProjectId = 0;
            List<HCF.BDO.TIcraProject> projectList = new List<HCF.BDO.TIcraProject>();
            if (!string.IsNullOrEmpty(ProjectId))
            {
                if (ProjectId.Contains(","))
                {
                    projectList = _IcraProjectService.GetAllActiveTIcraProject().Where(x => x.ParentProjectId == null && x.Status).ToList();
                }
                else
                {


                    var projectparent = _IcraProjectService.GetAllActiveTIcraProject(false).Where(x => x.ProjectId == Convert.ToInt32(ProjectId) && x.ParentProjectId != null).Select(x => x.ParentProjectId).FirstOrDefault();

                    if (projectparent != null)
                    {
                        parentProjectId = projectparent.Value;
                    }
                    projectList = _IcraProjectService.GetAllActiveTIcraProject(false).Where(x => x.ParentProjectId == null && (x.Status || x.ProjectId == Convert.ToInt32(ProjectId) || x.ProjectId == parentProjectId)).ToList();
                }

            }
            ViewBag.type = type;
            ViewBag.ProjectId = ProjectId;
            return PartialView("~/Views/Shared/_IcraProjectCustomDDL.cshtml", projectList);
        }

        public IActionResult ProjectTypeDDL()
        {
            var projecttype = _constructionService.GetProjectType().ToList();
            return PartialView("~/Views/Shared/_ProjectType.cshtml", projecttype);
        }

        public IActionResult floorCustomddl(string type)
        {
            var floors = _floorService.GetFloors().Where(x => x.IsActive).ToList();
            ViewBag.type = type;
            return PartialView("~/Views/Shared/_floorCustomddl.cshtml", floors);
        }
      
        public IActionResult userCustomddl(string type)
        {
            List<UserProfile> users = new();
            if (UserSession.CurrentOrg.IsTmsWo)
            {
                users = _userService.GetUsers().Where(x => x.ResourceNumber != null && x.IsActive).OrderBy(x => x.FullName).ToList();
            }
            else
            {
                users = _userService.GetUsers().Where(x => x.IsActive).OrderBy(x => x.FullName).ToList();
            }
            ViewBag.type = type;
            return PartialView("~/Views/Shared/_userCustomddl.cshtml", users);
        }

        public IActionResult userddl(string type)
        {
            List<UserProfile> users = _userService.GetUsers().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email) && !x.IsCRxUser).OrderBy(x => x.FullName).ToList();
            ViewBag.type = type;
            return PartialView("~/Views/Shared/_userddl.cshtml", users);
        }
       
        #endregion

        //#region One -Time Method for update Password 

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[AllowAnonymous]
        //public async Task<JsonResult> UpdatePassword()
        //{
        //    var users = _userService.GetAllUsersFromMasterDb().Where(x => x.PasswordHash == "" && x.Password != null).ToList();
        //    foreach (var item in users)
        //    {
        //        try
        //        {
        //            var userInfo = await _userManager.FindByEmailAsync(item.Email);
        //            string userSalt = _userService.GetUserSalt(item.UserName);
        //            if (!string.IsNullOrEmpty(userSalt))
        //            {
        //                userInfo.Salt = Guid.Parse(userSalt);                      
        //                var Password = Conversion.Decrypt(userInfo.Password, Convert.ToString(userInfo.Salt));
        //                var createNewUser = await _userManager.AddPasswordAsync(userInfo, Password);
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    return Json("");
        //}

        //#endregion
    }
}

