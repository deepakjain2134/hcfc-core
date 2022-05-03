//using HCF.BDO;
//using HCF.Utility;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace HCF.BAL
//{
//    public static class CommonRepository
//    {
//        #region Org

//        public static List<BDO.Organization> GetMasterOrg()
//        {
//            return DAL.CommonRepository.GetMasterOrg();
//        }

//        public static BDO.Organization GetMasterOrg(Guid orgKey)
//        {
//            return DAL.CommonRepository.GetMasterOrg(orgKey);
//        }

//        /// <summary>
//        /// This method is used to check that we need to send email or not on particular event.
//        /// </summary>
//        /// <param name="Category"></param>
//        /// <param name="Event"></param>
//        /// <returns></returns>
//        public static bool IsSendMail(string Category, string Event)
//        {
//            NotificationMapping obj = new NotificationMapping();
//            bool status = false;
//            obj = NotificationRepository.GetNotificationMatrix().FirstOrDefault(x => x.NotificationEvent.EventName == Event.Trim() && x.NotificationCategory.EnumValue == Category.Trim());
//            if (obj != null)
//            {
//                status = obj.Status == 1 ? true : false;
//            }
//            return status;
//        }

//        public static NotificationMapping MailNotification(string Category, string Event)
//        {
//            NotificationMapping obj = new NotificationMapping();
//            //bool status = false;
//            obj = NotificationRepository.GetNotificationMatrix().FirstOrDefault(x => x.NotificationEvent.EventName == Event.Trim() && x.NotificationCategory.EnumValue == Category.Trim());
//            //if (obj != null)
//            //{
//            //    status = obj.Status == 1 ? true : false;
//            //}
//            return obj;
//        }

//        #endregion

//        /// <summary>
//        /// 
//        /// </summary>
//        public static void SendConfigurationEmail()
//        {
//            if (IsSendMail(Enums.NotificationCategory.ConfigurationalItem.ToString(), Enums.NotificationEvent.OnChanged.ToString()))
//            {
//                Email.SendConfigurationMail(Convert.ToInt32(Enums.NotificationCategory.ConfigurationalItem));
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<Audit> GetAuditConfiguration()
//        {
//            return DAL.CommonRepository.GetAuditConfiguration();
//        }


//        public static int SaveDigitalFile(DigitalSignature objDigitalSignature, string folderName)
//        {
//            if (objDigitalSignature != null)
//            {
//                if (!string.IsNullOrEmpty(objDigitalSignature.FileName) && !string.IsNullOrEmpty(objDigitalSignature.FileContent)  )
//                {
//                    objDigitalSignature.FilePath = AmazonFileUpload.SaveFileUsingContent(objDigitalSignature.FileContent,
//                        objDigitalSignature.FileName,
//                        "DigitalSignPath", folderName).FilePath;
                    

//                }

//                if ((!string.IsNullOrEmpty(objDigitalSignature.FileName) && !string.IsNullOrEmpty(objDigitalSignature.FileContent)) && (objDigitalSignature.DigSignatureId>0))
//                {
//                    //donothing signature is already saved in this case at the time of save signature so remove duplicay 
//                }
//                else
//                {
                    
//                    objDigitalSignature.DigSignatureId = DigitalSignRepository.Save(objDigitalSignature);
//                }
//            }
//            return objDigitalSignature.DigSignatureId;
//        }


//        #region T Files

//        public static TFiles SaveTFile(TFiles item, int uploadedBy, string folderName)
//        {
//            if (item != null && !string.IsNullOrEmpty(item.FileContent) && item.IsDeleted == false)
//            {
//                item.UploadedBy = uploadedBy;
//                item.FilePath = AmazonFileUpload.SaveFileUsingContent(item.FileContent, item.FileName, "TFiles", folderName).FilePath;
//                item.TFileId = DAL.TFilesRepository.Save(item);
//            }
//            return item;
//        }



//        public static List<TFiles> SaveTFiles(List<TFiles> tFiles, int uploadedBy)
//        {
//            if (tFiles != null)
//            {
//                string folderName = DateTime.UtcNow.Ticks.ToString();
//                foreach (var item in tFiles.Where(x => x.AttachmentId > 0))
//                {
//                    var file = DocumentsRepository.GetAttacheMentFile(item.AttachmentId.Value);
//                    if (file != null)
//                    {
//                        item.UploadedBy = uploadedBy;
//                        item.FilePath = file.FilePath;
//                        item.FileName = file.FileName;
//                        //item.FileType = (item.FileType ==null) ? Enums.FileType.MiscDocuments :item.FileType;
//                        item.TFileId = DAL.TFilesRepository.Save(item);
//                    }
//                }
//                foreach (var item in tFiles.Where(x => x.AttachmentId == null || x.AttachmentId.Value == 0))
//                    if (!string.IsNullOrEmpty(item.FileContent) && item.IsDeleted == false)
//                    {
//                        item.UploadedBy = uploadedBy;
//                        item.FilePath = AmazonFileUpload.SaveFileUsingContent(item.FileContent, item.FileName, "TFiles", folderName).FilePath;
//                        item.TFileId = DAL.TFilesRepository.Save(item);
//                    }
//                    else if (!string.IsNullOrEmpty(item.FilePath))
//                    {
//                        item.UploadedBy = uploadedBy;
//                        item.TFileId = DAL.TFilesRepository.Save(item);
//                        if (item.IsDeleted)
//                        {
//                            item.TFileId = 0;
//                        }
//                    }
//            }
//            return tFiles;
//        }


//        public static TFiles GetFile(int fileId, string tbName)
//        {
//            return DAL.TFilesRepository.GetFile(fileId, tbName);

//        }

//        public static TFiles GetFile(int fileId)
//        {
//            return DAL.TFilesRepository.GetFile(fileId,"");

//        }

//        public static List<TFiles> GetTFiles(string files)
//        {
//            return DAL.CommonRepository.GetTFiles(files);
//        }

//        #endregion

//        #region Files

//        public static List<FileHistory> GetRecentFiles()
//        {
//            return DAL.CommonRepository.GetRecentFiles();
//        }

//        //public static List<TFiles> GetTFiles(string files)
//        //{
//        //    return DAL.CommonRepository.GetTFiles(files);
//        //}

//        #endregion
//        #region Drawingpathway
//        public static List<DrawingpathwayFiles> GetUploadedDrawings(string files)
//        {
//            return DAL.CommonRepository.GetUploadedDrawings(files);
//        }
//        #endregion
//    }
//}
