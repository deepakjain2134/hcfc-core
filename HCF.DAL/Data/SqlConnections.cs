using HCF.Utility;
using HCF.Utility.AppConfig;
using HCF.Utility.Tenant;

namespace HCF.DAL
{
    public class SqlConnections : ISqlConnection
    {
        private readonly IHCFSession _session;
        private readonly IAppSetting _appSetting;
        //public string TenantId { get; set; }
        // private readonly ITenantService _tenantService;

        public SqlConnections(IHCFSession session, IAppSetting appSetting)
        {
            _session = session;
            _appSetting = appSetting;
            //_tenantService = tenantService;
            //TenantId = _tenantService.GetTenant()?.TID;
        }

        public string ConnectionString()
        {
            string dbName = _session.ClientDbName;
            dbName = string.IsNullOrEmpty(dbName) ? _appSetting.CommonDB : dbName;
            return string.Format(_appSetting.HcfHospitalConnection, dbName);
        }

        public string CommonConnectionString()
        {
            return string.Format(_appSetting.HCFConnection, _appSetting.CommonDB);
        }
    }
}