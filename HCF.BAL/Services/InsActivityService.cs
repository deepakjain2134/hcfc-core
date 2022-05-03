using System;
using System.Collections.Generic;
using HCF.BDO;
using HCF.DAL;

namespace HCF.BAL
{
    public class InsActivityService : IInsActivityService
    {
        private readonly IInspectionActivityRepository _inspectionActivityRepository;
        public InsActivityService(IInspectionActivityRepository inspectionActivityRepository)
        {
            _inspectionActivityRepository = inspectionActivityRepository;
        }
        public List<TInspectionActivity> GetTInspectionActivity(int inspectionId)
        {
            return _inspectionActivityRepository.GetInspectionActivity(inspectionId);
        }

        public List<TInspectionActivity> GetDeficiencyAssets(int userid, string id, string Taggedid)
        {
            return _inspectionActivityRepository.GetDeficientAssets(userid, id, Taggedid);
        }

        public List<TInspectionActivity> GetCurrentActivities()
        {
            return _inspectionActivityRepository.GetCurrentActivities();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TInspectionActivity> GetAllInspectionActivity(int? inspectionId)
        {
            return _inspectionActivityRepository.GetAllInspectionActivity(inspectionId);
        }

        public List<TInspectionActivity> GetInspectionActivityDetails(int inspectionId)
        {
            return _inspectionActivityRepository.GetInspectionActivityDetails(inspectionId);
        }

        public List<TInspectionActivity> GetComplianceRpeort(string assetids)
        {
            return _inspectionActivityRepository.GetComplianceRpeort(assetids);
        }

        public TInspectionActivity GetInspectionActivitybyTinsStepsId(int TInsStepsId)
        {
            return _inspectionActivityRepository.GetInspectionActivitybyTinsStepsId(TInsStepsId);
        }

        public TComment SaveTComment(TComment tComment)
        {
            return _inspectionActivityRepository.SaveTComment(tComment);
        }

        public TInspectionActivity GetRouteInspectionActivity(Guid activityId)
        {
            return _inspectionActivityRepository.GetRouteInspectionActivity(activityId);
        }

        public List<TInspectionActivity> GetMatrixData(DateTime startDate, DateTime endDate)
        {
            return _inspectionActivityRepository.GetMatrixData(startDate, endDate);
        }
    }
}
