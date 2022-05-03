using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IGoalMaster
    {
        IEnumerable<EPAssignee> GetEPUsers();
        bool AddEpAssignees(EPAssignee newEpAssigne, string catId);
        bool Add(EPAssignee newEpAssigne);
        bool Update(EPAssignee newEpAssigne);
        int AddEpFrequency(EPFrequency newdata);
        void AssignEpResposibility(string fromUsers, string toUsers, int createdBy);
        bool Delete_CmsEpMapping(int? standardEps, int? cmstext);
        EPDetails EpRelationStatus(int Epdetailid);
        EPDetails EpSearchbyEpNumber(string epSearchText, int currentUserId, int userId);
        List<ActivityType> GetActivityType();
        List<AffectedEPs> GetAffectedEPs();
        List<AffectedEPs> GetAffectedEPs(int epDetailId);
        int GetAssignedEpCount(string users);
        //List<T> GetEpByUser<T>(List<T> collection, int userId);
        List<EPSteps> GetEpTransInfo(int epdetailId, int inspectionId, int? inspectiongroupId);
        //IEnumerable<EPAssignee> GetEPUsers();
        RiskManagement GetRiskCount(int userId);
        List<int> GetUsersEps(int[] userIds);
        int SaveCMSdata(CmsEpMapping cmsEpMapping);
        int SaveEpAssignee(EPAssignee ePAssignee);
        int SaveUserEPs(int userId, int epdetailId, string epdetailIds, int createdBy, string type, bool status);
        int UpdateEpFrequency(EPFrequency newdata);
    }
}