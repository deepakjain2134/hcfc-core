using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IReportsService
    {
        List<TRounds> GetAssetDeficiencyData(string from, string todate);
        List<TInspectionReport> GetAssetsReport(int assetId);
        HttpResponseMessage GetAutoReportdata();
        List<object> GetComplianceAssetsTrackingReports(string assetIds, string campusid, string buildingId);
        List<TFloorAssets> GetFloorAssetsForReports(string assetId, string ascIds);
        List<InspectionReport> GetInspectionDetailReports(string SortOrder, string OrderbySort, int year, int userId, int currentUserId, int? categoryId = null);
        List<TInspectionReport> GetInspectionReport();
        List<InspectionReport> GetInspectionDetail(int epdetailid, int? frequencyid, int year, int month);
        List<TInspectionReport> GetInspectionReport(int epDetailId);
        List<TInspectionReport> GetInspectionReport(int assetId, int floorAssetId);
        List<TInspectionReport> GetInspectionReport(string assetids, int? epDetailId = 0, DateTime? fromdate = null, DateTime? todate = null, int? signedby = null);
        List<TRounds> GetRoundReport(int locationGroupId, string from, string todate);
        int Save(TInspectionReport tInspectionReport);
        int Save(TInsReportDetails reportDetails);
    }
}