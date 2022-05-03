using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Data;

namespace HCF.DAL
{
    public interface IEpRepository
    {
        List<DashboardDetails> GetDashBoardData(int userId);
        List<EPDetails> GetEpDetailsAsync();
        IEnumerable<EPAssignee> GetEPUsers();
        List<CopDetails> GetCMSDashboard(int userid);
        List<Assets> Assets(int epDetailId);
        int CheckDocRequired(int epDetailId);
        int CreateEp(EPDetails newEpDetails);
        int DocStaus(int epDetailId);       
        string EpTransStatus(int epDetailId, Inspection currentInspection = null);
        List<EPDetails> GetAllEps(int? stdesc, int? ePDetailId);
        List<BinderSearch> GetBinderSearch(string searchString);       
        List<CmsEpMapping> GetCmsEpMapping();
        List<CopDetails> GetCopDetails();
        HttpResponseMessage GetDashBoardEp(Request objRequest, int userId, int? categoryId = null, int? status = null, int? noofdays = null, int? duemonth = null, int? dueyear = null);      
        EPDetails GetDocsbyEp(int userId, int epDetailId);
        List<EPDetails> GetDueWithInDaysEPs(int dueWithInDays);
        List<EPAssignee> GetEpAssignee(int userId, int epDetailId);
        List<EPDetails> GetEpAssignees();
        List<EPDetails> GetEpAssignees_Paging(Request request);
        List<EPDetails> GetEpAssignment();
        List<EPDetails> GetEpAssignmentUser();
        List<EPDetails> GetEpByAssets(int assetId);
        EPDetails GetEpDescription(int? epDetailId);
        List<EPDetails> GetEpDetails(bool isDocRequired);
        List<EPDetails> GetEpDetails(int stdescId);
        List<EPDetails> GetEpDetailsforBinders(int stDescId, int epBinderId = 0);
        List<EPFrequency> GetEpFrequency();
        List<EPFrequency> GetEpFrequency(int epDetailId);
        EPDetails GetEpInspectionHistory(int epdetailId);
        EPDetails GetEpInspectionHistory(int epDetailId, int inspectionId);
        List<EPDetails> GetEPNumber(int stDescId);
        List<EPDetails> GetEps();
        List<EPDetails> GetEpsAll();
        EPDetails GetEpShortDescription(int epDetailId);      
        List<EPDescriptions> GetHospitalTypeEpDescriptions();
        List<EPDetails> GetIlsmEPs();       
        DataTable GetMasterEPsData();
        List<EPDetails> GetNonAssetEPsSchedule();
        List<EPDetails> GetNonAssetsEp();
        DataTable GetNonAssetsEps();      
        List<StandardEps> GetStandardEPs();
        StandardEps GetStandardEPs(int epDetailId);
        List<StandardEps> GetStandardEPs(int[] epDetailId);
        List<InspectionReport> GetTinspectionCycle(string dataSortOrder, string orderBySort, int year, int userId, int currentUserId, int? categoryId = null);
        int GetTotalEpAssigned(string userIds, int catId);
        List<EPDetails> GetUserDeficientEPs(int userId);
        List<EPAssignee> GetUserEPs(int? epdetailId, int? userId, int? docTypeId);
        List<EPDetails> OngoingEpDetails();
        int SaveEpDescription(EPDescriptions objEpDescriptions);
        int SaveUserEPs(string mode, int userId, bool status, int epId);
        bool UpdateEPApplicablestatus(EPDetails epDetail);
        EpAssets UpdateEpAssets(EpAssets epAssets);
        int UpgradeEp(string stdescId, string epdetailId, int? CreatedBy);

        int UpdateEpInitScheduleDate(int[] epIds, DateTime scheduleDate);
        List<EPDetails> GetAssetsEPOnRoute(int? routeId,string assetids);
        List<EPDetails> GetEpByBulkAssets(string assetId);
        EPDetails GetEpShortDetails(int epDetailId);
    }
}