using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface ITipsRepository
    {
        bool ApproveTip(int tipId, int approveStatus);
        List<Tips> GetAllTipsByClientNo(int clientNo);
        List<Tips> GetTipsByClientNo(int clientNo, string routeUrl);
        bool InsertOrUpdateTips(Tips tip);
    }
}