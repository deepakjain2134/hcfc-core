using HCF.BDO;
using System.Collections.Generic;

namespace HCF.BAL.Interfaces.Services
{
    public interface IBinderService
    {
        List<EPsDocument> GetBinderDocument(int? binderId, string Year, int? DocTypeId);
        Binders GetEpBinderDocument(int? binderId);
    }
}
