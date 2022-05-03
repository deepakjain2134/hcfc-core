using HCF.BDO;
using HCF.BAL;
using HCF.Web.Base;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using HCF.Web.Filters;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using HCF.BAL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using HCF.Utility.Extensions;
using Microsoft.Extensions.Logging;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class ICRAController : BaseController
    {
        private readonly IConstructionService _constructionService;
        private readonly IIlsmService _ilsmService;
        private readonly ICommonService _commonService;
        private readonly ITIcraProjectService _icraProjectService;
        private readonly IEmailService _emailService;
        private readonly ILogger<ICRAController> _loggingService;
        private readonly IPermitService _permitService;
        private readonly IHttpPostFactory _httpService;
        private readonly IPdfService _pdfService;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IHCFSession _session;
        private readonly IUserService _userService;
        public ICRAController(
            ICommonModelFactory commonModelFactory, 
            IHCFSession session, 
            ILogger<ICRAController> loggingService, 
            IEmailService emailService, 
            IConstructionService constructionService,
            IIlsmService ilsmService, 
            ICommonService commonService, 
            ITIcraProjectService icraProjectService, 
            IPermitService permitService, 
            IHttpPostFactory httpService, 
            IPdfService pdfService, IUserService userService)            
        {
            _commonModelFactory = commonModelFactory;
            _session = session;
            _loggingService = loggingService;
            _emailService = emailService;
            _constructionService = constructionService;
            _ilsmService = ilsmService;
            _commonService = commonService;
            _icraProjectService = icraProjectService;
            _permitService = permitService;
            _httpService = httpService;
            _pdfService = pdfService;
            _userService = userService;
        }


        #region Construction Type AND Construction Activity
        // GET: ICRA
        // [CrxAuthorize(Roles = "ICRA_ConstructionType")]
        public ActionResult ConstructionType()
        {
            UISession.AddCurrentPage("ICRA_ConstructionType", 0, "Construction Type");
            var constructiontype = _constructionService.GetConstructionType();
            return View(constructiontype);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddConstructionType(ConstructionType objConstructionType)
        {
            if (ModelState.IsValid)
            {
                objConstructionType.CreatedBy = UserSession.CurrentUser.UserId;
                var postData = _commonModelFactory.JsonSerialize<ConstructionType>(objConstructionType);
                var uri = APIUrls.ICRA_AddConstructionType; //"icra/addConstructionType";
                var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                {

                    var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    if (httpResponse.Success)
                        SuccessMessage = httpResponse.Message;
                    else
                        ErrorMessage = httpResponse.Message;
                }
                else
                    ErrorMessage = ConstMessage.Internal_Server_Error;
                return RedirectToRoute("constructiontype");
            }
            return View(objConstructionType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddConstructionActivity(ConstructionActivity objConstructionActivity)
        {
            if (ModelState.IsValid)
            {
                objConstructionActivity.CreatedBy = UserSession.CurrentUser.UserId;
                var postData = _commonModelFactory.JsonSerialize<ConstructionActivity>(objConstructionActivity);
                var uri = APIUrls.ICRA_AddConstuctionActivity;
                var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                {
                    var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    if (httpResponse.Success)
                        SuccessMessage = httpResponse.Message;
                    else
                        ErrorMessage = httpResponse.Message;
                }
                else
                {
                    ErrorMessage = ConstMessage.Internal_Server_Error;
                }
                return RedirectToRoute("constructiontype");
            }
            return View(objConstructionActivity);
        }


        #region Partial view

        public ActionResult AddConstructionType(int? constructionTypeId)
        {
            var objConstructionType = new ConstructionType();
            if (constructionTypeId != null)
                objConstructionType = _constructionService.GetConstructionType(Convert.ToInt32(constructionTypeId));
            return PartialView("_AddConstructionType", objConstructionType);
        }

        public ActionResult AddConstructionActivity(int? constructionTypeId, int? constActivityId)
        {
            var objConstructionActivity = new ConstructionActivity
            {
                ConstructionTypeId = constructionTypeId
            };
            var objConstructionType = _constructionService.GetConstructionType(Convert.ToInt32(constructionTypeId));
            ViewBag.ConstructionType = objConstructionType;
            if (constActivityId != null && constActivityId > 0)
            {
                objConstructionActivity = objConstructionType.ConstructionActivity.FirstOrDefault(x => x.ConstActivityId == constructionTypeId);
            }
            return PartialView("_AddConstructionActivity", objConstructionActivity);
        }

        public ActionResult GetConstructionActivity(int constructiontypeId)
        {
            var objConstructionType = _constructionService.GetConstructionType(Convert.ToInt32(constructiontypeId));
            return PartialView("_ConstructionActivity", objConstructionType);
        }

        #endregion

        #endregion

        #region Construction Risk 

        // [CrxAuthorize(Roles = "ICRA_ConstructionRisk")]
        public ActionResult ConstructionRisk()
        {
            UISession.AddCurrentPage("ICRA_ConstructionRisk", 0, "Construction Risk");
            var constructionrisk = _constructionService.GetConstructionRisk();
            return View(constructionrisk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddConstructionRisk(ConstructionRisk objConstructionRisk)
        {
            if (ModelState.IsValid)
            {
                objConstructionRisk.CreatedBy = UserSession.CurrentUser.UserId;
                var postData = _commonModelFactory.JsonSerialize<ConstructionRisk>(objConstructionRisk);
                var uri = APIUrls.ICRA_AddConstructionRisk; //"icra/addConstructionRisk";
                var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                {
                    var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    if (httpResponse.Success)
                        SuccessMessage = httpResponse.Message;
                    else
                        ErrorMessage = httpResponse.Message;
                }
                else
                    ErrorMessage = ConstMessage.Internal_Server_Error;
                return RedirectToRoute("constructionrisk");
            }
            return View(objConstructionRisk);
        }

        public JsonResult AddRiskAreaToArea(int constructionRiskId, string riskAreaIds)
        {
            var res = AddRiskArea(constructionRiskId, riskAreaIds);
            return Json(res);
        }

        private int AddRiskArea(int constructionRiskId, string riskAreaIds)
        {
            var res = _constructionService.AddRiskAreaToArea(constructionRiskId, riskAreaIds, UserSession.CurrentUser.UserId);
            return res;
        }


        public ActionResult RiskAreaSelect(int constructionRiskId)
        {
            var constructionrisk = _constructionService.GetConstructionRisk(constructionRiskId);
            TempData["ICRARiskArea"] = _constructionService.GetICRARiskAraWithOutRisk();
            return PartialView("_RiskAreaSelect", constructionrisk);
        }


        #region Partial view

        public ActionResult AddConstructionRisk(int? constructionriskid)
        {
            UISession.AddCurrentPage("ICRA_AddConstructionRisk", 0, "Add Construction Type");
            var objConstructionrisk = new ConstructionRisk();
            if (constructionriskid != null)
                objConstructionrisk = _constructionService.GetConstructionRisk(Convert.ToInt32(constructionriskid));
            return PartialView("_AddConstructionRisk", objConstructionrisk);
        }

        #endregion

        #endregion

        #region Construction Class

        //[CrxAuthorize(Roles = "ICRA_ConstructionClass")]
        public ActionResult ConstructionClass()
        {
            UISession.AddCurrentPage("ICRA_ConstructionClass", 0, "Construction Class");
            var constructionclass = _constructionService.GetConstructionClass();
            return View(constructionclass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddConstructionClass(ConstructionClass objConstructionClass)
        {
            if (ModelState.IsValid)
            {
                objConstructionClass.CreatedBy = UserSession.CurrentUser.UserId;
                var postData = _commonModelFactory.JsonSerialize<ConstructionClass>(objConstructionClass);
                var uri = APIUrls.ICRA_AddConstructionClass;
                var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                {
                    var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    if (httpResponse.Success)
                        SuccessMessage = httpResponse.Message;
                    else
                        ErrorMessage = httpResponse.Message;
                }
                else
                    ErrorMessage = ConstMessage.Internal_Server_Error;
                return RedirectToRoute("constructionclass");
            }
            return View(objConstructionClass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddConstClassActivity(ConstructionClassActivity objConstructionClassActivity)
        {
            if (ModelState.IsValid)
            {
                objConstructionClassActivity.CreatedBy = UserSession.CurrentUser.UserId;
                var postData = _commonModelFactory.JsonSerialize<ConstructionClassActivity>(objConstructionClassActivity);
                var uri = APIUrls.ICRA_AddConstClassActivity;
                var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                {
                    var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    if (httpResponse.Success)
                        SuccessMessage = httpResponse.Message;
                    else
                        ErrorMessage = httpResponse.Message;
                }
                else
                    ErrorMessage = ConstMessage.Internal_Server_Error;
                return RedirectToRoute("constructionclass");
            }
            return View(objConstructionClassActivity);
        }


        #region Partial view

        public ActionResult AddConstructionClass(int? constructionclassid)
        {
            UISession.AddCurrentPage("ICRA_AddConstructionRisk", 0, "Add Construction Type");
            var objConstructionclass = new ConstructionClass();
            if (constructionclassid != null)
                objConstructionclass = _constructionService.GetConstructionClass(Convert.ToInt32(constructionclassid));
            return PartialView("_AddConstructionClass", objConstructionclass);
        }

        public ActionResult AddConstClassActivity(int? constructionclassid, int? ConstClassActivityId)
        {
            ConstructionClassActivity objconstructionClassActivity = new ConstructionClassActivity
            {
                ConstClassId = constructionclassid
            };
            ViewBag.ConstClassCategory = _constructionService.GetConstructionClassCategory();
            ConstructionClass objConstructionClass = _constructionService.GetConstructionClass(Convert.ToInt32(constructionclassid));
            ViewBag.ConstructionClass = objConstructionClass;
            if (ConstClassActivityId != null && ConstClassActivityId > 0)
            {
                objconstructionClassActivity = objConstructionClass.ConstructionClassActivity.FirstOrDefault(x => x.ConstClassActivityId == ConstClassActivityId);
            }
            return PartialView("_AddConstuctionClassActivity", objconstructionClassActivity);
        }

        public ActionResult GetConstClassActivity(int constructionclassid)
        {
            ConstructionClass objConstructionClass = new ConstructionClass();
            objConstructionClass = _constructionService.GetConstructionClass(Convert.ToInt32(constructionclassid));
            return PartialView("_ConstClassActivity", objConstructionClass);
        }


        #endregion

        #endregion

        #region ICRASteps

        // [CrxAuthorize(Roles = "ICRA_ICRASteps")]
        public ActionResult ICRASteps()
        {
            UISession.AddCurrentPage("ICRA_ICRASteps", 0, "ICRA Steps");
            var constructionclass = _constructionService.GetICRASteps();
            return View(constructionclass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddICRASteps(ICRASteps objICRASteps)
        {
            if (ModelState.IsValid)
            {
                objICRASteps.CreatedBy = UserSession.CurrentUser.UserId;
                var postData = _commonModelFactory.JsonSerialize<ICRASteps>(objICRASteps);
                var uri = APIUrls.ICRA_AddICRASteps; //"icra/addICRASteps";
                var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                {
                    var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    if (httpResponse.Success)
                        SuccessMessage = httpResponse.Message;
                    else
                        ErrorMessage = httpResponse.Message;
                }
                else
                    ErrorMessage = ConstMessage.Internal_Server_Error;

                return RedirectToRoute("icrasteps");
            }
            return View(objICRASteps);
        }


        #region Partial view

        public ActionResult AddICRASteps(int? icrastepId)
        {
            UISession.AddCurrentPage("ICRA_ICRASteps", 0, "Add ICRA Step");
            var objICRASteps = new ICRASteps();
            if (icrastepId != null)
                objICRASteps = _constructionService.GetICRASteps(Convert.ToInt32(icrastepId));
            return PartialView("_AddICRASteps", objICRASteps);
        }

        #endregion

        #endregion

        #region ICRA Inspection

        //[CrxAuthorize(Roles = "ICRA_InspectionIcra")]
        public ActionResult InspectionIcra(int? version)
        {
            UISession.AddCurrentPage("ICRA_InspectionIcra", 0, "ICRA");
            ViewBag.ConstructionRisk = _constructionService.GetConstructionRisk();
            if (version.HasValue && version.Value > 0)
            {
                ViewBag.version = version;
            }
            else
            {
                ViewBag.version = 1;
            }

            return View();
        }


        public ActionResult InspectionIcraPartial(string fromdate, string todate, string statusId, string constructionrikId, int version)
        {
            var objTIcraLog = new List<TIcraLog>();
            int? statusid = null;
            int? constructionrikid = null;
            if (!string.IsNullOrEmpty(statusId))
            {
                statusid = Convert.ToInt32(statusId);
                if (statusid == -2)
                {
                    statusid = null;
                }
            }
            ViewBag.version = version;
            if (!string.IsNullOrEmpty(constructionrikId))
            { constructionrikid = Convert.ToInt32(constructionrikId); }
            objTIcraLog = _constructionService.GetAllInspectionIcraSteps(-1, Convert.ToDateTime(fromdate), Convert.ToDateTime(todate), statusid, constructionrikid, version);

            if (UserSession.CurrentUser.IsVendorUser)
                objTIcraLog = objTIcraLog.Where(x => x.PermitRequestBy == UserSession.CurrentUser.UserId).ToList();
            else
                objTIcraLog = objTIcraLog.Where(x => x.Status != Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission) || x.PermitRequestBy == UserSession.CurrentUser.UserId).ToList();

            return PartialView("_inspectionIcra", objTIcraLog);
        }
        public bool CheckDirectoryExist(string directoryname)
        {
            bool exists = System.IO.Directory.Exists(_commonModelFactory.ServerMapPath(directoryname));
            return exists;
        }
        public void CreateDirectory(string fileName)
        {

            bool exists = CheckDirectoryExist("temp");
            if (!exists)
                DirectoryService.CreateDirectory(_commonModelFactory.ServerMapPath("temp"));
        }
        public ActionResult AddInspectionIcra(int? icraId, int? ceilingPermitId, int? linkicra, bool iseditable = true, int? version = 1)
        {
            bool isUserAuthorized = true;
            ViewBag.ShowIncomplete = 0;
            ViewBag.IsRequestEdited = "";
            string pageTitle = "Add ICRA";
            if (iseditable && icraId.HasValue)
                pageTitle = "Edit ICRA";
            else if (iseditable == false && icraId.HasValue)
                pageTitle = "View ICRA";



            UISession.AddCurrentPage("ICRA_AddInspectionIcra", 0, pageTitle);
            var objTIcraLog = TIcraLog(icraId, iseditable, version);
            if (icraId.HasValue && icraId > 0)
            {
                if (objTIcraLog != null && objTIcraLog.TicraId > 0)
                {
                    if (UserSession.CurrentUser.IsVendorUser)
                        isUserAuthorized = objTIcraLog.PermitRequestBy > 0 && objTIcraLog.PermitRequestBy == UserSession.CurrentUser.UserId;
                }
                else
                {
                    ErrorMessage = ConstMessage.Not_exist;
                    return RedirectToAction("InspectionIcra");
                }
                if (objTIcraLog.Status < 0 && UserSession.CurrentUser.IsVendorUser)
                {
                    //objTIcraLog.Status = Convert.ToInt32(Enums.ApprovalStatus.Requested);
                    ViewBag.ShowIncomplete = 1;
                }
                if (objTIcraLog.Status == 2)
                {
                    ViewBag.IsRequestEdited = "1";
                }

            }
            else
            {
                objTIcraLog.Version = version.HasValue ? version.Value : 1;
                objTIcraLog.Telephone = UserSession.CurrentUser.PhoneNumber;
                objTIcraLog.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);
                objTIcraLog.StartDate = DateTime.Now.ToClientTime();

                if (UserSession.CurrentUser != null)
                {

                    objTIcraLog.PermitRequestBy = UserSession.CurrentUser.UserId;
                    if (!string.IsNullOrEmpty(UserSession.CurrentUser.PhoneNumber))
                        objTIcraLog.Telephone = UserSession.CurrentUser.PhoneNumber;
                    else if (!string.IsNullOrEmpty(UserSession.CurrentUser.CellNo))
                        objTIcraLog.Telephone = UserSession.CurrentUser.CellNo;

                    if (UserSession.CurrentUser.IsVendorUser)
                        objTIcraLog.Contractor = UserSession.CurrentUser.Vendor.Name;
                    else
                        objTIcraLog.Contractor = UserSession.CurrentOrg.Name;

                }
            }
            if (ceilingPermitId.HasValue && linkicra.HasValue)
            {
                objTIcraLog.CeilingPermitId = Convert.ToString(ceilingPermitId.Value);
                objTIcraLog.LinkICRA = "1";
            }
            objTIcraLog.Contractor = !string.IsNullOrEmpty(objTIcraLog.Contractor) ? objTIcraLog.Contractor : string.Empty;
            if (objTIcraLog != null)
            {
                objTIcraLog.TIcraProject = new TIcraProject();
                if (objTIcraLog.ProjectId != 0)
                {
                    objTIcraLog.TIcraProject = _icraProjectService.GetCountAllActiveTIcraProject(UserSession.CurrentUser.UserId, UserSession.CurrentUser.VendorId != null ? UserSession.CurrentUser.VendorId : null, false).Where(x => x.ProjectId == objTIcraLog.ProjectId).FirstOrDefault();
                    objTIcraLog.TIcraProject.mode = "View";

                }
                ViewBag.lstUserProfile = _userService.Get().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email) && x.IsCRxUser == false && x.UserId != 0).ToList();
              
            }
            if (isUserAuthorized)
                return View(objTIcraLog);
            else
            {
                ErrorMessage = ConstMessage.NotAuthorized_Msg;
                return RedirectToAction("InspectionIcra");
            }

        }
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadICRAAttachments(int icraId, string PDFName, int hasattachment)
        {

            var mainRoot = "temp";
            var objTIcraLog = TIcraLog(icraId, false);

            var outpathfilename = $"{BDO.Enums.NotificationCategory.ICRA.ToString()}{"_"}{objTIcraLog.PermitNo}{".zip"}";
            var fileName = Guid.NewGuid() + ".zip";
            var tempOutPutPath = _commonModelFactory.ServerMapPath("temp" + fileName);
            DirectoryService.CreateDirectory(_commonModelFactory.ServerMapPath("temp"));
            try
            {

                using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
                {
                    s.SetLevel(9); // 0-9, 9 being the highest compression  

                    byte[] buffer = new byte[80321515];

                    MemoryStream outputMemStream = new MemoryStream();
                    ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

                    zipStream.SetLevel(3); //0-9, 9 being the highest level of compression

                    var base64EncodePdf = _pdfService.PCRAPermitWorksheetBytes(objTIcraLog.ProjectId, 0, objTIcraLog.TicraId, "ICRAPermitWorksheet");
                    byte[] fileBytes = Convert.FromBase64String(base64EncodePdf);
                    var filename = $"{BDO.Enums.NotificationCategory.ICRA.ToString()}{"_"}{objTIcraLog.PermitNo}{".pdf"}";
                    var entry = new ZipEntry(filename);
                    entry.DateTime = DateTime.Now;

                    s.PutNextEntry(entry);
                    MemoryStream inStream = new MemoryStream(fileBytes);
                    StreamUtils.Copy(inStream, s, new byte[80321515]);
                    inStream.Close();
                    for (int i = 0; i < objTIcraLog.TICRAFiles.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(objTIcraLog.TICRAFiles[i].FileName))
                        {
                            entry = new ZipEntry(Path.GetFileName(objTIcraLog.TICRAFiles[i].FileName));
                            entry.DateTime = DateTime.Now;
                            entry.IsUnicodeText = true;
                            s.PutNextEntry(entry);
                            var req = System.Net.WebRequest.Create(new Uri(_commonModelFactory.FilePath(objTIcraLog.TICRAFiles[i].FilePath)));
                            using (Stream stream = req.GetResponse().GetResponseStream())
                            {
                                int sourceBytes;
                                do
                                {
                                    sourceBytes = stream.Read(buffer, 0, buffer.Length);
                                    s.Write(buffer, 0, sourceBytes);
                                } while (sourceBytes > 0);
                            }
                        }
                    }
                    s.Finish();
                    s.Flush();
                    s.Close();

                }
                byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
                if (System.IO.File.Exists(tempOutPutPath))
                    System.IO.File.Delete(tempOutPutPath);

                return File(finalResult, "application/zip", outpathfilename);
            }
            catch (System.Exception ex)
            {
                _loggingService.LogError(ex.Message);
                return new EmptyResult();
            }

        }

        public TIcraLog TIcraLog(int? icraId, bool iseditable = true, int? version = 1)
        {
            TIcraLog objTIcraLog = new();
            ViewBag.ConstructionType = _constructionService.GetConstructionType();
            ViewBag.ConstructionRisk = _constructionService.GetConstructionRisk();
            if (version.HasValue && version.Value > 0)
            {
                ViewBag.ConstructionClass = _constructionService.GetConstructionClass().Where(x => x.Version == version || x.Version == 0).ToList();
            }

            ViewBag.IsEditable = iseditable;
            var enumData = from BDO.Enums.ICRAStatus e in Enum.GetValues(typeof(BDO.Enums.ICRAStatus))
                           select new
                           {
                               Value = (int)e,
                               Text = e.ToString()
                           };
            ViewBag.ICRAStatus = new SelectList(enumData.OrderByDescending(x => x.Value), "Value", "Text");
            objTIcraLog = _constructionService.GetInspectionIcraSteps(icraId);

            if (objTIcraLog.AreasSurroundings.Count == 0)
            {
                objTIcraLog.AreasSurroundings = new List<TICRAAreasNearBy>();
                foreach (BDO.Enums.AreasSurrounding item in Enum.GetValues(typeof(BDO.Enums.AreasSurrounding)))
                {
                    var newTICRAAreasNearBy = new TICRAAreasNearBy
                    {
                        AreasSurrounding = item,
                        AreasSurroundingId = (int)item
                    };
                    objTIcraLog.AreasSurroundings.Add(newTICRAAreasNearBy);
                }
            }


            if (objTIcraLog.TICRAFiles.Count == 0)
            {
                objTIcraLog.TICRAFiles = new List<TICRAFiles>();
                var ticrafieles = new TICRAFiles
                {
                    UploadedBy = UserSession.CurrentUser.UserId
                };
                objTIcraLog.TICRAFiles.Add(ticrafieles);
            }

            #region Set ICRA (project Name, Permit No) in ILSM note

            var IcraToIlsm = $"{objTIcraLog.ProjectName}\nPermit #: {objTIcraLog.PermitNo}";
            var icraarr = new ArrayList { IcraToIlsm, icraId };
            TempData.Put("IcraItem", icraarr);

            #endregion
            return objTIcraLog;
        }

        private int getSubmitButtonValues(string submit)
        {
            if (!string.IsNullOrEmpty(submit) && submit.ToLower() != "save incomplete")
                return 1;
            else
                return -1;
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult AddInspectionIcra(TIcraLog objTIcraLog, string attachFiles, string submit, string IsRequestEdited, string CeilingPermitId)
        {
            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            int submitValue = getSubmitButtonValues(submit);
            if (submitValue < 0)
                objTIcraLog.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission);
            else if (submitValue > 0 && objTIcraLog.Status == -1)
                objTIcraLog.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);

            for (int i = 0; i <= objTIcraLog.TPermitWorkFlowDetails.Count; i++)
            {
                ModelState.Remove("TPermitWorkFlowDetails[" + i + "].DSPermitSignature.DigSignatureId");
                ModelState.Remove("TPermitWorkFlowDetails[" + i + "].DSPermitSignature.LocalSignDateTime");
                ModelState.Remove("TPermitWorkFlowDetails[" + i + "].DSPermitSignature.CreatedBy");
                ModelState.Remove("TPermitWorkFlowDetails[" + i + "].DSPermitSignature.LabelSignDate");
                ModelState.Remove("TPermitWorkFlowDetails[" + i + "].DSPermitSignature.Path");
            }


            if (ModelState.IsValid)
            {
                int ticraId = AddTIcraLog(objTIcraLog, attachFiles, "2");
                if (!string.IsNullOrEmpty(objTIcraLog.PermitNo))
                {
                    foreach (var objpermitworkflow in objTIcraLog.TPermitWorkFlowDetails)
                    {
                        objpermitworkflow.CreatedBy = objTIcraLog.CreatedBy;
                        objpermitworkflow.PermitNumber = objTIcraLog.PermitNo;
                        objpermitworkflow.LabelSignDate = objpermitworkflow.DSPermitSignature != null && objpermitworkflow.DSPermitSignature.LocalSignDateTime != null ?
                            objpermitworkflow.DSPermitSignature.LocalSignDateTime : (DateTime?)null;
                        if (objTIcraLog.Status != 0)
                        {
                            _permitService.AddTPermitWorkFlowDetails(objpermitworkflow, objTIcraLog.ProjectId.ToString());
                        }
                        if (objTIcraLog.TicraId == 0)
                        {

                            if ((objpermitworkflow.LabelValue.HasValue && objpermitworkflow.LabelValue.Value > 0) && objpermitworkflow.IsNotifyEmail)
                                _emailService.SendICRANotifyEmail(objpermitworkflow.LabelValue.Value, objTIcraLog.PermitNo, "2", objTIcraLog.Version);

                        }
                    }

                }
                if (objTIcraLog.TicraId == ticraId)
                {
                    SuccessMessage = $"Permit # {objTIcraLog.PermitNo} {ConstMessage.Success_Updated}";
                }
                else if (objTIcraLog.TicraId == 0 && ticraId > 0)
                {
                    SuccessMessage = $"Permit # {objTIcraLog.PermitNo} {ConstMessage.Success}";
                }


                if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.ICRA.ToString(), BDO.Enums.NotificationEvent.OnSubmit.ToString()))
                {
                    if (submitValue > 0)
                        SendICRAMail(ticraId, IsRequestEdited);

                }
                else if (objTIcraLog.Status != 2)
                {
                    if (submitValue > 0)
                        SendICRAMail(ticraId, IsRequestEdited);
                }

                if (objTIcraLog.Version == 1)
                    return RedirectToRoute("inspectionicrav1");
                else
                    return RedirectToRoute("inspectionicrav2");
            }
            ViewBag.IsEditable = true;
            ViewBag.ConstructionType = _constructionService.GetConstructionType();
            ViewBag.ConstructionRisk = _constructionService.GetConstructionRisk();
            ViewBag.ConstructionClass = _constructionService.GetConstructionClass();

            return View(objTIcraLog);
        }
        public JsonResult DeleteICRADrawings(int TicraId, string fileIds)
        {
            var result = _constructionService.DeleteICRADrawings(TicraId, fileIds);
            return Json(result);
        }
        private void SendICRAMail(int ticraId, string IsRequestEdited)
        {
            var objTIcraLog = _constructionService.GetInspectionIcraSteps(ticraId);
            int CurrentSignSequence = objTIcraLog.TPermitWorkFlowDetails != null && objTIcraLog.TPermitWorkFlowDetails.Where(x => x.LabelSignId == 0 || x.LabelSignId == null).ToList().Count > 0 ? objTIcraLog.TPermitWorkFlowDetails.Where(x => x.LabelSignId == 0 || x.LabelSignId == null).OrderBy(x => x.Sequence).ToList().FirstOrDefault().FormSettingId : 0;
            var objPermitEmailModel = new PermitEmailModel
            {
                //PermitType = $"METHOD OF PROCEDURE [MOP]",
                PermitType = BDO.Enums.PermitType.ICRA,
                PermitNo = objTIcraLog.PermitNo != null ? objTIcraLog.PermitNo : "",
                ProjectName = objTIcraLog.ProjectName,
                ProjectNo = objTIcraLog.ProjectNo,
                Requestor = objTIcraLog.TPermitWorkFlowDetails != null && objTIcraLog.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.HasValue && objTIcraLog.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.Value > 0 ? objTIcraLog.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().UserDetail.Name : string.Empty,
                RequestedDate = !string.IsNullOrEmpty(Convert.ToString(objTIcraLog.RequestDate)) ? objTIcraLog.RequestDate : objTIcraLog.CreatedDate,
                Approver = objTIcraLog.TPermitWorkFlowDetails != null && objTIcraLog.TPermitWorkFlowDetails.OrderByDescending(x => x.Sequence).ToList().FirstOrDefault().LabelValue.HasValue && objTIcraLog.TPermitWorkFlowDetails.OrderByDescending(x => x.Sequence).ToList().FirstOrDefault().LabelValue.Value > 0 ? objTIcraLog.TPermitWorkFlowDetails.OrderByDescending(x => x.Sequence).ToList().FirstOrDefault().UserDetail.Name : string.Empty,
                ApprovalStatus = objTIcraLog.Status,
                Reason = objTIcraLog.ReasonRejection,
                RequesterEmail = objTIcraLog.TPermitWorkFlowDetails != null && objTIcraLog.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.HasValue && objTIcraLog.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.Value > 0 ? objTIcraLog.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().UserDetail.Email : string.Empty,
                Company = objTIcraLog.Contractor,
                IsRequestEdited = IsRequestEdited,
                EventId = objTIcraLog.Status == 1 ? Convert.ToInt32(BDO.Enums.NotificationEvent.OnApproved) : Convert.ToInt32(BDO.Enums.NotificationEvent.OnSubmit)
            };
            if (objPermitEmailModel != null && CurrentSignSequence > 0)
            {
                objPermitEmailModel.AdditionalNextLevelToEmails = objTIcraLog.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null ? objTIcraLog.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.NotificationEmail : string.Empty;
                objPermitEmailModel.AdditionalNextLevelCCEmails = objTIcraLog.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null ? objTIcraLog.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.NotificationCCEmail : string.Empty;
                objPermitEmailModel.SendMailToNextPhase = objTIcraLog.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.IsSendMail;

            }
            var base64EncodePdf = _pdfService.PCRAPermitWorksheetBytes(objTIcraLog.ProjectId, 0, objTIcraLog.TicraId, "ICRAPermitWorksheet");
            byte[] filebytes = Convert.FromBase64String(base64EncodePdf);
            var filename = $"{BDO.Enums.NotificationCategory.ICRA.ToString()}{"_"}{objTIcraLog.PermitNo}{".pdf"}";
            objPermitEmailModel.PermitUrl = "icra/addinspectionicra?icraId=" + objTIcraLog.TicraId+"&version="+ objTIcraLog.Version;
            _emailService.SendPermitCreatedMail(objPermitEmailModel, Convert.ToInt32(BDO.Enums.NotificationCategory.ICRA), filebytes, filename, "", "", string.Empty);
        }

        private List<TFiles> GetTFiles(string files)
        {
            var tfiles = new List<TFiles>();
            foreach (var item in files.Split(','))
            {
                int fileId = Convert.ToInt32(item.Split('_')[0]);
                string tblName = item.Split('_')[1];
                var tfile = _commonService.GetFile(fileId, tblName);
                if (tfile != null)
                    tfiles.Add(tfile);
            }
            return tfiles;
        }
        public int AddTIcraLog(TIcraLog objTIcraLog, string attachFiles, string permittype)
        {
            int ticraId = 0;
            objTIcraLog.CreatedBy = UserSession.CurrentUser.UserId;
            var tFiles = new List<TFiles>();
            if (!string.IsNullOrEmpty(attachFiles))
            {
                tFiles = GetTFiles(attachFiles).ToList();
            }
            foreach (var item in tFiles)
            {
                var tICRAFiles = new TICRAFiles();
                {
                    tICRAFiles.AttachmentID = Convert.ToInt32(item.TFileId);
                    tICRAFiles.FilePath = item.FilePath;
                    tICRAFiles.FileName = item.FileName;
                    tICRAFiles.UploadedBy = item.UploadedBy;
                    tICRAFiles.TicraId = objTIcraLog.TicraId;
                    tICRAFiles.Name = item.Name;


                    objTIcraLog.TICRAFiles.Add(tICRAFiles);
                };

            }
            //foreach (var item in attachFiles.Split(','))
            //{
            //    var tICRAFiles = new TICRAFiles();
            //    if (!string.IsNullOrEmpty(item))
            //    {
            //        tICRAFiles.AttachmentID = Convert.ToInt32(item);
            //        objTIcraLog.TICRAFiles.Add(tICRAFiles);
            //    }
            //}
            ticraId = _constructionService.AddIcra(objTIcraLog, permittype);
            return ticraId;

        }
        public ActionResult IcraHistory(string modulename, int icraId, int? permittype, string permitno)
        {
            var objaudit = new List<Audit>();
            if (permittype == 1)
            {
                UISession.AddCurrentPage("PCRA_PCRAHistory", 0, "CRA History");
            }
            else
            {
                UISession.AddCurrentPage("ICRA_ICRAHistory", 0, "ICRA History");
            }

            var objticralog = _constructionService.GetInspectionIcraSteps(icraId);
            objaudit = _constructionService.GetIcraHistory(icraId);
            objticralog.IcraHistory = new List<Audit>();
            objticralog.IcraHistory = objaudit;
            ViewBag.PermitType = permittype;
            ViewBag.permitno = !string.IsNullOrEmpty(permitno) ? permitno : null;
            return View(objticralog);
        }


        public ActionResult TicraFiles(string modulename, int icraId, int? permittype)
        {
            if (permittype == 1)
            {
                UISession.AddCurrentPage("PCRA_TicraFiles", 0, " CRA Documents");
            }
            else
            {
                UISession.AddCurrentPage("ICRA_TicraFiles", 0, " ICRA Documents");
            }

            var objticra = _constructionService.GetInspectionIcraSteps(icraId);
            var ticrafiles = objticra.TICRAFiles;
            ViewBag.PermitType = permittype;
            return View(ticrafiles);
        }


        public ActionResult TICRAICMatrix()
        {
            return PartialView("_TICRAICMatrix");
        }

        public ActionResult TICRAConstructionProject()
        {
            var constructionTypes = _constructionService.GetConstructionType();
            return PartialView("_TICRAConstructionProject", constructionTypes);
        }


        public ActionResult TICRARiskGroups()
        {
            var constructionRisks = _constructionService.GetConstructionRisk();
            return PartialView("_TICRARiskGroups", constructionRisks);
        }

        #endregion

        #region ICRA Risk Area 

        [CrxAuthorize(Roles = "ICRA_ICRARiskArea")]
        public ActionResult ICRARiskArea()
        {
            UISession.AddCurrentPage("icra_ICRARiskArea", 0, "Department Risk Types");
            var data = _constructionService.GetICRARiskAra();
            return View(data);
        }


        public ActionResult MngICRARiskArea(int? riskAreaId)
        {
            UISession.AddCurrentPage("icra_ICRARiskArea", 0, "Manage Department Risk Types");
            ICRARiskArea risArea = new ICRARiskArea();
            if (riskAreaId.HasValue)
            {
                risArea = _constructionService.GetICRARiskAra(riskAreaId.Value);
            }
            else { risArea.ApprovalStatus = 2; }

            if (risArea != null)
                risArea.lstconstructionrisk = _constructionService.GetConstructionRisk();

            return View(risArea);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult MngICRARiskArea(ICRARiskArea iCRARiskArea)
        {
            if (ModelState.IsValid)
            {
                List<int> riskids = new List<int>();
                iCRARiskArea.ApprovalStatus = 1;
                iCRARiskArea.CreatedBy = UserSession.CurrentUser.UserId;
                string postData = _commonModelFactory.JsonSerialize<ICRARiskArea>(iCRARiskArea);
                string uri = APIUrls.ICRA_MngICRARiskArea;// "icra/addICRARiskArea";
                int StatusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                string result = _httpService.CallPostMethod(postData, uri, ref StatusCode);
                if (StatusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                {
                    HttpResponseMessage httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    var constructionRiskID = httpresponse.Result.ICRARiskArea.RiskTypeID;
                    var riskAreaID = httpresponse.Result.ICRARiskArea.RiskAreaId.ToString();
                    //var firetypesystemID = httpresponse.Result.ICRARiskArea.FireRiskType;
                    if (!string.IsNullOrEmpty(riskAreaID))
                    {
                        var GetRiskDept = _constructionService.GetConstRiskDeptMap().Where(x => x.ConstructionRiskAreaId == Convert.ToInt32(riskAreaID)).ToList();
                        if (GetRiskDept.Count > 0)
                        {
                            var status = _constructionService.DeleteRiskAreaToArea(Convert.ToInt32(riskAreaID));
                        }
                    }
                    var GetConstRiskDept = _constructionService.GetConstRiskDeptMap().Where(x => x.ConstructionRiskId == constructionRiskID).ToList();
                    string riskareaids = String.Join(",", GetConstRiskDept.Select(x => x.ConstructionRiskAreaId).ToList());
                    riskareaids = riskareaids + "," + riskAreaID;
                    var res = AddRiskArea(constructionRiskID, riskareaids.Trim());


                    if (httpresponse.Success)
                        SuccessMessage = httpresponse.Message;
                    else
                        ErrorMessage = httpresponse.Message;
                }
                else
                    ErrorMessage = ConstMessage.Internal_Server_Error;
                return RedirectToRoute("ICRARiskArea");
            }
            return View(iCRARiskArea);
        }

        public JsonResult ApproveRisk(int riskAreaId)
        {
            ICRARiskArea risArea = _constructionService.GetICRARiskAra(riskAreaId);
            risArea.ApprovalStatus = 1;
            _constructionService.UpdateICRARiskAra(risArea);
            return Json(risArea);
        }

        #endregion

        #region ICRA Matix Precautions

        //  [CrxAuthorize(Roles = "ICRA_ICRAMatixPrecautions")]
        public ActionResult ICRAMatixPrecautions(int version)
        {

            UISession.AddCurrentPage("ICRA_Matrix", 0, "ICRA Matrix");
            ViewBag.Version = version;
            return View();
        }

        public ActionResult AddICRAMatrixPrecaution(int constructionRiskId, int constructiontypeId, int version)
        {
            ViewBag.ConstructionClass = _constructionService.GetConstructionClass().Where(x => x.Version == 0 || x.Version == version).ToList();
            ViewBag.ConstructionRiskId = constructionRiskId;
            ViewBag.ConstructionTypeId = constructiontypeId;
            ViewBag.Version = version;
            List<ICRAMatixPrecautions> lists = _constructionService.GetICRAMatixPrecautions().Where(x => x.ConstructionRiskId == constructionRiskId && x.ConstructionTypeId == constructiontypeId && x.Version == version).ToList();
            return PartialView("_AddICRAMatrixPrecaution", lists);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddICRAMatrixPrecaution(int constructionRiskId, int constructionTypeId, string classIds, string version)
        {
            string classIdss = "";
            if (!string.IsNullOrEmpty(classIds))
                classIdss = classIds.Substring(1, classIds.Length - 1);
            int res = _constructionService.AddICRAMatixPrecautions(constructionRiskId, constructionTypeId, classIdss, UserSession.CurrentUser.UserId, Convert.ToInt32(version));
            return Json(classIdss);
        }

        public static bool CheckisAssign(int classId, List<ICRAMatixPrecautions> objICRAMatixPrecautions)
        {
            return (objICRAMatixPrecautions.Any(x => x.ConstructionClassId == classId && x.IsActive));

        }


        public ActionResult ICRAMatrix()
        {
            ViewBag.Constructiontype = _constructionService.GetConstructionType();
            ViewBag.ConstructionRisk = _constructionService.GetConstructionRisk();
            List<ICRAMatixPrecautions> lists = _constructionService.GetICRAMatixPrecautions();
            return PartialView("_ICRAMatrix", lists);
        }

        #endregion

        #region ICRAIC Areas Surrounding


        public ActionResult TICRAICAreasSurrounding()
        {
            return PartialView("_TICRAICAreasSurrounding");
        }


        public ActionResult MultiSelectDropDown(string controlId)
        {
            var dataModel = _constructionService.GetConstructionRisk();
            ViewBag.controlId = controlId;
            return PartialView("_MultiSelectDropDown", dataModel);
        }
        #endregion

        #region Notify Email

        public JsonResult NotifyEmail(int userId, string permitno, string permitType, string version)
        {
            string Message = string.Empty;
            string uri = "icra/SendNotifyEmail/" + userId + "/" + permitno + "/" + permitType + "/" + version;
            int StatusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            string result = _httpService.CallGetMethod(uri, ref StatusCode);
            if (StatusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                //HttpResponseMessage httpresponse = jsonSerializer.Deserialize<HttpResponseMessage>(result);
                var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                Message = httpresponse!=null && httpresponse.Success ? "Email sent successfully" : ConstMessage.Internal_Server_Error;
            }
            else
            {
                Message = ConstMessage.Internal_Server_Error;
            }
            return Json(Message);
        }
        public JsonResult NotifyPCRAEmail(int userId, string permitno, string permitType, string tpcraquesid, string projectid)
        {
            string Message = string.Empty;
            string uri = "icra/SendNotifyPCRAEmail/" + userId + "/" + permitno + "/" + permitType + "/" + tpcraquesid + "/" + projectid;
            int StatusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            string result = _httpService.CallGetMethod(uri, ref StatusCode);
            if (StatusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                //HttpResponseMessage httpresponse = jsonSerializer.Deserialize<HttpResponseMessage>(result);
                var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                Message = httpresponse.Success ? "Email sent successfully" : ConstMessage.Internal_Server_Error;
            }
            else
            {
                Message = ConstMessage.Internal_Server_Error;
            }
            return Json(Message);
        }
       
        #endregion

        #region Observation Report

        public ActionResult ReportCheckPoints()
        {
            UISession.AddCurrentPage("ICRA_ReportChkPoints", 0, "ICRA Report Checkpoints");
            var list = _constructionService.GetReportCheckPoints();
            return View(list);
        }

        public ActionResult MngReportCheckPoints(int? id)
        {
            var checkPoints = new ICRAObsReportCheckPoints();
            if (id.HasValue)
                checkPoints = _constructionService.GetReportCheckPoints(id.Value);
            return PartialView("_MngReportCheckPoints", checkPoints);

        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult MngReportCheckPoints(ICRAObsReportCheckPoints model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = UserSession.CurrentUser.UserId;
                var id = _constructionService.AddObservationReportCheckPoint(model);
                if (id > 0)
                {
                    SuccessMessage = ConstMessage.Success;
                    return RedirectToAction("ReportCheckPoints");
                }

            }
            return View(model);
        }

        public ActionResult TICRAObservationReport(int? icraId)
        {
            UISession.AddCurrentPage("ICRA_TICRAObservationReport", 0, "Observation Report");
            var icraLog = new TIcraLog();
            if (icraId.HasValue)
            {
                icraLog = _constructionService.GetICRAObservationReport(icraId.Value);
                if (icraLog.ObservtionReport == null)
                    icraLog.ObservtionReport = new TICRAReports();
            }
            else
            {
                icraLog.TicraId = 0;
                icraLog.StartDate = DateTime.Today;
            }
            return View(icraLog);
        }

        //NEW_PRAVEEN
        public ActionResult TICRAProjectObservationReport(string modulename, string projectId = null, string icraId = null, string reportId = null, string permitnumber = null, int tilsmId = 0)
        {
            UISession.AddCurrentPage("ICRA_TICRAObservationReport", 0, "Observation Report");
            bool IsUserAuthorized = true;
            var ObservtionReport = new TICRAReports();
            if (projectId != null)
            {
                ObservtionReport = _constructionService.GetICRAProjectObservationReport(projectId, reportId) ?? new TICRAReports();
                if (ObservtionReport != null && ObservtionReport.TICRAReportId > 0)
                {
                    if (UserSession.CurrentUser.IsVendorUser)
                        IsUserAuthorized = ObservtionReport.CreatedBy == UserSession.CurrentUser.UserId ? true : false;


                }
                else
                {
                    //ErrorMessage = ConstMessage.Not_exist;
                    //return RedirectToAction("IcraProjectPermit");
                }


                if (ObservtionReport.RelatedReports == null)
                {
                    ObservtionReport.RelatedReports = new List<TICRAReports>();
                }
            }
            else
            {
                ObservtionReport.TICRAReportId = 0;
                ObservtionReport.ReportDate = DateTime.Today;
                ObservtionReport.RelatedReports = new List<BDO.TICRAReports>();
            }

            ViewBag.PermitType = (!string.IsNullOrEmpty(modulename) && modulename.ToLower() == "cra") ? 1 : 2;
            ViewBag.PermitNumber = Convert.ToString(permitnumber);
            ViewBag.TICRAId = icraId;
            ViewBag.TilsmId = tilsmId;
            if (ObservtionReport.TICRAId == null || ObservtionReport.TICRAId == 0)
            {
                ObservtionReport.TICRAId = Convert.ToInt32(icraId);
            }
            if (IsUserAuthorized)
                return View(ObservtionReport);
            else
            {
                ErrorMessage = ConstMessage.NotAuthorized_Msg;
                return RedirectToAction("IcraProjectPermit");
            }
            // View(ObservtionReport);
        }

        public ActionResult ObservationReports(string modulename, int icraId, int? permittype, string permitno)
        {

            if (permittype == 1)
            {
                UISession.AddCurrentPage("ICRA_ObservationReports", 0, "CRA Observation Reports ");
            }
            else
            {
                UISession.AddCurrentPage("ICRA_TICRAObservationReport", 0, "ICRA Observation Reports ");
            }
            var icraLog = _constructionService.GetICRAObservationReport(icraId, 0);
            if (icraLog.ObservtionReport == null)
                icraLog.ObservtionReport = new TICRAReports();

            ViewBag.PermitType = permittype;
            ViewBag.permitno = permitno;
            return View(icraLog);
        }

        //NEW_PRAVEEN
        public ActionResult ProjectObservationReports(string projectIds, string permittype)
        {
            UISession.AddCurrentPage("ICRA_TICRAObservationReport", 0, "ICRA Project Observation Reports ");
            var ticrareports = _constructionService.GetICRAProjectObservationReport(projectIds);
            ticrareports = (ticrareports.RelatedReports != null ? ticrareports.RelatedReports.FirstOrDefault() : new TICRAReports()) ??
                           new TICRAReports();
            if (string.IsNullOrEmpty(permittype))
            {
                permittype = "2";
            }
            if (ticrareports.TICRAId != null)
            {
                ticrareports.PermitNumber = _constructionService.GetPermitNumber(ticrareports.TICRAId.Value, permittype);
                ViewBag.PermitNumber = Convert.ToString(ticrareports.PermitNumber);
            }

            return View(ticrareports);
        }

        public ActionResult ObservationReport(int icraId, int reportId)
        {
            UISession.AddCurrentPage("ICRA_ObservationReport", 0, "Observation Report ");
            var icraLog = _constructionService.GetICRAObservationReport(icraId, reportId);
            icraLog.ObservtionReport = icraLog.ObservtionReports.FirstOrDefault(x => x.TICRAReportId == reportId);
            return PartialView("_TICRAObservationReport", icraLog.ObservtionReport);
        }

        //NEW_PRAVEEN
        public ActionResult ProjectObservationReport(string projectIds, string reportId, string permittype)
        {
            UISession.AddCurrentPage("ICRA_ObservationReport", 0, "Project Observation Report ");
            var iCRAReports = _constructionService.GetICRAProjectObservationReport(projectIds, reportId);
            if (string.IsNullOrEmpty(permittype))
            {
                permittype = "2";
            }
            if (iCRAReports.TICRAId != null)
            {
                iCRAReports.PermitNumber = _constructionService.GetPermitNumber(iCRAReports.TICRAId.Value, permittype);
                ViewBag.PermitNumber = Convert.ToString(iCRAReports.PermitNumber);
            }

            return PartialView("_TICRAProjectObservationReport", iCRAReports);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult TICRAObservationReport(TICRAReports ObservtionReport, string reportdatetime)
        {
            if (ObservtionReport.TICRAId > 0 && !string.IsNullOrEmpty(reportdatetime))
            {
                ObservtionReport.CreatedBy = UserSession.CurrentUser.UserId;
                ObservtionReport.ReportDate = Convert.ToDateTime(reportdatetime);
                int reportId = _constructionService.SaveReport(ObservtionReport, UserSession.CurrentUser.UserId);
                if (reportId > 0)
                {
                    foreach (var item in ObservtionReport.CheckPoints)
                    {
                        item.TICRAReportId = reportId;
                        _constructionService.SaveReportCheckPoints(item);
                    }
                }
            }
            return RedirectToRoute("IcraPermit");
        }

        //NEW_PRAVEEN
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult TICRAProjectObservationReport(TICRAReports ObservtionReport, string reportdatetime, string icraid, int tilsmId = 0)
        {
            if (!string.IsNullOrEmpty(ObservtionReport.ProjectIds) && !string.IsNullOrEmpty(reportdatetime))
            {
                ObservtionReport.CreatedBy = UserSession.CurrentUser.UserId;
                ObservtionReport.ReportDate = Convert.ToDateTime(reportdatetime);

                if (ObservtionReport.TICRAId == null)
                    ObservtionReport.TICRAId = Convert.ToInt32(icraid);

                if (ObservtionReport.TIlsmId == null || ObservtionReport.TIlsmId == 0)
                    ObservtionReport.TIlsmId = Convert.ToInt32(tilsmId);

                int reportId = _constructionService.SaveProjectReport(ObservtionReport, UserSession.CurrentUser.UserId);
                if (reportId > 0)
                {
                    foreach (var item in ObservtionReport.CheckPoints)
                    {
                        item.TICRAReportId = reportId;
                        _constructionService.SaveReportCheckPoints(item);
                    }
                    if (ObservtionReport.TICRAReportId > 0)
                    {
                        SuccessMessage = ConstMessage.Success_Updated;

                    }
                    else if (reportId > 0 && ObservtionReport.TICRAReportId == 0)
                    {

                        SuccessMessage = ConstMessage.Success;
                    }
                    else
                        ErrorMessage = ConstMessage.Error;
                }
            }
            if (tilsmId > 0)
            {
                SuccessMessage = ConstMessage.Observation_report_link_with_ILSM_Successfully;
                return RedirectToAction("GetIlsmInspection", "Goal", new { tilsmId = tilsmId });
            }
            else
            {
                return RedirectToAction("IcraProjectPermit");
            }

        }

        public JsonResult EmailICRAReport(string projectIds, string reportId)
        {
            var reportPath = string.Empty;
            var httpResponse = new HttpResponseMessage();
            TICRAReports ObservationReport = new TICRAReports();
            ObservationReport.TICRAReportId = Convert.ToInt32(reportId);
            var base64EncodePdf = _pdfService.PrintProjectObserverReportInbytes(projectIds, reportId, ref ObservationReport);
            ObservationReport.FileName = "ObserverReport_" + ObservationReport.ProjectIds.Replace(",", "_") + "_" + ObservationReport.TICRAReportId + ".pdf";
            ObservationReport.FilesContent = base64EncodePdf;
            var postData = _commonModelFactory.JsonSerialize<TICRAReports>(ObservationReport);
            var uri = APIUrls.ICRA_GenerateObservationReport;
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
            if (httpResponse.Result.TICRAReports != null)
            {
                reportPath = _commonModelFactory.FilePath(httpResponse.Result.TICRAReports.ReportPath);

            }
            var data = new
            {
                Result = statusCode,
                ReportPath = reportPath,
            };
            return Json(data);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult AddConstructionIlsm(TIlsm objtilsm, string epdetails)
        {
            var lists = objtilsm.MainGoal.SelectMany(x => x.Steps.Where(y => y.Status == 1)).ToList().Select(x => x.StepsId).ToList();
            string stepsId = string.Join(",", lists);
            objtilsm.ConstIlsmStepId = stepsId;
            var dt = _ilsmService.InsertConstructionTilsm(objtilsm, epdetails);
            if (dt != null && dt.Rows.Count > 0)
            {
                var tilsmId = Convert.ToInt32(dt.Rows[0]["TILsmId"].ToString());
                var incidentId = dt.Rows[0]["TILsmId"].ToString();
                if (tilsmId > 0)
                {
                    SuccessMessage = ConstMessage.ICRAToILSM_SaveMessage.Replace("[IncidentId]", incidentId);
                }
            }
            else
            {
                ErrorMessage = ConstMessage.Error;
            }
            return RedirectToAction("AddInspectionIcra", "ICRA", new { icraId = objtilsm.TicraId.Value, iseditable = true });
        }

        public ActionResult ObservationReportSeachPopUp(string type, int id)
        {
            ViewBag.tilsmId = id;
            return PartialView("_ObservationReportSeachPopUp");
        }

        public ActionResult ObservationReportSeach(string projectno)
        {
            var lists = new List<TICRAReports>();
            if (UserSession.CurrentUser.IsVendorUser)
                lists = _constructionService.GetICRAProjectReport().Where(x => x.CreatedBy == UserSession.CurrentUser.UserId).ToList();
            else
                lists = _constructionService.GetICRAProjectReport();
            lists = lists.Where(x => x.ProjectNos.Contains(projectno)).ToList();
            return PartialView("_ObservationReportSeach", lists);
        }



        public JsonResult ILSMlinkToObservationReport(string tilsmId, string ticraReportIds)
        {
            var status = _ilsmService.ILSMlinkToObservationReport(tilsmId, ticraReportIds);
            return Json(status);
        }

        public ActionResult ObservationReportBinder()
        {
            return View();
        }

        public ActionResult PartialObservationReportBinder(string ProjectId)
        {
            var lists = new List<TICRAReports>();
            if(!string.IsNullOrEmpty(ProjectId))
                lists = _constructionService.GetICRAProjectReport().Where(x => x.ProjectIds.Contains(ProjectId)).ToList();
            return PartialView("_observationReportBinder", lists);
        }



        #endregion

        #region Signed Report

        public ActionResult ICRASignedReport(int ticraId, string permitType)
        {
            ViewBag.TicraFils = _constructionService.GetTICRAFiles(ticraId).Where(x => x.IsReport).ToList();
            var files = new List<TICRAFiles>();
            var file = new TICRAFiles
            {
                FileName = "",
                TicraId = ticraId
            };
            files.Add(file);
            ViewBag.PermitType = permitType;
            return PartialView("_ICRASignedReport", files);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult ICRASignedReport(List<TICRAFiles> tICRAFiles)
        {
            foreach (var item in tICRAFiles)
            {
                item.IsReport = true;
                item.UploadedBy = UserSession.CurrentUser.UserId;
            }

            var postData = _commonModelFactory.JsonSerialize<TICRAFiles>(tICRAFiles);
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = _httpService.CallPostMethod(postData, APIUrls.ICRA_SaveSingReport, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                if (httpResponse.Success)
                    SuccessMessage = httpResponse.Message;
                else
                    ErrorMessage = httpResponse.Message;
            }
            else
                ErrorMessage = ConstMessage.Internal_Server_Error;

            return Json(new { Result = tICRAFiles });
        }

        public ActionResult DeleteFile(int fileId)
        {
            var status = _constructionService.DeleteFile(fileId);
            return Json(new { Result = status });

        }

        #endregion

        #region 

        public ActionResult IcraPermit()
        {
            UISession.AddCurrentPage("ICRA_IcraPermit", 0, "Observation Reports");
            var lists = _constructionService.GetIcras();
            return View(lists);

        }
        //NEW_PRAVEEN
        //[CrxAuthorize(Roles = "ICRA_IcraProjectPermit")]
        public ActionResult IcraProjectPermit()
        {

            UISession.AddCurrentPage("ICRA_IcraPermit", 0, "Observation Reports");
            var lists = new List<TICRAReports>();

            if (UserSession.CurrentUser.IsVendorUser)
                lists = _constructionService.GetICRAProjectReport().Where(x => x.CreatedBy == UserSession.CurrentUser.UserId).ToList();
            else
                lists = _constructionService.GetICRAProjectReport();
            return View(lists);
        }


        #endregion

        #region ICRA ILSM

        public ActionResult ICRALists(string type, int? id)
        {
            ViewBag.ILSMId = id;
            List<TIcraLog> icralists = null;
            return PartialView("_icraLists", icralists);
        }

        public ActionResult ICRAList(int tilsmId, string searchParam)
        {
            ViewBag.currentILSMId = tilsmId;
            var icralists = new List<TIcraLog>();
            bool IsICRA = _commonModelFactory.IsMenuExist("ICRA", Convert.ToString(_session.ClientNo));
            if (!string.IsNullOrEmpty(searchParam))
                icralists = _constructionService.GetIcrasWithIlsm(searchParam, IsICRA);
            return PartialView("_icraListPartial", icralists);
        }
        #endregion
    }
}