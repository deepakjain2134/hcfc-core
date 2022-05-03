using HCF.BDO;
using HCF.DAL.Interfaces;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL.Repository
{
    public class UserSiteRepository : IUserSiteRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public UserSiteRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public IEnumerable<UserSiteMapping> GetUserSiteMappings()
        {
            var results = new List<UserSiteMapping>();
            const string sql = StoredProcedures.Get_UserSiteMappings;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null)
                    results = _sqlHelper.ConvertDataTable<UserSiteMapping>(dt);
            }
            return results;
        }


        public IEnumerable<UserSiteMapping> GetUserSiteMappings(int userId)
        {
            return GetUserSiteMappings().Where(x => x.UserId == userId);
        }

        public IEnumerable<UserSiteMapping> GetVendorSiteMappings()
        {
            return GetUserSiteMappings().Where(x => x.VendorId.HasValue).ToList();
        }

        public IEnumerable<UserSiteMapping> GetVendorSiteMappings(int vendorId)
        {
            return GetVendorSiteMappings().Where(x => x.VendorId == vendorId).ToList();
        }
    }
}
