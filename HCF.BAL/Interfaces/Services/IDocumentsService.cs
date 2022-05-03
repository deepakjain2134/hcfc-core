using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HCF.BDO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HCF.BAL
{
    public interface IDocumentsService
    {
        int AddBinder(Binders modelData, string epBinders);
        bool DeleteBinderFolder(int folderId);
        bool DeleteDocumentMaster(int documentId);
        bool DeleteDocuments(int documentRepoId);
        List<Binders> Get();
        Binders Get(int binderId);
        HttpResponseMessage GetAHJDocumentNotificationdata();
        FilesRepository GetAttacheMentFile(int fileId);
        Binders GetBinder(int binderId);
        List<BinderFolders> GetBinderFolders(int? folderId);
        Task<DocumentSpaceStatus> GetDocumentSpaceStatus(Guid orgkey);

        List<Binders> GetBinders();
        List<Binders> GetBindersList();
        List<StandardEps> GetBinderStandardEps();
        List<Inspection> GetDeficiencyDoc();
        Documents GetDocument(int docRepoId);
        DocumentMaster GetDocumentFile(int documentId);
        List<DocumentMaster> GetDocumentMaster(int docTypeId);
        List<Documents> GetInboxMails(int clientNo);
        List<Documents> GetPolicyDocuments(int clientNo);
        int SaveBinderFolder(BinderFolders objbinderfolders);
        int SaveDocumentMaster(DocumentMaster documentMaster);
        int UpdateAttachDocType(FilesRepository file);
        Binders UpdateBinder(Binders modelData, string epBinders);
    }
}