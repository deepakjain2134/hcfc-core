
using HCF.BDO.Enums;

namespace HCF.BDO
{
    public sealed class UserProfileActivity : ActivityBase
    {
        public const string KeyUserId = @"UserID";

        public UserProfile User { get; set; }

        /// <summary>
        /// Constructor - useful when constructing a badge activity after reading database
        /// </summary>
        public UserProfileActivity(Activity activity, UserProfile user)
        {
            ActivityMapped = activity;
            User = user;
        }

        public static Activity UserProfileAdded(UserProfile user)
        {
            return new Activity
            {
                Data = KeyUserId + Equality + user.UserId,
                Timestamp = user.CreatedDate.Value,
                Type = UserActivityType.UserProfileAdded.ToString()
            };
        }

        public static Activity UserProfileUpdated(UserProfile user)
        {
            return new Activity
            {
                Data = KeyUserId + Equality + user.UserId,
                Timestamp = user.CreatedDate.Value,
                Type = UserActivityType.UserProfileUpdated.ToString()
            };
        }
    }
}