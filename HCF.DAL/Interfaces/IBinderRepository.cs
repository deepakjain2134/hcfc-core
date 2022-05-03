using HCF.BDO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public interface IBinderRepository
    {
        int AddBinder(Binders modelData, string epBinders);
        Binders Get(int binderid);
        List<Binders> GetAllBinders();
        Binders GetBinder(int binderId);
        List<EPsDocument> GetBinderDocument(int? binderId, string Year, int? DocTypeId);
        Task<List<EPsDocument>> GetBinderDocumentAsync(int? binderId);



        List<Binders> GetBinders();
        List<Binders> GetBindersList();
        List<StandardEps> GetBinderStandardEps();
        List<Binders> GetEpBinder(int epdetailId);
        Binders GetEPBinderDocument(int? binderId);
        Binders UpdateBinder(Binders modelData, string epBinders);
    }
}