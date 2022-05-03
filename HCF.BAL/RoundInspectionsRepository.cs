//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HCF.BAL
//{
//    public static class RoundInspectionsRepository
//    {

//        public static int AddRoundInspection(WorkOrder floorWorkOrder)
//        {
//            return DAL.RoundInspectionsRepository.AddRoundInspection(floorWorkOrder);
//        }

//        public static int UpdateRoundInspection(HCF.BDO.WorkOrder newWorkOrder)
//        {
//            return DAL.RoundInspectionsRepository.UpdateRoundInspection(newWorkOrder);
//        }
//        public static List<RoundGroup> GetUsers(int id)
//        {
//            return DAL.RoundInspectionsRepository.GetUsers(id);
//        }


//        public static List<RoundGroup> GetRoundGroup(int id)
//        {
//            return DAL.RoundInspectionsRepository.GetRoundGroup(id);
//        }

//        public static List<RoundGroup> GetUserRoundGroup(int userid)
//        {
//            return DAL.RoundInspectionsRepository.GetUserRoundGroup(userid);
//        }
//        public static RoundGroup GetUser_Volunteer_RoundGroup(int groupid,int userid)
//        {
//            return DAL.RoundInspectionsRepository.GetUser_Volunteer_RoundGroup(groupid,userid);
//        }
//        #region RoundUserGroup

//        public static int Save(RoundGroup RoundUser)
//        {
//            return DAL.RoundInspectionsRepository.Save(RoundUser);
//        }


//        public static List<RoundGroup> GetRoundUserGroup()
//        {
//            return DAL.RoundInspectionsRepository.GetRoundUserGroup();
//        }

//        public static int InsertOrUpdateGroup(RoundGroupUsers RoundUser)
//        {
//            return DAL.RoundInspectionsRepository.InsertOrUpdateGroup(RoundUser);
//        }
        
//        public static int InsertOrUpdateGroupUserType(RoundSchedules roundGroup, string dates)
//        {
//            return DAL.RoundInspectionsRepository.InsertOrUpdateGroupUserType(roundGroup, dates);
//        }
//        public static RoundGroup GetRoundGroupDetails(int roundgroupid)
//        {
//            return DAL.RoundInspectionsRepository.GetRoundGroupDetails(roundgroupid);
//        }

//        public static List<ActivityData> GetNotifyDetails_RoundsDetails()
//        {
//            return DAL.RoundInspectionsRepository.GetNotifyDetails_RoundsDetails();
//        }

//        public static int RemoveReplaceVolunteer(RoundVolunteer RoundUser)
//        {
//            return DAL.RoundInspectionsRepository.RemoveReplaceVolunteer(RoundUser);
//        }

//        //public static int SaveUserCategories(RoundGroupUsers roundGroup)
//        //{
//        //    return DAL.RoundInspectionsRepository.SaveUserCategories(roundGroup);
//        //}
//        public static List<RoundSchedules> GetTGroupRound(int id)
//        {
//            return DAL.RoundInspectionsRepository.GetTGroupRound(id);
//        }

//        public static List<RoundSchedules> GetTGroupRoundGroup(int id, string name)
//        {
//            return DAL.RoundInspectionsRepository.GetTGroupRoundGroup(id, name);
//        }

//        public static RoundGroup GetRoundUserGroup(int roundGroupId, string users)
//        {
//            return DAL.RoundInspectionsRepository.GetRoundUserGroup(roundGroupId, users);
//        }

//        public static int SaveRoundGroupLocations(RoundGroupLocations objroundGroupLocations)
//        {
//            return DAL.RoundInspectionsRepository.SaveRoundGroupLocations(objroundGroupLocations);
//        }

//        public static List<Buildings> GetRoundGroupLocations(int roundGroupId)
//        {
//            return DAL.RoundInspectionsRepository.GetRoundGroupLocations(roundGroupId);
//        }

//        public static IEnumerable<RoundGroupUsers> roundUserGroups(int userId)
//        {
//            return DAL.RoundInspectionsRepository.roundUserGroups(userId);
//        }

//        public static List<RoundScheduleDates> GetRoundScheduleDates(int? locationGroupId, int? userId)
//        {
//            return DAL.RoundInspectionsRepository.GetRoundScheduleDates(locationGroupId, userId);
//        }        
//        public static List<RoundGroup> GetRoundByDate(DateTime start, int? locationGroupId, int? userId)
//        {
//            return DAL.RoundInspectionsRepository.GetRoundByDate(start, locationGroupId, userId);
//        }

//        public static List<RoundGroup> GetRoundInspectionByDate(int userId)
//        {
//            return DAL.RoundInspectionsRepository.GetRoundInspectionByDate(userId);
//        }


//        public static bool UserReplacevolunteer(RoundVolunteer roundVolunteer)
//        {
//            return DAL.RoundInspectionsRepository.UserReplacevolunteer(roundVolunteer);
//        }


//        public static RoundGroup GetRoundGroupUsers()
//        {
//            return DAL.RoundInspectionsRepository.GetRoundGroupUsers();
//        }

//        public static bool IsmailNotified(Guid Id, string type)
//        {
//            return DAL.RoundInspectionsRepository.IsmailNotified(Id, type);
//        }

//        #endregion

//        public static List<RoundScheduleDates> GetRoundScheduleDatesIDS(int locationGroupId, int userId)
//        {
//            return DAL.RoundInspectionsRepository.GetRoundScheduleDatesIDS(locationGroupId, userId);
//        }

//        public static List<ActivityData> GetRoundRecentChangesByDate(string start, int? locationGroupId, int? userId)
//        {
//            return DAL.RoundInspectionsRepository.GetRoundRecentChangesByDate(start, locationGroupId, userId);
//        }

//    }


//}
