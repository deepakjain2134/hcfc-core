//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace HCF.BAL
//{
//    public static class InsStepsRepository
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="inspectionSteps"></param>
//        /// <param name="createdBy"></param>
//        /// <returns></returns>
//        public static bool Update(InspectionSteps inspectionSteps, int? createdBy)
//        {
//            return DAL.InsStepsRepository.Update(inspectionSteps, createdBy.Value);
//        }

//        public static bool Update(InspectionSteps inspectionSteps, int? createdBy,Guid activityId)
//        {
//            return DAL.InsStepsRepository.Update(inspectionSteps, createdBy.Value, activityId);
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static InspectionSteps GetInspectionSteps(int tInsStepsId)
//        {
//            return DAL.InsStepsRepository.GetInspectionSteps().FirstOrDefault(x => x.TInsStepsId == tInsStepsId);
//        }

//        public static List<InspectionSteps> GetInspectionSteps()
//        {
//            return HCF.DAL.InsStepsRepository.GetInspectionSteps();
//        }

//        public static List<InspectionSteps> GetTinspectionStepsByActivityId(Guid activityId)
//        {
//            return HCF.DAL.InsStepsRepository.GetTinspectionStepsByActivityId(activityId);
//        }


//        public static TInspectionActivity GetTransactionSteps(Guid activityId)
//        {
//            return DAL.InsStepsRepository.GetTransactionSteps(activityId);
//        }

//        //public static List<InspectionSteps> GetTinspectionStepsByFrequency(int? frequencyId, int? floorAssetId, int? stepsId, int? activityType)
//        //{
//        //    return DAL.InsStepsRepository.GetTinspectionStepsByFrequency(frequencyId, floorAssetId, stepsId, activityType);
//        //}
//    }
//}
