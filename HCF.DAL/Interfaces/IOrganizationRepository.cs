using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public interface IOrganizationRepository
    {
        List<Organization> GetMasterOrg();
        List<Organization> GetMasterOrganization();
        Organization GetMasterOrganization(string dbName);
        List<Organization> GetMasterOrganization(Guid parentOrgKey);
        List<Organization> UserOrganizations(int userId);
        Organization GetOrganization();

        Task<Organization> GetOrganizationAsync(Guid orgKey);

        bool CheckMenuExist(string menuname, string orgkey);
        Organization FireDrillSettings(Organization org);
        DataSet GetAllOrganizationDashInfo();
        Organization GetMasterData();
        HttpResponseMessage GetMastersData();
        List<OrgServices> GetMenuOrganization(int id);
        List<Score> GetScore();
        Organization GetUserDashBoard(int userId);
        Organization GetUserDashBoard(int userId, int menuId);
        Organization GetWoMasterData();
        HttpResponseMessage GetWoMastersData();
        int InsertMenuOrganization(OrgServices usermenu);
        Organization PermitSettings(Organization org);
        Organization UpdateOrganization(Organization newOrganization, string mode);

        bool IsPermitActiveByUserID(int userid, int permittype);
        Organization UpdateOrganizationSettings(Organization org);
    }
}