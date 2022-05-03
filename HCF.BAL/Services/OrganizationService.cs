using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IEmailRepository emailRepository, IOrganizationRepository organizationRepository)
        {

            _emailRepository = emailRepository;
            _organizationRepository = organizationRepository;
        }

        public List<Organization> GetMasterOrg()
        {
            return _organizationRepository.GetMasterOrg();
        }
        public List<Organization> UserOrganizations(int userId)
        {
            return _organizationRepository.UserOrganizations(userId);
        }

        public async Task<Organization> GetOrganizationAsync(Guid orgKey)
        {
            return await _organizationRepository.GetOrganizationAsync(orgKey);
        }

        public List<Organization> GetOrganizations()
        {
            return _organizationRepository.GetMasterOrganization();
        }

        public List<Organization> GetOrganizations(Guid parentOrgKey)
        {
            return _organizationRepository.GetMasterOrganization(parentOrgKey);
        }

        public DataSet GetAllOrganizationDashInfo()
        {
            return _organizationRepository.GetAllOrganizationDashInfo();
        }

        public Organization GetMasterOrganization(string dbName)
        {
            return _organizationRepository.GetMasterOrganization(dbName);
        }


        public Organization GetOrganization()
        {
            return _organizationRepository.GetOrganization();
        }

        public Organization UpdateOrganization(Organization newOrganization, string mode)
        {
            return _organizationRepository.UpdateOrganization(newOrganization, mode);
        }

        public List<Score> GetScores()
        {
            return _organizationRepository.GetScore();
        }

        public Organization GetMasterData()
        {
            return _organizationRepository.GetMasterData();
        }


        public Organization GetWoMasterData()
        {
            return _organizationRepository.GetWoMasterData();
        }

        public HttpResponseMessage GetMastersData()
        {
            return _organizationRepository.GetMastersData();
        }

        public HttpResponseMessage GetWOMastersData()
        {
            return _organizationRepository.GetWoMastersData();
        }

        public Organization GetUserDashBoard(int userId)
        {
            return _organizationRepository.GetUserDashBoard(userId);
        }

        public Organization GetUserDashBoard(int userId, int menuId)
        {
            return _organizationRepository.GetUserDashBoard(userId, menuId);
        }

        public List<OrgServices> GetMenuOrganization(int id)
        {
            return _organizationRepository.GetMenuOrganization(id);
        }

        public int InsertMenuOrganization(OrgServices usermenu)
        {
            return _organizationRepository.InsertMenuOrganization(usermenu);
        }
        public bool CheckMenuExist(string menuname, string orgkey)
        {
            return _organizationRepository.CheckMenuExist(menuname, orgkey);
        }
        public bool IsPermitActiveByUserID(int userid, int permittype)
        {
            return _organizationRepository.IsPermitActiveByUserID(userid, permittype);
        }

        #region Floor Shapes






        #endregion

        #region Inbox Configuration

        public List<PopEmails> Emails()
        {
            return _emailRepository.GetEmails();
        }

        public int SaveEmailSettings(PopEmails emails)
        {
            return _emailRepository.SaveEmailSettings(emails);
        }
        #endregion

        public object FireDrillSettings(Organization org)
        {
            return _organizationRepository.FireDrillSettings(org);
        }

        public object PermitSettings(Organization org)
        {
            return _organizationRepository.PermitSettings(org);
        }

        public object UpdateOrganizationSettings(Organization organization)
        {
            return _organizationRepository.UpdateOrganizationSettings(organization);
        }
    }
}
