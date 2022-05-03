using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IDocumentTypeRepository
    {
        int AddDocumntType(DocumentType newDocumentType);
        bool DeleteDocumentTypeMaster(int docTypeId);
        List<DocumentType> GetDocumentType();
        DocumentType GetDocumentType(int docTypeId);
        DocumentType GetDocumentTypeMaster(int docTypeId);
        List<DocumentType> GetDocumentTypes();
        List<DocumentType> GetDocumentTypes(int ePDetailId);
        List<DocumentType> GetDocumentTypesMaster();
        int InsertDocumentTypeMaster(DocumentType newDocumentType);
        int UpdateDocumntTypeMaster(DocumentType documentType);
    }
}