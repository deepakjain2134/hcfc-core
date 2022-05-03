using System.Collections.Generic;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.DAL
{
    public interface IDeviceTestingRepository
    {
        Task<List<TDeviceTesting>> Get();
        int Save(TDeviceTesting deviceTesting);
        int UpdateIssueID(WorkOrder workOrder);
    }
}