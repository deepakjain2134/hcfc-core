using HCF.BDO;

namespace HCF.DAL
{
    public interface IEpDocumentRepository
    {
        int Save(EpDocuments epDocuments);
    }
}