using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
namespace HCF.BAL
{
    public  class ReportsService : IReportsService
    {
        private readonly IReportsRepository _reportsRepository;
        public ReportsService(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }
        public  int Save(TInspectionReport tInspectionReport)
        {
            return _reportsRepository.Save(tInspectionReport);
        }

        public  int Save(TInsReportDetails reportDetails)
        {
            return _reportsRepository.Save(reportDetails);
        }

        public  List<TInspectionReport> GetInspectionReport()
        {
            return _reportsRepository.GetInspectionReport();
        }

        public  List<TInspectionReport> GetInspectionReport(int epDetailId)
        {
            return _reportsRepository.GetInspectionReport(epDetailId);
        }

        public  List<TInspectionReport> GetInspectionReport(int assetId, int floorAssetId)
        {
            return _reportsRepository.GetInspectionReport(assetId, floorAssetId);
        }

        public  List<TInspectionReport> GetAssetsReport(int assetId)
        {
            return _reportsRepository.GetAssetsReport(assetId);
        }
        public  List<TInspectionReport> GetInspectionReport(string assetids, int? epDetailId = 0, DateTime? fromdate = null, DateTime? todate = null, int? signedby = null)
        {
            return _reportsRepository.GetInspectionReports(assetids, epDetailId, fromdate, todate, signedby);
        }


        //public  List<InspectionReport> GetInspectionDetailReports(int year)
        //{
        //    return DAL.ReportsRepository.GetInspectionDetailReports(year);
        //}

        public  List<InspectionReport> GetInspectionDetailReports(string SortOrder, string OrderbySort, int year, int userId, int currentUserId, int? categoryId = null)
        {
            return _reportsRepository.GetInspectionDetailReports(SortOrder, OrderbySort, year, userId, currentUserId, categoryId);
        }


        public  List<TFloorAssets> GetFloorAssetsForReports(string assetId, string ascIds)
        {
            return _reportsRepository.GetFloorAssetsForInspections(assetId, ascIds);
        }

        public  List<TRounds> GetRoundReport(int locationGroupId, string from, string todate)
        {
            return _reportsRepository.GetRoundReport(locationGroupId, from, todate);
        }
        public  List<TRounds> GetAssetDeficiencyData(string from, string todate)
        {
            return _reportsRepository.GetAssetDeficiencyData(from, todate);
        }

        public List<InspectionReport> GetInspectionDetail(int epdetailid, int? frequencyid, int year, int month)
        {
            return _reportsRepository.GetInspectionDetail(epdetailid, frequencyid, year, month);
        }



        #region CRx Auto Report 

        public  HttpResponseMessage GetAutoReportdata()
        {
            return _reportsRepository.GetAutoReportdata();
        }

        #endregion

        #region Compliance Assets Tracking Reports

        public  List<object> GetComplianceAssetsTrackingReports(string assetIds, string campusid, string buildingId)
        {
            return _reportsRepository.GetComplianceAssetsTrackingReports(assetIds, campusid, buildingId);
        }

        #endregion
    }
}