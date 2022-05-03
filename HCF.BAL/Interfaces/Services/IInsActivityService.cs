using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IInsActivityService
    {
        List<TInspectionActivity> GetDeficiencyAssets(int userid, string id, string Taggedid);
        List<TInspectionActivity> GetTInspectionActivity(int inspectionId);
        List<TInspectionActivity> GetCurrentActivities();
        List<TInspectionActivity> GetAllInspectionActivity(int? inspectionId);
        List<TInspectionActivity> GetInspectionActivityDetails(int inspectionId);
        List<TInspectionActivity> GetComplianceRpeort(string assetids);
        TInspectionActivity GetInspectionActivitybyTinsStepsId(int TInsStepsId);
        TComment SaveTComment(TComment tComment);

        TInspectionActivity GetRouteInspectionActivity(Guid activityId);
        List<TInspectionActivity> GetMatrixData(DateTime startDate, DateTime endDate);
    }
}