using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Data;

namespace HCF.DAL
{
    public interface IInspectionActivityRepository
    {
        List<Assets> GetAssetsDashBoard(int userId, int? floorAssetId, int Inprogress = 0, int pastDue = 0, int dueWitndays = 0,
        string assetIds = null, string buildingIds = null, string floorIds = null);
        List<TFloorAssets> GetFilterAssetsReports(string assetids, string buildingId, string floorId, string fromdate, string todate);
        List<TFloorAssets> InventoryInspectionAssetsReport(Request objRequest,
               string ids, string buildingId, string floorId, string fromdate, string todate, string status = null);
        List<TInspectionActivity> GetCurrentTInspectionActivity(int? inspectionId, int epDetailId, int userId);
        List<TInspectionActivity> ConvertToInspectionActivities(DataTable dt);
        List<TInspectionActivity> GetAllInspectionActivity(int? inspectionId);
        List<TInspectionActivity> GetAssetInspectionActivity();
        List<TInspectionActivity> GetAssetInspectionActivityFromToDate(string fromDate = null, string toDate = null);
        List<TInspectionActivity> GetComplianceRpeort(string assetids);
        List<TInspectionActivity> GetCurrentActivities();
        List<TInspectionActivity> GetDeficientAssets(int userId, string id, string Taggedid);
        List<TInspectionActivity> GetFloorAssetInspectionActivity(int floorAssetId);
        List<TInspectionActivity> GetFloorAssetInspectionActivity(int floorassetId, int epdetailId, int userId);
        List<TInspectionActivity> GetInspectionActivity(Guid activityId);
        List<TInspectionActivity> GetInspectionActivity(int inspectionId);        
        List<TInspectionActivity> GetInspectionActivityByEpAssets(int epDetailId, int floorAssetId, int userId);
        TInspectionActivity GetInspectionActivitybyTinsStepsId(int insStepsId);
        List<TInspectionActivity> GetInspectionActivityDetails(int inspectionId);
        List<TInspectionActivity> GetMatrixData(DateTime startDate, DateTime endDate);
        TInspectionActivity GetRouteInspectionActivity(Guid activityId);
        TInspectionActivity GetTInspectionActivity(Guid activityId);
        List<TInspectionActivity> GetTInspectionActivity(int inspectionId);
        int Save(TInspectionActivity inspectionActivity);
        TComment SaveTComment(TComment comment);

        //List<TInspectionActivity> GetInspectionActivity(int inspectionId, int activityType);
    }
}