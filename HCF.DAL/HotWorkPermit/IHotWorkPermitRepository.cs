using System.Collections.Generic;
using HCF.BDO;

namespace HCF.DAL
{
    public interface IHotWorkPermitRepository
    {
        bool DeleteHWPDrawings(int HotWorkPermitId, string fileIds);
        List<THotWorkPermits> GetAllHotWorksPermit();
        THotWorkPermits GetTHotWorksPermit(int? tHotWorkPermitId);
        int InsertTHotWorkPermits(THotWorkPermits thotWorkPermits);
        int Insert_THotWorkItems(THotWorkItems thotWorkitems);
        int UpdateTHotWorkPermits(THotWorkPermits thotWorkPermits);
    }
}