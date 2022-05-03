using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface ITipsService
    {
        bool ApproveTip(int tipId, int ApproveStatus);
        List<Tips> GetAllTips(int clientNo);
        List<Tips> GetTipsByClientNo(int clientNo, string routeUrl);
        bool InsertOrUpdateTip(Tips tip);
    }
}