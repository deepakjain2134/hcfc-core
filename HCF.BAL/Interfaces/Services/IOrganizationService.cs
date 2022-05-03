using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IOrganizationService
    {
        Task<Organization> GetOrganizationAsync(Guid orgKey);
        Organization GetMasterOrganization(string dbName);


        List<Organization> GetMasterOrg();

        bool CheckMenuExist(string menuname, string orgkey);
        bool IsPermitActiveByUserID(int userid, int permittype);
        List<PopEmails> Emails();
        object FireDrillSettings(Organization org);
        DataSet GetAllOrganizationDashInfo();
        Organization GetMasterData();
        HttpResponseMessage GetMastersData();
        List<OrgServices> GetMenuOrganization(int id);
        Organization GetOrganization();
        List<Organization> GetOrganizations();
        List<Organization> GetOrganizations(Guid parentOrgKey);
        //List<Organization> GetOrganizations(int userId);
        List<Score> GetScores();
        Organization GetUserDashBoard(int userId);
        Organization GetUserDashBoard(int userId, int menuId);
        Organization GetWoMasterData();
        HttpResponseMessage GetWOMastersData();
        int InsertMenuOrganization(OrgServices usermenu);
        object PermitSettings(Organization org);
        int SaveEmailSettings(PopEmails emails);

        Organization UpdateOrganization(Organization newOrganization, string mode);
        List<Organization> UserOrganizations(int userId);

        object UpdateOrganizationSettings(Organization organization);
    }
}