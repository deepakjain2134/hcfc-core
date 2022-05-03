//using HCF.BDO;
//using System.Collections.Generic;

//namespace HCF.BAL
//{
//    public static class GoalMaster
//    {
//        public static int UpdateEpFrequency(EPFrequency newdata)
//        {
//            return DAL.GoalMaster.UpdateEpFrequency(newdata);
//        }

//        public static int AddEpAssignees(EPAssignee epAssignee, string catId = null)
//        {
//            var status = DAL.GoalMaster.AddEpAssignees(epAssignee, catId);
//            return (status) ? 1 : 0;
//        }

//        public static int SaveEpAssignee(EPAssignee epAssignee)
//        {
//            return DAL.GoalMaster.SaveEpAssignee(epAssignee);

//        }

//        public static List<AffectedEPs> GetAffectedEPs(int ePDetailId)
//        {
//            return DAL.GoalMaster.GetAffectedEPs(ePDetailId);
//        }

//        public static List<AffectedEPs> GetAffectedEPs()
//        {
//            return DAL.GoalMaster.GetAffectedEPs();
//        }

//        public static List<EPSteps> GetEpTransInfo(int epdetailId, int inspectionId, int? inspectionGroupId = null)
//        {
//            return DAL.GoalMaster.GetEpTransInfo(epdetailId, inspectionId, inspectionGroupId);
//        }

//        public static RiskManagement GetRiskCount(int userId)
//        {
//            return DAL.GoalMaster.GetRiskCount(userId);
//        }

//        public static int SaveUserEPs(int userId, int epdetailId, string epdetailIds, int createdBy, string type, bool status)
//        {
//            return DAL.GoalMaster.SaveUserEPs(userId, epdetailId, epdetailIds, createdBy, type, status);
//        }

//        public static List<ActivityType> GetActivityType()
//        {
//            return DAL.GoalMaster.GetActivityType();
//        }

//        public static void AssignEpResposibility(string fromUsers, string toUsers, int createdBy)
//        {
//            DAL.GoalMaster.AssignEpResposibility(fromUsers, toUsers, createdBy);
//        }

//        public static int GetAssignedEpCount(string users)
//        {
//            return DAL.GoalMaster.GetAssignedEpCount(users);
//        }


//        public static int SaveCMSdata(CmsEpMapping cmsEpMapping)
//        {
//            return DAL.GoalMaster.SaveCMSdata(cmsEpMapping);
//        }



//        public static bool Delete_CmsEpMapping(int? standardEps , int? cmstext)
//        {
//            return DAL.GoalMaster.Delete_CmsEpMapping(standardEps, cmstext);
//        }

//        public static EPDetails EpRelationStatus(int Epdetailid)
//        {
//            return DAL.GoalMaster.EpRelationStatus(Epdetailid);
//        }
//        public static EPDetails EpSearchbyEpNumber(string epSearchText, int currentUserId, int userId)
//        {
//            return DAL.GoalMaster.EpSearchbyEpNumber(epSearchText, currentUserId, userId);
//        }

        
//    }
//}