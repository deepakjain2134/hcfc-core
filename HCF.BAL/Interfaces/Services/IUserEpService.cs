using HCF.BDO;
using HCF.BDO.Enums;
using HCF.Utility.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCF.BAL.Interfaces.Services
{
    public interface IUserEpService
    {
        IEnumerable<UserProfile> GetEpUsers(int epdetailId);

        //IEnumerable<StandardEps> GetUserEps(int userId);

        IDictionary<UserActivityType, int[]> GetUpdatedRecordsByEp(int epDetailId, int[] userIds);

        IDictionary<UserActivityType, int[]> GetUpdatedRecordsByUser(int userId, int[] epDetailIds);

        bool UpdateEpUser(int epId, int modifiedBy);

        bool UpdateUserEps(int userId, int modifiedBy);

        bool SaveEPUser(List<EPAssignee> model);

        //IEnumerable<EPDetails> GetEpUsers();

        // Task<List<EPDetails>> GetEpUserList();

        List<EPAssignee> Get();
    }
}
