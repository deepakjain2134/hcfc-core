using HCF.BDO.Enums;
using System;


namespace HCF.BDO
{
    public class UserRoundActivity : ActivityBase
    {
        public const string KeyUserId = @"UserID";
        public const string KeyLocationGroupId = @"LocationGroupId";
        public const string KeyCategoriesGroupId = @"CategoriesGroupId";
        public const string KeyRoundGroupId = @"RoundGroupId";
        public const string KeystartDate = @"StartDate";
        public const string KeyendDate = @"EndDate";

        public UserProfile _user { get; set; }

        public RoundGroupLocations _roundGroupLocations { get; set; }

        public UserRoundActivity(Activity activity, UserProfile user, RoundGroupLocations roundGroupLocations)
        {
            ActivityMapped = activity;
            _user = user;
            _roundGroupLocations = roundGroupLocations;
        }

        public static Activity RoundUserAdded(UserProfile user, int roundgroupId, int categories, string date, int createdby)
        {    
            if(user!=null && user.CreatedDate.HasValue && createdby>0 && roundgroupId>0 && categories>0 && !string.IsNullOrEmpty(date))
            {
                return new Activity
                {
                    Data = KeyUserId + Equality + user.UserId + Separator + KeyCategoriesGroupId + Equality + categories + Separator + KeyRoundGroupId + Equality + roundgroupId,
                    //Data = KeyLocationGroupId + Equality + groupLocation.RoundGroupLocationId + Separator + KeyUserId + Equality + user.UserId,
                    Timestamp = user.CreatedDate.Value,
                    Type = UserActivityType.Added.ToString(),
                    EffectiveDate = Convert.ToDateTime(date),
                    CreatedDate = DateTime.Today,
                    CreatedBy = createdby,
                    IsMailed = false,
                    NotificationDate = null,
                    IsEnabled = false
                };
            }
            else
            {
                Activity Activity = new Activity();
                return Activity;

            }
            
        }

        public static Activity RoundUserRemoved(UserProfile user, string Data, string date, int createdby)
        {
            return new Activity
            {
                Data = Data,
                Timestamp = user.CreatedDate.Value,
                Type = UserActivityType.Removed.ToString(),
                EffectiveDate = Convert.ToDateTime(date),
                CreatedDate = DateTime.Today,
                CreatedBy = createdby,
                IsMailed = false,
                NotificationDate = null,
                IsEnabled = false
            };
        }

        public static Activity RoundUserTemporarilyReplaced(UserProfile user, int roundgroupId, string categories , string date , int createdby, DateTime startdate, DateTime? endate)
        {
            return new Activity
            {
                Data = KeyUserId + Equality + user.UserId + Separator + KeyCategoriesGroupId + Equality + categories + Separator + KeyRoundGroupId + Equality + roundgroupId + Separator + KeystartDate + Equality + startdate + Separator + KeyendDate + Equality + endate,
                // Data = KeyLocationGroupId + Equality + groupLocation.RoundGroupLocationId + Separator + KeyUserId + Equality + user.UserId,
                Timestamp = user.CreatedDate.HasValue?user.CreatedDate.Value:DateTime.Now,
                Type = UserActivityType.TemporaryReplaced.ToString(),
                EffectiveDate = Convert.ToDateTime(date),
                CreatedDate = DateTime.Today,
                CreatedBy = createdby,
                IsMailed = false,
                NotificationDate = null,
                IsEnabled = false
            };
        }


        public static Activity RoundUserPermanentlyReplaced(UserProfile user, int roundgroupId, string categories, string date, int createdby)
        {
            return new Activity
            {
                Data = KeyUserId + Equality + user.UserId + Separator + KeyCategoriesGroupId + Equality + categories + Separator + KeyRoundGroupId + Equality + roundgroupId,
                // Data = KeyLocationGroupId + Equality + groupLocation.RoundGroupLocationId + Separator + KeyUserId + Equality + user.UserId,

                Timestamp =  user.CreatedDate ==null ? Convert.ToDateTime(date): user.CreatedDate.Value,

                Type = UserActivityType.PermanentlyReplaced.ToString(),
                EffectiveDate = Convert.ToDateTime(date),
                CreatedDate = DateTime.Today,
                CreatedBy = createdby,
                IsMailed = false,
                NotificationDate = null,
                IsEnabled = false
            };
        }
    }
}
