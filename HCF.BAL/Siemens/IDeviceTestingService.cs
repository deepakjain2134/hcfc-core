using System.Collections.Generic;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IDeviceTestingService
    {
        Task<List<TDeviceTesting>> Get();
        int Save(TDeviceTesting deviceTesting);
        int UpdateIssueID(WorkOrder workOrder);
    }
}