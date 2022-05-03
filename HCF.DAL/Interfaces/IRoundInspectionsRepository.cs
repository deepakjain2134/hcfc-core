using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IRoundInspectionsRepository
    {
        List<RoundGroup> GetRoundGroupList();
        int AddRoundInspection(WorkOrder newWorkOrder);
        List<ActivityData> GetNotifyDetails_RoundsDetails();
        List<RoundGroup> GetRoundByDateApp(DateTime roundDate, int? locationGroupId, int? userId, bool? RequestFromApp,int? day,DateTime endDate);
        List<RoundGroup> GetRoundByDate(DateTime roundDate, int? locationGroupId, int? userId, bool? RequestFromApp,DateTime endDate);
        List<RoundGroup> GetRoundGroup(int id);
        RoundGroup GetRoundGroupDetails(int groupid);
        List<Buildings> GetRoundGroupLocations(int roundGroupId, int troundId);
        RoundGroup GetRoundGroupUsers();
        List<RoundGroup> GetRoundInspectionByDate(int userId);
        List<ActivityData> GetRoundRecentChangesByDate(string roundDate, int? locationGroupId, int? userId);
        List<RoundScheduleDates> GetRoundScheduleDates(int? locationGroupId, int? userId, DateTime startDate, DateTime endDate);
        List<RoundScheduleDates> GetRoundScheduleDatesIDS(int locationGroupId, int userId);
        List<RoundGroup> GetRoundUserGroup();
        RoundGroup GetRoundUserGroup(int roundGroupId, string users, int? roundtype);

        RoundGroup GetTRoundUserGroup(int tRoundId);

        List<RoundSchedules> GetTGroupRound(int id);
        List<RoundSchedules> GetTGroupRoundGroup(int id, string name);
        List<RoundGroup> GetUserRoundGroup(int userid);
        List<RoundGroup> GetUsers(int id);
        RoundGroup GetUser_Volunteer_RoundGroup(int groupid, int userid);
        int InsertOrUpdateGroup(RoundGroupUsers roundGroupUsers);
        int InsertOrUpdateGroupUserType(RoundSchedules roundGroup, string dates);
        bool IsmailNotified(Guid Id, string Type);
        int RemoveReplaceVolunteer(RoundVolunteer roundGroupUsers);
        
        int Save(RoundGroup roundUser);
        int SaveRoundGroupLocations(RoundGroupLocations objroundGroupLocations);
        int UpdateRoundInspection(WorkOrder newWorkOrder);
        bool UserReplacevolunteer(RoundVolunteer roundVolunteer);
        List<UserFloorRoundCategory> UserFloorRoundCategory(int userId, int floorId, int troundId);

        IEnumerable<RoundGroupUsers> GetRoundUserGroups(int roundGroupId);
        List<RoundGroupUsers> roundUserGroups(int userId);
        int SaveTRoundLocations(TRoundLocations objTRoundLocations);

        int InsertOrUpdateTRoundUsers(TRoundUsers roundGroupUsers);
    }
}