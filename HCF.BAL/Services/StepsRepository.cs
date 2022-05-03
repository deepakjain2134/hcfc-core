using HCF.BDO;
using System;
using System.Collections.Generic;
using HCF.DAL;

namespace HCF.BAL
{
    public  class StepsService : IStepsService
    {
        private readonly IStepsRepository _stepsRepository;
        public StepsService(IStepsRepository stepsRepository)
        {
            _stepsRepository = stepsRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public  Steps Save(Steps step)
        {
            return _stepsRepository.Save(step);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Steps> GetSteps()
        {
            return _stepsRepository.SetTransSteps();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Steps> GetSteps(int mainGoalId)
        {
            return _stepsRepository.GetSteps(mainGoalId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stepId"></param>
        /// <returns></returns>
        public  Steps GetStep(int stepId) {

            return _stepsRepository.GetStep(stepId);
        }


        public  List<Steps> GetAllSteps()
        {
            return _stepsRepository.GetAllSteps();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isRaSteps"></param>
        /// <returns></returns>
        public  List<Steps> GetRASetps()
        {
            return _stepsRepository.GetSteps(true);
        }

        public  List<TComment> GetStepsComments(Guid activityId,int tIlsmId)
        {
            return _stepsRepository.GetStepsComments(activityId, tIlsmId);
        }

        public  bool Update(Steps step)
        {
            return _stepsRepository.Update(step);
        }
    }
}
