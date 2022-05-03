using HCF.BDO;
using HCF.Utility;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HCF.DAL
{
    public  class SiteRepository : ISiteRepository
    {
        private readonly ISqlHelper _sqlHelper;

        public SiteRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public int Save(Site site)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Sites;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SiteId", site.SiteId);
                command.Parameters.AddWithValue("@SiteTypeId", site.SiteTypeId);
                command.Parameters.AddWithValue("@Name", site.Name);
                command.Parameters.AddWithValue("@Code", site.Code);
                command.Parameters.AddWithValue("@IsActive", site.IsActive);
                command.Parameters.AddWithValue("@Description", site.Description);                
                command.Parameters.AddWithValue("@IsInternal", site.IsInternal);
                command.Parameters.AddWithValue("@StateId", site.StateId.HasValue?site.StateId.Value:0);
                command.Parameters.AddWithValue("@CityId", site.CityId.HasValue?site.CityId.Value:0);
                command.Parameters.AddWithValue("@Address", site.Address);

                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Site> GetSites()
        {
            List<Site> sites = new List<Site>();
            string sql = StoredProcedures.Get_Sites;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null)
                    sites = _sqlHelper.ConvertDataTable<Site>(dt);
            }


            return sites;
        }

        public List<Site> GetSitesByUserId(int UserId)
        {
            List<Site> sites = new List<Site>();
            string sql = StoredProcedures.Get_Sites;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", UserId);
                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null)
                    sites = _sqlHelper.ConvertDataTable<Site>(dt);
            }


            return sites;
        }
        public  List<FireSystemType> GetFireSystemTypeDetails()
        {
            List<FireSystemType> FireSystemType = new List<FireSystemType>();
            string sql = StoredProcedures.Get_FireSystemType;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null)
                    FireSystemType = _sqlHelper.ConvertDataTable<FireSystemType>(dt);
            }
            return FireSystemType;
        }
        public  int Insert_FireSystemType(FireSystemType FireSystemType)
        {
            int newId;
            const string sql = StoredProcedures.Insert_FireSystemType;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", FireSystemType.ID);
                command.Parameters.AddWithValue("@SiteId", string.IsNullOrEmpty(FireSystemType.SiteId) ?string.Empty:FireSystemType.SiteId);
                command.Parameters.AddWithValue("@Name", FireSystemType.Name);
                command.Parameters.AddWithValue("@CreatedBy", FireSystemType.CreatedBy);
                command.Parameters.AddWithValue("@IsDelete", FireSystemType.IsDelete);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        #region State and City
        public  List<StateMaster> GetStates()
        {
            List<StateMaster> states = new List<StateMaster>();
            string sql = StoredProcedures.Get_States;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetCommonDataTable(cmd);
                if (dt != null)
                    states = _sqlHelper.ConvertDataTable<StateMaster>(dt);
            }
            return states;
        }
        public  int AddStateName(StateMaster state)
        {
            int newId;
            const string sql = StoredProcedures.Insert_AddState;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StateName", state.StateName);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId ;
        }

        public  List<CityMaster> GetCities(int stateId)
        {
            List<CityMaster> cities = new List<CityMaster>();
            string sql = StoredProcedures.Get_Cities;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stateId", stateId);
                var dt = _sqlHelper.GetCommonDataTable(cmd);
                if (dt != null)
                    cities = _sqlHelper.ConvertDataTable<CityMaster>(dt);
            }
            return cities;
        }
        public  int AddCity(CityMaster city)
        {
            int newId;
            const string sql = StoredProcedures.Insert_AddCity;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;               
                command.Parameters.AddWithValue("@CityName", city.CityName);
                command.Parameters.AddWithValue("@CityCode", city.CityCode);
                command.Parameters.AddWithValue("@StateId", city.StateId);
                command.Parameters.AddWithValue("@CreatedBy", city.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", city.IsActive);
                command.Parameters.AddWithValue("@CreatedDate", city.CreatedDate);                
                command.Parameters.AddWithValue("@Zipcode", city.Zipcode);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  List<SiteType> GetSiteType()
        {
            List<SiteType> siteTypes = new List<SiteType>();
            string sql = StoredProcedures.Get_SiteType;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null)
                    siteTypes = _sqlHelper.ConvertDataTable<SiteType>(dt);
            }
            return siteTypes;
        }
        #endregion
    }
}