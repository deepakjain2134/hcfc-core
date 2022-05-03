//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//namespace HCF.BAL
//{
//    public static class ReportsRepository
//    {
//        //public static int Save(TInspectionReport tInspectionReport)
//        //{
//        //    return DAL.ReportsRepository.Save(tInspectionReport);
//        //}

//        //public static int Save(TInsReportDetails reportDetails)
//        //{
//        //    return DAL.ReportsRepository.Save(reportDetails);
//        //}

//        //public static List<TInspectionReport> GetInspectionReport()
//        //{
//        //    return DAL.ReportsRepository.GetInspectionReport();
//        //}

//        //public static List<TInspectionReport> GetInspectionReport(int epDetailId)
//        //{
//        //    return DAL.ReportsRepository.GetInspectionReport(epDetailId);
//        //}

//        //public static List<TInspectionReport> GetInspectionReport(int assetId, int floorAssetId)
//        //{
//        //    return DAL.ReportsRepository.GetInspectionReport(assetId, floorAssetId);
//        //}

//        //public static List<TInspectionReport> GetAssetsReport(int assetId)
//        //{
//        //    return DAL.ReportsRepository.GetAssetsReport(assetId);
//        //}
//        //public static List<TInspectionReport> GetInspectionReport(string assetids, int? epDetailId = 0, DateTime? fromdate = null, DateTime? todate = null, int? signedby = null)
//        //{
//        //    return DAL.ReportsRepository.GetInspectionReports(assetids, epDetailId, fromdate, todate, signedby);
//        //}


//        ////public static List<InspectionReport> GetInspectionDetailReports(int year)
//        ////{
//        ////    return DAL.ReportsRepository.GetInspectionDetailReports(year);
//        ////}

//        //public static List<InspectionReport> GetInspectionDetailReports(string SortOrder, string OrderbySort, int year, int userId, int currentUserId, int? categoryId = null)
//        //{
//        //    return DAL.ReportsRepository.GetInspectionDetailReports(SortOrder, OrderbySort, year, userId, currentUserId, categoryId);
//        //}


//        //public static List<TFloorAssets> GetFloorAssetsForReports(string assetId, string ascIds)
//        //{
//        //    return DAL.ReportsRepository.GetFloorAssetsForInspections(assetId, ascIds);
//        //}

//        //public static List<TRounds> GetRoundReport(int locationGroupId, string from, string todate)
//        //{
//        //    return DAL.ReportsRepository.GetRoundReport(locationGroupId, from, todate);
//        //}
//        //public static List<TRounds> GetAssetDeficiencyData(string from, string todate)
//        //{
//        //    return DAL.ReportsRepository.GetAssetDeficiencyData(from, todate);
//        //}




//        //#region CRx Auto Report 

//        //public static HttpResponseMessage GetAutoReportdata()
//        //{
//        //    return DAL.ReportsRepository.GetAutoReportdata();
//        //}

//        //#endregion

//        //#region Compliance Assets Tracking Reports

//        //public static List<object> GetComplianceAssetsTrackingReports(string assetIds, string campusid, string buildingId)
//        //{
//        //    return DAL.ReportsRepository.GetComplianceAssetsTrackingReports(assetIds, campusid, buildingId);
//        //}

//        //#endregion
//    }
//}