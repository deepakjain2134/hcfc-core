using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public  class EpService : IEpService
    {
        private readonly IEpDocumentRepository _epDocumentRepository;
        private readonly IGoalMaster _goalMaster;
        private readonly IEpRepository _epRepository;
        private readonly ITransaction _transactionRepository;
        private readonly IInspectionRepository _inspectionRepository;

        public EpService(IInspectionRepository inspectionRepository,ITransaction transactionRepository,IEpDocumentRepository epDocumentRepository, IGoalMaster goalMaster, IEpRepository epRepository)
        {
            _inspectionRepository = inspectionRepository;
            _transactionRepository = transactionRepository;
            _epDocumentRepository = epDocumentRepository;
            _goalMaster = goalMaster;
            _epRepository = epRepository;
        }
        public  HttpResponseMessage GetDashBoardEp(Request objRequest, int userId, int? categoryId = null, int? status = null, int? noOfDays = null, int? dueMonth = null, int? dueyear = null)
        {
            return _epRepository.GetDashBoardEp(objRequest, userId, categoryId, status, noOfDays, dueMonth, dueyear);
        }

        public int Save(EPDetails newData)
        {
            return _epRepository.CreateEp(newData);
        }

        public int Save(EpDocuments epDocument)
        {
            return _epDocumentRepository.Save(epDocument);
        }

        public List<DashboardDetails> GetDashBoardData(int userId)
        {
            return _epRepository.GetDashBoardData(userId);
        }
        public  int SaveEpDescription(EPDescriptions objEpDescriptions)
        {
            return _epRepository.SaveEpDescription(objEpDescriptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDocRequired"></param>
        /// <returns></returns>
       


        public  DataTable GetNonAssetsEps()
        {
            return _epRepository.GetNonAssetsEps();
        }

        public  List<EPDetails> GetNonAssetsEp()
        {
            return _epRepository.GetNonAssetsEp();
        }

        public  List<Assets> GetEpAssets(int epId)
        {
            return _epRepository.Assets(epId);
        }

        public  List<EPDetails> GetEpDetails()
        {
            return _epRepository.GetEpsAll();
        }

        public List<EPDetails> GetEpDetails(bool isDocRequired)
        {
            return _epRepository.GetEpDetails(isDocRequired);
        }

        public List<EPDetails> GetEpDetails(int stDescId)
        {
            return _epRepository.GetEpDetails(stDescId);
        }

        public  List<EPDescriptions> GetHospitalTypeEpDescriptions()
        {
            return _epRepository.GetHospitalTypeEpDescriptions();
        }       

        public  List<EPDetails> GetAllEps(int? stdescId, int? epId)
        {
            return _epRepository.GetAllEps(stdescId, epId);
        }

        public  IEnumerable<EPDetails> GetEpNumber(int stDescId)
        {
            return _epRepository.GetEPNumber(stDescId);
        }

        public  IEnumerable<EPDetails> GetEpDetailsforBinders(int stDescId, int epBinderId)
        {
            return _epRepository.GetEpDetailsforBinders(stDescId, epBinderId);
        }

        public  List<EPDetails> GetEpAssignment()
        {
            return _epRepository.GetEpAssignment();
        }
        
        public  List<int> GetUsersEps(int[] userIds)
        {
            return _goalMaster.GetUsersEps(userIds);
        }

        public  IEnumerable<EPAssignee> GetEpAssignee(int userId, int epDetailId)
        {
            return _epRepository.GetEpAssignee(userId, epDetailId);
        }

        public  List<EPDetails> GetEpByAssets(int assetId)
        {
            return _epRepository.GetEpByAssets(assetId);
        }

        public  List<EPDetails> GetEpAssignmentUser()
        {
            return _epRepository.GetEpAssignmentUser();
        }

        public  List<EPDetails> OngoingEpDetails()
        {
            return _epRepository.OngoingEpDetails();
        }

        public  List<EPDetails> GetDeficientEPs(int userId)
        {
            return _transactionRepository.GetDeficientEPs(userId);
        }

        public  IEnumerable<EPDetails> GetUserDeficientEPs(int userId)
        {
            return _epRepository.GetUserDeficientEPs(userId);
        }

        public  StandardEps GetStandardEPs(int epdetailId)
        {
            return _epRepository.GetStandardEPs(epdetailId);
        }

        public  List<StandardEps> GetStandardEPs()
        {
            return _epRepository.GetStandardEPs();
        }


        public  EPDetails GetEpDescription(int? epDetailId)
        {
            return _epRepository.GetEpDescription(epDetailId);
        }

        public  List<EPDetails> GetIlsmEPs()
        {
            return _epRepository.GetIlsmEPs();
        }

        public  int DocStatus(int epDetailId)
        {
            return _epRepository.DocStaus(epDetailId);
        }       

        public  int CheckDocRequired(int epDetailId)
        {
            return _epRepository.CheckDocRequired(epDetailId);
        }      

        public  IEnumerable<EPDetails> GetDueWithInDaysEPs(int DueWithInDays)
        {
            return _epRepository.GetDueWithInDaysEPs(DueWithInDays);
        }

        public string EpTransStatus(int epDetailId)
        {
            return _inspectionRepository.EpTransStatus(epDetailId);
        }

        public  DataTable GetMasterEPsData()
        {
            return _epRepository.GetMasterEPsData();
        }

        public  List<EPAssignee> GetUserEPs(int userId)
        {
            return _epRepository.GetUserEPs(null, userId, null);
        }

        public  List<EPAssignee> GetEPUsers(int epDetailId, int? docTypeId)
        {
            return _epRepository.GetUserEPs(epDetailId, null, docTypeId);
        }

        public  bool UpdateEPApplicablestatus(EPDetails epDetail)
        {
            return _epRepository.UpdateEPApplicablestatus(epDetail);
        }

        public  int GetTotalEpAssigned(string userIds, int catId)
        {
            return _epRepository.GetTotalEpAssigned(userIds, catId);
        }

        public  List<EPDetails> GetEpAssignees()
        {
            return _epRepository.GetEpAssignees();
        }

        public  List<EPDetails> GetEpAssignees_Paging(Request objRequest)
        {
            return _epRepository.GetEpAssignees_Paging(objRequest);
        }

        public  int SaveUserEPs(string mode, int userId, bool status, int epId)
        {
            return _epRepository.SaveUserEPs(mode, userId, status, epId);
        }

        #region  CMS Dashboard

        public  List<CopDetails> GetCMSDashboard(int userid)
        {
            return _epRepository.GetCMSDashboard(userid);
        }

        public  List<CopDetails> GetCopDetails()
        {
            return _epRepository.GetCopDetails();
        }

        public  List<CmsEpMapping> GetCmsEpMapping()
        {
            return _epRepository.GetCmsEpMapping();
        }

        #endregion

        #region BinderSearch
         
        public  List<BinderSearch> GetBinderSearch(string searchString)
        {
            return _epRepository.GetBinderSearch(searchString);
        }


        #endregion

        #region Upgrade ep
        public  int UpgradeEp(string stdescId, string epdetailId, int? CreatedBy)
        {
            return _epRepository.UpgradeEp( stdescId,  epdetailId , CreatedBy);
        }
        #endregion
       
        #region Tinspection Cycle
        public  List<InspectionReport> GetTinspectionCycle(string SortOrder, string OrderbySort, int year, int userId, int currentUserId, int? categoryId = null)
        {
            return _epRepository.GetTinspectionCycle(SortOrder, OrderbySort, year, userId, currentUserId, categoryId);
        }

        #endregion

       

        public EPDetails GetEpShortDescription(int epDetailId)
        {
            return _epRepository.GetEpShortDescription(epDetailId);
        }

        public  List<EPDetails> GetEpDetailsAsync()
        {
            return  _epRepository.GetEpDetailsAsync();
        }

        public int UpdateEpInitScheduleDate(int[] epIds, DateTime scheduleData) {
            return _epRepository.UpdateEpInitScheduleDate(epIds, scheduleData);
        }
        public EPDetails GetEpShortDetails(int epDetailId)
        {
            return _epRepository.GetEpShortDetails(epDetailId);
        }
        
    }
}
