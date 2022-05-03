using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hcf.Api.Application;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Utility.AppConfig;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api.Controllers
{

    [Route("api/repo")] 
    [ApiController]
    public class RepositoryApiController : ApiController
    {
        //private readonly ILoggingService _loggingService;
        private readonly IDocumentTypeService _documentTypeService;
        private readonly IEmailService _emailService;
        private readonly ICommonService _commonService;
        private readonly IInspectionService _inspectionService;
        private readonly IApiCommon _apiCommon;
        private readonly IDocumentsService _documentsService;
        private readonly IEpService _epService;
        private readonly ITransactionService _transactionService;
        private readonly IFileUpload _fileUpload;
        private readonly IHCFSession _session;
        private readonly IAppSetting _appSettings;

        public RepositoryApiController(IHCFSession session, IAppSetting appSetting, IFileUpload fileUpload, IEpService epService, IDocumentsService documentsService, IApiCommon apiCommon, IInspectionService inspectionService,
            ICommonService commonService

            //,ILoggingService loggingService
            , IDocumentTypeService documentTypeService, ITransactionService transactionService,
            IEmailService emailService)
        {
            _appSettings = appSetting;
            _session = session;
            _fileUpload = fileUpload;
            _epService = epService;
            _documentsService = documentsService;
            _apiCommon = apiCommon;
            _inspectionService = inspectionService;
            _commonService = commonService;
            //_loggingService = loggingService;
            _emailService = emailService;
            _documentTypeService = documentTypeService;
            _transactionService = transactionService;
        }

        #region Documents

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetInbox()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            int clientNo = Convert.ToInt32(_session.ClientNo);
            var inboxMails = _documentsService.GetInboxMails(clientNo);
            if (inboxMails.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Documents = inboxMails };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetPolicyDocuments()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            int clientNo = Convert.ToInt32(_session.ClientNo);
            var documents = _documentsService.GetPolicyDocuments(clientNo);
            if (documents.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Documents = documents };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// </summary>
        /// <param name="documentRepoId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage DeleteDocuments(string documentRepoId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var id = Convert.ToInt32(documentRepoId);
            var status = _documentsService.DeleteDocuments(id);
            _objHttpResponseMessage.Success = status;
            _objHttpResponseMessage.Message = ConstMessage.Document_Deleted_Successfully;
            return _objHttpResponseMessage;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage DeleteDocumentMaster(string documentId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var status = _documentsService.DeleteDocumentMaster(Convert.ToInt32(documentId));
            _objHttpResponseMessage.Success = status;
            _objHttpResponseMessage.Message = ConstMessage.Document_Deleted_Successfully;
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetPolicyBinderHistory(string docTypeId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            // int clientNo = Convert.ToInt32(HcfSession.ClientNo);
            var documentType = _transactionService.PolicyBinderHistory(Convert.ToInt32(docTypeId));
            if (documentType != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { DocumentType = documentType };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }



        /// <summary>
        /// </summary>
        /// <param name="newRejectDoc"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("ReplyDocuments")]
        public HttpResponseMessage ReplyDocuments(Documents newRejectDoc)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var status = true;
            var docs = _documentsService.GetDocument(newRejectDoc.DocumentRepoId);//.Where(x => x.DocumentRepoId == newRejectDoc.DocumentRepoId).ToList();
            if (docs != null)
            {
                docs.Details = newRejectDoc.Details;
                docs.Subject = newRejectDoc.Subject;
                if (docs.Attachments.Count != 0 && newRejectDoc.RemovedAttach != null)
                {
                    foreach (var attach in newRejectDoc.RemovedAttach.Split(','))
                    {
                        docs.Attachments = docs.Attachments.Except(docs.Attachments.Where(a => a.Id == Convert.ToInt32(attach))).ToList();
                    }
                }
                status = _emailService.ReplyDocument(docs);
                _objHttpResponseMessage.Success = status;
                _objHttpResponseMessage.Message = ConstMessage.Document_Reply_Mail_Sent_Successfully;
            }
            else
            {
                status = false;
                _objHttpResponseMessage.Success = status;
                _objHttpResponseMessage.Message = ConstMessage.Document_Not_Found;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sampleDocument"></param>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("sampleDocuments/{epDetailId}")]
        public HttpResponseMessage SampleDocuments(Documents sampleDocument, string epDetailId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var epId = Convert.ToInt32(epDetailId);
            _objHttpResponseMessage.Success = _emailService.SampleDocument(epId, sampleDocument);
            _objHttpResponseMessage.Message = _objHttpResponseMessage.Success ? ConstMessage.Mail_Sent_Successfully : ConstMessage.Mail_UnSuccessful;
            return _objHttpResponseMessage;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage UpdateAttachDocType(FilesRepository file)
        {
            var status = true;
            var _objHttpResponseMessage = new HttpResponseMessage();
            var docMasterId = _documentsService.UpdateAttachDocType(file);
            _objHttpResponseMessage.Success = docMasterId > 0 && status;
            return _objHttpResponseMessage;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDocumentType"></param>
        /// <returns></returns>
        /// 

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addDocumentType")]
        public HttpResponseMessage AddDocumentType(DocumentType objDocumentType)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (!string.IsNullOrEmpty(objDocumentType.Path))
            {
                var fileSavePath = _appSettings.SampleFilePath + DateTime.Now.Ticks + "_" + Path.GetFileName(objDocumentType.Path.ToLower());
                var oldPath = objDocumentType.Path;
                var newPath = fileSavePath;
                var sourceBucketName = _apiCommon.GetBucketName();
                var descBucketName = _apiCommon.GetCommonBucketName();
                //var status = _fileUpload.CreateBucket(descBucketName);
                //if (status)
                _fileUpload.CopyingObject(sourceBucketName, descBucketName, oldPath, newPath);

                objDocumentType.Path = fileSavePath;
            }
            else if (!string.IsNullOrEmpty(objDocumentType.FileContent))
            {
                objDocumentType.Path = _apiCommon.SaveFile(objDocumentType.FileContent, objDocumentType.FileName, "SampleFilePath", null, true).FilePath;
            }
            var docTypeId = _documentTypeService.Save(objDocumentType);
            if (docTypeId > 0)
            {
                _objHttpResponseMessage.Message = ConstMessage.Success;
                _objHttpResponseMessage.Success = true;
                objDocumentType.DocTypeId = docTypeId;
            }
            else { _objHttpResponseMessage.Message = ConstMessage.Error; _objHttpResponseMessage.Success = false; }
            _objHttpResponseMessage.Result = new Result { DocumentType = objDocumentType };
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("saveDocumentMaster")]
        public HttpResponseMessage SaveDocumentMaster(DocumentMaster objDocumentMaster)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (!string.IsNullOrEmpty(objDocumentMaster.FileName))
            {
                if (objDocumentMaster.FileContent != null)
                    objDocumentMaster.FilePath = _apiCommon.SaveFile(objDocumentMaster.FileContent, objDocumentMaster.FileName, "BrowseFilePath", "Reposistory").FilePath;
            }
            var documentId = _documentsService.SaveDocumentMaster(objDocumentMaster);
            if (documentId > 0)
            {
                _objHttpResponseMessage.Message = ConstMessage.Success;
                _objHttpResponseMessage.Success = true;
                objDocumentMaster.DocumentId = documentId;
            }
            else { _objHttpResponseMessage.Message = ConstMessage.Error; _objHttpResponseMessage.Success = false; }
            _objHttpResponseMessage.Result = new Result { DocumentMaster = objDocumentMaster };
            return _objHttpResponseMessage;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetEpDocs()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            bool isDocRequired = true;
            var epdetails = _epService.GetEpDetails(isDocRequired);
            if (epdetails.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { EPDetails = epdetails };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        #endregion

        #region IHCFRepository

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage RepositoryDashboard(string userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var list = _transactionService.RepositoryDashboard(Convert.ToInt32(userId));
            if (list.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result { InspectionDocs = list };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }


        /// <summary>
        /// </summary>
        /// <param name="binderId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetEpDocByBinder(string binderId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            // var list = Transaction.GetEpDocByBinder(Convert.ToInt32(binderId));
            List<HCF.BDO.EPsDocument> lists = new List<EPsDocument>(); //HCF.BAL.BinderRepository.GetBinderDocument(Convert.ToInt32(binderId)).ToList();
            if (lists.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result { EpsDocuments = lists };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="epdetailId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetDocsByEp(string userId, string epdetailId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var epdetails = new EPDetails();
            epdetails = _epService.GetEpDescription(Convert.ToInt32(epdetailId));
            if (epdetails != null)
            {
                epdetails.EpDocs = _transactionService.GetEpsDocument(epdetails.EPDetailId);
                if (epdetails.IsAssetsRequired)
                    epdetails.Assets = _epService.GetEpAssets(epdetails.EPDetailId);

                // epdetails = Transaction.GetDocsbyEp(Convert.ToInt32(userId), Convert.ToInt32(epdetailId));
            }
            _objHttpResponseMessage.Result = new Result { EPDetail = epdetails };
            _objHttpResponseMessage.Success = true;
            return _objHttpResponseMessage;
        }

        #endregion

        #region Reports

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetDocumentReport(string userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var list = _transactionService.GetDocumentReport(Convert.ToInt32(userId));
            if (list.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result { InspectionDocs = list };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
                _objHttpResponseMessage.Success = true;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetDocumentHistorybyFile(string fileId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var list = _transactionService.GetDocumentHistory(Convert.ToInt32(fileId));
            if (list.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result { InspectionDocs = list };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetDeficiencyDoc()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var inspections = _documentsService.GetDeficiencyDoc();
            if (inspections.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result { Inspections = inspections };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }

            return _objHttpResponseMessage;
        }

        #endregion

        #region AddSubBinders



        #endregion


        #region Binders

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addBinders")]
        public HttpResponseMessage AddBinders(Binders modelData)
        {
            string epBinders = string.Empty;
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (modelData.EpBinders.Count > 0)
                epBinders = String.Join(",", modelData.EpBinders.Select(x => x.EPDetailId));

            if (!string.IsNullOrEmpty(modelData.FileContent))
                modelData.FilePath = _apiCommon.SaveFile(modelData.FileContent, modelData.FileName, "BinderImage", "").FilePath;

            if (modelData.BinderId > 0)
                modelData = _documentsService.UpdateBinder(modelData, epBinders);
            else
            {
                modelData.BinderId = _documentsService.AddBinder(modelData, epBinders);
            }
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result
            {
                Binder = modelData
            };
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addDocumentTypeMaster")]
        public HttpResponseMessage AddDocumentTypeMaster(DocumentType documentType)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (!string.IsNullOrEmpty(documentType.FileContent))
                documentType.Path = _apiCommon.SaveFile(documentType.FileContent, documentType.FileName, "SampleFilePath", null).FilePath;

            documentType.DocTypeId = _documentTypeService.InsertDocumentTypeMaster(documentType);
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("editDocumentTypeMaster")]
        public HttpResponseMessage EditDocumentTypeMaster(DocumentType documentType)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();

            if (!string.IsNullOrEmpty(documentType.FileContent))
                documentType.Path = _apiCommon.SaveFile(documentType.FileContent, documentType.FileName, "SampleFilePath", null).FilePath;

            documentType.DocTypeId = _documentTypeService.UpdateDocumentTypeMaster(documentType);
            return _objHttpResponseMessage;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetBinder(string binderId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var binder = _documentsService.GetBinder(Convert.ToInt32(binderId));
            if (binder != null)
            {
                _objHttpResponseMessage.Result = new Result { Binder = binder };
            }
            _objHttpResponseMessage.Success = true;
            return _objHttpResponseMessage;
        }

        #endregion


        #region Policy Binder

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetDocumentEp(string userId, string epDetailId)
        {
            //int EPdetailId = (!string.IsNullOrEmpty(epdetailId)) ? Convert.ToInt32(epdetailId) : 0;
            var _objHttpResponseMessage = new HttpResponseMessage();
            var docs = _transactionService.GetPolicyBinders(Convert.ToInt32(userId), Convert.ToInt32(userId), Convert.ToInt32(epDetailId)).ToList();
            if (Convert.ToInt32(epDetailId) > 0)
            {
                docs = docs.Where(x => x.EPDocument.Any(y => y.EPDetailId == Convert.ToInt32(epDetailId))).ToList();
            }
            if (docs.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { DocumentTypes = docs };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }


        #endregion

        #region 

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetRecentFiles()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var files = _commonService.GetRecentFiles(4);
            if (files.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    TFiles = files
                };
            }
            else
                _objHttpResponseMessage.Message =ConstMessage.No_Records_Found;

            return _objHttpResponseMessage;
        }

        #endregion


        #region Document Notification Data 

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getDocumentNotificationdata")]
        public HttpResponseMessage GetDocumentNotificationdata()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            _objHttpResponseMessage = _documentsService.GetAHJDocumentNotificationdata();
            if (_objHttpResponseMessage.Result != null)
            {
                NotificationEmails objNotificationEmails = new NotificationEmails()
                {
                    NotificationCatId = Convert.ToInt32(BDO.Enums.NotificationCategory.Binders),
                    NotificationEventId = Convert.ToInt32(BDO.Enums.NotificationEvent.OnExpiration)
                };
                _emailService.SendCommonEmail(objNotificationEmails, _objHttpResponseMessage);
            }
            _objHttpResponseMessage.Success = true;
            return _objHttpResponseMessage;
        }

        #endregion Document Notification Data 
    }
}