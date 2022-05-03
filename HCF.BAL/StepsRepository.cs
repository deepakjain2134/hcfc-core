//using HCF.BDO;
//using System;
//using System.Collections.Generic;

//namespace HCF.BAL
//{
//    public static class StepsRepository
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="step"></param>
//        /// <returns></returns>
//        public static Steps Save(Steps step)
//        {
//            return DAL.StepsRepository.Save(step);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<Steps> GetSteps()
//        {
//            return DAL.StepsRepository.SetTransSteps();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<Steps> GetSteps(int mainGoalId)
//        {
//            return DAL.StepsRepository.GetSteps(mainGoalId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="stepId"></param>
//        /// <returns></returns>
//        public static Steps GetStep(int stepId)
//        {

//            return DAL.StepsRepository.GetStep(stepId);
//        }


//        public static List<Steps> GetAllSteps()
//        {
//            return DAL.StepsRepository.GetAllSteps();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="isRaSteps"></param>
//        /// <returns></returns>
//        public static List<Steps> GetRASetps()
//        {
//            return DAL.StepsRepository.GetSteps(true);
//        }

//        public static List<TComment> GetStepsComments(Guid activityId, int tIlsmId)
//        {
//            return DAL.StepsRepository.GetStepsComments(activityId, tIlsmId);
//        }

//        public static bool Update(Steps step)
//        {
//            return DAL.StepsRepository.Update(step);
//        }
//    }
//}
