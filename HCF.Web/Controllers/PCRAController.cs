using System;
using System.Collections;
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
using Newtonsoft.Json;
using HCF.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using HCF.Utility.Extensions;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class PCRAController : BaseController
    {
        private readonly IConstructionService _constructionService;
        private readonly IPCRAService _pcraService;
        private readonly ICommonService _commonService;
        private readonly ITIcraProjectService _icraProjectService;
        private readonly IUserService _userService;
        private readonly IPermitService _permitService;
        private readonly IEmailService _emailService;      
        private readonly IPdfService _pdfService;
        private readonly ICommonModelFactory _commonModelFactory;

        public PCRAController( 
            IEmailService emailService, 
            IConstructionService constructionService,
            IPCRAService pcraService, 
            ICommonService commonService, 
            ITIcraProjectService icraProjectService, 
            ICommonModelFactory commonModelFactory, 
            IUserService userService, 
            IPermitService permitService, 
            IPdfService pdfService)
        {
            _commonModelFactory = commonModelFactory;          
            _emailService = emailService;
            _constructionService = constructionService;
            _pcraService = pcraService;
            _permitService = permitService;
            _commonService = commonService;
            _icraProjectService = icraProjectService;
            _userService = userService;
            _pdfService = pdfService;
        }

        #region Question PCRA

        //[CrxAuthorize(Roles = "PCRA_QuestionPCRA")]
        public ActionResult QuestionPCRA(string message)
        {
            UISession.AddCurrentPage("Question_PCRA", 0, "Question PCRA");
            List<QuestionPCRA> lists = new List<QuestionPCRA>();
            lists = _pcraService.GetQuestionPCRA();
            ViewBag.Message = String.IsNullOrEmpty(message) ? message : "";
            return View(lists);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public ActionResult QuestionPCRA(List<QuestionOptionPCRA> questionOptionPCRAs)
        {
            var success = AddEditQuestionOptionPCRA(questionOptionPCRAs);
            if (success.ToLower() == "Question Option inserted successfully".ToLower())
            {
                ViewBag.AddSuccess = "AddSuccess";
            }
            return View(_pcraService.GetQuestionPCRA());
        }

        // [CrxAuthorize(Roles = "PCRA_AddQuestionPCRA")]
        public ActionResult AddQuestionPCRA(int? QuesPCRAId)
        {
            QuestionPCRA objQuestionPCRA = new QuestionPCRA();
            if (QuesPCRAId.HasValue && QuesPCRAId.Value > 0)
            {
                objQuestionPCRA = _pcraService.GetQuestionPCRA().FirstOrDefault(x => x.QuesPCRAId == QuesPCRAId);
            }
            else
            {
                objQuestionPCRA.IsActive = true;
            }
            return View(objQuestionPCRA);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public ActionResult AddQuestionPCRA(QuestionPCRA newQuestionPCRA)
        {
            if (ModelState.IsValid)
            {
                var status = true;
                newQuestionPCRA.CreatedBy = HCF.Web.Base.UserSession.CurrentUser.UserId;
                newQuestionPCRA.QuesPCRAId = _pcraService.SaveQuestionPCRA(newQuestionPCRA);
                if (newQuestionPCRA.QuesPCRAId > 0 && status)
                    SuccessMessage = Utility.ConstMessage.Saved_Successfully;
                return RedirectToAction("QuestionPCRA");
            }
            return View(newQuestionPCRA);
        }

        #endregion

        #region PCRA
        public ActionResult AddPCRA(int? projectId, int? tpcraquestid, string mode, int? version = 1)
        {
            var currentUser = UserSession.CurrentUser;
            ViewBag.ShowIncomplete = 0;
            bool isUserAuthorized = true;
            UISession.AddCurrentPage("PCRA_AddPCRA", 0, "Add PCRA");
            TPCRAQuestion objQuestionTPCRA = new TPCRAQuestion();
            objQuestionTPCRA = _pcraService.GetQuestionTPCRA(projectId, tpcraquestid, null, true);
            TPCRAQuestion objFilteredQuestionTPCRA = new TPCRAQuestion();
            objFilteredQuestionTPCRA = objQuestionTPCRA;
            var data = objQuestionTPCRA.TPCRAQuestionDetails.Where(s => s.QuestionPCRA.IsActive == true).ToList();
            objFilteredQuestionTPCRA.TPCRAQuestionDetails = new List<TPCRAQuestionDetails>();
            objFilteredQuestionTPCRA.TPCRAQuestionDetails = data;
            ViewBag.ProjectId = projectId ?? 0;
            ViewBag.IsRequestEdited = "";
             if (tpcraquestid.HasValue && tpcraquestid > 0)
            {
                if (objQuestionTPCRA != null && objQuestionTPCRA.TPCRAQuesId > 0)
                {
                    if (currentUser.IsVendorUser)
                        isUserAuthorized = objQuestionTPCRA.RequestedBy > 0 && objQuestionTPCRA.RequestedBy == currentUser.UserId;
                    else
                    {

                        if ((objQuestionTPCRA.ContractorId.HasValue && objQuestionTPCRA.ContractorId > 0) && !objQuestionTPCRA.FacilityId.HasValue && !objQuestionTPCRA.SafetyId.HasValue && objQuestionTPCRA.ApprovalStatus > -1)
                        {
                            objQuestionTPCRA.FacilitySignatureDate = DateTime.Now.ToClientTime();
                            objQuestionTPCRA.SafetySignatureDate = DateTime.Now.ToClientTime();
                            //objQuestionTPCRA.InfectionControlSignatureDate = DateTime.Now.ToClientTime();
                        }

                    }
                }
                else
                {
                    ErrorMessage = ConstMessage.Not_exist;
                    return RedirectToAction("GetAllTPCRA");
                }

                if (objQuestionTPCRA.ApprovalStatus < 0 && currentUser.IsVendorUser)
                {
                    //objQuestionTPCRA.ApprovalStatus = Convert.ToInt32(Enums.ApprovalStatus.Requested);
                    ViewBag.ShowIncomplete = 1;
                }
                if (objQuestionTPCRA.ApprovalStatus == 2)
                {
                    ViewBag.IsRequestEdited = "1";
                }
            }
            else
            {
                objQuestionTPCRA.TIcraLog.Version = version.HasValue ? version.Value : 1;
                objQuestionTPCRA.ContractorSignatureDate = DateTime.Now.ToClientTime();
                objQuestionTPCRA.TIcraLog.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);
                objQuestionTPCRA.ApprovalStatus = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);
                objQuestionTPCRA.TIcraLog.StartDate = DateTime.Now.ToClientTime();


                if (currentUser != null)
                {
                    objQuestionTPCRA.TIcraLog.PermitRequestBy = currentUser.UserId;
                    objQuestionTPCRA.RequestedBy = currentUser.UserId;
                    objQuestionTPCRA.EmailAddress = currentUser.Email;
                    objQuestionTPCRA.ContractorId = currentUser.UserId;
                    if (!string.IsNullOrEmpty(currentUser.PhoneNumber))
                        objQuestionTPCRA.Phone = currentUser.PhoneNumber;
                    else if (!string.IsNullOrEmpty(currentUser.CellNo))
                        objQuestionTPCRA.Phone = currentUser.CellNo;
                }
            }
            if (objQuestionTPCRA.TIcraLog != null && objQuestionTPCRA.TIcraLog.TicraId > 0)
            {
                objQuestionTPCRA.TIcraLog.PermitAuthorizedBy = (objQuestionTPCRA.TIcraLog.PermitAuthorizedBy == null && _commonModelFactory.IsAdminUser()) ? currentUser.UserId : objQuestionTPCRA.TIcraLog.PermitAuthorizedBy;

            }
            if (objQuestionTPCRA != null)
            {
                objQuestionTPCRA.TIcraProject = new TIcraProject();
                if (objQuestionTPCRA.ProjectId != 0)
                {
                    objQuestionTPCRA.TIcraProject = _icraProjectService.GetCountAllActiveTIcraProject(currentUser.UserId, currentUser.VendorId != null ? currentUser.VendorId : null, false).Where(x => x.ProjectId == objQuestionTPCRA.ProjectId).FirstOrDefault();
                    objQuestionTPCRA.TIcraProject.mode = "View";

                }

                objQuestionTPCRA.lstUserProfile = new List<UserProfile>();
                objQuestionTPCRA.lstUserProfile = _userService.Get().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email) && x.IsCRxUser == false && x.UserId != 0).ToList();
            }
            if (isUserAuthorized)
                return View(objQuestionTPCRA);
            else
            {
                ErrorMessage = ConstMessage.NotAuthorized_Msg;
                return RedirectToAction("GetAllTPCRA");
            }

        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public ActionResult AddPCRA(TPCRAQuestion objQuestionTPCRA, string submit, string IsRequestEdited)
        {
            try
            {
                int submitValue = getSubmitButtonValues(submit);
                if (submitValue < 0)
                    objQuestionTPCRA.ApprovalStatus = Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission);
                else if (submitValue > 0 && objQuestionTPCRA.ApprovalStatus == -1)
                    objQuestionTPCRA.ApprovalStatus = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);



                objQuestionTPCRA.CreatedBy = UserSession.CurrentUser.UserId;
                objQuestionTPCRA.IsPCRA = true;
                var tPCRAQuesId = _pcraService.InsertQuestionTPCRA(objQuestionTPCRA);
                string PCRANumber;
                if (tPCRAQuesId > 0)
                {
                    PCRANumber = _pcraService.GetQuestionTPCRA(objQuestionTPCRA.ProjectId, tPCRAQuesId, null, true).PCRANumber;
                    if (!string.IsNullOrEmpty(PCRANumber))
                    {
                        foreach (var objpermitworkflow in objQuestionTPCRA.TPermitWorkFlowDetails)
                        {
                            objpermitworkflow.CreatedBy = objQuestionTPCRA.CreatedBy;
                            objpermitworkflow.PermitNumber = PCRANumber;
                            if(objQuestionTPCRA.ApprovalStatus!=0)
                            {
                                _permitService.AddTPermitWorkFlowDetails(objpermitworkflow, objQuestionTPCRA.ProjectId.ToString());
                            }
                           
                        }

                    }
                    if (objQuestionTPCRA.TPCRAQuesId > 0)
                        SuccessMessage = "PCRA# " + PCRANumber + " " + ConstMessage.Success_Updated;
                    else
                        SuccessMessage = "PCRA# " + PCRANumber + " " + ConstMessage.Success;

                    foreach (var tPCRAQuestion in objQuestionTPCRA.TPCRAQuestionDetails)
                    {
                        tPCRAQuestion.CreatedBy = objQuestionTPCRA.CreatedBy;
                        tPCRAQuestion.TPCRAQuesId = tPCRAQuesId;
                        tPCRAQuestion.QuesPCRAId = tPCRAQuestion.QuestionPCRA.QuesPCRAId;
                        _pcraService.InsertQuestionDetailsTPCRA(tPCRAQuestion);
                        foreach (var tquestoptions in tPCRAQuestion.QuestionPCRA.QuestionOptionPCRAs)
                        {
                            var optiondetails = new TPCRAQuestionDetails
                            {
                                CreatedBy = objQuestionTPCRA.CreatedBy,
                                TPCRAQuesId = tPCRAQuesId,
                                PCRAOptionId = tquestoptions.PCRAOptionId,
                                QuesPCRAId = tPCRAQuestion.QuestionPCRA.QuesPCRAId,
                                OptionStatus = tquestoptions.OptionStatus
                            };
                            _pcraService.InsertQuestionDetailsTPCRA(optiondetails);
                        }
                    }


                    if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.PCRA.ToString(), BDO.Enums.NotificationEvent.OnSubmit.ToString()))
                    {
                        if (submitValue > 0)
                            SendPCRAMail(tPCRAQuesId, objQuestionTPCRA.ProjectId, IsRequestEdited);

                    }
                    else if (objQuestionTPCRA.TIcraLog.Status != 2)
                    {
                        if (submitValue > 0)
                            SendPCRAMail(tPCRAQuesId, objQuestionTPCRA.ProjectId, IsRequestEdited);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return RedirectToAction("GetAllTPCRA");
        }
        public ActionResult GetAllTPCRA()
        {
            var currentUser = UserSession.CurrentUser;
            UISession.AddCurrentPage("PCRA_GetAllTCRA", 0, "PCRA");
            var result = new List<TPCRAQuestion>();
            result = _pcraService.GetAllTPCRA(true).Where(x => x.IsCurrent && (x.RiskAssessmentType == 1 || x.RiskAssessmentType == 0)).ToList();
            if (currentUser.IsVendorUser)
                result = result.Where(x => x.RequestedBy == currentUser.UserId).ToList();
            else
                result = result.Where(x => x.ApprovalStatus != Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission) || x.RequestedBy == currentUser.UserId).ToList();
            var TPCRAViewModel = new TPCRAViewModel
            {
                lstPCRAQuestions = result,
                lstPermitSetting = _permitService.GetPermitWorkFlowSettings().Where(x => x.PermitType == 10).ToList()

            };


            return View(TPCRAViewModel);
        }

        private void SendPCRAMail(int tPCRAQuesId, int ProjectId, string IsRequestEdited)
        {
            var pcra = _pcraService.GetQuestionTPCRA(ProjectId, tPCRAQuesId, null, true);
            int CurrentSignSequence = pcra.TPermitWorkFlowDetails != null && pcra.TPermitWorkFlowDetails.Where(x => x.LabelSignId == 0 || x.LabelSignId == null).ToList().Count > 0 ? pcra.TPermitWorkFlowDetails.Where(x => x.LabelSignId == 0 || x.LabelSignId == null).OrderBy(x => x.Sequence).ToList().FirstOrDefault().FormSettingId : 0;
            var objPermitEmailModel = new PermitEmailModel
            {
                //PermitType = $"METHOD OF PROCEDURE [MOP]",
                PermitType = BDO.Enums.PermitType.PCRA,
                PermitNo = pcra.PCRANumber ?? "",
                ProjectName = pcra.Project.ProjectName,
                ProjectNo = pcra.Project.ProjectNumber,
                Requestor = pcra.TPermitWorkFlowDetails != null && pcra.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.HasValue && pcra.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.Value > 0 ? pcra.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().UserDetail.Name : string.Empty,
                RequestedDate = pcra.DateSubmitted,
                Approver = pcra.TPermitWorkFlowDetails != null && pcra.TPermitWorkFlowDetails.OrderByDescending(x => x.Sequence).ToList().FirstOrDefault().LabelValue.HasValue && pcra.TPermitWorkFlowDetails.OrderByDescending(x => x.Sequence).ToList().FirstOrDefault().LabelValue.Value > 0 ? pcra.TPermitWorkFlowDetails.OrderByDescending(x => x.Sequence).ToList().FirstOrDefault().UserDetail.Name : string.Empty,
                ApprovalStatus = pcra.ApprovalStatus,
                Reason = pcra.TIcraLog.ReasonRejection,
                RequesterEmail = pcra.TPermitWorkFlowDetails != null && pcra.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.HasValue && pcra.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.Value > 0 ? pcra.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().UserDetail.Email : string.Empty,
                Company = pcra.TIcraLog.Contractor,
                IsRequestEdited = IsRequestEdited,
                EventId = pcra.ApprovalStatus == 1 ? Convert.ToInt32(BDO.Enums.NotificationEvent.OnApproved) : Convert.ToInt32(BDO.Enums.NotificationEvent.OnSubmit)
            };
            if (objPermitEmailModel != null && CurrentSignSequence > 0)
            {
                objPermitEmailModel.AdditionalNextLevelToEmails = pcra.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null ? pcra.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.NotificationEmail : string.Empty;
                objPermitEmailModel.AdditionalNextLevelCCEmails = pcra.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null ? pcra.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.NotificationCCEmail : string.Empty;
                objPermitEmailModel.SendMailToNextPhase = pcra.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting!=null ?pcra.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.IsSendMail:false;

            }
            var base64EncodePdf = _pdfService.PrintPCRAPdfInbytes(ProjectId, tPCRAQuesId, "", null);
            byte[] filebytes = Convert.FromBase64String(base64EncodePdf);
            var filename = $"{BDO.Enums.NotificationCategory.PCRA.ToString()}{"_"}{pcra.PCRANumber}{".pdf"}";
            objPermitEmailModel.PermitUrl = "pcra/addtpcra?projectId=" + ProjectId + "&tpcraquestid=" + tPCRAQuesId;
            _emailService.SendPermitCreatedMail(objPermitEmailModel, Convert.ToInt32(BDO.Enums.NotificationCategory.PCRA), filebytes, filename, pcra.BuildingId, pcra.FloorName, string.Empty);
        }
        public JsonResult DeletePCRADrawings(int TPCRAQuesNumber, int? TicraId, string fileIds)
        {
            var result = _pcraService.DeletePCRADrawings(TPCRAQuesNumber, fileIds);
            if (TicraId.HasValue)
            {
                var result1 = _constructionService.DeleteICRADrawings(TicraId.Value, fileIds);
            }

            return Json(result);
        }


        #endregion
        #region TPCRA Question

        // [CrxAuthorize(Roles = "PCRA_AddPCRA")]
        public ActionResult AddCRA(int? projectId, int? tpcraquestid, string mode, int? icraId, int? ceilingPermitId, int? linkicra, int? version = 1)
        {

            var currentUser = UserSession.CurrentUser;
            UISession.AddCurrentPage("PCRA_AddCRA", 0, "Add CRA");
            ViewBag.IsCRA = 1;
            ViewBag.ShowIncomplete = 0;
            ViewBag.IsRequestEdited = "";
            TPCRAQuestion objQuestionTPCRA = new TPCRAQuestion();
            objQuestionTPCRA = _pcraService.GetQuestionTPCRA(projectId, tpcraquestid, icraId, false);

            TPCRAQuestion objFilteredQuestionTPCRA = new TPCRAQuestion();
            objFilteredQuestionTPCRA = objQuestionTPCRA;
            var data = objQuestionTPCRA.TPCRAQuestionDetails.Where(s => s.QuestionPCRA.IsActive == true).ToList();
            objFilteredQuestionTPCRA.TPCRAQuestionDetails = new List<TPCRAQuestionDetails>();
            objFilteredQuestionTPCRA.TPCRAQuestionDetails = data;
            if (objQuestionTPCRA != null && icraId.HasValue)
            {
                if (objQuestionTPCRA.ProjectId > 0)
                    projectId = projectId.HasValue ? projectId : objQuestionTPCRA.ProjectId;

                if (objQuestionTPCRA.TPCRAQuesId > 0)
                    tpcraquestid = tpcraquestid.HasValue ? tpcraquestid : objQuestionTPCRA.TPCRAQuesId;

                mode = "view";
            }


            ViewBag.ProjectId = projectId ?? 0;

            //string pageTitle = "Add CRA";
            ViewBag.TIcraLog = "TIcraLog.";
            ViewBag.IsEditable = true;
            if (mode == "edit" && tpcraquestid.HasValue)
            {
               // pageTitle = "Edit CRA";
                ViewBag.IsEditable = true;
            }
            else if (mode == "view" && tpcraquestid.HasValue)
            {
                ViewBag.IsEditable = false;
                //pageTitle = "View CRA";
            }


            //UISession.AddCurrentPage("PCRA_AddCRA", 0, pageTitle);
            ViewBag.ConstructionType = _constructionService.GetConstructionType();
            ViewBag.ConstructionRisk = _constructionService.GetConstructionRisk();
            ViewBag.ConstructionClass = _constructionService.GetConstructionClass();
            //ViewBag.IsEditable = true;
            var enumData = from BDO.Enums.ICRAStatus e in Enum.GetValues(typeof(BDO.Enums.ICRAStatus))
                           select new
                           {
                               Value = (int)e,
                               Text = e.ToString()
                           };
            ViewBag.ICRAStatus = new SelectList(enumData.OrderByDescending(x => x.Value), "Value", "Text");
            objQuestionTPCRA.TIcraLog = TIcraLog((objQuestionTPCRA.TicraId > 0 ? objQuestionTPCRA.TicraId : 0), ViewBag.IsEditable);
            objQuestionTPCRA.CheckBoxRiskAssesmentType = new List<BDO.Enums.EnumModel>();
            foreach (BDO.Enums.RiskAssesmentType RiskAssesmentType in Enum.GetValues(typeof(BDO.Enums.RiskAssesmentType)))
            {
                objQuestionTPCRA.CheckBoxRiskAssesmentType.Add(new BDO.Enums.EnumModel() { RiskAssesmentType = RiskAssesmentType, IsSelected = objQuestionTPCRA.RiskAssessmentType == (int)RiskAssesmentType ? true : false });
            }
            bool isUserAuthorized = true;
            if (tpcraquestid.HasValue && tpcraquestid > 0)
            {
                if (objQuestionTPCRA != null && objQuestionTPCRA.TPCRAQuesId > 0)
                {
                    if (currentUser.IsVendorUser)
                        isUserAuthorized = objQuestionTPCRA.RequestedBy > 0 && objQuestionTPCRA.RequestedBy == currentUser.UserId;


                    if (objQuestionTPCRA.TIcraLog.Status == 2)
                    {
                        ViewBag.IsRequestEdited = "1";
                    }
                }
                else
                {
                    ErrorMessage = ConstMessage.Not_exist;
                    return RedirectToAction("GetAllTCRA");
                }
                if (objQuestionTPCRA.TIcraLog.Status < 0 && currentUser.IsVendorUser)
                {

                    ViewBag.ShowIncomplete = 1;
                }

                if (objQuestionTPCRA.TIcraLog != null && !string.IsNullOrEmpty(objQuestionTPCRA.TIcraLog.ReasonRejection))
                {
                    objQuestionTPCRA.RejectionReason = objQuestionTPCRA.TIcraLog.ReasonRejection;
                }
            }
            else
            {
                objQuestionTPCRA.TIcraLog.Version = version.HasValue ? version.Value : 1;
                objQuestionTPCRA.DateSubmitted = DateTime.Now.ToClientTime();
                //objQuestionTPCRA.ContractorSignatureDate = DateTime.Now.ToClientTime();
               
               
               
                objQuestionTPCRA.TIcraLog.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);
                objQuestionTPCRA.ApprovalStatus = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);
                objQuestionTPCRA.TIcraLog.StartDate = DateTime.Now.ToClientTime();
              
                if (currentUser != null)
                {
                    
                    objQuestionTPCRA.RequestedBy = currentUser.UserId;
                    objQuestionTPCRA.RequestedBy = currentUser.UserId;
                    objQuestionTPCRA.ContractorId = currentUser.UserId;
                    objQuestionTPCRA.EmailAddress = currentUser.Email;
                    if (!string.IsNullOrEmpty(currentUser.PhoneNumber))
                        objQuestionTPCRA.Phone = currentUser.PhoneNumber;
                    else if (!string.IsNullOrEmpty(currentUser.CellNo))
                        objQuestionTPCRA.Phone = currentUser.CellNo;

                    if (objQuestionTPCRA.TIcraLog != null)
                    {
                       
                        if (!string.IsNullOrEmpty(currentUser.PhoneNumber))
                            objQuestionTPCRA.TIcraLog.Telephone = currentUser.PhoneNumber;
                        else if (!string.IsNullOrEmpty(currentUser.CellNo))
                            objQuestionTPCRA.TIcraLog.Telephone = currentUser.CellNo;

                        if (currentUser.IsVendorUser)
                            objQuestionTPCRA.TIcraLog.Contractor = currentUser.Vendor.Name;
                        else
                            objQuestionTPCRA.TIcraLog.Contractor = UserSession.CurrentOrg.Name;

                        objQuestionTPCRA.TIcraLog.PermitRequestBy = currentUser.UserId;

                    }

                }
            }
            //ViewBag.CurrentTime = DateTime.Now.ToClientTime();
            objQuestionTPCRA.TIcraLog.Contractor = !string.IsNullOrEmpty(objQuestionTPCRA.TIcraLog.Contractor) ? objQuestionTPCRA.TIcraLog.Contractor : string.Empty;
            //ViewBag.Mode = mode;
            if (objQuestionTPCRA != null)
            {
                objQuestionTPCRA.TIcraProject = new TIcraProject();
                if (objQuestionTPCRA.ProjectId != 0)
                {
                    objQuestionTPCRA.TIcraProject = _icraProjectService.GetCountAllActiveTIcraProject(currentUser.UserId, currentUser.VendorId != null ? currentUser.VendorId : null, false).Where(x => x.ProjectId == objQuestionTPCRA.ProjectId).FirstOrDefault();
                    objQuestionTPCRA.TIcraProject.mode = "View";

                }
                objQuestionTPCRA.lstUserProfile = new List<UserProfile>();
                objQuestionTPCRA.lstUserProfile = _userService.Get().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email) && x.IsCRxUser == false && x.UserId != 0).ToList();
            }
            if (ceilingPermitId.HasValue && linkicra.HasValue)
            {
                objQuestionTPCRA.TIcraLog.CeilingPermitId = Convert.ToString(ceilingPermitId.Value);
                objQuestionTPCRA.TIcraLog.LinkICRA = "1";
            }
            if (isUserAuthorized)
                return View(objQuestionTPCRA);
            else
            {
                ErrorMessage = ConstMessage.NotAuthorized_Msg;
                return RedirectToAction("GetAllTCRA");
            }

        }

        public TIcraLog TIcraLog(int? icraId, bool iseditable = true, int? version = 1)
        {
            TIcraLog objTIcraLog = new TIcraLog();
            ViewBag.ConstructionType = _constructionService.GetConstructionType();
            ViewBag.ConstructionRisk = _constructionService.GetConstructionRisk();
            ViewBag.ConstructionClass = _constructionService.GetConstructionClass();
            ViewBag.IsEditable = iseditable;
            if (version.HasValue && version.Value > 0)
            {
                ViewBag.ConstructionClass = _constructionService.GetConstructionClass().Where(x => x.Version == version || x.Version == 0).ToList();
            }
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
        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }
        public JsonResult DeleteTIcraFiles(int TicraId, string TFileIds)
        {
            var result = _pcraService.DeleteTIcraFiles(TicraId, TFileIds);
            return Json(result);
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
        public ActionResult AddCRA(TPCRAQuestion objQuestionTPCRA, string attachFiles, string lstBuildingDetails, string IsFacilitySign, string IsInfectionistSign, string IsSafetySign, string submit, string IsRequestEdited)
        {
            try
            {
                int submitValue = getSubmitButtonValues(submit);
                if (submitValue < 0)
                    objQuestionTPCRA.TIcraLog.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission);
                else if (submitValue > 0 && objQuestionTPCRA.TIcraLog.Status == -1)
                    objQuestionTPCRA.TIcraLog.Status = Convert.ToInt32(BDO.Enums.ApprovalStatus.Requested);

                if (objQuestionTPCRA.TPermitWorkFlowDetails != null && objQuestionTPCRA.TPermitWorkFlowDetails.Count > 0)
                {
                    if (objQuestionTPCRA.TIcraLog.Status == 2 || objQuestionTPCRA.TIcraLog.Status == 6)
                    {
                        if (objQuestionTPCRA.TPermitWorkFlowDetails.Where(x => (!string.IsNullOrEmpty(x.DSPermitSignature.FileContent) || x.DSPermitSignature.DigSignatureId > 0 || (x.LabelSignId.HasValue && x.LabelSignId.Value > 0))).ToList().Count < objQuestionTPCRA.TPermitWorkFlowDetails.ToList().Count && objQuestionTPCRA.TPermitWorkFlowDetails.Where(x => x.LabelValue.HasValue && (!string.IsNullOrEmpty(x.DSPermitSignature.FileContent) || x.DSPermitSignature.DigSignatureId > 0 || (x.LabelSignId.HasValue && x.LabelSignId.Value > 0))).ToList().Count > 1)
                        {
                            objQuestionTPCRA.TIcraLog.Status = 6;
                        }
                    }
                }


                objQuestionTPCRA.ApprovalStatus = objQuestionTPCRA.TIcraLog.Status;
                objQuestionTPCRA.RejectionReason = objQuestionTPCRA.TIcraLog.ReasonRejection;
                int projectid = objQuestionTPCRA.ProjectId;
                objQuestionTPCRA.TIcraLog.ProjectId = projectid;
                objQuestionTPCRA.TIcraLog.TDrawingFields = objQuestionTPCRA.TDrawingFields;
                int ticraId = AddTIcraLog(objQuestionTPCRA.TIcraLog, attachFiles, "1");
                objQuestionTPCRA.TicraId = ticraId;
                objQuestionTPCRA.CreatedBy = UserSession.CurrentUser.UserId;
                if (!string.IsNullOrEmpty(lstBuildingDetails))
                {
                    var buildings = JsonConvert.DeserializeObject<List<Buildings>>(lstBuildingDetails);
                    objQuestionTPCRA.BuildingId = string.Join(",", buildings.Select(x => x.BuildingId));
                    objQuestionTPCRA.BuildingName = string.Join(",", buildings.Select(x => x.BuildingName));
                }
                objQuestionTPCRA.IsPCRA = false;
                var tPCRAQuesId = _pcraService.InsertQuestionTPCRA(objQuestionTPCRA);
                string PCRANumber;

                if (tPCRAQuesId > 0)
                {
                    PCRANumber = _pcraService.GetQuestionTPCRA(objQuestionTPCRA.ProjectId, tPCRAQuesId).PCRANumber;
                    if (!string.IsNullOrEmpty(PCRANumber))
                    {
                        foreach (var objpermitworkflow in objQuestionTPCRA.TPermitWorkFlowDetails)
                        {
                            if (objQuestionTPCRA.ApprovalStatus != 0)
                            {
                                objpermitworkflow.CreatedBy = objQuestionTPCRA.CreatedBy;
                                objpermitworkflow.PermitNumber = PCRANumber;
                                _permitService.AddTPermitWorkFlowDetails(objpermitworkflow, objQuestionTPCRA.ProjectId.ToString());
                            }
                        }

                    }
                    //string pcraname = (objQuestionTPCRA.RiskAssessmentType == 1 ? "PCRA# " : "MCRA# ");
                    if (objQuestionTPCRA.TPCRAQuesId > 0)
                        SuccessMessage = "CRA# " + PCRANumber + " " + ConstMessage.Success_Updated;
                    else
                        SuccessMessage = "CRA# " + PCRANumber + " " + ConstMessage.Success;

                    //_pcraService.InsertQuestionTPCRA(objQuestionTPCRA);
                    foreach (var tPCRAQuestion in objQuestionTPCRA.TPCRAQuestionDetails)
                    {
                        tPCRAQuestion.CreatedBy = objQuestionTPCRA.CreatedBy;
                        tPCRAQuestion.TPCRAQuesId = tPCRAQuesId;
                        tPCRAQuestion.QuesPCRAId = tPCRAQuestion.QuestionPCRA.QuesPCRAId;
                        //update ticra id in tpcraquestion
                        _pcraService.InsertQuestionDetailsTPCRA(tPCRAQuestion);
                        foreach (var tquestoptions in tPCRAQuestion.QuestionPCRA.QuestionOptionPCRAs)
                        {
                            var optiondetails = new TPCRAQuestionDetails
                            {
                                CreatedBy = objQuestionTPCRA.CreatedBy,
                                TPCRAQuesId = tPCRAQuesId,
                                PCRAOptionId = tquestoptions.PCRAOptionId,
                                QuesPCRAId = tPCRAQuestion.QuestionPCRA.QuesPCRAId,
                                OptionStatus = tquestoptions.OptionStatus
                            };
                            _pcraService.InsertQuestionDetailsTPCRA(optiondetails);
                        }
                    }
                    if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.CRA.ToString(), BDO.Enums.NotificationEvent.OnSubmit.ToString()))
                    {
                        if (submitValue > 0)
                            SendCRAMail(tPCRAQuesId, objQuestionTPCRA.ProjectId, IsRequestEdited);

                    }
                    else if (objQuestionTPCRA.TIcraLog.Status != 2)
                    {
                        if (submitValue > 0)
                            SendCRAMail(tPCRAQuesId, objQuestionTPCRA.ProjectId, IsRequestEdited);
                    }
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return RedirectToAction("GetAllTCRA");
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


        private void SendCRAMail(int tPCRAQuesId, int ProjectId, string IsRequestEdited)
        {
            var cra = _pcraService.GetQuestionTPCRA(ProjectId, tPCRAQuesId);
            cra.TIcraLog = TIcraLog((cra.TicraId > 0 ? cra.TicraId : 0), true, cra.TIcraLog.Version);
            int CurrentSignSequence = cra.TPermitWorkFlowDetails != null && cra.TPermitWorkFlowDetails.Where(x => x.LabelSignId == 0 || x.LabelSignId == null).ToList().Count > 0 ? cra.TPermitWorkFlowDetails.Where(x => x.LabelSignId == 0 || x.LabelSignId == null).OrderBy(x => x.Sequence).ToList().FirstOrDefault().FormSettingId : 0;
            bool sendmail = true;
            int? EventId = Convert.ToInt32(BDO.Enums.NotificationEvent.OnSubmit);
            string reviewername = string.Empty;
            string revieweremail = string.Empty;
            bool sendmailtoapprover = false;
            if (cra.TIcraLog.Status == 6)
            {
                EventId = Convert.ToInt32(BDO.Enums.NotificationEvent.OnReview);

                if (cra.TPermitWorkFlowDetails.Where(x => x.LabelSignId.HasValue && x.LabelSignId > 0).ToList().Count > 0 == cra.TPermitWorkFlowDetails.ToList().Count > 0 && cra.TPCRAQuesId > 0)
                {
                    reviewername = cra.TPermitWorkFlowDetails != null && cra.TPermitWorkFlowDetails.Where(x => x.LabelSignId.HasValue && x.LabelSignId.Value > 0).ToList().Count > 0 ? cra.TPermitWorkFlowDetails.Where(x => x.LabelSignId.HasValue && x.LabelSignId.Value > 0).OrderByDescending(x => x.DSPermitSignature.CreatedDate).FirstOrDefault().UserDetail.Name : string.Empty;
                    revieweremail = cra.TPermitWorkFlowDetails != null && cra.TPermitWorkFlowDetails.Where(x => x.LabelSignId.HasValue && x.LabelSignId.Value > 0).ToList().Count > 0 ? cra.TPermitWorkFlowDetails.Where(x => x.LabelSignId.HasValue && x.LabelSignId.Value > 0).OrderByDescending(x => x.DSPermitSignature.CreatedDate).FirstOrDefault().UserDetail.Email : string.Empty;
                    sendmailtoapprover = true;
                }
                else
                {
                    reviewername = cra.TPermitWorkFlowDetails != null && cra.TPermitWorkFlowDetails.Where(x => x.LabelSignId.HasValue && x.LabelSignId.Value > 0).ToList().Count > 0 ? cra.TPermitWorkFlowDetails.Where(x => x.LabelSignId.HasValue && x.LabelSignId.Value > 0).OrderByDescending(x => x.DSPermitSignature.CreatedDate).FirstOrDefault().UserDetail.Name : string.Empty;
                    revieweremail = cra.TPermitWorkFlowDetails != null && cra.TPermitWorkFlowDetails.Where(x => x.LabelSignId.HasValue && x.LabelSignId.Value > 0).ToList().Count > 0 ? cra.TPermitWorkFlowDetails.Where(x => x.LabelSignId.HasValue && x.LabelSignId.Value > 0).OrderByDescending(x => x.DSPermitSignature.CreatedDate).FirstOrDefault().UserDetail.Email : string.Empty;
                    sendmailtoapprover = false;
                }

            }
            else if (cra.TIcraLog.Status == 1)
            {
                EventId = Convert.ToInt32(BDO.Enums.NotificationEvent.OnApproval);
            }
            if (sendmail)
            {
                var objPermitEmailModel = new PermitEmailModel
                {
                    //PermitType = $"METHOD OF PROCEDURE [MOP]",
                    PermitType = BDO.Enums.PermitType.CRA,
                    PermitNo = cra.PCRANumber ?? "",
                    ProjectName = cra.TIcraLog.ProjectName,
                    ProjectNo = cra.TIcraLog.ProjectNo,
                    Requestor = cra.TPermitWorkFlowDetails != null && cra.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.HasValue && cra.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().LabelValue.Value > 0 ? cra.TPermitWorkFlowDetails.OrderBy(x => x.Sequence).ToList().FirstOrDefault().UserDetail.Name : string.Empty,
                    RequestedDate = cra.DateSubmitted,
                    //Approver = cra.SafetyId.HasValue && cra.SafetyId.Value > 0 ? cra.SafetyUser.Name : string.Empty,
                    Approver = cra.TPermitWorkFlowDetails != null && cra.TPermitWorkFlowDetails.OrderByDescending(x => x.Sequence).ToList().FirstOrDefault().LabelValue.HasValue && cra.TPermitWorkFlowDetails.OrderByDescending(x => x.Sequence).ToList().FirstOrDefault().LabelValue.Value > 0 ? cra.TPermitWorkFlowDetails.OrderByDescending(x => x.Sequence).ToList().FirstOrDefault().UserDetail.Name : string.Empty,
                    ApprovalStatus = cra.TIcraLog.Status,
                    Reason = cra.TIcraLog.ReasonRejection,
                    RequesterEmail = cra.EmailAddress,
                    Company = cra.TIcraLog.Contractor,
                    Reviewer = reviewername,
                    ReviewerEmail = revieweremail,
                    EventId = EventId,
                    SendMailToApprover = sendmailtoapprover,
                    IsRequestEdited = IsRequestEdited
                };
                if (objPermitEmailModel != null && CurrentSignSequence > 0)
                {
                    objPermitEmailModel.AdditionalNextLevelToEmails = cra.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null ? cra.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.NotificationEmail : string.Empty;
                    objPermitEmailModel.AdditionalNextLevelCCEmails = cra.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null ? cra.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.NotificationCCEmail : string.Empty;
                    //objPermitEmailModel.SendMailToNextPhase = cra.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.IsSendMail;
                    objPermitEmailModel.SendMailToNextPhase = cra.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting != null ? cra.TPermitWorkFlowDetails.Where(x => x.FormSettingId == CurrentSignSequence).FirstOrDefault().PermitSetting.IsSendMail : false;

                }
                var base64EncodePdf = _pdfService.PCRAPermitWorksheetBytes(ProjectId, tPCRAQuesId, cra.TicraId, "CRAPermitWorksheet");
                byte[] filebytes = Convert.FromBase64String(base64EncodePdf);
                var filename = $"{BDO.Enums.NotificationCategory.CRA.ToString()}{"_"}{cra.PCRANumber}{".pdf"}";
                objPermitEmailModel.PermitUrl = "cra/addtcra?projectId=" + ProjectId + "&tpcraquestid=" + tPCRAQuesId;
                _emailService.SendPermitCreatedMail(objPermitEmailModel, Convert.ToInt32(BDO.Enums.NotificationCategory.CRA), filebytes, filename, cra.BuildingId, cra.FloorName, string.Empty);
            }

        }

        public ActionResult GetPCRAActionPlans()
        {
            var questionPCRAs = new List<QuestionPCRA>();
            return PartialView("_GetPCRAActionPlans", questionPCRAs);
        }

        public ActionResult GetProjectData(int? projectid)
        {
            var projectData = new TIcraProject();
            if (projectid.HasValue && projectid.Value > 0)
            {
                projectData = _pcraService.GetProjectDetails().FirstOrDefault(m => m.ProjectId == projectid);
            }
            return PartialView("_GetProjectData", projectData);
        }

        // [CrxAuthorize(Roles = "PCRA_GetAllTPCRA")]
        public ActionResult GetAllTCRA()
        {
            var currentUser = UserSession.CurrentUser;
            UISession.AddCurrentPage("PCRA_GetAllTCRA", 0, "CRA");
            var result = new List<TPCRAQuestion>();
            result = _pcraService.GetAllTCRA().Where(x => x.IsCurrent).ToList();
            if (currentUser.IsVendorUser)
                result = result.Where(x => x.RequestedBy == currentUser.UserId).ToList();
            else
                result = result.Where(x => x.TIcraLog.Status != Convert.ToInt32(BDO.Enums.ApprovalStatus.PendingSubmission) ||
                x.RequestedBy == currentUser.UserId).ToList();
            return View(result);
        }

        public int CheckTPCRAdddorEdit(string projectId)
        {
            var checkedStatus = _pcraService.CheckTPCRAdddorEdit(projectId);
            return checkedStatus;
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
                System.IO.Directory.CreateDirectory(_commonModelFactory.ServerMapPath(Url.Content("temp")));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadCRAAttachments(int projectId, int tpcraquestid, int icraId, string PDFName, int hasattachment)
        {
            var cra = _pcraService.GetQuestionTPCRA(projectId, tpcraquestid);
            var fileName = Guid.NewGuid() + ".zip";
            var outpathfilename = $"{BDO.Enums.NotificationCategory.CRA.ToString()}{"_"}{cra.PCRANumber}{".zip"}";
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
                    var base64EncodePdf = _pdfService.PCRAPermitWorksheetBytes(projectId, tpcraquestid, cra.TicraId, "CRAPermitWorksheet");
                    byte[] fileBytes = Convert.FromBase64String(base64EncodePdf);
                    var filename = $"{BDO.Enums.NotificationCategory.CRA.ToString()}{"_"}{cra.PCRANumber}{".pdf"}";
                    var entry = new ZipEntry(filename)
                    {
                        DateTime = DateTime.Now
                    };

                    s.PutNextEntry(entry);
                    MemoryStream inStream = new MemoryStream(fileBytes);
                    StreamUtils.Copy(inStream, s, new byte[2021515]);
                    inStream.Close();
                    cra.TIcraLog = TIcraLog((cra.TicraId > 0 ? cra.TicraId : 0), false, cra.TIcraLog.Version);
                    for (int i = 0; i < cra.TIcraLog.TICRAFiles.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(cra.TIcraLog.TICRAFiles[i].FileName))
                        {
                            entry = new ZipEntry(Path.GetFileName(cra.TIcraLog.TICRAFiles[i].FileName))
                            {
                                DateTime = DateTime.Now,
                                IsUnicodeText = true
                            };
                            s.PutNextEntry(entry);
                            var req = System.Net.WebRequest.Create(new Uri(_commonModelFactory.FilePath(cra.TIcraLog.TICRAFiles[i].FilePath)));
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
            catch (Exception ex)
            {
               // _loggingService.Error(ex);
                return new EmptyResult();
            }

        }

        #endregion

        #region PCRA Questions

        public ActionResult AddEditQuestionOptionPCRA(int? quesPcraId)
        {
            var questionOptionPCRAs = new List<QuestionOptionPCRA>();
            var questionOptionPCRA = new QuestionOptionPCRA { QuesPCRAId = quesPcraId, IsActive = true };
            questionOptionPCRAs.Add(questionOptionPCRA);
            return PartialView("_AddEditQuestionOptionPCRA", questionOptionPCRAs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public string AddEditQuestionOptionPCRA(List<QuestionOptionPCRA> questionOptionPCRAs)
        {
            string success = "";
            var quesPCRAId = questionOptionPCRAs.FirstOrDefault().QuesPCRAId;
            try
            {
                foreach (var questionOptionPCRA in questionOptionPCRAs)
                {
                    questionOptionPCRA.CreatedBy = UserSession.CurrentUser.UserId;
                    if (questionOptionPCRA.PCRAOptionId == null)
                    {
                        questionOptionPCRA.QuesPCRAId = quesPCRAId;
                    }

                    success = _pcraService.AddEditQuestionOptionPCRA(questionOptionPCRA);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return success;
        }

        public ActionResult EditQuestionOptionPCRA(QuestionOptionPCRA questionOptionPCRA)
        {
            string status = _pcraService.AddEditQuestionOptionPCRA(questionOptionPCRA);
            ViewBag.Mode = "edit";
            return PartialView("_AddEditQuestionOptionPCRA", _pcraService.GetQuestionOptionPCRA(questionOptionPCRA.QuesPCRAId));
        }
        public ActionResult DeleteQuestionOptionPCRA(string pCRAOptionId, string quesPCRAId)
        {
            bool status = _pcraService.DeleteQuestionOptionPCRA(pCRAOptionId, quesPCRAId);
            ViewBag.Mode = "edit";
            return PartialView("_AddEditQuestionOptionPCRA", _pcraService.GetQuestionOptionPCRA(int.Parse(quesPCRAId)));
        }

        public ActionResult GetQuestionOptionPCRAList(string quesPCRAId, string mode)
        {
            string operationMode = "";
            operationMode = String.IsNullOrEmpty(mode) ? "" : (mode == "edit" ? "edit" : "add");

            ViewBag.Mode = operationMode;
            var questionOptionPCRAs = _pcraService.GetQuestionOptionPCRA(String.IsNullOrEmpty(quesPCRAId) ? 0 : int.Parse(quesPCRAId));

            return PartialView(operationMode == "edit" ? "_AddEditQuestionOptionPCRA" : "_QuestionOptionPCRAList", questionOptionPCRAs);
        }
        #endregion
    }
}