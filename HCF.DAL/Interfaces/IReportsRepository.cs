using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IReportsRepository
    {
        List<TRounds> GetAssetDeficiencyData(string from, string todate);
        List<TInspectionReport> GetAssetsReport(int assetId);
        HttpResponseMessage GetAutoReportdata();
        List<object> GetComplianceAssetsTrackingReports(string assetIds, string campusid, string buildingId);
        List<TInspectionReport> GetEpReport(int inspectionId, int epDetailId);
        List<TFloorAssets> GetFloorAssetsForInspections(string assetId, string ascIds);
        List<InspectionReport> GetInspectionDetailReports(int year, int userId);
        List<InspectionReport> GetInspectionDetailReports(string dataSortOrder, string orderBySort, int year, int userId, int currentUserId, int? categoryId = null);
        List<TInspectionReport> GetInspectionReport();
        List<TInspectionReport> GetInspectionReport(int epDetailId);
        List<TInspectionReport> GetInspectionReport(int assetId, int floorAssetId);
        List<TInspectionReport> GetInspectionReports(string assetids, int? epDetailId = 0, DateTime? fromdate = null, DateTime? todate = null, int? signedby = null);
        List<TRounds> GetRoundReport(int locationGroupId, string from, string todate);
        List<TInsReportDetails> GetTInsReportDetails();
        int Save(TInspectionReport tInspectionReport);
        int Save(TInsReportDetails reportDetails);
        List<InspectionReport> GetInspectionDetail(int epdetailid, int? frequencyid, int year, int month);
    }
}