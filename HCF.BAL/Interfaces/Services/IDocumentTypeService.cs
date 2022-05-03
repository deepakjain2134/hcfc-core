using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IDocumentTypeService
    {
        bool DeleteDocumentTypeMaster(int docTypeId);
        List<DocumentType> GetDocumentType();
        List<DocumentType> GetDocumentTypes();
        List<DocumentType> GetDocumentTypes(int ePdetailId);
        List<DocumentType> GetDocumentTypesMaster();
        DocumentType GetDocumentTypesMaster(int docTypeId);
        int InsertDocumentTypeMaster(DocumentType documentType);
        int Save(DocumentType documentType);
        int UpdateDocumentTypeMaster(DocumentType documentType);
    }
}