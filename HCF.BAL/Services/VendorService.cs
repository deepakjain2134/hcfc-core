using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;
      
        public VendorService(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }
      

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Vendors> GetVendors()
        {
            return _vendorRepository.GetVendors();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        public Vendors GetVendor(int vendorId)
        {
            return _vendorRepository.GetVendor(vendorId).FirstOrDefault();
        }


        public List<Vendors> GetVendorsUser()
        {
            return _vendorRepository.GetVendor(null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newVendors"></param>
        /// <returns></returns>
        public int AddVendors(Vendors newVendors, bool isMasterVendor, int masterVendorId, Guid? invitationId, string siteAssignees)
        {
            return _vendorRepository.AddVendor(newVendors, isMasterVendor, masterVendorId, invitationId, siteAssignees);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newVendor"></param>
        /// <returns></returns>
        public bool UpdateVendor(Vendors newVendor, string siteAssigneeType)
        {
            return _vendorRepository.UpdateVendor(newVendor, siteAssigneeType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        public bool DeleteCertificates(int vendorId)
        {
            return _vendorRepository.DeleteCertificates(vendorId);
        }

        public List<Vendors> GetVendorDetails()
        {
            return _vendorRepository.GetVendors();
        }

        public bool UpdateContactDetails(string contactDetails, int vendorId, string messageToContractor,int Status, int organizationId = 1)
        {
            return _vendorRepository.UpdateContactDetails(contactDetails, vendorId, messageToContractor,Status, organizationId);
        }
        //public  bool saveBuildingIds(string vendorId, string buildingsId)
        //{
        //    return DAL.VendorRepository.SaveBuildingIds(vendorId, buildingsId);
        //}
        //public  Vendors GetVendorsBuildings(int vendorId)
        //{
        //    return DAL.VendorRepository.GetVendorsBuildings(vendorId);
        //}
        public VendorRecordsCount GetVendorDashboardData(int vendorId, int userId)
        {
            return _vendorRepository.GetVendorDashboardData(vendorId, userId);
        }

        public object IsVendorExist(string OrganizationId, string Email)
        {
            return _vendorRepository.IsVendorExist(OrganizationId, Email);
        }
        public Vendors GetVendorbyOrgid(string OrganizationId)
        {
            return _vendorRepository.GetVendorbyOrgid(OrganizationId);
        }

       
        public List<Vendors> GetVendorResources(int? vendorId)
        {
            return _vendorRepository.GetVendorResources(vendorId);
        }
        public List<Vendors> GetVendorResourcesDashboard(int? vendorId)
        {
            return _vendorRepository.GetVendorResourcesDashboard(vendorId);
        }

        public List<Vendors> GetVendorByPrefix(string prefix)
        {
            return _vendorRepository.GetVendorByPrefix(prefix);
        }

        public int SaveVendorInvitation(VendorOrganizations org)
        {
            return _vendorRepository.SaveVendorInvitation(org);
        }

        public List<VendorResource> GetVendorResource(int? vendorId)
        {
            return _vendorRepository.GetVendorResource(vendorId);
        }

        public int AddVendorResources(VendorResource vmodel)
        {
            return _vendorRepository.AddVendorResources(vmodel);
        }

    }
}
