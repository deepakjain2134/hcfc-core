//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Threading.Tasks;

//namespace HCF.BAL
//{
//    public static class EpRepository
//    {
//        public static HttpResponseMessage GetDashBoardEp(Request objRequest, int userId, int? categoryId = null, int? status = null, int? noOfDays = null, int? dueMonth = null, int? dueyear = null)
//        {
//            return DAL.EpRepository.GetDashBoardEp(objRequest, userId, categoryId, status, noOfDays, dueMonth, dueyear);
//        }

//        public static int Save(EPDetails newData)
//        {
//            return DAL.EpRepository.CreateEp(newData);
//        }

//        public static int SaveEpDescription(EPDescriptions objEpDescriptions)
//        {
//            return DAL.EpRepository.SaveEpDescription(objEpDescriptions);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="isDocRequired"></param>
//        /// <returns></returns>
//        public static List<EPDetails> GetEpDetails(bool isDocRequired)
//        {
//            return DAL.EpRepository.GetEpDetails(isDocRequired);
//        }


//        public static DataTable GetNonAssetsEps()
//        {
//            return DAL.EpRepository.GetNonAssetsEps();
//        }

//        public static List<EPDetails> GetNonAssetsEp()
//        {
//            return DAL.EpRepository.GetNonAssetsEp();
//        }

//        public static List<Assets> GetEpAssets(int epId)
//        {
//            return DAL.EpRepository.Assets(epId);
//        }

//        public static List<EPDetails> GetEpDetails()
//        {
//            return DAL.EpRepository.GetEpsAll();
//        }

//        public static List<EPDescriptions> GetHospitalTypeEpDescriptions()
//        {
//            return DAL.EpRepository.GetHospitalTypeEpDescriptions();
//        }

//        public static List<EPDetails> GetEpDetails(int stDescId)
//        {
//            return DAL.EpRepository.GetEpDetails(stDescId);
//        }

//        public static List<EPDetails> GetAllEps(int? stdescId, int? epId)
//        {
//            return DAL.EpRepository.GetAllEps(stdescId, epId);
//        }



//        public static IEnumerable<EPDetails> GetEpNumber(int stDescId)
//        {
//            return DAL.EpRepository.GetEPNumber(stDescId);
//        }

//        public static IEnumerable<EPDetails> GetEpDetailsforBinders(int stDescId, int epBinderId)
//        {
//            return DAL.EpRepository.GetEpDetailsforBinders(stDescId, epBinderId);
//        }

//        public static List<EPDetails> GetEpAssignment()
//        {
//            return DAL.EpRepository.GetEpAssignment();
//        }


//        public static IEnumerable<EPAssignee> GetEpUsers()
//        {
//            return DAL.GoalMaster.GetEPUsers();
//        }
//        public static List<int> GetUsersEps(int[] userIds)
//        {
//            return DAL.GoalMaster.GetUsersEps(userIds);
//        }

//        public static IEnumerable<EPAssignee> GetEpAssignee(int userId, int epDetailId)
//        {
//            return DAL.EpRepository.GetEpAssignee(userId, epDetailId);
//        }

//        public static List<EPDetails> GetEpByAssets(int assetId)
//        {
//            return DAL.EpRepository.GetEpByAssets(assetId);
//        }

//        public static List<EPDetails> GetEpAssignmentUser()
//        {
//            return DAL.EpRepository.GetEpAssignmentUser();
//        }

//        public static List<EPDetails> OngoingEpDetails()
//        {
//            return DAL.EpRepository.OngoingEpDetails();
//        }

//        public static List<EPDetails> GetDeficientEPs(int userId)
//        {
//            return DAL.EpRepository.GetDeficientEPs(userId);
//        }

//        public static IEnumerable<EPDetails> GetUserDeficientEPs(int userId)
//        {
//            return DAL.EpRepository.GetUserDeficientEPs(userId);
//        }

//        public static StandardEps GetStandardEPs(int epdetailId)
//        {
//            return DAL.EpRepository.GetStandardEPs(epdetailId);
//        }

//        public static List<StandardEps> GetStandardEPs()
//        {
//            return DAL.EpRepository.GetStandardEPs();
//        }


//        public static EPDetails GetEpDescription(int? epDetailId)
//        {
//            return DAL.EpRepository.GetEpDescription(epDetailId);
//        }

//        public static List<EPDetails> GetIlsmEPs()
//        {
//            return DAL.EpRepository.GetIlsmEPs();
//        }

//        public static int DocStatus(int epDetailId)
//        {
//            return DAL.EpRepository.DocStaus(epDetailId);
//        }

//        public static List<EPDetails> GetNonAssetEPsSchedule()
//        {
//            return DAL.EpRepository.GetNonAssetEPsSchedule();
//        }

//        public static int CheckDocRequired(int epDetailId)
//        {
//            return DAL.EpRepository.CheckDocRequired(epDetailId);
//        }

//        public static List<EPFrequency> GetEPFrequency()
//        {
//            return DAL.EpRepository.GetEpFrequency();
//        }

//        public static IEnumerable<EPDetails> GetDueWithInDaysEPs(int DueWithInDays)
//        {
//            return DAL.EpRepository.GetDueWithInDaysEPs(DueWithInDays);
//        }

//        public static string EpTransStatus(int epDetailId)
//        {
//            return DAL.EpRepository.EpTransStatus(epDetailId);
//        }
//        public static Inspection EpStatus(int epDetailId)
//        {
//            return DAL.EpRepository.EpStatus(epDetailId);
//        }

//        public static DataTable GetMasterEPsData()
//        {
//            return DAL.EpRepository.GetMasterEPsData();
//        }

//        public static List<EPAssignee> GetUserEPs(int userId)
//        {
//            return DAL.EpRepository.GetUserEPs(null, userId, null);
//        }

//        public static List<EPAssignee> GetEPUsers(int epDetailId, int? docTypeId)
//        {
//            return DAL.EpRepository.GetUserEPs(epDetailId, null, docTypeId);
//        }

//        public static bool UpdateEPApplicablestatus(EPDetails epDetail)
//        {
//            return DAL.EpRepository.UpdateEPApplicablestatus(epDetail);
//        }

//        public static int GetTotalEpAssigned(string userIds, int catId)
//        {
//            return DAL.EpRepository.GetTotalEpAssigned(userIds, catId);
//        }

//        public static List<EPDetails> GetEpAssignees()
//        {
//            return DAL.EpRepository.GetEpAssignees();
//        }

//        public static List<EPDetails> GetEpAssignees_Paging(Request objRequest)
//        {
//            return DAL.EpRepository.GetEpAssignees_Paging(objRequest);
//        }

//        public static int SaveUserEPs(string mode, int userId, bool status, int epId)
//        {
//            return DAL.EpRepository.SaveUserEPs(mode, userId, status, epId);
//        }

//        #region  CMS Dashboard

//        public static List<CopDetails> GetCMSDashboard()
//        {
//            return DAL.EpRepository.GetCMSDashboard();
//        }

//        public static List<CopDetails> GetCopDetails()
//        {
//            return DAL.EpRepository.GetCopDetails();
//        }

//        public static List<CmsEpMapping> GetCmsEpMapping()
//        {
//            return DAL.EpRepository.GetCmsEpMapping();
//        }

//        #endregion

//        #region BinderSearch
         
//        public static List<BinderSearch> GetBinderSearch(string searchString)
//        {
//            return DAL.EpRepository.GetBinderSearch(searchString);
//        }


//        #endregion
//        #region Upgrade ep
//        public static int UpgradeEp(string stdescId, string epdetailId, int? CreatedBy)
//        {
//            return DAL.EpRepository.UpgradeEp( stdescId,  epdetailId , CreatedBy);
//        }
//        #endregion

//        #region Campus
//        public static List<Buildings> GetCampus(int userid)
//        {
//            return DAL.EpRepository.GetCampus(userid);
//        }
//        #endregion
//    }
//}
