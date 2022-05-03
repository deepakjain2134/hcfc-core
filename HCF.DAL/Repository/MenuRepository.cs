using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public MenuRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public  List<Menus> GetMenus()
        {
            var menus = new List<Menus>();
            const string sql = StoredProcedures.Get_Menus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    menus = _sqlHelper.ConvertDataTable<Menus>(ds.Tables[0]);
                    foreach (var menu in menus)
                    {
                        menu.Alias = menu.Alias.Trim();
                    }
                }
            }
            return menus;
        }

        public  List<OrgServices> GetHCFMenus(int id)
        {
            var menus = new List<OrgServices>();
            const string sql = StoredProcedures.Get_HCFMenus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganizationKey", id);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    menus = _sqlHelper.ConvertDataTable<OrgServices>(ds.Tables[0]);
                }
            }
            return menus;
        }

        public  bool UpdateMenuOrders(string newOrders)
        {
            const string sql = StoredProcedures.Update_MenuOrders;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@newOrders", newOrders);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return true;
        }

        public  int Update(Menus menus)
        {
            var newId = 0;
            const string sql = StoredProcedures.Update_Menus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", menus.Id);
                command.Parameters.AddWithValue("@ParentId", menus.ParentId);
                command.Parameters.AddWithValue("@Description", menus.Description);
                command.Parameters.AddWithValue("@Status", menus.IsActive);
                command.Parameters.AddWithValue("@PageUrl", menus.PageUrl);
                command.Parameters.AddWithValue("@CreatedBy", menus.CreatedBy);
                command.Parameters.AddWithValue("@ImagePath", menus.ImagePath);
                command.Parameters.AddWithValue("@Alias", menus.Alias);
                command.Parameters.AddWithValue("@Name", menus.Name);
                command.Parameters.AddWithValue("@Type", menus.Type);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }



        #region Module Master

        public  List<ModuleMaster> GetAllModules(int clientno)
        {
            var moduleMaster = new List<ModuleMaster>();
            var menus = new List<Menus>();
            const string sql = StoredProcedures.Get_ModuleMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Clientno", clientno);
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                {
                    moduleMaster = _sqlHelper.ConvertDataTable<ModuleMaster>(ds.Tables[0]);
                    menus = _sqlHelper.ConvertDataTable<Menus>(ds.Tables[1]);
                    foreach (var module in moduleMaster)
                    {
                        module.Menus = new List<Menus>();
                        if (!string.IsNullOrEmpty(module.MenuIds))
                        {
                            int[] menuArray = module.MenuIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                            module.Menus = menus.ToList().Where(x => menuArray.Contains(x.Id)).ToList();
                        }
                    }
                }
            }
            return moduleMaster;
        }

        #endregion
    }
}
