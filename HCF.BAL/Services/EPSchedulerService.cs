using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public class EPSchedulerService : IEPSchedulerService
    {
        private static IEPSchedulerRepository _iEPSchedulerRepository;

        public EPSchedulerService(IEPSchedulerRepository iEPSchedulerRepository)
        {
            _iEPSchedulerRepository = iEPSchedulerRepository;

        }
        public int Save(EPScheduler scheduler)
        {
            return _iEPSchedulerRepository.Save(scheduler);
        }

        public List<EPScheduler> GetEPSchedulers()
        {
            return _iEPSchedulerRepository.GetEpSchedulers();
        }
    }
}
