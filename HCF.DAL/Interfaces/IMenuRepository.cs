using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IMenuRepository
    {
        List<ModuleMaster> GetAllModules(int clientno);
        List<OrgServices> GetHCFMenus(int id);
        List<Menus> GetMenus();
        int Update(Menus menus);
        bool UpdateMenuOrders(string newOrders);
    }
}