using HCF.BAL.Models.General;
using HCF.BDO;
using HCF.BDO.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public partial interface IActivityService
    {
        void RoundUserAdded(UserProfile user, int roundgroupId , int categories , string date , int createdby);
       
        void RoundUserRemoved(UserProfile user ,string Data, string date, int createdby);

        void RoundUserTemporarilyReplaced(UserProfile user, int roundgroupId, string categories, string date, int createdby, DateTime startdate, DateTime? endate);

        void RoundUserPermanentlyReplaced(UserProfile user, int roundgroupId, string categories, string date, int createdby);

        Activity Add(Activity newActivity);

        //Activity Get(Guid id);

        //IList<Activity> GetList();

        bool EpUserAssigned(UserActivityType actionType, int[] userIds, int[] epIds,int createdby);        

       // Task<PaginatedList<ActivityBase>> GetPagedGroupedActivities(int pageIndex, int pageSize);

        bool EpCopyToUser(UserActivityType actionType, int[] userIds, int[] toUserIds, int createdby);
    }
}
