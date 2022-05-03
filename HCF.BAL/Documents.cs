//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using HCFBDO = HCF.BDO;

//namespace HCF.BAL
//{
//    public static class DocumentsRepository
//    {
//        #region Local Documents

//        public static Documents GetDocument(int docRepoId)
//        {
//            return DAL.DocumentsRepository.GetDocument(docRepoId);
//        }

//        public static List<Documents> GetInboxMails(int clientNo)
//        {
//            return DAL.DocumentsRepository.GetInboxMails(clientNo);
//        }

//        public static List<Documents> GetPolicyDocuments(int clientNo)
//        {
//            return DAL.DocumentsRepository.GetPolicyDocuments(clientNo);
//        }

//        public static FilesRepository GetAttacheMentFile(int fileId)
//        {
//            return DAL.DocumentsRepository.GetAttacheMentFile(fileId);
//        }

//        public static bool DeleteDocuments(int documentRepoId)
//        {
//            return DAL.DocumentsRepository.DeleteDocument(documentRepoId);
//        }

//        public static bool DeleteDocumentMaster(int documentId)
//        {
//            return DAL.DocumentsRepository.DeleteDocumentMaster(documentId);
//        }

//        internal  static bool UpdateAttachment(Documents newDoc, int type)
//        {
//            var ids = string.Join(",", newDoc.Attachments.Select(x => x.Id));
//            var status = DAL.DocumentsRepository.UpdateAttachment(ids, newDoc.DocumentRepoId, type);
//            return status;
//        }

//        //public static bool UpdateAttachmentStatus(FilesRepository updateAttachment)
//        //{
//        //    return HCF.DAL.DocumentsRepository.UpdateAttachmentStatus(updateAttachment);
//        //}

      
//        #endregion       

//        #region GetDeficiencyDoc

//        public static List<Inspection> GetDeficiencyDoc()
//        {
//            return DAL.DocumentsRepository.GetDeficiencyDoc();
//        }

//        public static DocumentMaster GetDocumentFile(int documentId)
//        {
//            return DAL.DocumentsRepository.GetDocumentFile(documentId);
//        }

//        #endregion

//        #region Binders

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="modelData"></param>
//        /// <returns></returns>
//        public static int AddBinder(Binders modelData,string epBinders)
//        {
//            return DAL.BinderRepository.AddBinder(modelData, epBinders);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="modelData"></param>
//        /// <returns></returns>
//        public static Binders UpdateBinder(Binders modelData,string epBinders)
//        {
//            return DAL.BinderRepository.UpdateBinder(modelData, epBinders);
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<Binders> GetBinders()
//        {
//            return DAL.BinderRepository.GetBinders();
//        }


//        public static Binders Get(int binderId)
//        {
//            return DAL.BinderRepository.Get(binderId);
//        }


//        public static List<Binders> Get()
//        {
//            return DAL.BinderRepository.GetAllBinders();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<Binders> GetBindersList()
//        {
//            var binders= DAL.BinderRepository.GetBindersList();
//            return binders;
//        } 

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="binderId"></param>
//        /// <returns></returns>
//        public static Binders GetBinder(int binderId)
//        {
//            return DAL.BinderRepository.GetBinder(binderId);
//        }

//        public static List<StandardEps> GetBinderStandardEps()
//        {
//            return DAL.BinderRepository.GetBinderStandardEps();
//        }



//        //public static bool UpdateEpBinder(EpBinder mdl)
//        //{
//        //    return DAL.BinderRepository.UpdateEPBinder(mdl);
//        //}

//        #endregion

//        //public static object DeleteEpBinder(int epBinderId)
//        //{
//        //    return DAL.BinderRepository.DeleteEPBinder(epBinderId);
//        //}


//        #region Document Master

//        public static int SaveDocumentMaster(DocumentMaster documentMaster)
//        {
//            return DAL.DocumentsRepository.SaveDocumentMaster(documentMaster);
//        }

//        public static int UpdateAttachDocType(FilesRepository file)
//        {
//            return DAL.DocumentsRepository.UpdateAttachDocType(file);
//        }

//        public static List<DocumentMaster> GetDocumentMaster(int docTypeId)
//        {
//            return DAL.DocumentsRepository.GetDocumentMaster(docTypeId);
//        }

//        //public static List<DocumentType>GetDocumentsEP()
//        //{
//        //    return DAL.DocumentsRepository.GetDocumentsEP();
//        //}
//        #endregion

//        #region Binder Folders

//        public static List<BinderFolders> GetBinderFolders(int? folderId)
//        {
//            return DAL.DocumentsRepository.GetBinderFolders(folderId);
//        }

//        public static int SaveBinderFolder(BinderFolders objbinderfolders)
//        {
//            return DAL.DocumentsRepository.SaveBinderFolder(objbinderfolders);
//        }

//        public static bool DeleteBinderFolder(int folderId)
//        {
//            return DAL.DocumentsRepository.DeleteBinderFolder(folderId);
//        }

//        public static HttpResponseMessage GetAHJDocumentNotificationdata()
//        {
//            return DAL.DocumentsRepository.GetAHJDocumentNotificationdata();
//        }

//        #endregion
//    }
//}