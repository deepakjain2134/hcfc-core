using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;

namespace HCF.BAL
{
    public  class DocumentTypeService : IDocumentTypeService
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;
        public DocumentTypeService(IDocumentTypeRepository documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ePdetailId"></param>
        /// <returns></returns>
        public  List<DocumentType> GetDocumentTypes(int ePdetailId)
        {
            return _documentTypeRepository.GetDocumentTypes(ePdetailId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<DocumentType> GetDocumentType()
        {
            return _documentTypeRepository.GetDocumentType();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<DocumentType> GetDocumentTypes()
        {
            return _documentTypeRepository.GetDocumentTypes();
        }

        public  List<DocumentType> GetDocumentTypesMaster()
        {
            return _documentTypeRepository.GetDocumentTypesMaster();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        public  int Save(DocumentType documentType)
        {
            return _documentTypeRepository.AddDocumntType(documentType);
        }

        public  int InsertDocumentTypeMaster(DocumentType documentType) 
        {            
            return _documentTypeRepository.InsertDocumentTypeMaster(documentType);
        }

        public  DocumentType GetDocumentTypesMaster(int docTypeId)
        {
            return _documentTypeRepository.GetDocumentTypeMaster(docTypeId);
        }

        public  int UpdateDocumentTypeMaster(DocumentType documentType)
        {
            return _documentTypeRepository.UpdateDocumntTypeMaster(documentType);
        }

        public  bool DeleteDocumentTypeMaster(int docTypeId)
        {
            return _documentTypeRepository.DeleteDocumentTypeMaster(docTypeId);
        }

        
    }
}
