using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public class DeviceTestingService : IDeviceTestingService
    {
        private readonly IDeviceTestingRepository _deviceTestingRepository;
        public DeviceTestingService(IDeviceTestingRepository deviceTestingRepository)
        {
            _deviceTestingRepository = deviceTestingRepository;
        }
        public  async Task<List<TDeviceTesting>> Get()
        {
            return await _deviceTestingRepository.Get().ConfigureAwait(false);
        }
        public  int  Save(TDeviceTesting deviceTesting)
        {
            return _deviceTestingRepository.Save(deviceTesting);
        }

        public  int UpdateIssueID(WorkOrder workOrder)
        {
            return _deviceTestingRepository.UpdateIssueID(workOrder);
        }
    }
}
