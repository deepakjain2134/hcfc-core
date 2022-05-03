using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IMenuService
    {
        List<ModuleMaster> GetAllModules(int clientno);
        List<OrgServices> GetHCFMenus(int Id);
        List<Menus> GetMenuMaster();
        List<Menus> GetMenus();
        int Update(Menus menu);
        bool UpdateMenuOrders(string newOrders);
    }
}