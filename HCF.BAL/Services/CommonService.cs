using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public  class CommonService: ICommonService
    {
        private readonly IEmailService _emailService;
        private readonly IDigitalSignRepository _digitalSignRepository;
        private readonly ITFilesRepository _tfilesRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IDocumentsRepository _documentsRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly IFileUpload _fileUpload;

        public CommonService(ICommonRepository commonRepository, IFileUpload fileUpload, IDocumentsRepository documentsRepository, IEmailService emailService, IDigitalSignRepository digitalSignRepository, ITFilesRepository tfilesRepository, INotificationRepository notificationRepository)
        {
            _fileUpload = fileUpload;
            _commonRepository = commonRepository;
            _documentsRepository = documentsRepository;
            _notificationRepository = notificationRepository;
            _tfilesRepository = tfilesRepository;
            _emailService = emailService;
            _digitalSignRepository = digitalSignRepository;

        }
        #region Org



        public  Organization GetMasterOrg(Guid orgKey)
        {
            return _commonRepository.GetMasterOrg(orgKey);
        }

        /// <summary>
        /// This method is used to check that we need to send email or not on particular event.
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="Event"></param>
        /// <returns></returns>
        public bool IsSendMail(string Category, string Event)
        {            
            bool status = false;
            NotificationMapping obj = MailNotification(Category, Event);
            if (obj != null)            
                status = obj.Status == 1;           
            return status;
        }

        public NotificationMapping MailNotification(string Category, string Event)
        {
            return _notificationRepository.GetNotificationMatrix().FirstOrDefault(x => x.NotificationEvent.EventName == Event.Trim() && x.NotificationCategory.EnumValue == Category.Trim());
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void SendConfigurationEmail()
        {
            if (IsSendMail(BDO.Enums.NotificationCategory.ConfigurationalItem.ToString(), BDO.Enums.NotificationEvent.OnChanged.ToString()))
            {
                _emailService.SendConfigurationMail(Convert.ToInt32(BDO.Enums.NotificationCategory.ConfigurationalItem));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Audit> GetAuditConfiguration()
        {
            return _commonRepository.GetAuditConfiguration();
        }


        public  int SaveDigitalFile(DigitalSignature objDigitalSignature, string folderName)
        {
            if (objDigitalSignature != null)
            {
                if (!string.IsNullOrEmpty(objDigitalSignature.FileName) && !string.IsNullOrEmpty(objDigitalSignature.FileContent))
                {
                    objDigitalSignature.FilePath = _fileUpload.SaveFileUsingContent(objDigitalSignature.FileContent,
                        objDigitalSignature.FileName,
                       "DigitalSignPath", folderName).FilePath;
                }

                if ((!string.IsNullOrEmpty(objDigitalSignature.FileName) && !string.IsNullOrEmpty(objDigitalSignature.FileContent)) 
                    && (objDigitalSignature.DigSignatureId > 0))
                {
                    //donothing signature is already saved in this case at the time of save signature so remove duplicay 
                }
                else
                {
                    objDigitalSignature.DigSignatureId = _digitalSignRepository.AddDigitalSignature(objDigitalSignature);
                }
                return objDigitalSignature.DigSignatureId;
            }
            else
                return 0;
           
        }


        #region T Files

        public TFiles SaveTFile(TFiles item, int uploadedBy, string folderName)
        {
            if (item != null && !string.IsNullOrEmpty(item.FileContent) && !item.IsDeleted)
            {
                var fileObject = _fileUpload.SaveFileUsingContent(item.FileContent, item.FileName, "TFiles", folderName);
                if (fileObject != null)
                {
                    item.UploadedBy = uploadedBy;
                    item.FilePath = fileObject.FilePath;
                    item.MD5Digest = fileObject.MD5Digest;
                    item.TFileId = _tfilesRepository.Save(item);
                }
            }
            return item;
        }

        public List<TFiles> FindUploadeFiles(string key, string fileName, int userId)
        {
            return _commonRepository.FindUploadeFiles(key, fileName, userId);
        }


        public List<TFiles> SaveTFiles(List<TFiles> tFiles, int uploadedBy)
        {
            if (tFiles != null)
            {
                string folderName = DateTime.UtcNow.Ticks.ToString();
                foreach (var item in tFiles.Where(x => x.AttachmentId > 0))
                {
                    var file = _documentsRepository.GetAttacheMentFile(item.AttachmentId.Value);
                    if (file != null)
                    {
                        if(item.FileSize == 0 && file.FileSize==0)
                            item.FileSize = _fileUpload.GetFileSize(file.FilePath).Result;                        

                        item.UploadedBy = uploadedBy;
                        item.FilePath = file.FilePath;
                        item.FileName = file.FileName;                                             
                        item.TFileId = _tfilesRepository.Save(item);
                    }
                }
                foreach (var item in tFiles.Where(x => x.AttachmentId == null || x.AttachmentId.Value == 0))
                    if (!string.IsNullOrEmpty(item.FileContent) && !item.IsDeleted)
                    {
                        item.UploadedBy = uploadedBy;
                        item.FilePath = _fileUpload.SaveFileUsingContent(item.FileContent, item.FileName, "TFiles", folderName).FilePath;
                        item.TFileId = _tfilesRepository.Save(item);
                    }
                    else if (!string.IsNullOrEmpty(item.FilePath))
                    {
                        item.UploadedBy = uploadedBy;
                        item.TFileId = _tfilesRepository.Save(item);
                        if (item.IsDeleted)
                        {
                            item.TFileId = 0;
                        }
                    }
            }
            return tFiles;
        }

        public TFiles GetFile(int fileId, string tbName)
        {
            return _tfilesRepository.GetFile(fileId, tbName);

        }

        public TFiles GetFile(int fileId)
        {
            return _tfilesRepository.GetFile(fileId,"");

        }

        public List<TFiles> GetTFiles(string files)
        {
            return _tfilesRepository.GetTFiles(files);
        }         

        public List<TFiles> GetRecentFiles(int userId)
        {
            return _commonRepository.GetRecentFiles(userId);
        }

        public async Task<bool> UpdateFileSize(TFiles item) {

            if (item.FileSize == 0)
                item.FileSize =await _fileUpload.GetFileSize(item.FilePath);

            if (item.FileSize > 0)
                item.TFileId = _tfilesRepository.Save(item);
            return true;
        }

        #endregion

        #region Drawingpathway
        public  List<DrawingpathwayFiles> GetUploadedDrawings(string files)
        {
            return _commonRepository.GetUploadedDrawings(files);
        }
        #endregion
    }
}
