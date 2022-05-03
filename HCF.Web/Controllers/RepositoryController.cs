using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using HCF.BAL;
using HCF.Web.Base;
using HCF.Web.Filters;
using HCF.BAL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class RepositoryController : BaseController
    {
        private readonly IBinderService _binderService;
        private readonly IDocumentsService _documentsService;
        private readonly IEpService _epService;
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;
        private readonly IHttpPostFactory _httpService;
        private readonly ICommonModelFactory _commonModelFactory;

        public RepositoryController(ICommonModelFactory commonModelFactory, ITransactionService transactionService, IEpService epService,
            IBinderService binderService,
            IDocumentsService documentsService,
            IUserService userService, IHttpPostFactory httpService)
        {
            _commonModelFactory = commonModelFactory;
            _epService = epService;
            _binderService = binderService;
            _documentsService = documentsService;
            _userService = userService;
            _httpService = httpService;
            _transactionService = transactionService;
        }

        #region Repository Document

        //[OutputCache(Duration = 60 * 5)]
        public ActionResult DocRepoIndex()
        {
            UISession.AddCurrentPage("repository_docRepoIndex", 0, "Binders");
            var binders = _documentsService.GetBindersList().Where(x => x.ParentBinderId.HasValue).ToList();
            return View(binders);
        }

        public ActionResult GetEpDocByBinder(int? id,string Year,int? DocTypeId)
        {
            UISession.AddCurrentPage("repository_GetEpDocByBinder", 0, "All Documents");
            ViewBag.BinderName = "ALL";
            var year = Year ?? DateTime.Now.Year.ToString();
            ViewBag.Year = year;
            if (Year == "All")
            { Year = string.Empty; }  
            SetYearList();
            var binderDocument = _binderService.GetBinderDocument(id, Year, DocTypeId);
            if (id > 0)
            {
                var newBinder = binderDocument.FirstOrDefault(x => x.BinderId == id);
                if (newBinder != null)
                {
                    ViewBag.BinderName = newBinder.BinderName;
                }
            }
            return View(binderDocument);
        }

        private void SetYearList()
        {
            var listYear = new List<SelectListItem>();
            int startyear = DateTime.Now.AddYears(-7).Year;
            listYear.Add(new SelectListItem
            {
                Text = @"All",
                Value = "All"
            });
            for (int i = startyear; i <= startyear + 7; i++)
            {
                listYear.Add(new SelectListItem
                {
                    Text = "" + i,
                    Value = "" + i
                });
            }
            ViewBag.YearFilter = listYear.OrderByDescending(x => x.Value);
        }

        public ActionResult Inbox()
        {
            return RedirectToAction("EPDocs", "Goal", new { epId = 0, InspectionId = 0 });
        }

        #endregion

        #region Binders

        [CrxAuthorize(Roles = "repository_binders")]
        public ActionResult Binders()
        {
            UISession.AddCurrentPage("Repository_Binders", 0, "Binders");
            var binders = _documentsService.GetBindersList();
            return View(binders);
        }

        public ActionResult AddBinders(int? binderId)
        {
            var binder = new Binders();
            ViewBag.SelectedEps = null;
            if (binderId.HasValue)
            {
                UISession.AddCurrentPage("Repository_UpdateBinder", 0, "Update Binder");
                binder = _documentsService.Get(binderId.Value);
                var SelectedEps = _documentsService.GetBindersList().Where(x => x.BinderId == binderId);
                ViewBag.SelectedEps = SelectedEps.FirstOrDefault().StandardEps.ToList();
            }
            else
            {
                if (UserSession.CurrentUser.IsPowerUser)
                    UISession.AddCurrentPage("Repository_AddBinders", 0, "Add Binder");
                else
                {
                    return RedirectToAction("Binders");
                }
            }
            ViewBag.Standards = _documentsService.GetBinderStandardEps();

            return View(binder);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddBinders(Binders Binders, string epids)
        {
            if (ModelState.IsValid)
            {
                List<int> epLists = new List<int>();

                if (!string.IsNullOrEmpty(epids))
                    epLists = epids.Split(',').Select(x => int.Parse(x)).ToList();

                List<EpBinder> epBinder = new List<EpBinder>();
                Binders.EpBinders = new List<EpBinder>();
                foreach (var item in epLists)
                {
                    EpBinder binder = new EpBinder
                    {
                        EPDetailId = item
                    };
                    binder.BinderId = binder.BinderId;
                    binder.IsActive = true;
                    Binders.EpBinders.Add(binder);
                }


                Binders.CreatedBy = UserSession.CurrentUser.UserId;
                string postData = _commonModelFactory.JsonSerialize<Binders>(Binders);
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                string result = _httpService.CallPostMethod(postData, Utility.APIUrls.Repository_AddBinders, ref statusCode);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                {
                    var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    if (httpresponse.Success)
                        SuccessMessage = httpresponse.Message;
                    else
                        ErrorMessage = httpresponse.Message;
                }
                else
                    ErrorMessage = Utility.ConstMessage.Internal_Server_Error;
                //TempData["notice"] = "Successfully registered";
                //int insertedId = 8;
                //TempData["msg"] = "<script>alert('" + insertedId + "Recored inserted successfully');</script>";
                SuccessMessage = Utility.ConstMessage.Saved_Successfully;
                return RedirectToAction("Binders", "Repository");
            }
            ViewBag.Standards = _documentsService.GetBinderStandardEps();
            return View(Binders);
        }

        public ActionResult SubBinders(int binderId)
        {
            var binder = _documentsService.Get(binderId);
            var standardEps = _documentsService.GetBinderStandardEps();
            binder.StandardEps = standardEps.Where(x => x.BinderId == binderId).ToList();
            return PartialView("_SubBinders", binder);
        }


        //public ActionResult UpdateBinder(int? binderId)
        //{
        //    UISession.AddCurrentPage("Repository_UpdateBinder", 0,  "Update Binder");
        //    var binder = BAL.DocumentsRepository.GetBinder(Convert.ToInt32(binderId));
        //    if (binder != null)
        //    {
        //        if (binder.EpBinders == null)
        //        {
        //            List<EpBinder> epBinders = new List<EpBinder>();
        //            EpBinder epBinder = new EpBinder
        //            {
        //                EPBinderId = 0,
        //                BinderId = 0,
        //                EPDetailId = 0,
        //                EpDetails = new EPDetails()
        //                {
        //                    EPDetailId = 0,
        //                    Description = "",
        //                    EPNumber = "",
        //                    StDescID = 0,
        //                    Standard = new HCF.BDO.Standard
        //                    {
        //                        StDescID = 0,
        //                        TJCStandard = ""
        //                    }
        //                },
        //            };
        //            binder.EpBinders = epBinders;
        //            epBinders.Add(epBinder);
        //        }
        //    }
        //    ViewBag.Standards = HCF.BAL.StandardRepository.GetStandards();
        //    return View(binder);
        //}

        //[HttpPost]
        //public ActionResult UpdateBinder(Binders vModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        vModel.CreatedBy = UserSession.CurrentUser.UserId;
        //        JavaScriptSerializer js = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
        //        string postData = js.Serialize(vModel);
        //        string uri = "repo/addBinders";
        //        int StatusCode = Convert.ToInt32(HCF.Utility.Enums.HttpReponseStatusCode.Success);
        //        _httpService.CallPostMethod(postData, uri , ref StatusCode);
        //        return RedirectToAction("Binders", "Repository");
        //        //}
        //        //var status = BAL.DocumentsRepository.UpdateBinder(vModel);
        //        //foreach (EpBinder item in vModel.EpBinders)
        //        //{
        //        //    item.CreatedBy = HCF.Web.Base.UserSession.GetUserId();
        //        //    item.BinderId = vModel.BinderId;
        //        //    var EPBinder = BAL.DocumentsRepository.UpdateEPBinder(item);
        //    }
        //    ViewBag.Standards = StandardRepository.GetStandards();
        //    return View(vModel);
        //}

        //public void DeleteEPBinders(int EPBinderId)
        //{
        //    var EPBinder = BAL.DocumentsRepository.DeleteEpBinder(EPBinderId);
        //    //return PartialView();
        //}


        #endregion  

        #region Policy Binder

        public ActionResult Policies()
        {
            UISession.AddCurrentPage("Repository_Policies", 0, "Policies");
            var users = _userService.GetMasterUsers().Where(x => (x.IsActive && !string.IsNullOrEmpty(x.Email) && x.EpsCount > 0) || x.UserId == HCF.Web.Base.UserSession.CurrentUser.UserId).OrderBy(x => x.FullName).ToList();
            return View(users);
        }

        public ActionResult PolicyBinders(int? epId, int? fileId, int? doctypeId, string fileName, int mode = 1)
        {
            UISession.AddCurrentPage("Repository_PolicyBinders", 0, "Policy Binders");
            ViewBag.EPdetailId = epId;
            ViewBag.FileId = fileId;
            ViewBag.Mode = mode;
            ViewBag.DocTypeId = doctypeId;
            ViewBag.TempDocFileName = fileName;

            return View();
        }

        public ActionResult GetPolicylists(int? fileId, int? doctypeId, string tempFileName, int mode = 1)
        {
            int clientNo = UserSession.CurrentOrg.ClientNo;
            ViewBag.DocTypeId = doctypeId;
            var fileRepo = _documentsService.GetPolicyDocuments(clientNo).ToList().SelectMany(x => x.Attachments).ToList();
            if (fileId.HasValue)
            {
                if (mode == 1)
                {
                    fileRepo = fileRepo.Where(x => x.DocumentId == fileId).ToList();
                    if (fileRepo.Any())
                        ViewBag.FileId = fileRepo.FirstOrDefault()?.DocumentId;
                }
                else
                {
                    ViewBag.FileId = fileId;
                }

                ViewBag.TempDocFileName = tempFileName;
            }
            return PartialView("_Policylists", fileRepo);
        }

        public ActionResult GetDocumentEP(string mode, int selectedUser, int? epdetailId, int? dueWitndays, int? inprogress, int pastDueordef = 0)
        {
            ViewBag.PolicyView = mode;
            var docs = _transactionService.GetPolicyBinders(UserSession.CurrentUser.UserId, selectedUser, epdetailId);
            if (epdetailId.HasValue && epdetailId > 0)
                docs = docs.Where(x => x.EPDocument.Any(y => y.EPDetailId == epdetailId.Value)).ToList();
            ViewBag.DueDays = dueWitndays;
            if (dueWitndays.HasValue && dueWitndays.Value > 0)
            {
                docs = docs.Where(x => x.DueWithInDays > 0 && x.DueWithInDays <= Convert.ToInt32(dueWitndays.Value)).ToList();
            }
            if (inprogress.HasValue && inprogress.Value > -1)
            {
                docs = docs.Where(x => x.DocStatus == Convert.ToInt32(inprogress.Value)).ToList();
            }
            if (pastDueordef > 0)
            {
                docs = docs.Where(x => x.DocStatus != 1 && x.DocStatus != 2).ToList();
            }
            return PartialView("_DocumentEP", docs);
        }

        public ActionResult PolicyBinderHistory(int docTypeId)
        {
            UISession.AddCurrentPage("Repository_PolicyBinderHistory", 0, "Policy Binders History");
            var documents = _transactionService.PolicyBinderHistory(docTypeId);
            return View(documents);
        }

        #endregion

        #region Binder New Design 

        public ActionResult EPBinders()
        {
            UISession.AddCurrentPage("Repository_EPBinders", 0, "Binders", true);
            return View();
        }

        public ActionResult GetEP(int id, string epdetailId)
        {
            var ePbinder = _documentsService.GetBinder(id);
            if (!string.IsNullOrEmpty(epdetailId))
                ViewBag.EpdetailID = epdetailId;
            return PartialView("_GetEPsBinders", ePbinder);
        }

        [Obsolete]
        public ActionResult GetEpByBinder(int binderId)
        {
            SetYearList();
            var binderDocument = _binderService.GetEpBinderDocument(binderId);
            if (binderId > 0)
            {
                var newBinder = _documentsService.GetBindersList().FirstOrDefault(x => x.BinderId == binderId);
                if (newBinder != null)
                {
                    ViewBag.BinderName = newBinder.BinderName;
                }
            }
            return PartialView("_getEPDocByBinder", binderDocument.EPdocument);
        }

        public ActionResult GetEpDocs(int epId, int id)
        {
            //SetYearList();
            if (id > 0)
            {
                var newBinder = _documentsService.GetBindersList().FirstOrDefault(x => x.BinderId == id);
                if (newBinder != null)
                {
                    ViewBag.BinderName = newBinder.BinderName;
                }
            }
            var epDetails = _epService.GetEpDescription(epId);
            if (epDetails != null)
            {
                epDetails.EpDocs = _transactionService.GetEpsDocument(epDetails.EPDetailId).Where(x => x.IsDeleted == false).ToList();
                if (epDetails.IsAssetsRequired)
                    epDetails.Assets = _epService.GetEpAssets(epDetails.EPDetailId);
            }
            return PartialView("_GetEpDocs", epDetails);
        }


        public JsonResult GetBinderSearch(string searchString)
        {
            var list = _epService.GetBinderSearch(searchString);
            return Json(list);
        }

        #endregion

        #region Recent Documents

        public ActionResult DocRecentFiles(string epnumber)
        {
            UISession.AddCurrentPage("repository_DocRecentFiles", 0, "Recent Files");
            SetYearList();
            var binderDocument = _binderService.GetBinderDocument(0,"",0);
            var recentBinders = binderDocument.Where(x => x.CreatedDate > DateTime.Now.Date.AddDays(-60)).ToList();
            foreach (var binder in recentBinders)
            {
                if (binder.FullName == "System " || binder.FullName == "system ")
                {
                    binder.FirstName = "CRx";
                }
            }
            return View(binderDocument);
        }

        #endregion


        #region Deficiencies Binders 

        public ActionResult DeficienciesBinder()
        {
            UISession.AddCurrentPage("Repository_DeficienciesBinder", 0, "Deficiencies Binder");
            return View();
        }
        #endregion
    }
}