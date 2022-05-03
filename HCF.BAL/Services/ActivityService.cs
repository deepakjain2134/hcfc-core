using System;
using System.Collections.Generic;
using HCF.BDO;
using HCF.DAL;
using HCF.BDO.Enums;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL.Models.General;
using HCF.BAL.Interfaces.Services;
using HCF.DAL.Interfaces;

namespace HCF.BAL
{
    public partial class ActivityService : IActivityService
    {
        
        private readonly ILoggingService _loggingService;
        private readonly IUserService _userService;
        private readonly IEpRepository _epRepository;
        private readonly IActivityRepository _activityRepository;


        public ActivityService(IEpRepository epRepository, ILoggingService loggingService, IUserService userService,IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
            _epRepository = epRepository;         
            _loggingService = loggingService;
            _userService = userService;
        }


        //public async Task<PaginatedList<ActivityBase>> GetPagedGroupedActivities(int pageIndex, int pageSize)
        //{

        //    var query =
        //        from activity in _activityRepository.GetAll().Where(x => x.CreatedBy > 0)
        //        select activity;

        //    var results = query.OrderByDescending(x => x.CreatedDate);

        //    var activities = await PaginatedList<Activity>.CreateAsync(results.AsQueryable(), pageIndex, pageSize);

        //    // Convert
        //    var specificActivities = ConvertToSpecificActivities(activities, pageIndex, pageSize);

        //    return specificActivities;
        //}

        private PaginatedList<ActivityBase> ConvertToSpecificActivities(PaginatedList<Activity> activities,
            int pageIndex, int pageSize)
        {
            var listedResults = ConvertToSpecificActivities(activities);
            return new PaginatedList<ActivityBase>(listedResults.ToList(), pageIndex, pageSize, activities.Count);
        }

        public Activity Add(Activity newActivity)
        {
            _activityRepository.Insert(newActivity);
            return newActivity;
        }

        //public Activity Get(Guid id)
        //{
        //    return _activityRepository.GetAll().FirstOrDefault(x => x.Id == id);
        //}

        //public IList<Activity> GetList()
        //{
        //    return _activityRepository.GetAll().ToList();
        //}

        #region Round User Activity

        public void RoundUserAdded(UserProfile user, int roundgroupId, int categories, string date, int createdby)
        {
            var memberJoinedActivity = UserRoundActivity.RoundUserAdded(user, roundgroupId, categories, date, createdby);
            if(memberJoinedActivity!=null && memberJoinedActivity.CreatedBy>0)
            {
                Add(memberJoinedActivity);
            }
           
        }

        public void RoundUserRemoved(UserProfile user, string Data, string date, int createdby)
        {
            var memberJoinedActivity = UserRoundActivity.RoundUserRemoved(user, Data, date, createdby);
            Add(memberJoinedActivity);
        }

        public void RoundUserTemporarilyReplaced(UserProfile user, int roundgroupId, string categories, string date, int createdby, DateTime startdate, DateTime? endate)
        {
            var memberJoinedActivity = UserRoundActivity.RoundUserTemporarilyReplaced(user, roundgroupId, categories, date, createdby, startdate, endate);
            Add(memberJoinedActivity);
        }
        public void RoundUserPermanentlyReplaced(UserProfile user, int roundgroupId, string categories, string date, int createdby)
        {
            var memberJoinedActivity = UserRoundActivity.RoundUserPermanentlyReplaced(user, roundgroupId, categories, date, createdby);
            Add(memberJoinedActivity);
        }


        #endregion

        #region EPAssigned Activity

        public bool EpUserAssigned(UserActivityType actionType, int[] userIds, int[] epIds, int createdby)
        {
            var memberJoinedActivity = EpAssignmentActivity.EpUserAssignmentActivity(actionType, userIds, epIds, createdby);
            Add(memberJoinedActivity);
            return true;
        }

        public bool EpCopyToUser(UserActivityType actionType, int[] sourceUserIds, int[] toUserIds, int createdby)
        {
            var memberJoinedActivity = EpAssignmentActivity.EpCopyToUser(UserActivityType.EPTransferToUser, sourceUserIds, toUserIds, createdby);
            Add(memberJoinedActivity);
            return true;
        }

        #endregion

        #region Private Methods

        private IEnumerable<ActivityBase> ConvertToSpecificActivities(IEnumerable<Activity> activities)
        {
            var listedResults = new List<ActivityBase>();
            foreach (var activity in activities)
            {
                if (activity.Type == UserActivityType.EPAssigned.ToString() || activity.Type == UserActivityType.EPUnAssigned.ToString()
                    || activity.Type == UserActivityType.EPTransferToUser.ToString()
                    )
                {
                    var memberJoinedActivity = GenerateEPAssignedActivity(activity);

                    if (memberJoinedActivity != null)
                    {
                        listedResults.Add(memberJoinedActivity);
                    }
                }

            }
            return listedResults;
        }

        private EpAssignmentActivity GenerateEPAssignedActivity(Activity activity)
        {
            var dataPairs = ActivityBase.UnpackData(activity);

            if (!dataPairs.ContainsKey(EpAssignmentActivity.KeyUserId))
            {
                // Log the problem then skip
                _loggingService.Error($"An EP assignment activity record with id '{activity.Id}' has no user id in its data.");
                return null;
            }

            var baseUser = new List<UserProfile>();
            var toUser = new List<UserProfile>();
            var standardEps = new List<StandardEps>();

            var userId = dataPairs[EpAssignmentActivity.KeyUserId];
            if (!string.IsNullOrEmpty(userId))
            {
                int[] userIds = userId.Split(',').Select(Int32.Parse).ToArray();
                baseUser = _userService.GetUsers(userIds).ToList();
            }

            if (dataPairs.ContainsKey(EpAssignmentActivity.KeyEpIds))
            {
                var epIds = dataPairs[EpAssignmentActivity.KeyEpIds];
                if (!string.IsNullOrEmpty(epIds))
                {
                    int[] epIdArray = epIds.Split(',').Select(Int32.Parse).ToArray();
                    standardEps = _epRepository.GetStandardEPs(epIdArray);

                }
            }

            if (dataPairs.ContainsKey(EpAssignmentActivity.KeyUserIdTo))
            {
                var toUsers = dataPairs[EpAssignmentActivity.KeyUserIdTo];
                if (!string.IsNullOrEmpty(toUsers))
                {
                    int[] toUserArray = toUsers.Split(',').Select(Int32.Parse).ToArray();
                    toUser = _userService.GetUsers(toUserArray).ToList();
                }
            }

            //List<StandardEps> eps = new List<StandardEps>();
            var modifedUser = dataPairs[EpAssignmentActivity.KeyModifiedBy];

            if (baseUser == null)
            {
                // Log the problem then skip
                _loggingService.Error(
                    $"A member joined activity record with id '{activity.Id}' has a user id '{userId}' that is not found in the user table.");
                return null;
            }

            return new EpAssignmentActivity(activity, baseUser, standardEps, toUser);
        }


        #endregion
    }
}
