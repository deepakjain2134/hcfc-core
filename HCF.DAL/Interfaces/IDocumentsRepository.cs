using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public interface IDocumentsRepository
    {
        bool DeleteBinderFolder(int folderId);
        bool DeleteDocument(int documentRepoId);
        bool DeleteDocumentMaster(int documentId);
        HttpResponseMessage GetAHJDocumentNotificationdata();
        FilesRepository GetAttacheMentFile(int fileId);
        List<BinderFolders> GetBinderFolders(int? folderId);
        List<Inspection> GetDeficiencyDoc();
        Documents GetDocument(int docRepoId);
        DocumentMaster GetDocumentFile(int documentId);
        List<DocumentMaster> GetDocumentMaster(int docTypeId);
        List<Documents> GetInboxMails(int clientNo);
        List<Documents> GetPolicyDocuments(int clientNo);
        int InsertAttachment(FilesRepository newDocument);
        int InsertDocument(Documents newDocument);
        int SaveBinderFolder(BinderFolders objbinderfolders);
        int SaveDocumentMaster(DocumentMaster documentMaster);
        int UpdateAttachDocType(FilesRepository file);
        bool UpdateAttachment(string ids, int docId, int type);
        bool UpdateAttachmentStatus(FilesRepository updateAttachment);
        bool UpdateAttachment(Documents newDoc, int type);

        Task<DocumentSpaceStatus> GetDocumentSpaceStatus(Guid orgkey);
    }
}