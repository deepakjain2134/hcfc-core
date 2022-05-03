using System;
using System.Collections.Generic;
using HCF.BDO;


namespace HCF.BAL
{
    public interface IRoundInspectionsService
    {
        int AddRoundInspection(WorkOrder floorWorkOrder);
        List<ActivityData> GetNotifyDetails_RoundsDetails();
        List<RoundGroup> GetRoundByDateApp(DateTime roundDate, int? locationGroupId, int? userId, bool? RequestFromApp,int? day, DateTime date);
        List<RoundGroup> GetRoundByDate(DateTime roundDate, int? locationGroupId, int? userId, bool? RequestFromApp);
        List<RoundGroup> GetRoundGroup(int id);
        RoundGroup GetRoundGroupDetails(int roundgroupid);
        List<Buildings> GetRoundGroupLocations(int roundGroupId, int troundId);
        RoundGroup GetRoundGroupUsers();
        List<RoundGroup> GetRoundInspectionByDate(int userId);
        List<ActivityData> GetRoundRecentChangesByDate(string start, int? locationGroupId, int? userId);
        List<RoundScheduleDates> GetRoundScheduleDates(int? locationGroupId, int? userId, DateTime startDate, DateTime endDate);
        List<RoundScheduleDates> GetRoundScheduleDatesIDS(int locationGroupId, int userId);
        List<RoundGroup> GetRoundUserGroup();
        List<RoundGroup> GetRoundGroupList();
        RoundGroup GetRoundUserGroup(int roundGroupId, string users, int? roundtype);

        RoundGroup GetTRoundUserGroup(int troundId);

        List<UserFloorRoundCategory> UserFloorRoundCategory(int userId, int floorId, int troundId);
        List<RoundSchedules> GetTGroupRound(int id);
        List<RoundSchedules> GetTGroupRoundGroup(int id, string name);
        List<RoundGroup> GetUserRoundGroup(int userid);
        List<RoundGroup> GetUsers(int id);
        RoundGroup GetUser_Volunteer_RoundGroup(int groupid, int userid);
        int InsertOrUpdateGroup(RoundGroupUsers RoundUser);
        int InsertOrUpdateGroupUserType(RoundSchedules roundGroup, string dates);
        bool IsmailNotified(Guid Id, string Type);
        int RemoveReplaceVolunteer(RoundVolunteer RoundUser);
        IEnumerable<RoundGroupUsers> roundUserGroups(int userId);
        int Save(RoundGroup RoundUser);
        int SaveRoundGroupLocations(RoundGroupLocations objroundGroupLocations);
        int UpdateRoundInspection(WorkOrder newWorkOrder);
        bool UserReplacevolunteer(RoundVolunteer roundVolunteer);
        IEnumerable<RoundGroupUsers> GetRoundUserGroups(int roundGroupId);
        int SaveTRoundLocations(TRoundLocations objTRoundLocations);

        int ManageTroundUsers(TRoundUsers RoundUser);

    }
}