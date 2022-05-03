using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Web.Base;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;   

namespace HCF.Web.Controllers
{
    [Authorize]
    public class HotWorkPermitController : BaseController
    {
        private readonly ILogger<HotWorkPermitController> _loggingService;
        private readonly IEmailService _emailService;
        private readonly IHotWorkPermitService _hotWorkPermitService;
        private readonly ICommonService _commonService;
        private readonly ITIcraProjectService _icraProjectService;
        private readonly IPermitService _permitService;
        private readonly IUserService _userService;
        private readonly IPdfService _pdfService;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly ICommonProvider _commonProvider;
        public HotWorkPermitController(ICommonModelFactory commonModelFactory, ILogger<HotWorkPermitController> loggingService, IEmailService emailService, ICommonService commonService, IHotWorkPermitService hotWorkPermitService,
            ITIcraProjectService icraProjectService, ICommonProvider commonProvider, IPermitService permitService, IUserService userService, IPdfService pdfService)
        {
            _commonModelFactory = commonModelFactory;
            _loggingService = loggingService;
            _commonService = commonService;
            _emailService = emailService;
            _hotWorkPermitService = hotWorkPermitService;
            _icraProjectService = icraProjectService;
            _permitService = permitService;
            _userService = userService;
            _pdfService = pdfService;
            _commonProvider = commonProvider;
        }

        //// GET: HotWorkPermit
        //[CrxAuthorize(Roles = "HotWorkPermit_GetAllHotWorkPermit")]
        public ActionResult GetAllHotWorkPermit()
        {
            var currentUser = UserSession.CurrentUser;
            UISession.AddCurrentPage("HotWorkPermit_GetAllHotWorkPermit", 0, "Hot Work Permit");
            List<THotWorkPermits> tHotWorkPermits;
            tHotWorkPermits = _hotWorkPermitService.GetAllHotWorksPermit().ToList();
            if (currentUser.IsVendorUser)
                tHotWorkPermits = tHotWorkPermits.Where(x => x.PermitRequestBy == currentUser.UserId).ToList();
            else
                tHotWorkPermits = tHotWorkPermits.Where(x => x.Status != Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission) || x.CreatedBy == currentUser.UserId).ToList();
            return View(tHotWorkPermits);
        }

        public JsonResult GetUserInfo(int userId)
        {
            var newUserProfile = _userService.GetUser(userId);
            if (!newUserProfile.IsVendorUser)
            {
                newUserProfile.Vendor = new Vendors
                {
                    Name = UserSession.CurrentOrg.Name
                };
            }
            return Json(new { Result = newUserProfile });
        }

        public ActionResult AddHotWorkPermit(int? projectId, int? thotworkpermitid, string mode, int? mopid, string submit)
        {
            var currentUser = UserSession.CurrentUser;
            UISession.AddCurrentPage("HotWorkPermit_AddHotWorkPermit", 0, "Add Hot Work Permit");
            ViewBag.ProjectId = projectId ?? 0;
            var objhotWorkPermits = _hotWorkPermitService.GetTHotWorksPermit(thotworkpermitid);
            bool isUserAuthorized = true;
            ViewBag.ShowIncomplete = 0;
            ViewBag.IsRequestEdited = "";
            if (thotworkpermitid.HasValue && thotworkpermitid.Value > 0)
            {
                if (objhotWorkPermits != null && objhotWorkPermits.THotWorkPermitId > 0)
                {
                    if (currentUser.IsVendorUser)
                        isUserAuthorized = objhotWorkPermits.CreatedBy == currentUser.UserId;
                }
                else
                {
                    ErrorMessage = ConstMessage.Not_exist;
                    return RedirectToAction("GetAllHotWorkPermit");
                }
                if (objhotWorkPermits.Status < 0 && currentUser.IsVendorUser)                                 
                    ViewBag.ShowIncomplete = 1;                

                if (objhotWorkPermits.Status == 2)               
                    ViewBag.IsRequestEdited = "1";
                
                objhotWorkPermits.PermitAuthorizedBy = (objhotWorkPermits.PermitAuthorizedBy == null && _commonModelFactory.IsAdminUser()) ? currentUser.UserId : objhotWorkPermits.PermitAuthorizedBy;
            }
            else
            {
                objhotWorkPermits.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);
                objhotWorkPermits.PermitRequestByDate = DateTime.Now.ToClientTime();
                if (currentUser != null)
                {
                    objhotWorkPermits.PermitRequestBy = currentUser.UserId;
                    objhotWorkPermits.EmailAddress = currentUser.Email;
                    if (currentUser.IsVendorUser)
                        objhotWorkPermits.Company = currentUser.Vendor.Name;
                    else
                        objhotWorkPermits.Company = UserSession.CurrentOrg.Name;

                    if (!string.IsNullOrEmpty(currentUser.PhoneNumber))
                        objhotWorkPermits.PhoneNumber = currentUser.PhoneNumber;
                    else if (!string.IsNullOrEmpty(currentUser.CellNo))
                        objhotWorkPermits.PhoneNumber = currentUser.CellNo;
                }
            }

            objhotWorkPermits.Company = !string.IsNullOrEmpty(objhotWorkPermits.Company) ? objhotWorkPermits.Company : string.Empty;
            if (mopid > 0)
            {
                TempData["MopId"] = mopid;
            }
            TempData.Keep();
            if (objhotWorkPermits != null)
            {
                objhotWorkPermits.TIcraProject = new TIcraProject();
                if (objhotWorkPermits.ProjectId != 0)
                {
                    objhotWorkPermits.TIcraProject = _icraProjectService.GetCountAllActiveTIcraProject(currentUser.UserId, currentUser.VendorId != null ? currentUser.VendorId : null, false).Where(x => x.ProjectId == objhotWorkPermits.ProjectId).FirstOrDefault();
                    objhotWorkPermits.TIcraProject.mode = "View";
                }
            }

            if (isUserAuthorized)
                return View(objhotWorkPermits);
            else
            {
                ErrorMessage = ConstMessage.NotAuthorized_Msg;
                return RedirectToAction("GetAllHotWorkPermit");
            }
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
        public ActionResult AddHotWorkPermit(THotWorkPermits objTHotWorkPermits, string lstBuildingDetails, string lstFloorDetails, string submit, string IsRequestEdited)
        {
            try
            {
                var currentUser = UserSession.CurrentUser;
                int submitValue = getSubmitButtonValues(submit);
                objTHotWorkPermits.CreatedBy = currentUser.UserId;
                objTHotWorkPermits.UpdatedBy = currentUser.UserId;

                if (submitValue < 0)
                    objTHotWorkPermits.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission);
                else if (submitValue > 0 && objTHotWorkPermits.Status == -1)
                    objTHotWorkPermits.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);


                int hotWorkPermitsId;
                //if (objTHotWorkPermits.THotWorkPermitId == 0)
                //{
                //}
                if (objTHotWorkPermits.ConstructionWorkType.Count > 0)
                {
                    var list = objTHotWorkPermits.ConstructionWorkType.Where(x => x.IsChecked == 1).Select(x => x.Id);
                    objTHotWorkPermits.WorkType = String.Join(",", list);
                }
                if (!string.IsNullOrEmpty(lstBuildingDetails))
                {
                    var buildings = JsonConvert.DeserializeObject<List<Buildings>>(lstBuildingDetails);
                    objTHotWorkPermits.BuildingId = string.Join(",", buildings.Select(x => x.BuildingId));
                    objTHotWorkPermits.BuildingName = string.Join(",", buildings.Select(x => x.BuildingName));
                }
                if (!string.IsNullOrEmpty(lstFloorDetails))
                {
                    var floors = JsonConvert.DeserializeObject<List<Floor>>(lstFloorDetails);
                    objTHotWorkPermits.FloorId = string.Join(",", floors.Select(x => x.FloorId));
                    objTHotWorkPermits.FloorName = string.Join(",", floors.Select(x => x.FloorName));
                }
                if (objTHotWorkPermits.THotWorkPermitId > 0)
                {
                    hotWorkPermitsId = _hotWorkPermitService.UpdateTHotWorkPermits(objTHotWorkPermits);
                    if (hotWorkPermitsId > 0)
                    {
                        hotWorkPermitsId = objTHotWorkPermits.THotWorkPermitId;
                    }
                }
                else
                {
                    hotWorkPermitsId = _hotWorkPermitService.InsertTHotWorkPermits(objTHotWorkPermits);
                }
                if (hotWorkPermitsId > 0)
                {
                    foreach (var items in objTHotWorkPermits.THotWorkItems)
                    {
                        var optiondetails = new THotWorkItems
                        {
                            CreatedBy = objTHotWorkPermits.CreatedBy,
                            UpdatedBy = objTHotWorkPermits.CreatedBy,
                            HotWorkPermitId = hotWorkPermitsId,
                            Item = items.Item,
                            ItemId = items.ItemId,
                            ParentId = items.ParentId
                        };
                        _hotWorkPermitService.InsertTHotWorkItems(optiondetails);
                    }

                    var permitNumber = _hotWorkPermitService.GetTHotWorksPermit(hotWorkPermitsId).PermitNo;
                    if (objTHotWorkPermits.THotWorkPermitId > 0)
                    {                       
                        SuccessMessage = "Permit # " + permitNumber + " " + ConstMessage.Success_Updated;                       
                    }
                    else if (hotWorkPermitsId > 0 && objTHotWorkPermits.THotWorkPermitId == 0)
                    {                        
                        SuccessMessage = "Permit # " + permitNumber + " " + ConstMessage.Success;
                    }
                    else
                        ErrorMessage = ConstMessage.Error;


                    if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.HWP.ToString(), BDO.Enums.NotificationEvent.OnSubmit.ToString())
                        || objTHotWorkPermits.Status != 2)
                    {
                        if (submitValue > 0)
                            SendHotWorkPermitEmail(hotWorkPermitsId, objTHotWorkPermits.BuildingId, IsRequestEdited);
                    }
                    //else if (objTHotWorkPermits.Status != 2)
                    //{
                    //    if (submitValue > 0)
                    //        SendHotWorkPermitEmail(hotWorkPermitsId, objTHotWorkPermits.BuildingId, IsRequestEdited);
                    //}

                    if (TempData["MopId"] != null)
                    {
                        SetRedirectForm(Convert.ToInt32(TempData["MopId"]), 3, hotWorkPermitsId.ToString(), permitNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return RedirectToAction("GetAllHotWorkPermit");
        }
        private void SendHotWorkPermitEmail(int thotworkpermitid, string buildingId, string IsRequestEdited)
        {
            var hotworkpermit = _hotWorkPermitService.GetTHotWorksPermit(thotworkpermitid);
            var objPermitEmailModel = new PermitEmailModel
            {
                // PermitType = $"Hot Work Permit",
                PermitType = BDO.Enums.PermitType.HotWorkPermit,
                PermitNo = hotworkpermit.PermitNo ?? "",
                ProjectName = $"{ hotworkpermit.ProjectName }({hotworkpermit.ProjectNumber })",
                ProjectNo = hotworkpermit.Project.ProjectNumber,
                Requestor = (hotworkpermit.RequestorByUser != null ? hotworkpermit.RequestorByUser.Name : string.Empty),
                RequestedDate = hotworkpermit.PermitRequestByDate,
                ApprovedDate = hotworkpermit.PermitAuthorizedByDate,
                Approver = (hotworkpermit.AuthorizedByUser != null ? hotworkpermit.AuthorizedByUser.Name : string.Empty),
                ApprovalStatus = hotworkpermit.Status,
                Reason = hotworkpermit.Reason,
                RequesterEmail = hotworkpermit.EmailAddress,
                Company = hotworkpermit.Company,
                IsRequestEdited = IsRequestEdited,
                EventId = hotworkpermit.Status == 1 ? Convert.ToInt32(BDO.Enums.NotificationEvent.OnApproved) : Convert.ToInt32(BDO.Enums.NotificationEvent.OnSubmit)
            };
            var base64EncodePdf = _pdfService.HWPPermitInbytes(thotworkpermitid);
            byte[] filebytes = Convert.FromBase64String(base64EncodePdf);
            var filename = $"{hotworkpermit.PermitNo}{".pdf"}";           
            objPermitEmailModel.PermitUrl = "hotworkpermit/addhotWorkpermit?projectid=" + hotworkpermit.ProjectId.ToString() + "&thotworkpermitid=" + hotworkpermit.THotWorkPermitId.ToString();
            _emailService.SendPermitCreatedMail(objPermitEmailModel, Convert.ToInt32(BDO.Enums.NotificationCategory.HWP), filebytes, filename, buildingId, string.Empty, string.Empty);
        }

        public int SetRedirectForm(int TmopId, int FormId, string PermitId, string permitnumber)
        {
            var currentUser = UserSession.CurrentUser;
            var item = new TMopAdditionalForms
            {
                FormId = Convert.ToInt32(FormId),
                TMopId = Convert.ToInt32(TmopId),
                UpdatedBy = currentUser.UserId,
                CreatedBy = currentUser.UserId,
                HasCompleted = 1,
                PermitId = PermitId,
                PermitNo = permitnumber
            };
            int isDone = _permitService.AddTMopAdditionalFormsRedirect(item);
            TempData["MopId"] = null;
            return isDone;
        }

        public JsonResult DeleteHWPDrawings(int HotWorkPermitId, string fileIds)
        {
            var result = _hotWorkPermitService.DeleteHWPDrawings(HotWorkPermitId, fileIds);
            return Json(result);
        }
        public ActionResult DownloadHWPAttachments(int thotworkpermitid)
        {

            var mainRoot = "//temp//";
            THotWorkPermits objhotWorkPermits;
            objhotWorkPermits = _hotWorkPermitService.GetTHotWorksPermit(thotworkpermitid);
            var fileName = Guid.NewGuid() + ".zip";
            var outpathfilename = $"{objhotWorkPermits.PermitNo}{".pdf"}";
            var tempOutPutPath = _commonModelFactory.ServerMapPath("//temp//" + fileName);
            //PermitController ctrl = new PermitController();
         
            DirectoryService.CreateDirectory(string.Format(_commonProvider.GetContentRootPath() + "{0}", mainRoot));
            try
            {

                using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
                {
                    s.SetLevel(9); // 0-9, 9 being the highest compression  

                    byte[] buffer = new byte[2021515];

                    MemoryStream outputMemStream = new MemoryStream();
                    ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

                    zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
                    var base64EncodePdf = _pdfService.HWPPermitInbytes(thotworkpermitid);
                    byte[] fileBytes = Convert.FromBase64String(base64EncodePdf);
                    var filename = $"{objhotWorkPermits.PermitNo}{".pdf"}";
                    var entry = new ZipEntry(filename)
                    {
                        DateTime = DateTime.Now
                    };

                    s.PutNextEntry(entry);
                    MemoryStream inStream = new MemoryStream(fileBytes);
                    StreamUtils.Copy(inStream, s, new byte[2021515]);
                    inStream.Close();
                    //for (int i = 0; i < objhotWorkPermits.Attachments.Count; i++)
                    //{
                    //    entry = new ZipEntry(Path.GetFileName(objhotWorkPermits.Attachments[i].FileName));
                    //    entry.DateTime = DateTime.Now;
                    //    entry.IsUnicodeText = true;
                    //    s.PutNextEntry(entry);
                    //    var req = System.Net.WebRequest.Create(new Uri(Base.Common.FilePath(objhotWorkPermits.Attachments[i].FilePath)));
                    //    using (Stream stream = req.GetResponse().GetResponseStream())
                    //    {
                    //        int sourceBytes;
                    //        do
                    //        {
                    //            sourceBytes = stream.Read(buffer, 0, buffer.Length);
                    //            s.Write(buffer, 0, sourceBytes);
                    //        } while (sourceBytes > 0);
                    //    }

                    //}
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
                _loggingService.LogError(ex.Message);
                return new EmptyResult();
            }

        }
    }
}