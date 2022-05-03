using HCF.BDO.Enums;
using System;
using System.Collections.Generic;

namespace HCF.BDO
{
    public class EpAssignmentActivity : ActivityBase
    {
        public const string KeyUserId = @"UserID";
        public const string KeyEpIds = @"EpDetailIds";
        public const string KeyUserIdTo = @"UserIdTo";
        public const string KeyModifiedBy = @"ModifiedBy";

        public List<UserProfile> _user { get; set; }

        public List<UserProfile> _toUser { get; set; }

        public List<StandardEps> _standardEps { get; set; }

        public UserProfile _modifedBy { get; set; }

        public EpAssignmentActivity(Activity activity, List<UserProfile> user, List<StandardEps> standardEps, List<UserProfile> toUsers)
        {
            ActivityMapped = activity;
            _user = user;
            _standardEps = standardEps;
            _toUser = toUsers;
        }

        public static Activity EpUserAssignmentActivity(UserActivityType activityAction, int[] userIds, int[] epIds, int createdBy)
        {
            return new Activity
            {
                Data = KeyUserId + Equality +string.Join(",", userIds) + Separator + KeyEpIds + Equality + string.Join(",", epIds) + Separator + KeyModifiedBy + Equality + createdBy,
                Timestamp = DateTime.Today,
                Type = activityAction.ToString(),
                CreatedDate = DateTime.UtcNow,
                CreatedBy = createdBy,
                NotificationDate = null,
                IsEnabled = false
            };
        }

        public static Activity EpCopyToUser(UserActivityType activityAction, int[] sourcUserIds, int[] userIdTo, int createdBy)
        {
            return new Activity
            {
                Data = KeyUserId + Equality + string.Join(",", sourcUserIds) + Separator + KeyUserIdTo + Equality + string.Join(",", userIdTo) + Separator + KeyModifiedBy + Equality + createdBy,
                Timestamp = DateTime.Today,
                Type = activityAction.ToString(),
                CreatedDate = DateTime.UtcNow,
                CreatedBy = createdBy,
                NotificationDate = null
            };
        }
    }
}