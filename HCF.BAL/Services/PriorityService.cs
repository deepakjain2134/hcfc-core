using HCF.DAL;
using System.Collections.Generic;
namespace HCF.BAL
{
    public  class PriorityService : IPriorityService
    {
        private readonly IPriorityRepository _priorityRepository;
        public PriorityService(IPriorityRepository priorityRepository)
        {
            _priorityRepository = priorityRepository;
        }
        #region Priority

        public  int Save(BDO.Priority newPriority)
        {
            return _priorityRepository.Save(newPriority);
        }

        public  List<BDO.Priority> GetPriority()
        {
            return _priorityRepository.GetPriority();
        }

        #endregion

    }
}
