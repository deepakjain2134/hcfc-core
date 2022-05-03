using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.BAL.Interfaces.Services
{
    public interface IVendorService
    {
        List<Vendors> GetVendors();

        Vendors GetVendor(int vendorId);

        List<Vendors> GetVendorsUser();

        int AddVendors(Vendors newVendors, bool isMasterVendor, int masterVendorId, Guid? invitationId, string siteAssignees);

        bool UpdateVendor(Vendors newVendor, string siteAssigneeType);

        bool DeleteCertificates(int vendorId);

        List<Vendors> GetVendorDetails();

        bool UpdateContactDetails(string contactDetails, int vendorId, string messageToContractor, int Status , int organizationId = 1);

        VendorRecordsCount GetVendorDashboardData(int vendorId, int userId);

        object IsVendorExist(string OrganizationId, string Email);

        Vendors GetVendorbyOrgid(string OrganizationId);

        

        List<Vendors> GetVendorResources(int? vendorId);

        List<Vendors> GetVendorResourcesDashboard(int? vendorId);

        List<Vendors> GetVendorByPrefix(string prefix);

        int SaveVendorInvitation(VendorOrganizations org);

        List<VendorResource> GetVendorResource(int? vendorId);
        int AddVendorResources(VendorResource vmodel);
    }
}
