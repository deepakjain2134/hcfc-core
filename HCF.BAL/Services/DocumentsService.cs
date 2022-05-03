using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCFBDO = HCF.BDO;

namespace HCF.BAL
{
    public  class DocumentsService : IDocumentsService
    {
        private readonly IDocumentsRepository _documentsRepository;
        private readonly IBinderRepository _binderRepository;

        public DocumentsService(IDocumentsRepository documentsRepository, IBinderRepository binderRepository)
        {
            _documentsRepository = documentsRepository;
            _binderRepository = binderRepository;
        }

        #region Local Documents

        public  Documents GetDocument(int docRepoId)
        {
            return _documentsRepository.GetDocument(docRepoId);
        }

        public  List<Documents> GetInboxMails(int clientNo)
        {
            return _documentsRepository.GetInboxMails(clientNo);
        }

        public  List<Documents> GetPolicyDocuments(int clientNo)
        {
            return _documentsRepository.GetPolicyDocuments(clientNo);
        }

        public  FilesRepository GetAttacheMentFile(int fileId)
        {
            return _documentsRepository.GetAttacheMentFile(fileId);
        }

        public  bool DeleteDocuments(int documentRepoId)
        {
            return _documentsRepository.DeleteDocument(documentRepoId);
        }

        public  bool DeleteDocumentMaster(int documentId)
        {
            return _documentsRepository.DeleteDocumentMaster(documentId);
        }

        internal   bool UpdateAttachment(Documents newDoc, int type)
        {
            var ids = string.Join(",", newDoc.Attachments.Select(x => x.Id));
            var status = _documentsRepository.UpdateAttachment(ids, newDoc.DocumentRepoId, type);
            return status;
        }

        //public  bool UpdateAttachmentStatus(FilesRepository updateAttachment)
        //{
        //    return HCF.DAL.DocumentsRepository.UpdateAttachmentStatus(updateAttachment);
        //}

      
        #endregion       

        #region GetDeficiencyDoc

        public  List<Inspection> GetDeficiencyDoc()
        {
            return _documentsRepository.GetDeficiencyDoc();
        }

        public  DocumentMaster GetDocumentFile(int documentId)
        {
            return _documentsRepository.GetDocumentFile(documentId);
        }

        #endregion

        #region Binders

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelData"></param>
        /// <returns></returns>
        public  int AddBinder(Binders modelData,string epBinders)
        {
            return _binderRepository.AddBinder(modelData, epBinders);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelData"></param>
        /// <returns></returns>
        public  Binders UpdateBinder(Binders modelData,string epBinders)
        {
            return _binderRepository.UpdateBinder(modelData, epBinders);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Binders> GetBinders()
        {
            return _binderRepository.GetBinders();
        }


        public  Binders Get(int binderId)
        {
            return _binderRepository.Get(binderId);
        }


        public  List<Binders> Get()
        {
            return _binderRepository.GetAllBinders();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Binders> GetBindersList()
        {
            var binders= _binderRepository.GetBindersList();
            return binders;
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binderId"></param>
        /// <returns></returns>
        public  Binders GetBinder(int binderId)
        {
            return _binderRepository.GetBinder(binderId);
        }

        public  List<StandardEps> GetBinderStandardEps()
        {
            return _binderRepository.GetBinderStandardEps();
        }
        

        #endregion   

        #region Document Master

        public  int SaveDocumentMaster(DocumentMaster documentMaster)
        {
            return _documentsRepository.SaveDocumentMaster(documentMaster);
        }

        public  int UpdateAttachDocType(FilesRepository file)
        {
            return _documentsRepository.UpdateAttachDocType(file);
        }

        public  List<DocumentMaster> GetDocumentMaster(int docTypeId)
        {
            return _documentsRepository.GetDocumentMaster(docTypeId);
        }

        //public  List<DocumentType>GetDocumentsEP()
        //{
        //    return DAL.DocumentsRepository.GetDocumentsEP();
        //}
        #endregion

        #region Binder Folders

        public  List<BinderFolders> GetBinderFolders(int? folderId)
        {
            return _documentsRepository.GetBinderFolders(folderId);
        }

        public  int SaveBinderFolder(BinderFolders objbinderfolders)
        {
            return _documentsRepository.SaveBinderFolder(objbinderfolders);
        }

        public  bool DeleteBinderFolder(int folderId)
        {
            return _documentsRepository.DeleteBinderFolder(folderId);
        }

        public  HttpResponseMessage GetAHJDocumentNotificationdata()
        {
            return _documentsRepository.GetAHJDocumentNotificationdata();
        }

        #endregion


        #region AHJ Docuemnt

        public async Task<DocumentSpaceStatus> GetDocumentSpaceStatus(Guid orgkey)
        {
            return await _documentsRepository.GetDocumentSpaceStatus(orgkey);
        }

        #endregion
    }
}
