using System.Collections.Generic;
using HCF.BDO;
using HCF.DAL;

namespace HCF.BAL
{
    public  class InsStepsTaskService : IInsStepsTaskService
    {
        private readonly IInsStepsTaskRepository _insStepsTaskRepository;
        public InsStepsTaskService(IInsStepsTaskRepository insStepsTaskRepository)
        {
            _insStepsTaskRepository = insStepsTaskRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tInsStepsTask"></param>
        /// <returns></returns>
        public  int Save(TInsStepsTask tInsStepsTask)
        {
            return _insStepsTaskRepository.Save(tInsStepsTask);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tInsStepsId"></param>
        /// <returns></returns>
        public  List<TInsStepsTask> GetTInsStepsTask(int tInsStepsId)
        {
            return _insStepsTaskRepository.GetTInsStepsTask(tInsStepsId);
        }
    }
}
