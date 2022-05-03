using System;
using System.Linq;
using HCF.BDO;
using HCF.BAL;
using HCF.Web.Base;
using HCF.Utility;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using System.Threading.Tasks;
using HCF.Web.Filters;
using HCF.Web.Models;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using HCF.BAL.Interfaces.Services;
using HCF.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using HCF.Utility.AppConfig;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class PermitController : BaseController
    {
        private readonly IDepartmentService _departmentService;       
        private readonly ITIcraProjectService _icraProjectService;
        private readonly IPermitService _permitService;
        private readonly ICommonService _commonService;
        private readonly IBuildingsService _buildingsService;
        private readonly IOrganizationService _orgService;
        private readonly IFloorService _floorService;
        private readonly IConstructionService _constructionService;
        private readonly ITypeService _typeService;
        private readonly IEmailService _emailService;
        private readonly IHttpPostFactory _httpService;
        private readonly IHCFSession _session;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IAppSetting _appSettings;
        private readonly IPdfService _pdfService;      
        private readonly IUserService _userService;
        private readonly ILogger<PermitController> _logger;

        public PermitController(
            ILogger<PermitController> logger,
            IEmailService emailService, IDepartmentService departmentService,
            IConstructionService constructionService, IBuildingsService buildingsService, IOrganizationService orgService,
            IFloorService floorService, ITIcraProjectService icraProjectService,
            IPermitService permitService, ICommonService commonService ,           
            ITypeService typeService, IHttpPostFactory httpService, IHCFSession session, ICommonModelFactory commonModelFactory, IAppSetting appSettings, IPdfService pdfService, IUserService userService)
        {
            _emailService = emailService;
            _constructionService = constructionService;          
            _icraProjectService = icraProjectService;
            _permitService = permitService;
            _commonService = commonService;
            _buildingsService = buildingsService;
            _orgService = orgService;
            _floorService = floorService;
            _departmentService = departmentService;
            _typeService = typeService;
            _httpService = httpService;
            _session = session;
            _commonModelFactory = commonModelFactory;
            _appSettings = appSettings;
            _pdfService = pdfService;
            // _commonProvider = commonProvider;
            _userService = userService;
            _logger = logger;
        }

        #region CeilingPermit

        // GET: Permit
        //[CrxAuthorize(Roles = "Permit_Index")]
        public ActionResult Index(string Taggedid)
        {
            UISession.AddCurrentPage("Permit_Index", 0, "Ceiling Permit");
            var permit = _permitService.GetCeilingPermit();
            List<CeilingPermit> permitlistwithmytag = new List<CeilingPermit>();
            if (Taggedid != "0" && Taggedid != null)
            {
                ViewBag.TaggedID = Taggedid;
            }
            _session.Set(SessionKey.IsExistTagDeficiency, false);
            //Session["IsExistTagDeficiency"] = false;
            var userid = UserSession.CurrentUser.UserId;
            var taggedMaster = _typeService.GetTaggedList(userid, null, 3);
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
                    permitlistwithmytag = permit;
                    permit = new List<CeilingPermit>();

                    foreach (var tag in taggedMaster)
                    {
                        var records = permitlistwithmytag.Where(x => x.PermitNo == tag.PermitNo).FirstOrDefault();
                        if (records != null)
                        {
                            permit.Add(records);
                        }

                    }


                }


            }
            else
            {
                ViewBag.IsExistTagDeficiency = "false";
            }

            permit = UserSession.CurrentUser.IsVendorUser ? permit.Where(x => x.TPermitWorkFlowDetails != null
           && x.TPermitWorkFlowDetails.Count > 0 && x.TPermitWorkFlowDetails.Where(y => y.Sequence == 1).FirstOrDefault().LabelValue == UserSession.CurrentUser.UserId).ToList() : permit.Where(x => x.Status != Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission) || x.CreatedBy == UserSession.CurrentUser.UserId).ToList();

            foreach (var item in permit)
            {
                item.TaggedMaster = taggedMaster.Where(x => x.PermitNo == item.PermitNo).ToList();
            }
            var CeilingPermitViewModel = new CeilingPermitViewModel
            {
                lstCeilingPermit = permit,
                IsTaggingEnabled = _orgService.GetOrganization().IsTaggingEnabled,
            };

            return View(CeilingPermitViewModel);
        }
        public ActionResult DownloadCeilingPermitAttachments(int ceilingpermitid)
        {
            var ceilingPermit = _permitService.GetCeilingPermit(ceilingpermitid);
            var outpathfilename = $"{ceilingPermit.PermitNo}{".zip"}";
            var fileName = Guid.NewGuid() + ".zip";
            var tempOutPutPath = _commonModelFactory.ServerMapPath("temp" + fileName); //Server.MapPath("temp" + fileName);
            DirectoryService.CreateDirectory(_commonModelFactory.ServerMapPath("temp"));
            try
            {

                using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
                {
                    s.SetLevel(9); // 0-9, 9 being the highest compression 
                    byte[] buffer = new byte[2021515];
                    MemoryStream outputMemStream = new MemoryStream();
                    ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);
                    zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
                    var base64EncodePdf = _pdfService.CeilingPermitInbytes(ceilingpermitid, true);
                    byte[] fileBytes = Convert.FromBase64String(base64EncodePdf);
                    var filename = $"{ceilingPermit.PermitNo}{".pdf"}";
                    var entry = new ZipEntry(filename)
                    {
                        DateTime = DateTime.Now
                    };

                    s.PutNextEntry(entry);
                    MemoryStream inStream = new MemoryStream(fileBytes);
                    StreamUtils.Copy(inStream, s, new byte[2021515]);
                    inStream.Close();
                    for (int i = 0; i < ceilingPermit.Attachments.Count; i++)
                    {
                        entry = new ZipEntry(Path.GetFileName(ceilingPermit.Attachments[i].FileName))
                        {
                            DateTime = DateTime.Now,
                            IsUnicodeText = true
                        };
                        s.PutNextEntry(entry);
                        var req = System.Net.WebRequest.Create(new Uri(_commonModelFactory.FilePath(ceilingPermit.Attachments[i].FilePath)));
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
                    s.Finish();
                    s.Flush();
                    s.Close();
                }
                byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
                if (System.IO.File.Exists(tempOutPutPath))
                    System.IO.File.Delete(tempOutPutPath);

                return File(finalResult, "application/zip", outpathfilename);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new EmptyResult();
            }

        }
        [HttpGet]
        public ActionResult AddCeilingPermit(int? ceilingpermitId, int? mopid)
        {
            var currentUser = UserSession.CurrentUser;

            UISession.AddCurrentPage("Permit_AddCeilingPermit", 0, "Add Ceiling Permit");
            var ceilingPermit = new CeilingPermit();
            var enumData = from BDO.Enums.AdditionalItems e in Enum.GetValues(typeof(BDO.Enums.AdditionalItems))
                           select new
                           {
                               Value = (int)e,
                               Text = e.ToString()
                           };
            bool isUserAuthorized = true;
            ViewBag.ShowIncomplete = 0;
            ViewBag.IsRequestEdited = "";
            if (ceilingpermitId.HasValue && ceilingpermitId.Value > 0)
            {
                ceilingPermit = _permitService.GetCeilingPermit(ceilingpermitId.Value);
                if (ceilingPermit != null && ceilingPermit.CeilingPermitId > 0)
                {
                    if (currentUser.IsVendorUser)
                        isUserAuthorized = ceilingPermit.CreatedBy == currentUser.UserId;
                }
                else
                {
                    ErrorMessage = ConstMessage.Not_exist;
                    return RedirectToAction("Index");
                }
                if (ceilingPermit.Status < 0 && currentUser.IsVendorUser)
                {
                    //ceilingPermit.Status = Convert.ToInt32(Enums.ApprovalStatus.Requested);
                    ViewBag.ShowIncomplete = 1;
                }

                if (ceilingPermit.Status == 2)
                {
                    ViewBag.IsRequestEdited = "1";
                }


                ceilingPermit.FinalInspectionBy = (ceilingPermit.FinalInspectionBy == null && _commonModelFactory.IsAdminUser()) ? currentUser.UserId : ceilingPermit.FinalInspectionBy;
                if (ceilingPermit != null && ceilingPermit.TCeilingPermitDetail != null && ceilingPermit.TCeilingPermitDetail.Count == 0)
                {
                    ceilingPermit.TCeilingPermitDetail = _permitService.Get_PermitMappingForm();
                }
            }
            else
            {
                ceilingPermit.TCeilingPermitDetail = _permitService.Get_PermitMappingForm();
                ceilingPermit.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);
                ceilingPermit.RequestedDate = DateTime.Now.ToClientTime();
              
                if (currentUser != null)
                {
                    ceilingPermit.Requestedby = currentUser.UserId;
                    ceilingPermit.Email = currentUser.Email;
                    if (currentUser.IsVendorUser)
                        ceilingPermit.RequestedDept = currentUser.Vendor.Name;
                    else
                        ceilingPermit.RequestedDept = UserSession.CurrentOrg.Name;

                    if (!string.IsNullOrEmpty(currentUser.PhoneNumber))
                        ceilingPermit.PhoneNo = currentUser.PhoneNumber;
                    else if (!string.IsNullOrEmpty(currentUser.CellNo))
                        ceilingPermit.PhoneNo = currentUser.CellNo;
                }
                
                if (currentUser.IsVendorUser)
                    ceilingPermit.RequestedDept = currentUser.Vendor.Name;
                else
                    ceilingPermit.RequestedDept = UserSession.CurrentOrg.Name;

                List<TPermitWorkFlowDetails> TPermitWorkFlowDetails = new List<TPermitWorkFlowDetails>();
                var permitworkflow = _permitService.GetPermitWorkFlowSettings().Where(x => x.PermitType == 1).ToList();
                foreach (var workflow in permitworkflow)
                {
                    TPermitWorkFlowDetails objdetail = new TPermitWorkFlowDetails();
                    objdetail.FormSettingId = workflow.Id;
                    objdetail.LabelText = workflow.LabelText;
                    objdetail.LabelType = workflow.LabelType;
                    objdetail.Sequence = workflow.Sequence;
                    objdetail.Required = workflow.Required;
                    TPermitWorkFlowDetails.Add(objdetail);
                }
                ceilingPermit.RequestedDept = !string.IsNullOrEmpty(ceilingPermit.RequestedDept) ? ceilingPermit.RequestedDept : string.Empty;
                ceilingPermit.TPermitWorkFlowDetails = TPermitWorkFlowDetails;
            }

            ViewBag.additionalItems = new SelectList(enumData.OrderByDescending(x => x.Value), "Value", "Text", ceilingPermit.AdditionalItems ?? 0);
            if (mopid > 0)
            {
                TempData["MopId"] = mopid;
            }
            TempData.Keep();
            if (ceilingPermit.ProjectId != 0)
            {

                ceilingPermit.TIcraProject = _icraProjectService.GetCountAllActiveTIcraProject(currentUser.UserId, currentUser.VendorId != null ? currentUser.VendorId : null, false)
                    .FirstOrDefault(x => x.ProjectId == ceilingPermit.ProjectId);
                ceilingPermit.TIcraProject.mode = "View";


            }
            ViewBag.lstUserProfile = _userService.Get().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email) && x.IsCRxUser == false && x.UserId != 0).ToList();
            if (isUserAuthorized)
                return View(ceilingPermit);
            else
            {
                ErrorMessage = ConstMessage.NotAuthorized_Msg;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public JsonResult AddCeilingPermit(CeilingPermit ceilingpermit, string submit, string lstBuildingDetails, string lstFloorDetails, string IsRequestEdited, string NotSendMail)
        {
            int submitValue = getSubmitButtonValues(submit);
            ceilingpermit.CreatedBy = UserSession.CurrentUser.UserId;

            if (!string.IsNullOrEmpty(lstBuildingDetails))
            {
                var buildings = JsonConvert.DeserializeObject<List<Buildings>>(lstBuildingDetails);
                ceilingpermit.BuildingId = string.Join(",", buildings.Select(x => x.BuildingId));
                ceilingpermit.BuildingName = string.Join(",", buildings.Select(x => x.BuildingName));
            }
            //if (!string.IsNullOrEmpty(lstFloorDetails))
            //{
            //    var floors = JsonConvert.DeserializeObject<List<Floor>>(lstFloorDetails);
            //    ceilingpermit.FloorId = string.Join(",", floors.Select(x => x.FloorId));
            //    ceilingpermit.FloorName = string.Join(",", floors.Select(x => x.FloorName));
            //}
            if (!string.IsNullOrEmpty(ceilingpermit.Stime))
            {
                var slot = DateTime.ParseExact(ceilingpermit.Stime, "hh:mm tt", CultureInfo.InvariantCulture);
                ceilingpermit.StartTime = slot.TimeOfDay;
            }
            if (!string.IsNullOrEmpty(ceilingpermit.Etime))
            {
                var slot = DateTime.ParseExact(ceilingpermit.Etime, "hh:mm tt", CultureInfo.InvariantCulture);
                ceilingpermit.EndTime = slot.TimeOfDay;
            }
            if (!string.IsNullOrEmpty(NotSendMail) && NotSendMail == "1")
            {
            }
            else
            {
                if (submitValue < 0)
                    ceilingpermit.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission);
                else if (submitValue > 0 && ceilingpermit.Status == -1)
                    ceilingpermit.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);
            }


            List<Int32> tfileIds = new List<int>();
            if (!string.IsNullOrEmpty(ceilingpermit.AttachFiles))
            {
                foreach (var item in ceilingpermit.AttachFiles.Split(','))
                {
                    int fileId = Convert.ToInt32(item.Split('_')[0]);
                    tfileIds.Add(fileId);
                }
                if (!string.IsNullOrEmpty(ceilingpermit.TFileIds))
                {
                    string file = string.Join(",", tfileIds.Select(n => n.ToString()).ToArray());
                    ceilingpermit.TFileIds = ceilingpermit.TFileIds + ',' + file;
                }
                else
                {
                    ceilingpermit.TFileIds = string.Join(",", tfileIds.Select(n => n.ToString()).ToArray());
                }

            }
            int ceilinpermitId = _permitService.AddCeilingPermit(ceilingpermit);
            var isShowPopUp = 0;
            var permitNumber = "";
            if (ceilinpermitId > 0)
            {
                isShowPopUp = setPopupStatusCP(ceilingpermit.TCeilingPermitDetail);
                foreach (var item in ceilingpermit.TCeilingPermitDetail)
                {
                    item.CreatedBy = UserSession.CurrentUser.UserId;
                    item.CeilingPermitId = ceilinpermitId;
                    _permitService.InsertCPermitDetails(item);
                }
                permitNumber = _permitService.GetCeilingPermit(ceilinpermitId).PermitNo;

                if (!string.IsNullOrEmpty(permitNumber))
                {
                    foreach (var objpermitworkflow in ceilingpermit.TPermitWorkFlowDetails)
                    {
                        objpermitworkflow.CreatedBy = ceilingpermit.CreatedBy;
                        objpermitworkflow.PermitNumber = permitNumber;
                        if (ceilingpermit.Status != 0)
                        {
                            _permitService.AddTPermitWorkFlowDetails(objpermitworkflow, ceilingpermit.ProjectId.ToString());
                        }
                    }

                }
                if (TempData["MopId"] != null)
                {
                    SetRedirectForm(Convert.ToInt32(TempData["MopId"]), 1, ceilinpermitId.ToString(), permitNumber);
                }
                if (ceilingpermit.CeilingPermitId > 0)
                {
                    SuccessMessage = "Permit# " + permitNumber + " " + ConstMessage.Success_Updated;
                }
                else if (ceilinpermitId > 0 && ceilingpermit.CeilingPermitId == 0)
                {
                    int retval = _floorService.UpdatePermitsDrawingViewer(permitNumber, ceilingpermit.TempPermitNumber, UserSession.CurrentUser.UserId);
                    SuccessMessage = "Permit# " + permitNumber + " " + ConstMessage.Success;
                }
                else
                    ErrorMessage = ConstMessage.Error;

                if (!string.IsNullOrEmpty(NotSendMail) && NotSendMail == "1")
                {
                }
                else
                {
                    if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.CP.ToString(), BDO.Enums.NotificationEvent.OnSubmit.ToString()))
                    {
                        if (submitValue > 0)
                            SendCeilingPermitEmail(ceilinpermitId, ceilingpermit.BuildingId, IsRequestEdited);
                    }
                    else if (ceilingpermit.Status != 2)
                    {
                        if (submitValue > 0)
                            SendCeilingPermitEmail(ceilinpermitId, ceilingpermit.BuildingId, IsRequestEdited);
                    }
                }


            }
            var result = new
            {
                Result = ceilinpermitId > 0,
                ceilingpermit.TCeilingPermitDetail,
                IsShowPopUp = isShowPopUp,
                ApprovalStatus = ceilingpermit.Status,
                PermitId = ceilinpermitId,
                permitNumber
            };
            //return Json(result, JsonRequestBehavior.AllowGet);
            return ReturnJsonResult(result);
        }

        private int setPopupStatusCP(List<TCeilingPermitDetail> TCeilingPermitDetail)
        {
            var status = 0;
            if (TCeilingPermitDetail.All(x => x.RespondStatus))
                status = 1;
            else if (TCeilingPermitDetail.Any(x => x.RespondStatus))
                status = 2;
            return status;
        }

        private void SendCeilingPermitEmail(int ceilinpermitId, string buildingId, string IsRequestEdited)
        {
            var ceilinpermit = _permitService.GetCeilingPermit(ceilinpermitId);
            int CurrentSignSequence = ceilinpermit.TPermitWorkFlowDetails != null && ceilinpermit.TPermitWorkFlowDetails.Where(x => x.LabelSignId == 0 || x.LabelSignId == null).ToList().Count > 0 ? ceilinpermit.TPermitWorkFlowDetails.Where(x => x.LabelSignId == 0 || x.LabelSignId == null).OrderBy(x => x.Sequence).ToList().FirstOrDefault().FormSettingId : 0;

            var requesterName = ceilinpermit.TPermitWorkFlowDetails != null && ceilinpermit.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.HasValue && ceilinpermit.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.Value > 0 ? ceilinpermit.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().UserDetail.Name : string.Empty;
            var approverName = ceilinpermit.TPermitWorkFlowDetails != null && ceilinpermit.TPermitWorkFlowDetails.OrderByDescending(x => x.Id).ToList().FirstOrDefault().LabelValue.HasValue && ceilinpermit.TPermitWorkFlowDetails.OrderByDescending(x => x.Id).ToList().FirstOrDefault().LabelValue.Value > 0 ? ceilinpermit.TPermitWorkFlowDetails.OrderByDescending(x => x.Id).ToList().FirstOrDefault().UserDetail.Name : string.Empty;

            var objPermitEmailModel = ceilinpermit.GetPermitEmailModel(ceilinpermit.GetPermitType(), requesterName, ceilinpermit.RequestedDate,
                ceilinpermit.ApprovedDate, ceilinpermit.Status, approverName, IsRequestEdited, ceilinpermit.RequestedDept);
           
            //var objPermitEmailModel = new PermitEmailModel
            //{
            //    PermitType = BDO.Enums.PermitType.CeilingPermit,
            //    PermitNo = ceilinpermit.PermitNo ?? "",
            //    Requestor = requestorName, //ceilinpermit.TPermitWorkFlowDetails != null && ceilinpermit.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.HasValue && ceilinpermit.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.Value > 0 ? ceilinpermit.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().UserDetail.Name : string.Empty,
            //    //Requestor = (ceilinpermit.RequestedbyUser != null ? ceilinpermit.RequestedbyUser.Name : string.Empty),
            //    RequestedDate = ceilinpermit.RequestedDate,
            //    ApprovedDate = ceilinpermit.ApprovedDate,
            //    // Approver = (ceilinpermit.FinalInspectionByUser != null ? ceilinpermit.FinalInspectionByUser.Name : string.Empty),
            //    Approver = approverName, //ceilinpermit.TPermitWorkFlowDetails != null && ceilinpermit.TPermitWorkFlowDetails.OrderByDescending(x => x.Id).ToList().FirstOrDefault().LabelValue.HasValue && ceilinpermit.TPermitWorkFlowDetails.OrderByDescending(x => x.Id).ToList().FirstOrDefault().LabelValue.Value > 0 ? ceilinpermit.TPermitWorkFlowDetails.OrderByDescending(x => x.Id).ToList().FirstOrDefault().UserDetail.Name : string.Empty,
            //    ApprovalStatus = ceilinpermit.Status,
            //    Reason = ceilinpermit.Reason,
            //    RequesterEmail = ceilinpermit.Email,
            //    Company = ceilinpermit.RequestedDept,
            //    IsRequestEdited = IsRequestEdited,
            //    ProjectName = ceilinpermit.Project != null ? ceilinpermit.Project.ProjectName : string.Empty,
            //    ProjectNo = ceilinpermit.Project != null ? ceilinpermit.Project.ProjectNumber : string.Empty,
            //    EventId = ceilinpermit.Status == 1 ? Convert.ToInt32(BDO.Enums.NotificationEvent.OnApproved) : Convert.ToInt32(BDO.Enums.NotificationEvent.OnSubmit)

            //};
            if (objPermitEmailModel != null && CurrentSignSequence > 0)
            {
                objPermitEmailModel.AdditionalNextLevelToEmails = ceilinpermit.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null ? ceilinpermit.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.NotificationEmail : string.Empty;
                objPermitEmailModel.AdditionalNextLevelCCEmails = ceilinpermit.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null ? ceilinpermit.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.NotificationCCEmail : string.Empty;
                objPermitEmailModel.SendMailToNextPhase = ceilinpermit.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null ? ceilinpermit.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.IsSendMail : false;

            }
            var base64EncodePdf = _pdfService.CeilingPermitInbytes(ceilinpermitId, true);
            byte[] fileBytes = Convert.FromBase64String(base64EncodePdf);
            var filename = $"{ceilinpermit.PermitNo}{".pdf"}";
            objPermitEmailModel.PermitUrl = "ceiling/permit/update/" + ceilinpermit.CeilingPermitId.ToString();
            _emailService.SendPermitCreatedMail(objPermitEmailModel, Convert.ToInt32(BDO.Enums.NotificationCategory.CP), fileBytes, filename, buildingId, string.Empty, string.Empty);
        }

        public JsonResult DeleteCeilingPermitFiles(int ceilingPermitId, string fileIds)
        {
            var result = _permitService.DeleteCeilingPermitFiles(ceilingPermitId, fileIds);
            return ReturnJsonResult(result);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteCeilingDrawings(int ceilingPermitId, string fileIds)
        {
            var result = _permitService.DeleteCeilingDrawings(ceilingPermitId, fileIds);
            return ReturnJsonResult(result);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Life Safety Devices Forms

        // [CrxAuthorize(Roles = "Permit_LifeSafetyDevicesForms")]
        public ActionResult LifeSafetyDevicesForms()
        {
            var currentUser = UserSession.CurrentUser;
            UISession.AddCurrentPage("Permit_LifeSafetyDevicesForms", 0, "Life Safety Devices Forms");
            var data = _permitService.LifeSafetyDevicesForms();

            if (currentUser.IsVendorUser)
                data = data.Where(x => x.Requestor == currentUser.UserId).ToList();
            else
                data = data.Where(x => x.Status != Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission) || x.CreatedBy == currentUser.UserId).ToList();


            return View(data);
        }

        public ActionResult AddLifeSafetyDevicesFormsPermit(string sformType, string formId, int? mopid)
        {
            var currentUser = UserSession.CurrentUser;
            if (string.IsNullOrEmpty(sformType) && string.IsNullOrEmpty(formId))
                return RedirectToAction("LifeSafetyDevicesForms");
            if(formId=="-1")
            {
                formId = string.Empty;
            }
            UISession.AddCurrentPage("Permit_AddLifeSafetyDevicesFormsPermit", 0, "Add Life Safety Devices Forms");
            TLifeSafetyDevicesForms newForm = new TLifeSafetyDevicesForms();

            bool isUserAuthorized = true;
            ViewBag.IsRequestEdited = "";
            if (!string.IsNullOrEmpty(formId))
            {
                ViewBag.ShowIncomplete = 0;

                //Guid guidResult = Guid.Parse(formId);
                bool isValid = Guid.TryParse(formId, out _);
                if (isValid)
                {
                    newForm = _permitService.GetLifeSafetyForm(Guid.Parse(formId));
                    if (newForm != null && !string.IsNullOrEmpty(Convert.ToString(newForm.LsdFormId)))
                    {
   
                        if (currentUser.IsVendorUser)
                            isUserAuthorized = newForm.Requestor != null && newForm.Requestor == currentUser.UserId;
                    }
                    else
                    {
                        ErrorMessage = ConstMessage.Not_exist;
                        return RedirectToAction("LifeSafetyDevicesForms");
                    }
                    if (newForm.Status < 0 && currentUser.IsVendorUser)
                    {
                        //newForm.Status = Convert.ToInt32(Enums.ApprovalStatus.Requested);
                        ViewBag.ShowIncomplete = 1;
                    }
                    if (newForm.Status == 2)
                    {
                        ViewBag.IsRequestEdited = "1";
                    }

                    newForm.ApprovedBy = (newForm.ApprovedBy == null && _commonModelFactory.IsAdminUser()) ? currentUser.UserId : newForm.ApprovedBy;
                }
                else
                {
                    ErrorMessage = ConstMessage.Not_exist;
                    return RedirectToAction("LifeSafetyDevicesForms");

                }
            }
            else
            {
                // set default value for first time
                newForm.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);
                newForm.FormType = (sformType == "addition") ? 1 : 0;
                newForm.DateOfRequest = DateTime.Now.ToClientTime();

                if (currentUser != null)
                {
                    newForm.Requestor = currentUser.UserId;
                    newForm.EmailAddress = currentUser.Email;
                    if (currentUser.IsVendorUser)
                        newForm.Contractor = currentUser.Vendor.Name;
                    else
                        newForm.Contractor = UserSession.CurrentOrg.Name;


                    if (!string.IsNullOrEmpty(currentUser.PhoneNumber))
                        newForm.PhoneNumber = currentUser.PhoneNumber;
                    else if (!string.IsNullOrEmpty(currentUser.CellNo))
                        newForm.PhoneNumber = currentUser.CellNo;
                }
            }
            newForm.Contractor = !string.IsNullOrEmpty(newForm.Contractor) ? newForm.Contractor : string.Empty;
            if (newForm.LifeSafetyDeviceList.Count == 0)
            {
                LifeSafetyDeviceList device = new LifeSafetyDeviceList();
                newForm.LifeSafetyDeviceList.Add(device);
            }

            if (mopid > 0)
            {
                TempData["MopId"] = mopid;
            }
            TempData.Keep();
            if (newForm != null)
            {
                newForm.TIcraProject = new TIcraProject();
                if (newForm.ProjectId != 0)
                {
                    newForm.TIcraProject = _icraProjectService.GetCountAllActiveTIcraProject(currentUser.UserId, currentUser.VendorId != null ? currentUser.VendorId : null, false).FirstOrDefault(x => x.ProjectId == newForm.ProjectId);
                    newForm.TIcraProject.mode = "View";

                }
            }
           
            if (isUserAuthorized)
                return View(newForm);
            else
            {
                ErrorMessage = ConstMessage.NotAuthorized_Msg;
                return RedirectToAction("LifeSafetyDevicesForms");
            }
            //return View(newForm);
        }

        public ActionResult lifesafetybuildingddl(string type, string buildingId = null, string sequence = null)
        {
            List<Buildings> newist = _buildingsService.GetBuildings().ToList();
            List<Buildings> buildings = new List<Buildings>();
            if (!string.IsNullOrEmpty(buildingId))
            {
                string[] buildingval = buildingId.Split(',');
                buildings = newist.Where(t => buildingval.Contains(Convert.ToString(t.BuildingId))).ToList();
                //string[] buildingval = buildingId.Split(',');
                //foreach (var buildingitem in newist)
                //{
                //    if (buildingval.Length > 0)
                //    {
                //        foreach (string building in buildingval)
                //        {
                //            if (!string.IsNullOrEmpty(building))
                //            {
                //                if (buildingitem.BuildingId == Convert.ToInt32(building))
                //                {
                //                    buildings.Add(buildingitem);
                //                }
                //            }
                //        }
                //    }
                //}
            }
            else
            {
                buildings = newist;
            }
            ViewBag.type = type;
            ViewBag.sequence = sequence != null ? sequence : string.Empty;
            return PartialView("~/Views/Permit/_lifesafetybuildingddl.cshtml", buildings);
        }

        public ActionResult assetdevicechangebuildingddl(string type, string buildingId = null, string sequence = null)
        {
            return RedirectToAction(nameof(PermitController.lifesafetybuildingddl), new { type = type, buildingId = buildingId, sequence = sequence });
            //List<Buildings> newist = _buildingsService.GetBuildings().ToList();
            //List<Buildings> buildings = new List<Buildings>();
            //if (!string.IsNullOrEmpty(buildingId))
            //{

            //    string[] buildingval = buildingId.Split(',');
            //    foreach (var buildingitem in newist)
            //    {
            //        if (buildingval.Length > 0)
            //        {
            //            foreach (string building in buildingval)
            //            {
            //                if (!string.IsNullOrEmpty(building))
            //                {
            //                    if (buildingitem.BuildingId == Convert.ToInt32(building))
            //                    {
            //                        buildings.Add(buildingitem);
            //                    }

            //                }

            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    buildings = newist;
            //}
            //ViewBag.type = type;
            //ViewBag.sequence = sequence != null ? sequence : string.Empty;
            //return PartialView("~/Views/Permit/_lifesafetybuildingddl.cshtml", buildings);
        }


        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult AddLifeSafetyDevicesFormsPermit(TLifeSafetyDevicesForms objData, string submit, string lstBuildingDetails, string lstFloorDetails, string IsRequestEdited)
        {
            int submitValue = getSubmitButtonValues(submit);
            objData.CreatedBy = UserSession.CurrentUser.UserId;
            if (!string.IsNullOrEmpty(lstBuildingDetails))
            {
                var buildings = JsonConvert.DeserializeObject<List<Buildings>>(lstBuildingDetails);
                objData.BuildingId = string.Join(",", buildings.Select(x => x.BuildingId));
                objData.BuildingName = string.Join(",", buildings.Select(x => x.BuildingName));
            }
            if (submitValue < 0)
                objData.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission);
            else if (submitValue > 0 && objData.Status == -1)
                objData.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);

            List<Int32> tfileIds = new List<int>();
            if (!string.IsNullOrEmpty(objData.AttachFiles))
            {
                foreach (var item in objData.AttachFiles.Split(','))
                {
                    int fileId = Convert.ToInt32(item.Split('_')[0]);
                    tfileIds.Add(fileId);
                }
                if (!string.IsNullOrEmpty(objData.TFileIds))
                {
                    string file = string.Join(",", tfileIds.Select(n => n.ToString()).ToArray());
                    objData.TFileIds = objData.TFileIds + ',' + file;
                }
                else
                {
                    objData.TFileIds = string.Join(",", tfileIds.Select(n => n.ToString()).ToArray());
                }

            }
            objData.LsdFormId = Guid.NewGuid();
            int tlsdFormno;
            tlsdFormno = objData.LsdFormNo;
            objData = _permitService.AddLifeSafetyDevices(objData);
            if (objData.LsdFormNo > 0)
            {

                //foreach (var item in objData.LifeSafetyDeviceList.Where(x => !string.IsNullOrEmpty(x.DeviceNumber)))
                foreach (var item in objData.LifeSafetyDeviceList)
                {
                    item.LsdFormId = objData.LsdFormId;
                    _permitService.AddLifeSafetyDevice(item);
                }
                string PermitNumber;
                PermitNumber = _permitService.GetLifeSafetyForm(objData.LsdFormId).PermitNo;
                if (tlsdFormno > 0)
                {
                    //if (objData.Status != 2)
                    //{
                    //    if (submitValue > 0)
                    //        SendLSDPermitEmail(objData.LsdFormId, objData.BuildingId);
                    //}
                    SuccessMessage = "Permit# " + PermitNumber + " " + ConstMessage.Success_Updated;

                }
                else if (objData.LsdFormNo > 0 && tlsdFormno == 0)
                {
                    //if (_commonService.IsSendMail(Enums.NotificationCategory.LSDAR.ToString(), Enums.NotificationEvent.OnSubmit.ToString()))
                    //{
                    //    if (submitValue > 0)
                    //        SendLSDPermitEmail(objData.LsdFormId, objData.BuildingId);
                    //}
                    //else if (objData.Status != 2)
                    //{
                    //    if (submitValue > 0)
                    //        SendLSDPermitEmail(objData.LsdFormId, objData.BuildingId);
                    //}
                    SuccessMessage = "Permit # " + PermitNumber + " " + ConstMessage.Success;

                }
                else
                    ErrorMessage = ConstMessage.Error;
                if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.LSDAR.ToString(), BDO.Enums.NotificationEvent.OnSubmit.ToString()))
                {
                    if (submitValue > 0)
                        SendLSDPermitEmail(objData.LsdFormId, objData.BuildingId, IsRequestEdited);
                }
                else if (objData.Status != 2)
                {
                    if (submitValue > 0)
                        SendLSDPermitEmail(objData.LsdFormId, objData.BuildingId, IsRequestEdited);
                }

                var formId = (objData.FormType == 1) ? 4 : 5;
                if (TempData["MopId"] != null)
                {
                    SetRedirectForm(Convert.ToInt32(TempData["MopId"]), formId, objData.LsdFormId.ToString(), PermitNumber);
                }
                return RedirectToAction("LifeSafetyDevicesForms");
            }
            return View(objData);
        }
        //mail
        private void SendLSDPermitEmail(Guid lsdFormId, string buildingId, string IsRequestEdited)
        {
            var newForm = _permitService.GetLifeSafetyForm(lsdFormId);
            var base64EncodePdf = _pdfService.LifeSafetyFormInbytes(lsdFormId.ToString());
            byte[] filebytes = Convert.FromBase64String(base64EncodePdf);
            string permitType = $"{"Life Safety Device"} - {(newForm.FormType == 1 ? "Addition Form" : "Removal Form")}";
            var filename = $"{newForm.PermitNo}{(newForm.FormType == 1 ? "_A" : "_R")}{".pdf"}";
            var objPermitEmailModel = new PermitEmailModel
            {
                //PermitType = permitType,
                PermitType = (newForm.FormType == 1) ? BDO.Enums.PermitType.LsdAdditionForm : BDO.Enums.PermitType.LsdRemovalForm,
                PermitNo = newForm.PermitNo ?? "",
                ProjectName = newForm.ProjectName,
                ProjectNo = newForm.Project.ProjectNumber,
                Requestor = (newForm.RequestorUser != null ? newForm.RequestorUser.Name : string.Empty),
                RequestedDate = newForm.DateOfRequest,
                ApprovedDate = newForm.SignDate,
                Approver = newForm.ApprovedByUser.Name,
                ApprovalStatus = newForm.Status,
                Reason = newForm.Reason,
                RequesterEmail = newForm.EmailAddress,
                Company = newForm.Contractor,
                IsRequestEdited = IsRequestEdited,
                EventId = newForm.Status == 1 ? Convert.ToInt32(BDO.Enums.NotificationEvent.OnApproved) : Convert.ToInt32(BDO.Enums.NotificationEvent.OnSubmit)
            };
            objPermitEmailModel.PermitUrl = "life-safety-device/addition/form/" + newForm.LsdFormId.ToString();
            _emailService.SendPermitCreatedMail(objPermitEmailModel, Convert.ToInt32(BDO.Enums.NotificationCategory.LSDAR), filebytes, filename, buildingId, string.Empty, string.Empty);
        }

        public ActionResult BindDeviceList(int sequence, int type, string BuildingId)
        {
            LifeSafetyDeviceList device = new LifeSafetyDeviceList();
            ViewData["type"] = type;
            ViewData["sequence"] = sequence;
            ViewData["BuildingId"] = "";
            ViewData["DeviceType"] = "";
            if (!string.IsNullOrEmpty(BuildingId))
            {
                ViewData["BuildingId"] = BuildingId;
            }
            ViewBag.sequence = sequence;

            return PartialView("_LifeSafetyDevicesForms", device);
        }
        public JsonResult GetLSDBuilding(int BuildingId)
        {
            var result = _buildingsService.GetBuildings().Where(x => x.BuildingId == BuildingId);
            return ReturnJsonResult(result);
            // return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteLSDDrawings(int LsdFormNo, string fileIds)
        {
            var result = _permitService.DeleteLSDDrawings(LsdFormNo, fileIds);
            return ReturnJsonResult(result);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteLSDFiles(int LsdFormNo, string fileIds)
        {
            var result = _permitService.DeleteLSDDrawings(LsdFormNo, fileIds);
            return ReturnJsonResult(result);
            // return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadLSDAttachments(string formId)
        {

            //var mainRoot = "temp";
            TLifeSafetyDevicesForms newForm = _permitService.GetLifeSafetyForm(Guid.Parse(formId));
            var fileName = Guid.NewGuid() + ".zip";
            var outpathfilename = $"{newForm.PermitNo}{(newForm.FormType == 1 ? "_A" : "_R")}{".zip"}";
            var tempOutPutPath = _commonModelFactory.ServerMapPath("temp" + fileName);
            DirectoryService.CreateDirectory(_commonModelFactory.ServerMapPath("temp"));
            try
            {

                using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
                {
                    s.SetLevel(9); // 0-9, 9 being the highest compression  

                    byte[] buffer = new byte[2021515];

                    MemoryStream outputMemStream = new MemoryStream();
                    ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

                    zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
                    var base64EncodePdf = _pdfService.LifeSafetyFormInbytes(newForm.LsdFormId.ToString());
                    byte[] fileBytes = Convert.FromBase64String(base64EncodePdf);
                    var filename = $"{newForm.PermitNo}{(newForm.FormType == 1 ? "_A" : "_R")}{".pdf"}";
                    var entry = new ZipEntry(filename)
                    {
                        DateTime = DateTime.Now
                    };

                    s.PutNextEntry(entry);
                    MemoryStream inStream = new MemoryStream(fileBytes);
                    StreamUtils.Copy(inStream, s, new byte[2021515]);
                    inStream.Close();
                    for (int i = 0; i < newForm.Attachments.Count; i++)
                    {
                        entry = new ZipEntry(Path.GetFileName(newForm.Attachments[i].FileName))
                        {
                            DateTime = DateTime.Now,
                            IsUnicodeText = true
                        };
                        s.PutNextEntry(entry);
                        var req = System.Net.WebRequest.Create(new Uri(_commonModelFactory.FilePath(newForm.Attachments[i].FilePath)));
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
                    s.Finish();
                    s.Flush();
                    s.Close();

                }
                byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
                if (System.IO.File.Exists(tempOutPutPath))
                    System.IO.File.Delete(tempOutPutPath);

                return File(finalResult, "application/zip", outpathfilename);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new EmptyResult();
            }

        }
        #endregion

        #region Fire System Bypass Permit 

        // [CrxAuthorize(Roles = "Permit_FSBPermit")]
        public ActionResult FSBPermit()
        {
            UISession.AddCurrentPage("Permit_FSBPermit", 0, "Fire System Bypass Permit");
            var result = _permitService.GetAllFSBPermit();
            if (UserSession.CurrentUser.IsVendorUser)
                result = result.Where(x => x.RequestorBy == UserSession.CurrentUser.UserId).ToList();
            else
                result = result.Where(x => x.ApprovalStatus != Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission) || x.CreatedBy == UserSession.CurrentUser.UserId).ToList();
            return View(result);
        }

        [HttpGet]
        public ActionResult AddFSBPermit(int? tfsbPermitId, int? mopid)
        {
            var currentUser = UserSession.CurrentUser;
            bool isUserAuthorized = true;
            UISession.AddCurrentPage("Permit_AddFSBPermit", 0, "Add Fire System Bypass Permit");
            TFireSystemByPassPermit objTFireSystemByPassPermit;
            objTFireSystemByPassPermit = _permitService.GetFSBPermit(tfsbPermitId);
            ViewBag.ShowIncomplete = 0;
            ViewBag.IsRequestEdited = "";
            if (tfsbPermitId.HasValue && tfsbPermitId.Value > 0)
            {
                if (objTFireSystemByPassPermit != null && objTFireSystemByPassPermit.TFSBPermitId > 0)
                {
                    if (currentUser.IsVendorUser)
                        isUserAuthorized = objTFireSystemByPassPermit.CreatedBy == currentUser.UserId;
                }
                else
                {
                    ErrorMessage = ConstMessage.Not_exist;
                    return RedirectToAction("FSBPermit");
                }


                if (objTFireSystemByPassPermit.ApprovalStatus < 0 && currentUser.IsVendorUser)
                {
                    //objTFireSystemByPassPermit.ApprovalStatus = Convert.ToInt32(Enums.ApprovalStatus.Requested);
                    ViewBag.ShowIncomplete = 1;

                }
                if (objTFireSystemByPassPermit.ApprovalStatus == 2)
                {
                    ViewBag.IsRequestEdited = "1";
                }

                objTFireSystemByPassPermit.ApprovedBy = (objTFireSystemByPassPermit.ApprovedBy == null && _commonModelFactory.IsAdminUser()) ? UserSession.CurrentUser.UserId : objTFireSystemByPassPermit.ApprovedBy;
            }
            else
            {
                objTFireSystemByPassPermit.ApprovalStatus = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);
                objTFireSystemByPassPermit.RequestedDate = DateTime.Now.ToClientTime();

                if (currentUser != null)
                {
                    objTFireSystemByPassPermit.RequestorBy = currentUser.UserId;
                    objTFireSystemByPassPermit.Email = currentUser.Email;

                    if (currentUser.IsVendorUser)
                        objTFireSystemByPassPermit.Company = currentUser.Vendor.Name;
                    else
                        objTFireSystemByPassPermit.Company = UserSession.CurrentOrg.Name;

                    if (!string.IsNullOrEmpty(currentUser.PhoneNumber))
                        objTFireSystemByPassPermit.PhoneNo = currentUser.PhoneNumber;
                    else if (!string.IsNullOrEmpty(currentUser.CellNo))
                        objTFireSystemByPassPermit.PhoneNo = currentUser.CellNo;
                }
            }

            objTFireSystemByPassPermit.Company = !string.IsNullOrEmpty(objTFireSystemByPassPermit.Company) ? objTFireSystemByPassPermit.Company : string.Empty;
            if (mopid > 0)
            {
                TempData["MopId"] = mopid;
            }
            TempData.Keep();
            if (objTFireSystemByPassPermit != null && objTFireSystemByPassPermit.TFSBPermitId > 0)
            {
                objTFireSystemByPassPermit.TIcraProject = new TIcraProject();
                if (objTFireSystemByPassPermit.ProjectId != 0)
                {

                    objTFireSystemByPassPermit.TIcraProject = _icraProjectService.GetCountAllActiveTIcraProject(currentUser.UserId, currentUser.VendorId != null ? currentUser.VendorId : null, false).Where(x => x.ProjectId == objTFireSystemByPassPermit.ProjectId).FirstOrDefault();
                    objTFireSystemByPassPermit.TIcraProject.mode = "View";
                }
            }

            if (isUserAuthorized)
                return View(objTFireSystemByPassPermit);
            else
            {
                ErrorMessage = ConstMessage.NotAuthorized_Msg;
                return RedirectToAction("FSBPermit");
            }
        }

        public ActionResult FSBPBuildingDDL(bool isMultiple, string selectdValue)
        {
            var selectPicker = new MultiSelectPicker { IsMultiple = isMultiple, SelectedValue = selectdValue };
            return PartialView("_BuildingMultiSingle", selectPicker);
        }
        public ActionResult GetBuildingDetails(string Id)
        {
            List<Buildings> lstbuilding = new List<Buildings>();
            List<Buildings> buildinglst = _buildingsService.GetBuildings();
            if (!string.IsNullOrEmpty(Id))
            {
                foreach (string buildingid in Id.Split(','))
                {
                    var building = buildinglst.Where(x => x.BuildingId == Convert.ToInt32(buildingid)).FirstOrDefault();
                    if (building != null)
                    {
                        lstbuilding.Add(building);
                    }
                }
            }


            // return Json(lstbuilding, JsonRequestBehavior.AllowGet);
            return ReturnJsonResult(lstbuilding);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public JsonResult AddFSBPermit(TFireSystemByPassPermit objTFireSystemByPassPermit, string lstTfsbpBuildingDetails, string btnSubmit, string IsRequestEdited)
        {
            var currentUser = UserSession.CurrentUser;
            var tfsbPermitId = 0;
            var isShowPopUp = 0;
            string buildingIds = "";
            string permitNumber = "";
            try
            {
                int submitValue = getSubmitButtonValues(btnSubmit);
                objTFireSystemByPassPermit.CreatedBy = currentUser.UserId;
                if (submitValue < 0)
                    objTFireSystemByPassPermit.ApprovalStatus = Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission);
                else if (submitValue > 0 && objTFireSystemByPassPermit.ApprovalStatus == -1)
                    objTFireSystemByPassPermit.ApprovalStatus = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);

                if (!string.IsNullOrEmpty(lstTfsbpBuildingDetails))
                {
                    //
                    var tfsbpBuildingDetails = JsonConvert.DeserializeObject<List<TFSBPBuildingDetails>>(lstTfsbpBuildingDetails);
                    if (tfsbpBuildingDetails.Count > 0)
                    {
                        foreach (var item in tfsbpBuildingDetails)
                        {
                            var data = objTFireSystemByPassPermit.TFSBPBuildingDetails.Where(x => x.BuildingId == item.BuildingId).FirstOrDefault();
                            if(data!=null)
                            item.TFireSystemId = data.fireSystemTypes != null ? string.Join(",", data.fireSystemTypes.Where(x => x.CheckVal == 1).Select(x => x.ID.ToString())) : string.Empty;
                        }
                        objTFireSystemByPassPermit.TFSBPBuildingDetails = tfsbpBuildingDetails;
                    }
                    buildingIds = string.Join(",", tfsbpBuildingDetails.Select(x => x.BuildingId));
                }
                if (!string.IsNullOrEmpty(objTFireSystemByPassPermit.Stime))
                {
                    var slot = DateTime.ParseExact(objTFireSystemByPassPermit.Stime, "hh:mm tt", CultureInfo.InvariantCulture);
                    objTFireSystemByPassPermit.StartTime = slot.TimeOfDay;
                }
                if (!string.IsNullOrEmpty(objTFireSystemByPassPermit.Etime))
                {
                    var slot = DateTime.ParseExact(objTFireSystemByPassPermit.Etime, "hh:mm tt", CultureInfo.InvariantCulture);
                    objTFireSystemByPassPermit.EndTime = slot.TimeOfDay;
                }
                if (!string.IsNullOrEmpty(objTFireSystemByPassPermit.ByEngActionTime))
                {
                    var slot = DateTime.ParseExact(objTFireSystemByPassPermit.ByEngActionTime, "hh:mm tt", CultureInfo.InvariantCulture);
                    objTFireSystemByPassPermit.BypassEngActionTime = slot.TimeOfDay;
                }
                if (!string.IsNullOrEmpty(objTFireSystemByPassPermit.ByReturnEngActionTime))
                {
                    var slot = DateTime.ParseExact(objTFireSystemByPassPermit.ByReturnEngActionTime, "hh:mm tt", CultureInfo.InvariantCulture);
                    objTFireSystemByPassPermit.BypassReturnEngActionTime = slot.TimeOfDay;
                }
                List<Int32> tfileIds = new List<int>();
                if (!string.IsNullOrEmpty(objTFireSystemByPassPermit.AttachFiles))
                {
                    foreach (var item in objTFireSystemByPassPermit.AttachFiles.Split(','))
                    {
                        int fileId = Convert.ToInt32(item.Split('_')[0]);
                        tfileIds.Add(fileId);
                    }

                    if (!string.IsNullOrEmpty(objTFireSystemByPassPermit.TFileIds))
                    {
                        string file;
                        file = string.Join(",", tfileIds.Select(n => n.ToString()).ToArray());
                        objTFireSystemByPassPermit.TFileIds = objTFireSystemByPassPermit.TFileIds + ',' + file;
                    }
                    else
                    {
                        objTFireSystemByPassPermit.TFileIds = string.Join(",", tfileIds.Select(n => n.ToString()).ToArray());
                    }

                }
                tfsbPermitId = _permitService.InsertFSBPermit(objTFireSystemByPassPermit);

                if (tfsbPermitId > 0)
                {
                    if (objTFireSystemByPassPermit.TFSBPBuildingDetails != null)
                    {
                        foreach (var item in objTFireSystemByPassPermit.TFSBPBuildingDetails)
                        {
                            item.Status = true;
                            item.CreatedBy = currentUser.UserId;
                            item.TFSBPermitId = tfsbPermitId;
                            _permitService.InsertFSBPBuildingDetails(item);
                        }
                    }
                    isShowPopUp = setPopupStatus(objTFireSystemByPassPermit.ILSMRequiredChecklist);
                    foreach (var item in objTFireSystemByPassPermit.ILSMRequiredChecklist)
                    {
                        item.CreatedBy = currentUser.UserId;
                        item.TFSBPermitId = tfsbPermitId;
                        _permitService.InsertFSBPermitDetails(item);
                    }
                    foreach (var item in objTFireSystemByPassPermit.BypassEngineeringActions)
                    {
                        item.CreatedBy = currentUser.UserId;
                        item.TFSBPermitId = tfsbPermitId;
                        _permitService.InsertFSBPermitDetails(item);
                    }
                    foreach (var item in objTFireSystemByPassPermit.BypassReturnEngineeringActions)
                    {
                        item.CreatedBy = currentUser.UserId;
                        item.TFSBPermitId = tfsbPermitId;
                        _permitService.InsertFSBPermitDetails(item);
                    }

                    permitNumber = _permitService.GetFSBPermit(tfsbPermitId).PermitNo;

                    if (objTFireSystemByPassPermit.TFSBPermitId > 0)
                    {

                        SuccessMessage = $"{"Permit#  "} {permitNumber} {ConstMessage.Success_Updated}";
                    }
                    else if (tfsbPermitId > 0 && objTFireSystemByPassPermit.TFSBPermitId == 0)
                    {
                        SuccessMessage = $"{"Permit# "} {permitNumber} {ConstMessage.Success}";
                    }
                    else
                        ErrorMessage = ConstMessage.Error;

                    if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.FSBP.ToString(), BDO.Enums.NotificationEvent.OnSubmit.ToString()))
                    {
                        if (submitValue > 0)
                            SendFSBPermitEmail(tfsbPermitId, buildingIds, IsRequestEdited);
                    }
                    else if (objTFireSystemByPassPermit.ApprovalStatus != 2)
                    {
                        if (submitValue > 0)
                            SendFSBPermitEmail(tfsbPermitId, buildingIds, IsRequestEdited);
                    }
                    if (TempData["MopId"] != null)
                    {
                        SetRedirectForm(Convert.ToInt32(TempData["MopId"]), 2, tfsbPermitId.ToString(), permitNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            var result = new
            {
                Result = tfsbPermitId > 0,
                objTFireSystemByPassPermit.ILSMRequiredChecklist,
                IsShowPopUp = isShowPopUp,
                permitNumber
            };
            return ReturnJsonResult(result);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFsbBuilding(int id, int sequence)
        {
            var buildings = _permitService.GetAllBuildingFiresystem().ToList();
            TFSBPBuildingDetails objTFireSystemByPassPermit = buildings.FirstOrDefault(x => x.BuildingId == id);
            if (sequence == -1)
            {
                ViewData["sequence"] = 0;
            }
            else
            {
                ViewData["sequence"] = sequence;
            }

            return PartialView("_FSBPFireSystemtype", objTFireSystemByPassPermit);
        }
        private int getSubmitButtonValues(string submit)
        {
            if (!string.IsNullOrEmpty(submit) && submit.ToLower() != "save incomplete")
                return 1;
            else
                return -1;
        }

        private int setPopupStatus(List<TFSBPermitDetail> iLsmRequiredChecklist)
        {
            var status = 0;
            if (iLsmRequiredChecklist.All(x => x.RespondStatus))
                status = 1;
            else if (iLsmRequiredChecklist.Any(x => x.RespondStatus))
                status = 2;
            return status;
        }
        public JsonResult DeleteTFSBPFiles(int TFSBPermitId, string fileIds)
        {
            var result = _permitService.DeleteFSBPFiles(TFSBPermitId, fileIds);
            return ReturnJsonResult(result);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }
        private void SendFSBPermitEmail(int tfsbPermitId, string buildingId, string IsRequestEdited)
        {

            var fsbpermit = _permitService.GetFSBPermit(tfsbPermitId);
            var requesterName = (fsbpermit.RequestorByUser != null ? fsbpermit.RequestorByUser.Name : string.Empty);
            var approverName = (fsbpermit.ApprovedByUser != null ? fsbpermit.ApprovedByUser.Name : string.Empty);

            var objPermitEmailModel = fsbpermit.GetPermitEmailModel(fsbpermit.GetPermitType(), requesterName, fsbpermit.RequestedDate,
                fsbpermit.ApprovedDate, fsbpermit.ApprovalStatus, approverName, IsRequestEdited, fsbpermit.Company);

            //var objPermitEmailModel = new PermitEmailModel
            //{
            //    //PermitType = $"Fire System Bypass Permit [FSBP]",
            //    PermitType = BDO.Enums.PermitType.FireSystemBypassPermit,
            //    PermitNo = fsbpermit.PermitNo ?? "",
            //    ProjectName = fsbpermit.ProjectName,
            //    ProjectNo = fsbpermit.Project.ProjectNumber,
            //    Requestor = (fsbpermit.RequestorByUser != null ? fsbpermit.RequestorByUser.Name : string.Empty),
            //    RequestedDate = fsbpermit.RequestedDate,
            //    ApprovedDate = fsbpermit.ApprovedDate,
            //    Approver = (fsbpermit.ApprovedByUser != null ? fsbpermit.ApprovedByUser.Name : string.Empty),
            //    ApprovalStatus = fsbpermit.ApprovalStatus,
            //    Reason = fsbpermit.Reason,
            //    RequesterEmail = fsbpermit.Email,
            //    Company = fsbpermit.Company,
            //    IsRequestEdited = IsRequestEdited,
            //    EventId = fsbpermit.ApprovalStatus==1? Convert.ToInt32(BDO.Enums.NotificationEvent.OnApproved) :Convert.ToInt32(BDO.Enums.NotificationEvent.OnSubmit)
            //};
            var base64EncodePdf = _pdfService.FireSystemByPassPermitInbytes(tfsbPermitId);
            byte[] filebytes = Convert.FromBase64String(base64EncodePdf);
            var filename = $"{fsbpermit.PermitNo}{".pdf"}";
            objPermitEmailModel.PermitUrl = "permit/addfsbpermit?tfsbPermitId=" + fsbpermit.TFSBPermitId;
            _emailService.SendPermitCreatedMail(objPermitEmailModel, Convert.ToInt32(BDO.Enums.NotificationCategory.FSBP), filebytes, filename, buildingId, string.Empty, string.Empty);
        }
        public JsonResult DeleteFSBPDrawings(int TFSBPermitId, string fileIds)
        {
            var result = _permitService.DeleteFSBPDrawings(TFSBPermitId, fileIds);
            return ReturnJsonResult(result);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DownloadFSBPAttachments(int tfsbPermitId)
        {

            var mainRoot = "temp";
            TFireSystemByPassPermit objTFireSystemByPassPermit;
            objTFireSystemByPassPermit = _permitService.GetFSBPermit(tfsbPermitId);
            var fileName = Guid.NewGuid() + ".zip";
            var outpathfilename = $"{objTFireSystemByPassPermit.PermitNo}{".zip"}";
            var tempOutPutPath = _commonModelFactory.ServerMapPath("temp" + fileName);
            DirectoryService.CreateDirectory(mainRoot);
            try
            {

                using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
                {
                    s.SetLevel(9); // 0-9, 9 being the highest compression  

                    byte[] buffer = new byte[2021515];

                    MemoryStream outputMemStream = new MemoryStream();
                    ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

                    zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
                                           // byte[] bytes = null;
                    var base64EncodePdf = _pdfService.FireSystemByPassPermitInbytes(tfsbPermitId);
                    byte[] fileBytes = Convert.FromBase64String(base64EncodePdf);
                    var filename = $"{objTFireSystemByPassPermit.PermitNo}{".pdf"}";
                    var entry = new ZipEntry(filename)
                    {
                        DateTime = DateTime.Now
                    };

                    s.PutNextEntry(entry);
                    MemoryStream inStream = new MemoryStream(fileBytes);
                    StreamUtils.Copy(inStream, s, new byte[2021515]);
                    inStream.Close();
                    for (int i = 0; i < objTFireSystemByPassPermit.Attachments.Count; i++)
                    {
                        entry = new ZipEntry(Path.GetFileName(objTFireSystemByPassPermit.Attachments[i].FileName))
                        {
                            DateTime = DateTime.Now,
                            IsUnicodeText = true
                        };
                        s.PutNextEntry(entry);
                        var req = System.Net.WebRequest.Create(new Uri(_commonModelFactory.FilePath(objTFireSystemByPassPermit.Attachments[i].FilePath)));
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
                    s.Finish();
                    s.Flush();
                    s.Close();

                }
                byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
                if (System.IO.File.Exists(tempOutPutPath))
                    System.IO.File.Delete(tempOutPutPath);

                return File(finalResult, "application/zip", outpathfilename);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new EmptyResult();
            }

        }
        #endregion

        #region Method of Procedure (MOP)  

        [CrxAuthorize(Roles = "method_of_procedure")]
        public ActionResult MethodofProcedure()
        {
            var currentUser = UserSession.CurrentUser;
            UISession.AddCurrentPage("Permit_MethodofProcedure", 0, "Method of Procedure");
            var result = _permitService.GetMethodofProcedure();

            if (currentUser.IsVendorUser)
                result = result.Where(x => x.CreatedBy == currentUser.UserId).ToList();
            else
                result = result.Where(x => x.Status != Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission) || x.CreatedBy == currentUser.UserId).ToList();
            return View(result);
        }

        public ActionResult AddMethodofProcedure(int? id)
        {
            var currentUser = UserSession.CurrentUser;
            bool isUserAuthorized = true;
            ViewBag.ShowIncomplete = 0;
            UISession.AddCurrentPage("Permit_AddMethodofProcedure", 0, "Add Method of Procedure");
            TMOP mop = new TMOP();
            ViewBag.IsRequestEdited = "";
            if (id > 0)
            {
                mop = _permitService.GetMethodofProcedure(id.Value);
                if (mop != null && mop.TmopId > 0)
                {
                    if (currentUser.IsVendorUser)
                        isUserAuthorized = mop.CreatedBy != null && mop.CreatedBy == currentUser.UserId;
                }
                else
                {
                    ErrorMessage = ConstMessage.Not_exist;
                    return RedirectToAction("MethodofProcedure");
                }
                if (mop.Status < 0 && currentUser.IsVendorUser)
                {
                    // mop.Status = Convert.ToInt32(Enums.ApprovalStatus.Requested);
                    ViewBag.ShowIncomplete = 1;
                }
                if (mop.Status == 2)
                {
                    ViewBag.IsRequestEdited = "1";
                }

                ViewBag.ProjectId = mop.ProjectId ?? 0;
                int parentProjectId = 0;
                if (mop != null)
                {
                    var project = _icraProjectService.GetAllActiveTIcraProject(false).Where(x => x.ProjectId == mop.ProjectId && x.ParentProjectId != null).Select(x => x.ParentProjectId).FirstOrDefault();

                    if (project != null)
                    {
                        parentProjectId = project.Value;
                    }
                }

                ViewBag.ParentProjectId = parentProjectId;

            }
            else
            {
                mop.RequestBy = currentUser.UserId;
                mop.EmailAddress = currentUser.Email;
                mop.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);
                ViewBag.ParentProjectId = 0;
                var formData = _permitService.GetMopAdditionalForms().Where(x => x.IsActive).ToList();
                mop.SystemImpactArea = new List<TSystemImpactArea>();
                for (int i = 0; i < 5; i++)
                {
                    TSystemImpactArea list = new TSystemImpactArea();
                    mop.SystemImpactArea.Add(list);
                }
                mop.ProjectContactList = new List<TProjectContactList>();
                for (int i = 0; i < 3; i++)
                {
                    TProjectContactList list = new TProjectContactList();
                    mop.ProjectContactList.Add(list);
                }

                mop.SystemsImpacted = new List<TMopAdditionalForms>();
                foreach (var item in formData.Where(x => x.Type == 2).ToList())
                {
                    var tform = new TMopAdditionalForms
                    {
                        FormId = item.Id,
                        AdditionalForms = item
                    };
                    mop.SystemsImpacted.Add(tform);
                }

                mop.AdditionalForms = new List<TMopAdditionalForms>();
                mop.DrawingAttachments = new List<DrawingpathwayFiles>();
                foreach (MopAdditionalForms item in formData.Where(x => x.Type == 3).ToList())
                {
                    var tform = new TMopAdditionalForms
                    {
                        FormId = item.Id,
                        AdditionalForms = item
                    };
                    mop.AdditionalForms.Add(tform);
                }
                ViewBag.ProjectId = 0;
                if (currentUser.IsVendorUser)
                    mop.Contractor = currentUser.Vendor.Name;
                else
                    mop.Contractor = UserSession.CurrentOrg.Name;
            }
            mop.Contractor = !string.IsNullOrEmpty(mop.Contractor) ? mop.Contractor : string.Empty;

            if (id.HasValue && id.Value > 0)
            {
                mop.ApproveBy = (mop.ApproveBy == null && _commonModelFactory.IsAdminUser()) ? currentUser.UserId : mop.ApproveBy;
            }
            if (mop != null && mop.TmopId > 0)
            {
                mop.TIcraProject = new TIcraProject();
                if (mop.ProjectId != 0)
                {
                    mop.TIcraProject = _icraProjectService.GetCountAllActiveTIcraProject(currentUser.UserId, currentUser.VendorId != null ? currentUser.VendorId : null, false).Where(x => x.ProjectId == mop.ProjectId).FirstOrDefault();
                    mop.TIcraProject.mode = "View";
                }
            }
            if (isUserAuthorized)
                return View(mop);
            else
            {
                ErrorMessage = ConstMessage.NotAuthorized_Msg;
                return RedirectToAction("MethodofProcedure");
            }


        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public JsonResult AddMethodofProcedure(TMOP mop, string lstBuildingDetails, string lstFloorDetails, string submit, string IsRequestEdited)
        {
            var isShowPopUp = 0;

            int submitValue = getSubmitButtonValues(submit);
            mop.CreatedBy = UserSession.CurrentUser.UserId;

            if (submitValue < 0)
                mop.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission);
            else if (submitValue > 0 && mop.Status == -1)
                mop.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);

            int tmopid;
            if (mop.TmopId == 0)
            {
                tmopid = 0;
            }
            if (!string.IsNullOrEmpty(lstBuildingDetails))
            {
                var buildings = JsonConvert.DeserializeObject<List<Buildings>>(lstBuildingDetails);
                mop.BuildingId = string.Join(",", buildings.Select(x => x.BuildingId));
                mop.BuildingName = string.Join(",", buildings.Select(x => x.BuildingName));
            }
            if (!string.IsNullOrEmpty(lstFloorDetails))
            {
                var floors = JsonConvert.DeserializeObject<List<Floor>>(lstFloorDetails);
                mop.FloorId = string.Join(",", floors.Select(x => x.FloorId));
                mop.FloorName = string.Join(",", floors.Select(x => x.FloorName));
            }
            List<Int32> tfileIds = new List<int>();
            if (!string.IsNullOrEmpty(mop.AttachFiles))
            {
                foreach (var item in mop.AttachFiles.Split(','))
                {
                    int fileId = Convert.ToInt32(item.Split('_')[0]);
                    tfileIds.Add(fileId);
                }

                if (!string.IsNullOrEmpty(mop.TFileIds))
                {
                    string file;
                    file = string.Join(",", tfileIds.Select(n => n.ToString()).ToArray());
                    mop.TFileIds = mop.TFileIds + ',' + file;
                }
                else
                {
                    mop.TFileIds = string.Join(",", tfileIds.Select(n => n.ToString()).ToArray());
                }

            }
            //if(!string.IsNullOrEmpty(mop.AttachDrawingFiles))
            //mop.TDrawingFields = mop.AttachDrawingFiles;

            if (!string.IsNullOrEmpty(mop.TFileIds))
            {
                mop.HasAttachment = true;
            }
            tmopid = _permitService.AddMethodofProcedure(mop);
            if (tmopid > 0)
            {
                bool redirectToFormSet = false;
                isShowPopUp = setPopupMOPStatus(mop.AdditionalForms);
                foreach (var item in mop.AdditionalForms)
                {
                    if (item.RespondStatus && !redirectToFormSet)
                    {
                        redirectToFormSet = true;
                    }
                    item.TMopId = tmopid;
                    item.CreatedBy = mop.CreatedBy;
                    item.UpdatedBy = mop.CreatedBy;
                    _permitService.AddTMopAdditionalForms(item);
                }
                foreach (var item in mop.SystemsImpacted)
                {
                    item.TMopId = tmopid;
                    item.CreatedBy = mop.CreatedBy;
                    item.UpdatedBy = mop.CreatedBy;
                    _permitService.AddTMopAdditionalForms(item);
                }

                foreach (var item in mop.SystemImpactArea)
                {
                    item.TmopId = tmopid;
                    item.CreatedBy = mop.CreatedBy;
                    item.UpdatedBy = mop.CreatedBy;
                    _permitService.AddTSystemImpactArea(item);
                }

                foreach (var item in mop.ProjectContactList)
                {
                    item.TmopId = tmopid;
                    item.CreatedBy = mop.CreatedBy;
                    item.UpdatedBy = mop.CreatedBy;

                    _permitService.AddTProjectContactList(item);
                }
                var permit = _permitService.GetMethodofProcedure(tmopid);
                if(permit!=null && permit.TmopId>0)
                {
                    string permitNo = !string.IsNullOrEmpty(permit.PermitNo)?permit.PermitNo:string.Empty;
                    if (mop.TmopId > 0)
                    {

                        SuccessMessage = $"{"Permit# " + permitNo + " "}{ConstMessage.Success_Updated}";
                    }
                    else if (tmopid > 0 && mop.TmopId == 0)
                    {
                      
                        SuccessMessage = $"{"Permit # " + permitNo + " "}{ConstMessage.Success}";
                        //SuccessMessage = $"{"[MOP] with Permit no " + PermitNo}{ConstMessage.Success}";
                    }
                    else
                        ErrorMessage = ConstMessage.Error;

                    if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.MOP.ToString(), BDO.Enums.NotificationEvent.OnSubmit.ToString()))
                    {
                        if (submitValue > 0)
                            SendMOPEmail(tmopid, IsRequestEdited);

                    }
                    else if (mop.Status != 2)
                    {
                        if (submitValue > 0)
                            SendMOPEmail(tmopid, IsRequestEdited);
                    }
                }
               
            }

            var result = new
            {
                Result = tmopid > 0,
                mop.AdditionalForms,
                IsShowPopUp = isShowPopUp
            };
            return ReturnJsonResult(result);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        private int setPopupMOPStatus(List<TMopAdditionalForms> mopAdditionalForms)
        {
            var status = 0;
            if (mopAdditionalForms.All(x => x.RespondStatus && x.HasCompleted == 1))
                status = 0;
            else if (mopAdditionalForms.Any(x => x.RespondStatus))
                status = 1;
            return status;
        }
        private void SendMOPEmail(int mopid, string IsRequestEdited)
        {
            var mop = _permitService.GetMethodofProcedure(mopid);
            var objPermitEmailModel = new PermitEmailModel
            {
                //PermitType = $"METHOD OF PROCEDURE [MOP]",
                PermitType = BDO.Enums.PermitType.Mop,
                PermitNo = mop.PermitNo ?? "",
                ProjectName = mop.ProjectName,
                ProjectNo = mop.Project.ProjectNumber,
                Requestor = (mop.RequestByUser != null ? mop.RequestByUser.Name : string.Empty),
                RequestedDate = mop.Date,
                ApprovedDate = mop.ApproveDate,
                Approver = (mop.ApprovedByUser != null ? mop.ApprovedByUser.Name : string.Empty),
                ApprovalStatus = mop.Status,
                Reason = mop.RejectReason,
                RequesterEmail = mop.EmailAddress,
                Company = mop.Contractor,
                IsRequestEdited = IsRequestEdited,
                EventId = mop.Status == 1 ? Convert.ToInt32(BDO.Enums.NotificationEvent.OnApproved) : Convert.ToInt32(BDO.Enums.NotificationEvent.OnSubmit)
            };
            var base64EncodePdf = _pdfService.MOPPermitInbytes(mopid);
            byte[] filebytes = Convert.FromBase64String(base64EncodePdf);
            // var filename = $"{Enums.NotificationCategory.MOP.ToString()}{"_"}{mopid}{".pdf"}";
            var filename = $"{mop.PermitNo}{".pdf"}";
            objPermitEmailModel.PermitUrl = "permit/addmethodofprocedure?id=" + mopid;
            _emailService.SendPermitCreatedMail(objPermitEmailModel, Convert.ToInt32(BDO.Enums.NotificationCategory.MOP), filebytes, filename, mop.BuildingId, mop.FloorId, string.Empty);
        }
        public int SetRedirectForm(int TmopId, int formId, string permitId, string permitno)
        {
            var item = new TMopAdditionalForms
            {
                FormId = Convert.ToInt32(formId),
                TMopId = Convert.ToInt32(TmopId),
                UpdatedBy = UserSession.CurrentUser.UserId,
                CreatedBy = UserSession.CurrentUser.UserId,
                HasCompleted = 1,
                PermitId = permitId,
                PermitNo = permitno
            };
            int isDone = _permitService.AddTMopAdditionalFormsRedirect(item);
            TempData["MopId"] = null;
            return isDone;
        }

        public JsonResult GetSubProjectByProjectId(int projectId)
        {
            var project = _icraProjectService.GetAllActiveTIcraProject(false).Where(x => x.ParentProjectId == projectId).ToList();
            return ReturnJsonResult(project);
            //return Json(project, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTMOPFiles(int tmopId, string fileIds)
        {
            var result = _permitService.DeleteTMOPFiles(tmopId, fileIds);
            return ReturnJsonResult(result);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteTMOPDrawing(int tmopId, string fileIds)
        {
            var result = _permitService.DeleteTMOPDrawing(tmopId, fileIds);
            return ReturnJsonResult(result);
            // return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadMOPAttachments(int id)
        {
            TMOP mop = new TMOP();
            mop = _permitService.GetMethodofProcedure(id);
            var fileName = Guid.NewGuid() + ".zip";
            var tempOutPutPath = _commonModelFactory.ServerMapPath("temp" + fileName);
            var outpathfilename = $"{mop.PermitNo}{".zip"}";
            DirectoryService.CreateDirectory(_commonModelFactory.ServerMapPath("temp"));
            try
            {

                using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
                {
                    s.SetLevel(9); // 0-9, 9 being the highest compression  

                    byte[] buffer = new byte[2021515];

                    MemoryStream outputMemStream = new MemoryStream();
                    ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

                    zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
                    var base64EncodePdf = _pdfService.MOPPermitInbytes(id);
                    byte[] fileBytes = Convert.FromBase64String(base64EncodePdf);
                    var filename = $"{mop.PermitNo}{".pdf"}";
                    ZipEntry entry = new ZipEntry(filename)
                    {
                        DateTime = DateTime.Now
                    };

                    s.PutNextEntry(entry);
                    MemoryStream inStream = new MemoryStream(fileBytes);
                    StreamUtils.Copy(inStream, s, new byte[2021515]);
                    inStream.Close();
                    for (int i = 0; i < mop.Attachments.Count; i++)
                    {
                        entry = new ZipEntry(Path.GetFileName(mop.Attachments[i].FileName));
                        entry.DateTime = DateTime.Now;
                        entry.IsUnicodeText = true;
                        s.PutNextEntry(entry);
                        var req = System.Net.WebRequest.Create(new Uri(_commonModelFactory.FilePath(mop.Attachments[i].FilePath)));
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
                    s.Finish();
                    s.Flush();
                    s.Close();

                }
                byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
                if (System.IO.File.Exists(tempOutPutPath))
                    System.IO.File.Delete(tempOutPutPath);

                return File(finalResult, "application/zip", outpathfilename);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new EmptyResult();
            }

        }
        #endregion

        #region Hot work Permit

        #endregion

        #region Facilities Maintenance Occurrence [FMC]  

        [CrxAuthorize(Roles = "permit_FacilitiesMaintenanceOccurrence")]
        public ActionResult FacilitiesMaintenanceOccurrence()
        {
            UISession.AddCurrentPage("Permit_FacilitiesMaintenanceOccurrence", 0, "Facilities Maintenance Occurrence");

            var result = _permitService.GetAllFacilitiesMaintenanceOccurrence();
            if (UserSession.CurrentUser.IsVendorUser)
            {
                result = result.Where(x => x.CreatedBy == UserSession.CurrentUser.UserId).ToList();
            }

            return View(result);
        }

        public async Task<ActionResult> AddFacilitiesMaintenanceOccurrence(int? id)
        {
            var currentUser = UserSession.CurrentUser;
            UISession.AddCurrentPage("Permit_AddFacilitiesMaintenanceOccurrence", 0, "Add Facilities Maintenance Occurrence");
            TFacilitiesMaintenanceOccurrencePermit tfmc = new TFacilitiesMaintenanceOccurrencePermit();
            bool isUserAuthorized = true;
            ViewBag.IsRequestEdited = "";
            if (id.HasValue && id.Value > 0)
            {
                tfmc = _permitService.GetFacilitiesMaintenanceOccurrenceAsync(id.Value);
                if (tfmc != null && tfmc.TfmcId > 0)
                {
                    if (currentUser.IsVendorUser)
                        isUserAuthorized = tfmc.CreatedBy != null && tfmc.CreatedBy == currentUser.UserId ? true : false;
                }
                else
                {
                    ErrorMessage = ConstMessage.Not_exist;
                    return RedirectToAction("FacilitiesMaintenanceOccurrence");
                }



            }
            else
            {
                tfmc.CreatedBy = currentUser.UserId;
                tfmc.CompletelyResolved = 0;
                tfmc.Name = currentUser.FullName;
                tfmc.Status = 2;
                tfmc.lstClassificationType = _permitService.GetClassificationType();
                if (currentUser.IsVendorUser)
                    tfmc.Company = currentUser.Vendor.Name;
                else
                    tfmc.Company = UserSession.CurrentOrg.Name;
            }
            tfmc.Company = !string.IsNullOrEmpty(tfmc.Company) ? tfmc.Company : string.Empty;
            var department = await _departmentService.GetDepartmentAsync();
            // var shifts = await _permitService.GetShiftsAsync();
            ViewBag.Departments = new SelectList(department.Where(x => x.IsActive).Select(s => new
            {
                s.DepartmentId,
                Name = string.Format("{0}-{1}", s.Name, s.Code)
            }).OrderBy(x => x.Name), "DepartmentId", "Name", tfmc != null && tfmc.DepartmentId.HasValue ? tfmc.DepartmentId.Value : 0);
            // ViewBag.Shifts = new SelectList(shifts.Where(x => x.IsActive == true).OrderBy(x => x.Name), "ShiftId", "Name", TFMC!=null && TFMC.ShiftId.HasValue ? TFMC.ShiftId.Value : 0);
                      

            if (isUserAuthorized)
                return View(tfmc);
            else
            {
                ErrorMessage = ConstMessage.NotAuthorized_Msg;
                return RedirectToAction("FacilitiesMaintenanceOccurrence");
            }
            // return View(TFMC);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult AddFacilitiesMaintenanceOccurrence(TFacilitiesMaintenanceOccurrencePermit tfmc, string submit)
        {
            try
            {

                tfmc.Status = (submit.ToLower() == "submit") ? 1 : 2;               

                int submitValue = getSubmitButtonValues(submit);
                tfmc.CreatedBy = UserSession.CurrentUser.UserId;
                tfmc.UpdatedBy = UserSession.CurrentUser.UserId;


                var tfmcId = 0;
                if (tfmc.lstClassificationType.Count > 0)
                {
                    var list = tfmc.lstClassificationType.Where(x => x.IsChecked == 1).Select(x => x.Id);
                    tfmc.ClassificationType = string.Join(",", list);
                }

                tfmcId = _permitService.AddFacilitiesMaintenanceOccurrenceAsync(tfmc);
                string PermitNumber;
                PermitNumber = _permitService.GetFacilitiesMaintenanceOccurrence(tfmcId).PermitNo;
                if (tfmc.TfmcId > 0)
                {
                    if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.FMC.ToString(), BDO.Enums.NotificationEvent.OnSubmit.ToString()))
                    {
                        if (submitValue > 0)
                            SendFMCPermitEmail(tfmc.TfmcId);
                    }
                    else if (tfmc.Status != 2)
                    {
                        if (!string.IsNullOrEmpty(submit) && submit.ToLower() != "save incomplete")
                            SendFMCPermitEmail(tfmc.TfmcId);
                    }

                    SuccessMessage = "Report# " + PermitNumber + " " + ConstMessage.Success_Updated;
                }
                else if (tfmcId > 0 && tfmc.TfmcId == 0)
                {
                    if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.FMC.ToString(), BDO.Enums.NotificationEvent.OnSubmit.ToString()))
                    {
                        if (submitValue > 0)
                            SendFMCPermitEmail(tfmcId);
                    }
                    else if (tfmc.Status != 2)
                    {
                        if (submitValue > 0)
                            SendFMCPermitEmail(tfmcId);
                    }
                    SuccessMessage = "Report# " + PermitNumber + " " + ConstMessage.Generated_Successfully;
                }
                else
                    ErrorMessage = ConstMessage.Error;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return RedirectToAction("FacilitiesMaintenanceOccurrence");
        }

        private void SendFMCPermitEmail(int tfmcId)
        {
            var fmcpermit = _permitService.GetFacilitiesMaintenanceOccurrence(tfmcId);
            PermitEmailModel objPermitEmailModel = new PermitEmailModel
            {
                //PermitType = $"Facilities Maintenance Occurrence [FMC]",
                PermitType = BDO.Enums.PermitType.OccurrencePermit,
                PermitNo = fmcpermit.PermitNo ?? "",
                Requestor = (fmcpermit.Name ?? string.Empty),
                RequestedDate = fmcpermit.RequesterDate,
                ApprovedDate = fmcpermit.ApprovedDate,
                Approver = (fmcpermit.AuthorizedByUser != null ? fmcpermit.AuthorizedByUser.Name : string.Empty),
                ApprovalStatus = fmcpermit.Status == 1 ? 4 : fmcpermit.Status,
                Reason = fmcpermit.Reason,
                RequesterEmail = (fmcpermit.PermitRequestUser != null ? fmcpermit.PermitRequestUser.Email : string.Empty),
                Company = fmcpermit.Company,
                EventId = fmcpermit.Status == 1 ? Convert.ToInt32(BDO.Enums.NotificationEvent.OnApproved) : Convert.ToInt32(BDO.Enums.NotificationEvent.OnSubmit)
            };
            var base64EncodePdf = _pdfService.FMCPermitInbytes(tfmcId);
            byte[] filebytes = Convert.FromBase64String(base64EncodePdf);
            // var filename = $"{Enums.NotificationCategory.FMC.ToString()}{"_"}{fmcpermit.TfmcId}{".pdf"}";
            var filename = $"{fmcpermit.PermitNo}{".pdf"}";
            objPermitEmailModel.PermitUrl = "permit/addfacilitiesmaintenanceoccurrence?id=" + fmcpermit.TfmcId.ToString();
            _emailService.SendPermitCreatedMail(objPermitEmailModel, Convert.ToInt32(BDO.Enums.NotificationCategory.FMC), filebytes, filename, string.Empty, string.Empty, string.Empty);
        }

        public JsonResult DeletePermit(int id, int permittype)
        {
            var result = _permitService.DeletePermit(id, permittype);
            return ReturnJsonResult(result);
           
        }
        public JsonResult DeleteFMCDrawings(int TfmcId, string fileIds)
        {
            var result = _permitService.DeleteFMCDrawings(TfmcId, fileIds);
            return ReturnJsonResult(result);
            // return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion`

        #region Paper Permit

        // GET: Permit
        //[CrxAuthorize(Roles = "Permit_Index")]
        public ActionResult PaperPermit()
        {
            UISession.AddCurrentPage("Permit_PaperPermit", 0, "Paper Permit");
            var permit = _permitService.GetPaperPermit();
            permit = UserSession.CurrentUser.IsVendorUser ? permit.Where(x => x.CreatedBy == UserSession.CurrentUser.UserId).ToList() : permit.ToList();

            return View(permit);
        }
        //
        [HttpGet]
        public ActionResult AddPaperPermit(int? id, int? mopid)
        {
            UISession.AddCurrentPage("Permit_AddPaperPermit", 0, "Add Paper Permit");
            var PaperPermit = new PaperPermit();

            bool isUserAuthorized = true;
            ViewBag.ShowIncomplete = 0;
            var enumData = from BDO.Enums.PermitType e in Enum.GetValues(typeof(BDO.Enums.PermitType))
                           select new
                           {
                               Value = (int)e,
                               Text = e.GetDescription().ToString()
                           };

            if (id > 0)
            {
                PaperPermit = _permitService.GetPaperPermitById(id.Value);
                if (PaperPermit != null && PaperPermit.PermitId > 0)
                {
                    if (UserSession.CurrentUser.IsVendorUser)
                        isUserAuthorized = PaperPermit.CreatedBy == UserSession.CurrentUser.UserId;
                }
                else
                {
                    ErrorMessage = ConstMessage.Not_exist;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                PaperPermit.Attachments = new List<TFiles>();
            }
            ViewBag.ProjectId = (PaperPermit.ProjectId > 0 ? PaperPermit.ProjectId : 0);
            ViewBag.PermitType = new SelectList(enumData.OrderBy(x => x.Text), "Value", "Text", PaperPermit.PermitType ?? 0);
            if (mopid > 0)
            {
                TempData["MopId"] = mopid;
            }
            TempData.Keep();
            if (isUserAuthorized)
                return View(PaperPermit);
            else
            {
                ErrorMessage = ConstMessage.NotAuthorized_Msg;
                return RedirectToAction("Index");
            }
        }

        public ActionResult CheckPermitNo(string value)
        {
            var permit = _permitService.GetPaperPermit().Where(x => x.PermitNo == value && x.IsActive == true).ToList();
            var result = "null";
            if (permit.Count != 0)
            {
                ErrorMessage = "Permit # " + " " + ConstMessage.PermitNoExists;
                result = ErrorMessage;
            }
            return ReturnJsonResult(result);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatedPermit(int id)
        {
            var PaperPermit = _permitService.GetPaperPermitById(id);
            var result = new { PaperPermit };
            return ReturnJsonResult(result);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult AddPaperPermit(PaperPermit PaperPermit, string submit)
        {
            if (PaperPermit.PermitId > 0)
            {
                var paperPermit = _permitService.GetPaperPermitById(PaperPermit.PermitId);
            }
            int submitValue = getSubmitButtonValues(submit);
            PaperPermit.CreatedBy = UserSession.CurrentUser.UserId;


            List<Int32> tfileIds = new List<int>();
            if (!string.IsNullOrEmpty(PaperPermit.AttachFiles))
            {
                foreach (var item in PaperPermit.AttachFiles.Split(','))
                {
                    int fileId = Convert.ToInt32(item.Split('_')[0]);
                    tfileIds.Add(fileId);
                }
                if (!string.IsNullOrEmpty(PaperPermit.TFileIds))
                {
                    string file = string.Join(",", tfileIds.Select(n => n.ToString()).ToArray());
                    PaperPermit.TFileIds = PaperPermit.TFileIds + ',' + file;
                }
                else
                {
                    PaperPermit.TFileIds = string.Join(",", tfileIds.Select(n => n.ToString()).ToArray());
                }

            }
            int PaperPermitId = _permitService.AddPaperPermit(PaperPermit);
            if (PaperPermitId > 0)
            {
                var permitNumber = _permitService.GetPaperPermitById(PaperPermitId).PermitNo;
                if (PaperPermit.PermitId > 0)
                {

                    SuccessMessage = "Permit# " + permitNumber + " " + ConstMessage.Success_Updated;
                }
                else if (PaperPermitId > 0 && PaperPermit.PermitId == 0)
                {
                    SuccessMessage = "Permit# " + permitNumber + " " + ConstMessage.Success;
                }
                else
                    ErrorMessage = ConstMessage.Error;
                
            }

            return RedirectToAction("PaperPermit");
        }

        public JsonResult Delete_TPaperPermitFiles(int permitid, string fileIds)
        {
            var result = _permitService.Delete_TPaperPermitFiles(permitid, fileIds);
            return ReturnJsonResult(result);           
        }
        #endregion

        #region All Permit

        // GET: Permit
        //[CrxAuthorize(Roles = "Permit_Index")]
        public ActionResult GetAllPermits(string Status = null, int? FilterBy = 30, int? PermitType = null, int? ProjectID = null)
        {
            var currentUser = UserSession.CurrentUser;
            UISession.AddCurrentPage("Permit_GetAllPermit", 0, "All Permit");
            if (!FilterBy.HasValue)
            {
                FilterBy = 0;
            }
            _session.Set(SessionKey.FilterBy, FilterBy.Value);
            //Session["FilterBy"] = FilterBy.Value;
            ViewBag.Status = Status ?? "-2,0,1,2,3,5";
            ViewBag.FilterBy = FilterBy;
            ViewBag.PermitType = PermitType;
            ViewBag.ProjectID = ProjectID.HasValue && ProjectID.Value > 0 ? ProjectID : 0;
            if (Status == null && (FilterBy.HasValue && FilterBy.Value == 30) && PermitType == null)
            {
                Status = "2,3";
                ViewBag.Status = Status;
            }
            if (!string.IsNullOrEmpty(Status) && Status == "-2")
            {
                Status = "";
                ViewBag.Status = "-2,0,1,2,3,5";
            }

            var permit = _permitService.GetAllPermit(FilterBy, PermitType, Status, ProjectID);
            if (currentUser.IsVendorUser)
                permit = permit.Where(x => x.RequestedBy == currentUser.UserId).ToList();
            else
                permit = permit.Where(x => x.Status != Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission) || x.RequestedBy == currentUser.UserId).ToList();

            return View(permit);
        }

        #endregion

        #region Drawing Viewer
        public JsonResult SaveDrawingViewer(string FloorPlanId, string PermitId)
        {
            var drawingViewer = new DrawingViewer
            {
                PermitNo = PermitId,
                FloorPlanId = Guid.Parse(FloorPlanId)
            };
            var result = _floorService.SaveDrawingViewer(drawingViewer);
            return ReturnJsonResult(result);           
        }
        #endregion

        #region AllPermitReport

        public ActionResult PermitReport(string from, string todate, string permittype)
        {
            AllPermitReport permitcount = _constructionService.GetAllPermitCount(from, todate, "8");
            return PartialView("_PermitReport", permitcount);
        }
        public JsonResult GetAllPermitReport(string fromdate, string todate, string permittype)
        {
            string Message = string.Empty;
            string uri = "reports/getAllPermitReport/" + fromdate + "/" + todate + "/" + permittype;
            int StatusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            string result = _httpService.CallGetMethod(uri, ref StatusCode);
            if (StatusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {

                var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                //return Json(httpresponse, JsonRequestBehavior.AllowGet);
                return ReturnJsonResult(httpresponse);
            }
            else
            {
                Message = ConstMessage.Internal_Server_Error;
            }
            //return Json(Message, JsonRequestBehavior.AllowGet);
            return ReturnJsonResult(Message);
        }
        #endregion

        #region TaggedUsers 

        public ActionResult DraftEmailPopup(string PermitNo, int Tagtype, int PermitType)
        {

            TaggedMaster taggedMaster = new TaggedMaster();
            Guid id = Guid.NewGuid();
            ViewBag.TaggedGuid = id;
            ViewBag.Tagtype = Tagtype;
            ViewBag.PermitType = PermitType;
            if (PermitType == 1)
            {
                ViewBag.Path = _appSettings.WebUrlPath + "permit/ceiling/" + id;
            }

            ViewBag.ActivityIDs = PermitNo;
            return PartialView("~/Views/Home/_draftEmailPopup.cshtml", taggedMaster);
        }

        #endregion

        #region Permit Setting
        public ActionResult PermitSettings()
        {
            UISession.AddCurrentPage("PermitSettings", 0, "Permit Settings");
            var org = _orgService.GetOrganization();
            return View(org);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult PermitSettings(IFormFile file, Organization org)
        {
            var result = _orgService.PermitSettings(org);
            return RedirectToAction("PermitSettings");
        }


        [HttpGet]
        public ActionResult PermitFormSettings()
        {
            UISession.AddCurrentPage("permit_PermitFormettings", 0, "Permit Work Flow Setting");
            return View();
        }

        [HttpGet]
        public ActionResult AssignpermitWorkFlowEmail(string id)
        {
            PermitSetting data = new PermitSetting();
            data = _permitService.GetPermitWorkFlowSettings().FirstOrDefault(x=> x.Id== Convert.ToInt32(id));
            ViewBag.Users = _userService.GetUsers().Where(x => !x.IsCRxUser).ToList();
            return PartialView("_addPermitSettingEmail", data);
        }

        [HttpPost]
        public JsonResult SavePermitSettingemails(PermitSetting PermitSetting)
        {
            PermitSetting.Mode = 2;
            int id = _permitService.AddPermitWorkFlow(PermitSetting);
            return Json(new { data = PermitSetting.NotificationEmail });
        }

        public ActionResult PermitFormSettingsMatrix(string mode, string id)
        {
            List<PermitSetting> data = _permitService.GetPermitWorkFlowSettings();
            if (data != null)
            {
                foreach (int val in Enum.GetValues(typeof(BDO.Enums.PermitType)))
                {
                    if (data != null && !data.Any(x => x.PermitType == val))
                    {
                        PermitSetting PermitSetting = new PermitSetting();
                        PermitSetting.Required = true;
                        PermitSetting.ControlType = 1;
                        PermitSetting.LabelType = "Requester";
                        PermitSetting.LabelText = "Requester";
                        PermitSetting.PermitType = Convert.ToInt32(val);
                        PermitSetting.Sequence = 1;
                        data.Add(PermitSetting);
                    }

                }
            }
            return PartialView("_PermitSettingMatrix", data);
        }
        public ActionResult BindPermitSettingList(int sequence, int PermiType,string assetpathtype)
        {
            PermitSetting device = new PermitSetting();
            ViewData["sequence"] = sequence;
            ViewData["type"] = PermiType;
            ViewData["Path"] = assetpathtype;
            device.Sequence = sequence;
            return PartialView("_PermitWorkFlow", device);
        }
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult PermitFormSettingsSave(List<PermitSetting> PermitSetting)
        {
            //List<PermitSetting> data = new List<PermitSetting>();
            //var orderedByAsc = PermitSetting.OrderBy(d => d.Sequence).Select(x=>x.Sequence).ToList();
            //if (!PermitSetting.ToList().Select(x => x.Sequence).SequenceEqual(orderedByAsc))
            //{
            //    //Console.WriteLine("not Ordered by Asc");
            //    ErrorMessage = ConstMessage.AscOrderSequence;
            //}
            var isOrderedAscending = PermitSetting.SequenceEqual(PermitSetting.OrderBy(x => x.Sequence));
            if (!isOrderedAscending)
            {
                ErrorMessage = ConstMessage.AscOrderSequence;
                return RedirectToAction("PermitFormSettings");
            }
            List<PermitSetting> data = _permitService.GetPermitWorkFlowSettings();
            foreach (var permit in PermitSetting)
            {
                if(permit.PermitType==13)
                {
                    if (data.Any(x => x.PermitType == permit.PermitType && x.PathId==permit.PathId && (x.LabelText.ToLower().Trim() == permit.LabelText.ToLower().Trim() || x.LabelType.ToLower().Trim() == permit.LabelType.ToLower().Trim()) && x.Id != permit.Id))
                    {
                        ErrorMessage = ConstMessage.PermitSettingAlreadyExists;
                        return RedirectToAction("PermitFormSettings");
                    }
                }
                else
                {
                    if (data.Any(x => x.PermitType == permit.PermitType && (x.LabelText.ToLower().Trim() == permit.LabelText.ToLower().Trim() || x.LabelType.ToLower().Trim() == permit.LabelType.ToLower().Trim()) && x.Id != permit.Id))
                    {
                        ErrorMessage = ConstMessage.PermitSettingAlreadyExists;
                        return RedirectToAction("PermitFormSettings");
                    }
                }

                permit.IsAssetDeviceChange = permit.PathId > 0 ? true : false;
                permit.Mode = 1;
                permit.CreatedBy = UserSession.CurrentUser.UserId;
                int id = _permitService.AddPermitWorkFlow(permit);
            }

            SuccessMessage = ConstMessage.PermitSetting_Saved_Successfully;
            return RedirectToAction("PermitFormSettings");
        }

        public JsonResult DeletePermitWorkFlow(int Id, int PermitType)
        {
            var result = _permitService.DeletePermitWorkFlow(Id, PermitType);           
            return ReturnJsonResult(result);
        }
        #endregion

        #region Asset Device Change  Forms

        // [CrxAuthorize(Roles = "Permit_LifeSafetyDevicesForms")]
        public ActionResult AssetDeviceChangeForm()
        {
            var currentUser = UserSession.CurrentUser;
            UISession.AddCurrentPage("Permit_AssetDeviceChangeForm", 0, "Asset Device Change Form");
            var data = _permitService.AssetDeviceChangeForms();

            if (currentUser.IsVendorUser)
                data = data.Where(x => x.Requestor == currentUser.UserId).ToList();
            else
                data = data.Where(x => x.Status != Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission) || x.CreatedBy == currentUser.UserId).ToList();


            return View(data);
        }

        public ActionResult AddAssetDeviceChangeFormsPermit(string sformType, string formId, int? mopid)
        {
            var currentUser = UserSession.CurrentUser;
            if (string.IsNullOrEmpty(sformType) && string.IsNullOrEmpty(formId))
                return RedirectToAction("AssetDeviceChangeForm");
            if (formId == "-1")
            {
                formId = string.Empty;
            }
            UISession.AddCurrentPage("Permit_AddAssetDeviceChangeFormsPermit", 0, "Asset Device Change Form");
            TAssetDeviceChangeForms newForm = new TAssetDeviceChangeForms();

            bool isUserAuthorized = true;
            ViewBag.IsRequestEdited = "";
            if (!string.IsNullOrEmpty(formId))
            {
                ViewBag.ShowIncomplete = 0;

                //Guid guidResult = Guid.Parse(formId);
                bool isValid = Guid.TryParse(formId, out _);
                if (isValid)
                {
                    newForm = _permitService.GetAssetChangeDeviceFormsById(Guid.Parse(formId));
                    if (newForm != null && !string.IsNullOrEmpty(Convert.ToString(newForm.AdcFormId)))
                    {

                        if (currentUser.IsVendorUser)
                            isUserAuthorized = newForm.Requestor != null && newForm.Requestor == currentUser.UserId;
                    }
                    else
                    {
                        ErrorMessage = ConstMessage.Not_exist;
                        return RedirectToAction("AssetDeviceChangeForm");
                    }
                    if (newForm.Status < 0 && currentUser.IsVendorUser)
                    {
                        //newForm.Status = Convert.ToInt32(Enums.ApprovalStatus.Requested);
                        ViewBag.ShowIncomplete = 1;
                    }
                    if (newForm.Status == 2)
                    {
                        ViewBag.IsRequestEdited = "1";
                    }

                    
                }
                else
                {
                    ErrorMessage = ConstMessage.Not_exist;
                    return RedirectToAction("AssetDeviceChangeForm");

                }
            }
            else
            {
                // set default value for first time
                newForm.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);
                   if(sformType == "addition")
                {
                    newForm.FormType = 1;
                }
                else if (sformType == "removal")
                {
                    newForm.FormType = 0;
                }
                else
                {
                    newForm.FormType = 2;
                }
                newForm.DateOfRequest = DateTime.Now.ToClientTime();
                if(currentUser != null)
                {
                    newForm.Requestor = currentUser.UserId;
                    newForm.EmailAddress = currentUser.Email;
                    if (!string.IsNullOrEmpty(currentUser.PhoneNumber))
                        newForm.PhoneNumber = currentUser.PhoneNumber;
                    else if (!string.IsNullOrEmpty(currentUser.CellNo))
                        newForm.PhoneNumber = currentUser.CellNo;

                    if (currentUser.IsVendorUser)
                        newForm.Contractor = currentUser.Vendor.Name;
                    else
                        newForm.Contractor = UserSession.CurrentOrg.Name;
                }
               
                var permitSettings = _permitService.GetPermitWorkFlowSettings().Where(x=>x.PermitType==13);
                var assetlist = _permitService.GetAssetDevicePath();
                if(assetlist!=null && assetlist.Count>0)
                newForm.AssetDevicePathSettings = assetlist;

                List<TPermitWorkFlowDetails> permitworkflow = new List<TPermitWorkFlowDetails>();
                foreach (var setting in permitSettings)
                {
                    TPermitWorkFlowDetails objflow = new TPermitWorkFlowDetails();
                    objflow.LabelText = setting.LabelText;
                    objflow.LabelType = setting.LabelType;
                    objflow.Sequence = setting.Sequence;
                    objflow.FormSettingId = setting.Id;
                    objflow.PathId = setting.PathId;
                    objflow.Required = setting.Required;
                    permitworkflow.Add(objflow);
                }
                if(permitworkflow!=null && permitworkflow.Count>0)
                newForm.TPermitWorkFlowDetails = permitworkflow;
            }
            newForm.Contractor = !string.IsNullOrEmpty(newForm.Contractor) ? newForm.Contractor : string.Empty;
            if (newForm.AssetDeviceList.Count == 0)
            {
                AssetDeviceList device = new AssetDeviceList();
                newForm.AssetDeviceList.Add(device);
            }

            if (mopid > 0)
            {
                TempData["MopId"] = mopid;
            }
            TempData.Keep();
            if (newForm != null)
            {
                newForm.TIcraProject = new TIcraProject();
                if (newForm.ProjectId != 0)
                {
                    newForm.TIcraProject = _icraProjectService.GetCountAllActiveTIcraProject(currentUser.UserId, currentUser.VendorId != null ? currentUser.VendorId : null, false).Where(x => x.ProjectId == newForm.ProjectId).FirstOrDefault();
                    newForm.TIcraProject.mode = "View";

                }
            }
            ViewBag.lstUserProfile = _userService.Get().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email) && x.IsCRxUser == false && x.UserId != 0).ToList();
            if (isUserAuthorized)
                return View(newForm);
            else
            {
                ErrorMessage = ConstMessage.NotAuthorized_Msg;
                return RedirectToAction("AssetDeviceChangeForm");
            }
            //return View(newForm);
        }

       
       
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult AddAssetDeviceChangeFormsPermit(TAssetDeviceChangeForms objData, string submit, string lstBuildingDetails, string lstFloorDetails, string IsRequestEdited)
        {
            int submitValue = getSubmitButtonValues(submit);
            objData.CreatedBy = UserSession.CurrentUser.UserId;
            if (!string.IsNullOrEmpty(lstBuildingDetails))
            {
                var buildings = JsonConvert.DeserializeObject<List<Buildings>>(lstBuildingDetails);
                objData.BuildingId = string.Join(",", buildings.Select(x => x.BuildingId));
                objData.BuildingName = string.Join(",", buildings.Select(x => x.BuildingName));
            }
            if (submitValue < 0)
                objData.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission);
            else if (submitValue > 0 && objData.Status == -1)
                objData.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);

            List<Int32> tfileIds = new List<int>();
            if (!string.IsNullOrEmpty(objData.AttachFiles))
            {
                foreach (var item in objData.AttachFiles.Split(','))
                {
                    int fileId = Convert.ToInt32(item.Split('_')[0]);
                    tfileIds.Add(fileId);
                }
                if (!string.IsNullOrEmpty(objData.TFileIds))
                {
                    string file = string.Join(",", tfileIds.Select(n => n.ToString()).ToArray());
                    objData.TFileIds = objData.TFileIds + ',' + file;
                }
                else
                {
                    objData.TFileIds = string.Join(",", tfileIds.Select(n => n.ToString()).ToArray());
                }

            }
            objData.AdcFormId = Guid.NewGuid();
            int tlsdFormno;
            tlsdFormno = objData.AdcFormNo;
            objData = _permitService.AddAssetChangeDevices(objData);
            if (objData.AdcFormNo > 0)
            {

                //foreach (var item in objData.LifeSafetyDeviceList.Where(x => !string.IsNullOrEmpty(x.DeviceNumber)))
                foreach (var item in objData.AssetDeviceList)
                {
                    item.AdcFormId = objData.AdcFormId;
                    _permitService.AddAsetChangeDeviceList(item);
                }
                string PermitNumber;
                PermitNumber = _permitService.GetAssetDeviceForm(objData.AdcFormId).PermitNo;

                if (!string.IsNullOrEmpty(PermitNumber))
                {
                    foreach (var objpermitworkflow in objData.TPermitWorkFlowDetails)
                    {
                        objpermitworkflow.CreatedBy = objData.CreatedBy;
                        objpermitworkflow.PermitNumber = PermitNumber;
                        if (objData.Status != 0)
                        {
                            _permitService.AddTPermitWorkFlowDetails(objpermitworkflow, objData.ProjectId.ToString());
                        }
                    }

                }
                if (tlsdFormno > 0)
                    SuccessMessage = "Permit# " + PermitNumber + " " + ConstMessage.Success_Updated;
                else if (objData.AdcFormNo > 0 && tlsdFormno == 0)
                    SuccessMessage = "Permit # " + PermitNumber + " " + ConstMessage.Success;
                else
                    ErrorMessage = ConstMessage.Error;
                if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.ADC.ToString(), BDO.Enums.NotificationEvent.OnSubmit.ToString()))
                {
                    if (submitValue > 0)
                        SendAssetDeviceChangePermitEmail(objData.AdcFormId, objData.BuildingId, IsRequestEdited);
                }
                else if (objData.Status != 2)
                {
                    if (submitValue > 0)
                        SendAssetDeviceChangePermitEmail(objData.AdcFormId, objData.BuildingId, IsRequestEdited);
                }


                return RedirectToAction("AssetDeviceChangeForm");
            }
            return View(objData);
        }
        //mail
        private void SendAssetDeviceChangePermitEmail(Guid lsdFormId, string buildingId, string IsRequestEdited)
        {
            var newForm = _permitService.GetAssetChangeDeviceFormsById(lsdFormId);
            var base64EncodePdf = _pdfService.AssetDeviceChangeFormInbytes(lsdFormId.ToString());
            byte[] filebytes = Convert.FromBase64String(base64EncodePdf);
            string permitType = $"{"Asset Device Change"} - {(newForm.FormType == 1 ? "Addition Form" : "Removal Form")}";
            var filename = $"{newForm.PermitNo}{(newForm.FormType == 1 ? "_A" : "_R")}{".pdf"}";
            string approveremail = string.Empty;
            string RequesterEmail = string.Empty;
           
            var objPermitEmailModel = new PermitEmailModel
            {
                //PermitType = permitType,
                PermitType = (newForm.FormType == 1) ? BDO.Enums.PermitType.ADCAdditionForm : BDO.Enums.PermitType.ADCRemovalForm,
                PermitNo = newForm.PermitNo ?? "",
                ProjectName = newForm.ProjectName,
                ProjectNo = newForm.Project.ProjectNumber,
                Requestor = (newForm.RequestorUser != null ? newForm.RequestorUser.Name : string.Empty),
                RequestedDate = newForm.DateOfRequest,
                Approver = approveremail.TrimEnd(','),
                ApprovalStatus = newForm.Status,
                Reason = newForm.Reason,
                RequesterEmail = RequesterEmail.TrimEnd(','),
                Company = newForm.Contractor,
                IsRequestEdited = IsRequestEdited,
                EventId = Convert.ToInt32(BDO.Enums.NotificationEvent.OnSubmit)
            };

            foreach (var path in newForm.AssetDevicePathSettings)
            {

                var newworkfloorbypath = newForm.TPermitWorkFlowDetails.Where(x => x.PathId == path.PathId);
                int CurrentSignSequence = newworkfloorbypath != null && newworkfloorbypath.Where(x => x.LabelSignId == 0 || x.LabelSignId == null).ToList().Count > 0 ? newworkfloorbypath.Where(x => x.LabelSignId == 0 || x.LabelSignId == null).OrderBy(x => x.Sequence).ToList().FirstOrDefault().FormSettingId : 0;
                approveremail += newworkfloorbypath != null && newworkfloorbypath.OrderByDescending(x => x.Sequence).ToList().FirstOrDefault().LabelValue.HasValue && newworkfloorbypath.OrderByDescending(x => x.Sequence).ToList().FirstOrDefault().LabelValue.Value > 0 ? newworkfloorbypath.OrderByDescending(x => x.Sequence).ToList().FirstOrDefault().UserDetail.Name + "," : string.Empty ;
                RequesterEmail += newworkfloorbypath != null && newworkfloorbypath.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.HasValue && newworkfloorbypath.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.Value > 0 ? newworkfloorbypath.OrderBy(x => x.Sequence).ToList().FirstOrDefault().UserDetail.Email + "," : string.Empty;

                if (objPermitEmailModel != null && CurrentSignSequence > 0)
                {

                    objPermitEmailModel.AdditionalNextLevelToEmails += newworkfloorbypath.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null ? newworkfloorbypath.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.NotificationEmail + "," : string.Empty;
                    objPermitEmailModel.AdditionalNextLevelCCEmails += newworkfloorbypath.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null ? newworkfloorbypath.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.NotificationCCEmail + "," : string.Empty;
                    if(newworkfloorbypath.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null && newworkfloorbypath.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.IsSendMail )
                        objPermitEmailModel.SendMailToNextPhase = newworkfloorbypath.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null ? newworkfloorbypath.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.IsSendMail : false;

                }
            }
            if(!string.IsNullOrEmpty(approveremail))
            {
                List<string> uniques = approveremail.TrimEnd(',').Split(',').Reverse().Distinct().Take(2).Reverse().ToList();
                string newStr = string.Join(",", uniques);
                objPermitEmailModel.Approver = newStr;
            }
            if (!string.IsNullOrEmpty(RequesterEmail))
            {
                List<string> uniques = RequesterEmail.TrimEnd(',').Split(',').Reverse().Distinct().Take(2).Reverse().ToList() ;
                string newStr = string.Join(",", uniques);
                objPermitEmailModel.RequesterEmail = newStr;
            }
            if (!string.IsNullOrEmpty(objPermitEmailModel.AdditionalNextLevelToEmails))
            {
                List<string> uniques = objPermitEmailModel.AdditionalNextLevelToEmails.TrimEnd(',').Split(',').Reverse().Distinct().Take(2).Reverse().ToList();
                string newStr = string.Join(",", uniques);
                objPermitEmailModel.AdditionalNextLevelToEmails = newStr;
            }
            if (!string.IsNullOrEmpty(objPermitEmailModel.AdditionalNextLevelCCEmails))
            {
                List<string> uniques = objPermitEmailModel.AdditionalNextLevelCCEmails.TrimEnd(',').Split(',').Reverse().Distinct().Take(2).Reverse().ToList();
                string newStr = string.Join(",", uniques);
                objPermitEmailModel.AdditionalNextLevelCCEmails = newStr;
            }


            objPermitEmailModel.PermitUrl = "asset-device-change/addition/form/" + newForm.AdcFormId.ToString();
            _emailService.SendPermitCreatedMail(objPermitEmailModel, Convert.ToInt32(BDO.Enums.NotificationCategory.ADC), filebytes, filename, buildingId, string.Empty, string.Empty);
        }

        public ActionResult BindACDDeviceList(int sequence, int type, string BuildingId)
        {
            AssetDeviceList device = new AssetDeviceList();
            ViewData["type"] = type;
            ViewData["sequence"] = sequence;
            ViewData["BuildingId"] = "";
            ViewData["DeviceType"] = "";
            if (!string.IsNullOrEmpty(BuildingId))
            {
                ViewData["BuildingId"] = BuildingId;
            }
            ViewBag.sequence = sequence;

            return PartialView("_AssetDeviceChangeForms", device);
        }
        public JsonResult GetACDBuilding(int BuildingId)
        {
            var result = _buildingsService.GetBuildings().Where(x => x.BuildingId == BuildingId);
            return ReturnJsonResult(result);
            // return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteACDDrawings(int LsdFormNo, string fileIds)
        {
            var result = _permitService.DeleteLSDDrawings(LsdFormNo, fileIds);
            return ReturnJsonResult(result);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteACDFiles(int LsdFormNo, string fileIds)
        {
            var result = _permitService.DeleteLSDDrawings(LsdFormNo, fileIds);
            return ReturnJsonResult(result);
            // return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadACDAttachments(string formId)
        {

            //var mainRoot = "temp";
            TAssetDeviceChangeForms newForm = _permitService.GetAssetChangeDeviceFormsById(Guid.Parse(formId));
            var fileName = Guid.NewGuid() + ".zip";
            var outpathfilename = $"{newForm.PermitNo}{(newForm.FormType == 1 ? "_A" : "_R")}{".zip"}";
            var tempOutPutPath = _commonModelFactory.ServerMapPath("temp" + fileName);
            DirectoryService.CreateDirectory(_commonModelFactory.ServerMapPath("temp"));
            try
            {

                using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
                {
                    s.SetLevel(9); // 0-9, 9 being the highest compression  

                    byte[] buffer = new byte[2021515];

                    MemoryStream outputMemStream = new MemoryStream();
                    ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

                    zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
                    var base64EncodePdf = _pdfService.AssetDeviceChangeFormInbytes(newForm.AdcFormId.ToString());
                    byte[] fileBytes = Convert.FromBase64String(base64EncodePdf);
                    var filename = $"{newForm.PermitNo}{(newForm.FormType == 1 ? "_A" : "_R")}{".pdf"}";
                    var entry = new ZipEntry(filename)
                    {
                        DateTime = DateTime.Now
                    };

                    s.PutNextEntry(entry);
                    MemoryStream inStream = new MemoryStream(fileBytes);
                    StreamUtils.Copy(inStream, s, new byte[2021515]);
                    inStream.Close();
                    for (int i = 0; i < newForm.Attachments.Count; i++)
                    {
                        entry = new ZipEntry(Path.GetFileName(newForm.Attachments[i].FileName))
                        {
                            DateTime = DateTime.Now,
                            IsUnicodeText = true
                        };
                        s.PutNextEntry(entry);
                        var req = System.Net.WebRequest.Create(new Uri(_commonModelFactory.FilePath(newForm.Attachments[i].FilePath)));
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
                    s.Finish();
                    s.Flush();
                    s.Close();

                }
                byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
                if (System.IO.File.Exists(tempOutPutPath))
                    System.IO.File.Delete(tempOutPutPath);

                return File(finalResult, "application/zip", outpathfilename);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new EmptyResult();
            }

        }

        public ActionResult AssetDevicesList()
        {
            UISession.AddCurrentPage("Permit_AssetDevicesList", 0, "Asset Devices List");
            var AssetDevicesList = _permitService.GetAssetDevicesList();
            return View(AssetDevicesList);
        }
        public ActionResult AddAssetDevices(int? assetformid)
        {
            var objassetdevice = new AssetDevices();
            if (assetformid != null)
                objassetdevice = _permitService.GetAssetDevicesList().Where(x=>x.AssetFormId==Convert.ToInt32(assetformid.Value)).FirstOrDefault();
            return PartialView("_AddAssetDevices", objassetdevice);
        }
        [HttpPost]
        [Route("addAssetDevice")]
        [ValidateAntiForgeryToken]
        public ActionResult AddAssetDevices(AssetDevices objAssetDeviceList)
        {
            if (ModelState.IsValid)
            {
                objAssetDeviceList.CreatedBy= UserSession.CurrentUser.UserId;
                int retval = _permitService.AddAssetDevices(objAssetDeviceList);
            }
            return RedirectToAction("AssetDevicesList");
        }
        #endregion

        #region Private methods

        private JsonResult ReturnJsonResult(Object obj)
        {
            return Json(obj);
        }

        #endregion
    }
}