using System;
using System.Collections.Generic;
using System.Linq;
using HCF.BDO;
using HCF.BAL;
using HCF.Web.Base;
using System.IO;
using System.Net;
using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HCF.Utility;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using HCF.BAL.Interfaces.Services;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class DocumentsController : BaseController
    {
        private readonly IDocumentTypeService _documentTypeService;
        private readonly IDocumentsService _documentsService;
        private readonly ILogger<DocumentsController> _loggingService;
        private readonly IHttpPostFactory _httpService;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IBinderService _binderService;

        public DocumentsController(ICommonModelFactory commonModelFactory, ILogger<DocumentsController> logger, IDocumentTypeService documentTypeService, IDocumentsService documentsService,
            IHttpPostFactory httpService, IBinderService binderService)

        {
            _commonModelFactory = commonModelFactory;
            _documentTypeService = documentTypeService;
            _documentsService = documentsService; _loggingService = logger;
            _httpService = httpService;
            _binderService = binderService;


        }
        #region
        public ActionResult DocumentType()
        {
            UISession.AddCurrentPage("Documents_DocumentTypeMaster", 0, "Document Types");
            var docTypes = _documentTypeService.GetDocumentTypesMaster();
            return View(docTypes);
        }

        public ActionResult AddDocumentTypeMaster(int? docTypeId)
        {
            UISession.AddCurrentPage("Documents_AddDocumentTypeMaster", 0, "Add DocumentType Master");
            DocumentType docTypes = null;
            if (docTypeId != null)
            {
                docTypes = _documentTypeService.GetDocumentTypesMaster(Convert.ToInt32(docTypeId));
            }
            return View(docTypes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDocumentTypeMaster(DocumentType documentType)
        {
            var isDocAlredyExist = _documentTypeService.GetDocumentTypesMaster().Any(x => x.DocTypeId == documentType.DocTypeId);
            if (!isDocAlredyExist)
            {
                var isDocNameExists = _documentTypeService.GetDocumentTypesMaster().Any(x => x.Name == documentType.Name);
                if (!isDocNameExists)
                {
                    if (!ModelState.IsValid)
                        return View();
                    documentType.CreatedBy = UserSession.CurrentUser.UserId;

                    string postData = _commonModelFactory.JsonSerialize<DocumentType>(documentType);
                    int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                    string result = _httpService.CallPostMethod(postData, Utility.APIUrls.Repository_AddDocumentTypeMaster, ref statusCode);
                    if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    {
                        HttpResponseMessage httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                        if (httpresponse.Success)
                            SuccessMessage = httpresponse.Message;
                        else
                            ErrorMessage = httpresponse.Message;
                    }
                    else
                        ErrorMessage = Utility.ConstMessage.Internal_Server_Error;
                    SuccessMessage = Utility.ConstMessage.Saved_Successfully;
                    return RedirectToAction("DocumentType");
                }
                else
                    ErrorMessage = Utility.ConstMessage.DocumentTypeMaster_Error;
                return RedirectToAction("DocumentType");
            }
            else
            {
                if (string.IsNullOrEmpty(documentType.FileContent))
                {
                    var document = _documentTypeService.GetDocumentTypesMaster().FirstOrDefault(x => x.DocTypeId == documentType.DocTypeId);
                    if (document != null) documentType.Path = document.Path;
                }
                string postData = _commonModelFactory.JsonSerialize<DocumentType>(documentType);
                if (ModelState.IsValid)
                {
                    documentType.FileName = documentType.Path;
                    var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                    string result = _httpService.CallPostMethod(postData, Utility.APIUrls.Repository_EditDocumentTypeMaster, ref statusCode);
                    var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    {
                        if (httpResponse.Success)
                            SuccessMessage = httpResponse.Message;
                        else
                            ErrorMessage = httpResponse.Message;
                    }
                    else
                        ErrorMessage = Utility.ConstMessage.Internal_Server_Error;
                }
                SuccessMessage = Utility.ConstMessage.Success_Updated;

                return RedirectToAction("DocumentType");
            }
        }
        [HttpGet]
        public ActionResult EditDocumentTypeMaster(int docTypeId)
        {
            UISession.AddCurrentPage("Documents_EditDocumentTypeMaster", 0, "Edit DocumentType Master");
            var docTypes = _documentTypeService.GetDocumentTypesMaster(docTypeId);
            return View(docTypes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDocumentTypeMaster(DocumentType documentType)
        {
            var isDocTypeUpdated = documentType.OldName != documentType.Name;
            var isDocAlreadyExist = _documentTypeService.GetDocumentTypesMaster().Any(x => x.Name == documentType.Name) && isDocTypeUpdated;
            if (!isDocAlreadyExist)
            {
                string postData = _commonModelFactory.JsonSerialize<DocumentType>(documentType);
                if (ModelState.IsValid)
                {
                    documentType.FileName = documentType.Path;
                    var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                    string result = _httpService.CallPostMethod(postData, Utility.APIUrls.Repository_EditDocumentTypeMaster, ref statusCode);
                    var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    {
                        if (httpResponse.Success)
                            SuccessMessage = httpResponse.Message;
                        else
                            ErrorMessage = httpResponse.Message;
                    }
                    else
                        ErrorMessage = ConstMessage.Internal_Server_Error;
                }
                SuccessMessage = ConstMessage.Success_Updated;
                return RedirectToAction("DocumentType");
            }
            else
            {
                ErrorMessage = ConstMessage.DocumentTypeMaster_Error;
                return View(documentType);
            }
        }

        public ActionResult DeleteDocumentTypeMaster(int docTypeId)
        {
            _documentTypeService.DeleteDocumentTypeMaster(docTypeId);
            return RedirectToAction("DocumentType");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddDocumentType(int? fileId, string fileContent, string fileName, int doctypeId, int documentId)
        {
            var objDocumentType = new DocumentType
            {
                CreatedBy = UserSession.CurrentUser.UserId,
                DocTypeId = Convert.ToInt32(doctypeId),
                Name = _documentTypeService.GetDocumentType().FirstOrDefault(x => x.DocTypeId == doctypeId)?.Name
            };
            if (fileId.HasValue)
            {
                objDocumentType.FileContent = string.Empty;
                objDocumentType.Path = _documentsService.GetDocumentFile(fileId.Value).FilePath;
            }
            else if (!string.IsNullOrEmpty(fileContent))
            {
                objDocumentType.FileContent = fileContent;
                objDocumentType.Path = string.Empty;
            }

            objDocumentType.FileName = fileName;
            var postData = _commonModelFactory.JsonSerialize<DocumentType>(objDocumentType);
            if (objDocumentType.DocTypeId > 0)
            {
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                var result = _httpService.CallPostMethod(postData, Utility.APIUrls.Documents_AddDocumentType, ref statusCode);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                {
                    var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    var results = new { Result = true, httpResponse.Message };
                    return Json(results);
                }
            }
            return Json("");
        }

        #endregion

        #region Document Master

        public ActionResult Inbox()
        {
            UISession.AddCurrentPage("Documents_Inbox", 0, "Inbox");
            var docTypes = _documentTypeService.GetDocumentTypes();
            return View(docTypes);
        }


        public ActionResult DocumentMasters(int docTypeId)
        {
            List<DocumentMaster> documentMasters = _documentsService.GetDocumentMaster(docTypeId);
            return PartialView("_documentMasters", documentMasters);
        }

        public JsonResult DeleteDocumentMaster(int documentId)
        {
            var status = _documentsService.DeleteDocumentMaster(documentId);
            return Json(status);
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult SaveDocumentMaster(DocumentMaster documentMaster)
        {
            documentMaster.UploadedBy = UserSession.CurrentUser.UserId;
            documentMaster.IsDeleted = false;
            var postData = _commonModelFactory.JsonSerialize<DocumentType>(documentMaster);
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = _httpService.CallPostMethod(postData, APIUrls.Documents_SaveDocumentMasters, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                var results = new { Result = true, httpresponse.Message, documentId = httpresponse.Result.DocumentMaster.DocumentId, FilePath = _commonModelFactory.FilePath(httpresponse.Result.DocumentMaster.FilePath) };
                return Json(results);
            }
            return Json("");
        }

        public ActionResult DocumentMaster()
        {
            var documentMasters = _documentsService.GetDocumentMaster(0);
            return PartialView("_documents", documentMasters);
        }

        #endregion

        #region Attachments

        public ActionResult UpdateAttachDocType(FilesRepository file)
        {
            _documentsService.UpdateAttachDocType(file);
            return Json("");
        }

        #endregion

        #region Folder System Management


        public JsonResult SearchBinderFolders(bool isSurveyPrepBinder, string searchval)
        {
            var allfolders = _documentsService.GetBinderFolders(null).Where(x => x.IsSurveyPrepBinder == isSurveyPrepBinder).ToList();
            var results = allfolders.Where(x => x.CreatedByUserProfile != null).Where(x => x.CommonName.ToUpper().Contains(searchval.Trim().ToUpper())
            || x.FolderName.ToUpper().Contains(searchval.Trim().ToUpper()) ||
            x.CreatedByUserProfile.FirstName.ToUpper().Contains(searchval.Trim().ToUpper())).
            ToList();
            return Json(results);
        }

        public ActionResult BinderFolders(bool isSurveyPrepBinder, int? folderId, string searchval = null)
        {
            ViewBag.IsSurveyPrepBinder = isSurveyPrepBinder;
            if (folderId.HasValue && folderId.Value == 0)
                folderId = null;
            ViewBag.IsRoot = false;
            var currentPage = new BinderFolders();
            var folders = new List<BinderFolders>();
            var parentNodes = new List<BinderFolders>();
            var search = new List<BinderFolders>();
            var allfolders = _documentsService.GetBinderFolders(folderId).Where(x => x.IsSurveyPrepBinder == isSurveyPrepBinder).ToList();
            if (!folderId.HasValue)
            {
                folders = allfolders.Where(x => x.ParentFolderId == null).ToList();
                ViewBag.IsRoot = true;
            }
            else
            {
                var currentItem = allfolders.FirstOrDefault(x => x.FolderId == folderId.Value);
                if (currentItem != null)
                {
                    folders = currentItem.ChildBinderFolders;
                    parentNodes = FindAllParents(allfolders, currentItem).OrderBy(x => x.CreatedDate).ToList();
                    currentPage = currentItem;
                }
            }
            ViewBag.Routs = parentNodes;
            ViewBag.CurrentPage = currentPage;
            ViewBag.searchedvalue = "false";
            return PartialView("_BinderFolders", folders);
        }
        private static IEnumerable<BinderFolders> FindAllParents(List<BinderFolders> allData, BinderFolders child)
        {
            var parent = allData.FirstOrDefault(x => x.FolderId == child.ParentFolderId);

            if (parent == null)
                return Enumerable.Empty<BinderFolders>();

            return new[] { parent }.Concat(FindAllParents(allData, parent));
        }

        [HttpPost]
        public ActionResult UpdateFileFolder(List<BinderFolders> folders)
        {
            var documentStatus = _documentsService.GetDocumentSpaceStatus(Base.UserSession.CurrentOrg.Orgkey).Result;
            long remainingSpace = documentStatus.RemainingSpace;
            long availablespace = 0;
            long totalfilesize = 0;
            var status = true;
            var msg = "";
            foreach (var folder in folders)
            {
                totalfilesize = totalfilesize + folder.FileSize;
            }
            availablespace = remainingSpace - (totalfilesize / 1000);
            if (availablespace > 0)
            {
                foreach (var folder in folders)
                {
                    folder.CreatedBy = UserSession.CurrentUser.UserId;
                    if (folder.ParentFolderId == 0)
                        folder.ParentFolderId = null;

                    folder.FolderId = _documentsService.SaveBinderFolder(folder);
                }
            }
            else
            {
                status = false;
                msg = "Your storage limit exceed from your available space!";
            }
            var data = new { result = folders, status = status, statusmsg = msg };
            return Json(data);
        }

        [HttpPost]
        public ActionResult EditFileFolder(BinderFolders objfolders)
        {
            objfolders.CreatedBy = UserSession.CurrentUser.UserId;
            objfolders.FolderId = _documentsService.SaveBinderFolder(objfolders);
            return Json(objfolders);
        }

        [HttpGet]
        public ActionResult EditFileFolder(int folderId)
        {
            var obj = _documentsService.GetBinderFolders(null).FirstOrDefault(x => x.FolderId == folderId);
            return PartialView("_editBinderFolder", obj);
        }

        public JsonResult DeleteBinderFolder(int folderId)
        {
            var status = _documentsService.DeleteBinderFolder(folderId);
            return Json(status);
        }


        public ActionResult FolderLists(int fileId)
        {
            ViewBag.FileId = fileId;
            var allfolders = _documentsService.GetBinderFolders(null).Where(x => x.IsSurveyPrepBinder && x.ParentFolderId == null).ToList();
            var parentNodes = new List<BinderFolders>();
            ViewBag.Routs = parentNodes;
            return PartialView("_FolderLists", allfolders);
        }

        public ActionResult FolderListsView(int folderId)
        {
            var currentPage = new BinderFolders();
            List<BinderFolders> folders = new();
            List<BinderFolders> parentNodes = new();
            List<BinderFolders> search = new();
            var allfolders = _documentsService.GetBinderFolders(folderId).Where(x => x.IsSurveyPrepBinder).ToList();
            if (folderId == 0)
            {
                folders = allfolders.Where(x => x.ParentFolderId == null).ToList();
                ViewBag.IsRoot = true;
            }
            else
            {
                var currentItem = allfolders.FirstOrDefault(x => x.FolderId == folderId);
                if (currentItem != null)
                {
                    folders = currentItem.ChildBinderFolders;
                    parentNodes = FindAllParents(allfolders, currentItem).OrderBy(x => x.CreatedDate).ToList();
                    currentPage = currentItem;
                }
            }
            ViewBag.Routs = parentNodes;
            ViewBag.CurrentPage = currentPage;
            if (folderId > 0)
                return PartialView("_folderListView", allfolders.Where(x => x.ParentFolderId == folderId).ToList());
            else
                return PartialView("_folderListView", allfolders.Where(x => x.ParentFolderId == null).ToList());
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult GetZipFile(int folderId)
        {
            int? newFolderId = folderId > 0 ? folderId : 0;
            string folderName = Guid.NewGuid().ToString().Replace("-", "");
            string zipFileName = string.Format("{0}.zip", folderName);
            var mainRoot = _commonModelFactory.ServerMapPath("//zipfiles//" + folderName);
            var allfolders = _documentsService.GetBinderFolders(newFolderId).Where(x => x.IsSurveyPrepBinder).ToList();
            try
            {
                if (!Directory.Exists(mainRoot))
                {
                    DirectoryInfo di = Directory.CreateDirectory(mainRoot);
                }
                CreateList(allfolders, newFolderId, mainRoot);

                var dataDir = _commonModelFactory.ServerMapPath("//zipfiles//");
                ZipFile.CreateFromDirectory(dataDir + folderName, dataDir + zipFileName);
                DirectoryInfo yourRootDir = new DirectoryInfo(mainRoot);
                foreach (DirectoryInfo dir in yourRootDir.GetDirectories())
                    DeleteDirectory(dir.FullName, true);

                foreach (FileInfo file in yourRootDir.GetFiles())
                {
                    file.Delete();
                }
                Directory.Delete(mainRoot, true);
                var fs = new FileStream(dataDir + zipFileName, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);
                return File(
                    fileStream: fs,
                    contentType: System.Net.Mime.MediaTypeNames.Application.Zip,
                    fileDownloadName: "tjc_survey_file.zip");
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex.Message);
                Directory.Delete(mainRoot, true);
            }
            return new EmptyResult();
        }

        public static void DeleteDirectory(string directoryName, bool checkDirectiryExist)
        {
            if (Directory.Exists(directoryName))
                Directory.Delete(directoryName, true);
            else if (checkDirectiryExist)
                throw new SystemException("Directory you want to delete is not exist");
        }

        private void CreateList(List<BinderFolders> folders, int? folderId, string mainRoot)
        {
            List<BinderFolders> tempFolders = new();
            if (folderId == 0)
                tempFolders = folders.Where(x => (x.ParentFolderId == null)).ToList();
            else
                tempFolders = folders.Where(x => (x.ParentFolderId == folderId.Value)).ToList();

            foreach (var item in tempFolders.OrderBy(x => x.IsFolder))
            {
                if (item.IsFolder)
                {
                    DirectoryInfo di = Directory.CreateDirectory(mainRoot + "//" + item.FolderName);
                    var childs = folders.Where(x => x.ParentFolderId == item.FolderId).ToList();
                    if (childs.Any())
                    {
                        CreateList(folders, item.FolderId, mainRoot + "//" + item.FolderName);
                    }
                }

                if (item.File != null && item.TFileId > 0 && !string.IsNullOrEmpty(item.File.FilePath))
                {
                    using (var myWebClient = new WebClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        string fileLocation = $"{mainRoot}/{item.File.FileName}";
                        myWebClient.DownloadFile(_commonModelFactory.FilePath(item.File.FilePath), fileLocation);
                    }
                }
            }
        }
        #endregion

        #region Binder documents
        public ActionResult GetMiscEpDocByBinder(int? id, string Year, int? DocTypeId)
        {
            UISession.AddCurrentPage("document_GetMiscEpDocByBinder", 0, "Other Document");
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

        public ActionResult GetReqEpDocWithoutBinder(int? id, string Year, int? DocTypeId)
        {
            UISession.AddCurrentPage("document_GetReqEpDocWithoutBinder", 0, "Required Documents");
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
        #endregion
    }
}