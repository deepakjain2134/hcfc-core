using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;

namespace HCF.BAL
{
    public class EpDocumentService : IEpDocumentService
    {
        private readonly IEpDocumentRepository _epDocumentRepository;
        public EpDocumentService(IEpDocumentRepository epDocumentRepository)
        {
            _epDocumentRepository = epDocumentRepository;
        }
        public int Save(EpDocuments epDocument)
        {
            return _epDocumentRepository.Save(epDocument);
        }
    }
}
