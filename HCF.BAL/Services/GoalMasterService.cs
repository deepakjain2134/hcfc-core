using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public  class GoalMasterService : IGoalMasterService
    {
        private readonly IGoalMaster _goalMaster;
        public GoalMasterService(IGoalMaster goalMaster)
        {
            _goalMaster = goalMaster;
        }
        public  int UpdateEpFrequency(EPFrequency newdata)
        {
            return _goalMaster.UpdateEpFrequency(newdata);
        }

        public  int AddEpAssignees(EPAssignee epAssignee, string catId = null)
        {
            var status = _goalMaster.AddEpAssignees(epAssignee, catId);
            return (status) ? 1 : 0;
        }

        public  int SaveEpAssignee(EPAssignee epAssignee)
        {
            return _goalMaster.SaveEpAssignee(epAssignee);

        }

        public  List<AffectedEPs> GetAffectedEPs(int ePDetailId)
        {
            return _goalMaster.GetAffectedEPs(ePDetailId);
        }

        public  List<AffectedEPs> GetAffectedEPs()
        {
            return _goalMaster.GetAffectedEPs();
        }

        public  List<EPSteps> GetEpTransInfo(int epdetailId, int inspectionId, int? inspectionGroupId = null)
        {
            return _goalMaster.GetEpTransInfo(epdetailId, inspectionId, inspectionGroupId);
        }

        public RiskManagement GetRiskCount(int userId)
        {
            return _goalMaster.GetRiskCount(userId);
        }

        public  int SaveUserEPs(int userId, int epdetailId, string epdetailIds, int createdBy, string type, bool status)
        {
            return _goalMaster.SaveUserEPs(userId, epdetailId, epdetailIds, createdBy, type, status);
        }

        public  List<ActivityType> GetActivityType()
        {
            return _goalMaster.GetActivityType();
        }

        public  void AssignEpResposibility(string fromUsers, string toUsers, int createdBy)
        {
            _goalMaster.AssignEpResposibility(fromUsers, toUsers, createdBy);
        }

        public  int GetAssignedEpCount(string users)
        {
            return _goalMaster.GetAssignedEpCount(users);
        }


        public  int SaveCMSdata(CmsEpMapping cmsEpMapping)
        {
            return _goalMaster.SaveCMSdata(cmsEpMapping);
        }



        public  bool Delete_CmsEpMapping(int? standardEps , int? cmstext)
        {
            return _goalMaster.Delete_CmsEpMapping(standardEps, cmstext);
        }

        public  EPDetails EpRelationStatus(int Epdetailid)
        {
            return _goalMaster.EpRelationStatus(Epdetailid);
        }
        public  EPDetails EpSearchbyEpNumber(string epSearchText, int currentUserId, int userId)
        {
            return _goalMaster.EpSearchbyEpNumber(epSearchText, currentUserId, userId);
        }

        
    }
}