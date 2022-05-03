using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL
{
    public  class InsStepsService : IInsStepsService
    {
        private readonly IInsStepsRepository _insStepsRepository;

        private readonly IInsStepsTaskRepository _insStepsTaskRepository;
        public InsStepsService(IInsStepsRepository insStepsRepository, IInsStepsTaskRepository insStepsTaskRepository)
        {
            _insStepsRepository = insStepsRepository;
            _insStepsTaskRepository = insStepsTaskRepository;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionSteps"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public  bool Update(InspectionSteps inspectionSteps, int? createdBy)
        {
            return _insStepsRepository.Update(inspectionSteps, createdBy.Value);
        }

        public  bool Update(InspectionSteps inspectionSteps, int? createdBy,Guid activityId)
        {
            return _insStepsRepository.Update(inspectionSteps, createdBy.Value, activityId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  InspectionSteps GetInspectionSteps(int tInsStepsId)
        {
            return _insStepsRepository.GetInspectionSteps().FirstOrDefault(x => x.TInsStepsId == tInsStepsId);
        }

        public  List<InspectionSteps> GetInspectionSteps()
        {
            return _insStepsRepository.GetInspectionSteps();
        }

        public  List<InspectionSteps> GetTinspectionStepsByActivityId(Guid activityId)
        {
            return _insStepsRepository.GetTinspectionStepsByActivityId(activityId);
        }


        public  TInspectionActivity GetTransactionSteps(Guid activityId)
        {
            return _insStepsRepository.GetTransactionSteps(activityId);
        }

        //public  List<InspectionSteps> GetTinspectionStepsByFrequency(int? frequencyId, int? floorAssetId, int? stepsId, int? activityType)
        //{
        //    return DAL.InsStepsRepository.GetTinspectionStepsByFrequency(frequencyId, floorAssetId, stepsId, activityType);
        //}

        public int Save(TInsStepsTask tInsStepsTask)
        {
            return _insStepsTaskRepository.Save(tInsStepsTask);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tInsStepsId"></param>
        /// <returns></returns>
        public List<TInsStepsTask> GetTInsStepsTask(int tInsStepsId)
        {
            return _insStepsTaskRepository.GetTInsStepsTask(tInsStepsId);
        }
    }
}
