using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IVendorRepository
    {
        int AddVendor(Vendors newVendors, bool isMasterVendor, int masterVendorId, Guid? invitationId, string siteAssignees);
        int AddVendorResources(VendorResource vmodel);
        bool DeleteCertificates(int vendorId);
        List<Vendors> GetVendor(int? vendorId);
        Vendors GetVendorbyOrgid(string OrganizationId);
        List<Vendors> GetVendorByPrefix(string prefix);
        VendorRecordsCount GetVendorDashboardData(int vendorId, int userId);
        List<Vendors> GetVendorResources(int? vendorId);
        List<Vendors> GetVendorResourcesDashboard(int? vendorId);
        List<Vendors> GetVendors();
        object IsVendorExist(string OrganizationId, string Email);
        int SaveVendorInvitation(VendorOrganizations org);
        bool UpdateContactDetails(string contactDetails, int vendorId, string messageToContractor, int Status, int organizationId = 1);
        bool UpdateVendor(Vendors newVendors, string siteAssigneeType);
        List<VendorResource> GetVendorResource(int? vendorId);
    }
}