using System.Collections.Generic;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IGoalMasterService
    {
        int AddEpAssignees(EPAssignee epAssignee, string catId = null);
        void AssignEpResposibility(string fromUsers, string toUsers, int createdBy);
        bool Delete_CmsEpMapping(int? standardEps, int? cmstext);
        EPDetails EpRelationStatus(int Epdetailid);
        EPDetails EpSearchbyEpNumber(string epSearchText, int currentUserId, int userId);
        List<ActivityType> GetActivityType();
        List<AffectedEPs> GetAffectedEPs();
        List<AffectedEPs> GetAffectedEPs(int ePDetailId);
        int GetAssignedEpCount(string users);
        List<EPSteps> GetEpTransInfo(int epdetailId, int inspectionId, int? inspectionGroupId = null);
        RiskManagement GetRiskCount(int userId);
        int SaveCMSdata(CmsEpMapping cmsEpMapping);
        int SaveEpAssignee(EPAssignee epAssignee);
        int SaveUserEPs(int userId, int epdetailId, string epdetailIds, int createdBy, string type, bool status);
        int UpdateEpFrequency(EPFrequency newdata);
    }
}