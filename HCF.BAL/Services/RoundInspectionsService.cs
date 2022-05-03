using HCF.BDO;

using HCF.DAL;
using System;
using System.Collections.Generic;

namespace HCF.BAL
{
    public class RoundInspectionsService : IRoundInspectionsService
    {
        private readonly IRoundInspectionsRepository _roundInspectionsRepository;
        public RoundInspectionsService(IRoundInspectionsRepository roundInspectionsRepository)
        {
            _roundInspectionsRepository = roundInspectionsRepository;
        }


        public int AddRoundInspection(WorkOrder floorWorkOrder)
        {
            return _roundInspectionsRepository.AddRoundInspection(floorWorkOrder);
        }

        public int UpdateRoundInspection(HCF.BDO.WorkOrder newWorkOrder)
        {
            return _roundInspectionsRepository.UpdateRoundInspection(newWorkOrder);
        }
        public List<RoundGroup> GetUsers(int id)
        {
            return _roundInspectionsRepository.GetUsers(id);
        }


        public List<RoundGroup> GetRoundGroup(int id)
        {
            return _roundInspectionsRepository.GetRoundGroup(id);
        }

        public List<RoundGroup> GetUserRoundGroup(int userid)
        {
            return _roundInspectionsRepository.GetUserRoundGroup(userid);
        }
        public RoundGroup GetUser_Volunteer_RoundGroup(int groupid, int userid)
        {
            return _roundInspectionsRepository.GetUser_Volunteer_RoundGroup(groupid, userid);
        }
        #region RoundUserGroup

        public int Save(RoundGroup RoundUser)
        {
            return _roundInspectionsRepository.Save(RoundUser);
        }
        public List<RoundGroup> GetRoundGroupList()
        {
            return _roundInspectionsRepository.GetRoundGroupList();

        }

        public List<RoundGroup> GetRoundUserGroup()
        {
            return _roundInspectionsRepository.GetRoundUserGroup();
        }

       

        public int InsertOrUpdateGroupUserType(RoundSchedules roundGroup, string dates)
        {
            return _roundInspectionsRepository.InsertOrUpdateGroupUserType(roundGroup, dates);
        }
        public RoundGroup GetRoundGroupDetails(int roundgroupid)
        {
            return _roundInspectionsRepository.GetRoundGroupDetails(roundgroupid);
        }

        public List<ActivityData> GetNotifyDetails_RoundsDetails()
        {
            return _roundInspectionsRepository.GetNotifyDetails_RoundsDetails();
        }

        public int RemoveReplaceVolunteer(RoundVolunteer RoundUser)
        {
            return _roundInspectionsRepository.RemoveReplaceVolunteer(RoundUser);
        }

      
        public List<RoundSchedules> GetTGroupRound(int id)
        {
            return _roundInspectionsRepository.GetTGroupRound(id);
        }

        public List<RoundSchedules> GetTGroupRoundGroup(int id, string name)
        {
            return _roundInspectionsRepository.GetTGroupRoundGroup(id, name);
        }

        public RoundGroup GetRoundUserGroup(int roundGroupId, string users, int? roundtype)
        {
            return _roundInspectionsRepository.GetRoundUserGroup(roundGroupId, users, roundtype);
        }

        public RoundGroup GetTRoundUserGroup(int troundId)
        {
            return _roundInspectionsRepository.GetTRoundUserGroup(troundId);
        }

        public int SaveRoundGroupLocations(RoundGroupLocations objroundGroupLocations)
        {
            return _roundInspectionsRepository.SaveRoundGroupLocations(objroundGroupLocations);
        }

        public int SaveTRoundLocations(TRoundLocations objTRoundLocations)
        {
            return _roundInspectionsRepository.SaveTRoundLocations(objTRoundLocations);
        }

        public List<Buildings> GetRoundGroupLocations(int roundGroupId, int troundId)
        {
            return _roundInspectionsRepository.GetRoundGroupLocations(roundGroupId, troundId);
        }
             

        public List<RoundScheduleDates> GetRoundScheduleDates(int? locationGroupId, int? userId, DateTime start, DateTime end)
        {
            return _roundInspectionsRepository.GetRoundScheduleDates(locationGroupId, userId, start, end);
        }
        public List<RoundGroup> GetRoundByDate(DateTime roundDate, int? locationGroupId, int? userId, bool? RequestFromApp)
        {
            return _roundInspectionsRepository.GetRoundByDate(roundDate, locationGroupId, userId, RequestFromApp, roundDate);
        }
        public List<RoundGroup> GetRoundByDateApp(DateTime roundDate, int? locationGroupId, int? userId, bool? RequestFromApp, int? day, DateTime endDate)
        {
            return _roundInspectionsRepository.GetRoundByDateApp(roundDate, locationGroupId, userId, RequestFromApp, day, endDate);
        }

        public List<RoundGroup> GetRoundInspectionByDate(int userId)
        {
            return _roundInspectionsRepository.GetRoundInspectionByDate(userId);
        }


        public bool UserReplacevolunteer(RoundVolunteer roundVolunteer)
        {
            return _roundInspectionsRepository.UserReplacevolunteer(roundVolunteer);
        }


        public RoundGroup GetRoundGroupUsers()
        {
            return _roundInspectionsRepository.GetRoundGroupUsers();
        }

        public bool IsmailNotified(Guid Id, string Type)
        {
            return _roundInspectionsRepository.IsmailNotified(Id, Type);
        }

        #endregion

        public List<RoundScheduleDates> GetRoundScheduleDatesIDS(int locationGroupId, int userId)
        {
            return _roundInspectionsRepository.GetRoundScheduleDatesIDS(locationGroupId, userId);
        }

        public List<ActivityData> GetRoundRecentChangesByDate(string start, int? locationGroupId, int? userId)
        {
            return _roundInspectionsRepository.GetRoundRecentChangesByDate(start, locationGroupId, userId);
        }

        public List<UserFloorRoundCategory> UserFloorRoundCategory(int userId, int floorId, int troundId)
        {
            return _roundInspectionsRepository.UserFloorRoundCategory(userId, floorId, troundId);
        }

        public IEnumerable<RoundGroupUsers> GetRoundUserGroups(int roundGroupId)
        {
            return _roundInspectionsRepository.GetRoundUserGroups(roundGroupId);
        }

        public IEnumerable<RoundGroupUsers> roundUserGroups(int userId)
        {
            return _roundInspectionsRepository.roundUserGroups(userId);
        }

        public int InsertOrUpdateGroup(RoundGroupUsers RoundUser)
        {
            return _roundInspectionsRepository.InsertOrUpdateGroup(RoundUser);
        }

        public int ManageTroundUsers(TRoundUsers roundUser)
        {
            return _roundInspectionsRepository.InsertOrUpdateTRoundUsers(roundUser);
        }
    }
}