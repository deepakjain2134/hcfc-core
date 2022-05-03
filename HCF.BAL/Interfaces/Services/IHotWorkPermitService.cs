using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IHotWorkPermitService
    {
        bool DeleteHWPDrawings(int HotWorkPermitId, string fileIds);
        List<THotWorkPermits> GetAllHotWorksPermit();
        THotWorkPermits GetTHotWorksPermit(int? tHotWorkPermitId);
        int InsertTHotWorkItems(THotWorkItems tobjhotworkitem);
        int InsertTHotWorkPermits(THotWorkPermits thotWorkPermits);
        int UpdateTHotWorkPermits(THotWorkPermits thotWorkPermits);
    }
}