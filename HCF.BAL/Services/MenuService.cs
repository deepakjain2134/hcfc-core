using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }
        public List<Menus> GetMenus()
        {
            return _menuRepository.GetMenus().ToList();
        }

        public List<Menus> GetMenuMaster()
        {
            return _menuRepository.GetMenus();
        }

        public List<OrgServices> GetHCFMenus(int Id)
        {
            return _menuRepository.GetHCFMenus(Id);
        }

        public int Update(Menus menu)
        {
            return _menuRepository.Update(menu);
        }

        public bool UpdateMenuOrders(string newOrders)
        {
            return _menuRepository.UpdateMenuOrders(newOrders);
        }



        #region Module Master

        public List<ModuleMaster> GetAllModules(int clientno)
        {
            return _menuRepository.GetAllModules(clientno);
        }

        #endregion
    }
}
