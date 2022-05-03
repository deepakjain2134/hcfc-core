using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IEpService
    {
        int CheckDocRequired(int epDetailId);
        int DocStatus(int epDetailId);
        List<DashboardDetails> GetDashBoardData(int userId);
        string EpTransStatus(int epDetailId);
        List<EPDetails> GetAllEps(int? stdescId, int? epId);
        List<BinderSearch> GetBinderSearch(string searchString);
       
        List<CopDetails> GetCMSDashboard(int userid);
        List<CmsEpMapping> GetCmsEpMapping();
        List<CopDetails> GetCopDetails();
        HttpResponseMessage GetDashBoardEp(Request objRequest, int userId, int? categoryId = null, int? status = null, int? noOfDays = null, int? dueMonth = null, int? dueyear = null);
        List<EPDetails> GetDeficientEPs(int userId);
        IEnumerable<EPDetails> GetDueWithInDaysEPs(int DueWithInDays);
        List<Assets> GetEpAssets(int epId);
        IEnumerable<EPAssignee> GetEpAssignee(int userId, int epDetailId);
        List<EPDetails> GetEpAssignees();
        List<EPDetails> GetEpAssignees_Paging(Request objRequest);
        List<EPDetails> GetEpAssignment();
        List<EPDetails> GetEpAssignmentUser();
        List<EPDetails> GetEpByAssets(int assetId);
        EPDetails GetEpDescription(int? epDetailId);
        List<EPDetails> GetEpDetails();
        List<EPDetails> GetEpDetails(bool isDocRequired);
        List<EPDetails> GetEpDetails(int stDescId);
        IEnumerable<EPDetails> GetEpDetailsforBinders(int stDescId, int epBinderId);
       // List<EPFrequency> GetEPFrequency();
        IEnumerable<EPDetails> GetEpNumber(int stDescId);
        //IEnumerable<EPAssignee> GetEpUsers();
        List<EPAssignee> GetEPUsers(int epDetailId, int? docTypeId);
        List<EPDescriptions> GetHospitalTypeEpDescriptions();
        List<EPDetails> GetIlsmEPs();
        DataTable GetMasterEPsData();
        //List<EPDetails> GetNonAssetEPsSchedule();
        List<EPDetails> GetNonAssetsEp();
        DataTable GetNonAssetsEps();
        List<StandardEps> GetStandardEPs();
        StandardEps GetStandardEPs(int epdetailId);
        int GetTotalEpAssigned(string userIds, int catId);
        IEnumerable<EPDetails> GetUserDeficientEPs(int userId);
        List<EPAssignee> GetUserEPs(int userId);
        List<int> GetUsersEps(int[] userIds);
        List<EPDetails> OngoingEpDetails();
        int Save(EPDetails newData);
        int SaveEpDescription(EPDescriptions objEpDescriptions);
        int SaveUserEPs(string mode, int userId, bool status, int epId);
        bool UpdateEPApplicablestatus(EPDetails epDetail);
        int UpgradeEp(string stdescId, string epdetailId, int? CreatedBy);
        List<InspectionReport> GetTinspectionCycle(string SortOrder, string OrderbySort, int year, int userId, int currentUserId, int? categoryId = null);

        int Save(EpDocuments epDocument);
        EPDetails GetEpShortDescription(int epDetailId);

        List<EPDetails> GetEpDetailsAsync();

        int UpdateEpInitScheduleDate(int[] epIds, DateTime scheduleData);
        EPDetails GetEpShortDetails(int epdetailId);
    }
}