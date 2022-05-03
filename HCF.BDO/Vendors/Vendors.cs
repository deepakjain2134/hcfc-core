using HCF.BDO.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;


namespace HCF.BDO
{
    [DataContract]
    public class Vendors
    {
        [DataMember]
        [Key]
        public int VendorId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Required]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        //[Required]
        public string Address { get; set; }

        [DataMember(EmitDefaultValue = false)]
        //[Required]
        public string RegistrationNo { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long CreatedDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(CreatedDate);
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        public List<Certificates> VendorCertificates { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<UserProfile> Users { get; set; }


        [DataMember]
        //[Required]        
        public string PhoneNo { get; set; }

        [DataMember]
        public string CellNo { get; set; }

        [DataMember]
        //[Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataMember]
        public string ContactDetails { get; set; }

        [DataMember]
        public string MessageToContractor { get; set; }

        [DataMember]
        public string BuildingIds { get; set; }

        #region VendorResource
        [DataMember]
        public string UploadLink { get; set; }
        [DataMember]
        //[Required(ErrorMessage = "Custom Name is required ")]        
        public string CustomName { get; set; }

        [DataMember]
        public string TFileId { get; set; }
        [DataMember]
        public DateTime? EffectiveDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long EffectiveDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(EffectiveDate);
            set { }
        }
        [DataMember]
        public List<TFiles> File { get; set; }

        #endregion

        public int? ClientNo { get; set; }

        public bool IsAdded { get; set; }

        public virtual List<UserSiteMapping> VendorSites { get; set; }

        [NotMapped]
        public int[] DrpDrawingsIds { get; set; }

        [NotMapped]
        public List<SelectListItem> DrawingTypes { get; set; }

        public List<VendorResource> VendorResource { get; set; }

        public Vendors()
        {
            VendorCertificates = new List<Certificates>();
            File = new List<TFiles>();
            VendorSites = new List<UserSiteMapping>();
            VendorResource = new List<VendorResource>();

        }
    }

    public class VendorRecordsCount
    {
        public int TotalPendingSubmission
        {
            get => getTotalPendingSubmission();
            set { }
        }

        public int TotalSubmittedPermits
        {

            get => getTotalSubmittedPermits();
            set { }
        }

        public int TotalApprovedPermits
        {
            get => getTotalApprovedPermits();
            set { }
        }

        public int FSBPPendingSubmission { get; set; }

        public int FSBPSubmittedPermits { get; set; }

        public int FSBPApprovedPermits { get; set; }

        public int CAPPendingSubmission { get; set; }

        public int CAPSubmittedPermits { get; set; }

        public int CAPApprovedPermits { get; set; }

        public int MOPPendingSubmission { get; set; }

        public int MOPSubmittedPermits { get; set; }

        public int MOPApprovedPermits { get; set; }

        public int HWPPendingSubmission { get; set; }

        public int HWPSubmittedPermits { get; set; }

        public int HWPApprovedPermits { get; set; }

        public int LSDPendingSubmission { get; set; }

        public int LSDSubmittedPermits { get; set; }

        public int LSDApprovedPermits { get; set; }

        public int PCRAPendingSubmission { get; set; }

        public int PCRASubmittedPermits { get; set; }

        public int PCRAApprovedPermits { get; set; }
        public int CRAPendingSubmission { get; set; }

        public int CRASubmittedPermits { get; set; }

        public int CRAApprovedPermits { get; set; }
        public int ICRAPendingSubmission { get; set; }

        public int ICRASubmittedPermits { get; set; }

        public int ICRAApprovedPermits { get; set; }

        public string ContactDetails { get; set; }

        public string MessageToContractor { get; set; }

        public string OrgEmail { get; set; }

        private int getTotalPendingSubmission()
        {
            return FSBPPendingSubmission + CAPPendingSubmission + MOPPendingSubmission + HWPPendingSubmission + LSDPendingSubmission + ICRAPendingSubmission + CRAPendingSubmission + PCRAPendingSubmission;
        }

        private int getTotalSubmittedPermits()
        {
            return FSBPSubmittedPermits + CAPSubmittedPermits + MOPSubmittedPermits + HWPSubmittedPermits + LSDSubmittedPermits + ICRASubmittedPermits + CRASubmittedPermits + PCRASubmittedPermits;
        }

        private int getTotalApprovedPermits()
        {
            return FSBPApprovedPermits + CAPApprovedPermits + MOPApprovedPermits + HWPApprovedPermits + LSDApprovedPermits + ICRAApprovedPermits + CRAApprovedPermits + PCRAApprovedPermits;
        }

    }

    [DataContract]
    public class VendorOrganizations
    {
        [DataMember] [Key] public int Id { get; set; }

        [DataMember] public Guid InvitationId { get; set; }

        [DataMember] public int VendorId { get; set; }

        [DataMember] public Guid OrgKey { get; set; }

        [DataMember] public bool IsRequested { get; set; }

        [DataMember] public DateTime RequestedDate { get; set; }

        [DataMember] public Guid RequestedBy { get; set; }

        [DataMember] public Organization Organization { get; set; }

        [DataMember] public Vendors Vendors { get; set; }

    }

    [DataContract]
    public class VendorSettings
    {
        [DataMember] public int VendorsSettingId { get; set; }

        [DataMember] public int VendorId { get; set; }

        [DataMember] public string MessageToContractor { get; set; }

        [DataMember] public string ContactDetails { get; set; }

    }

    [DataContract]
    public class VendorResource
    {
        [DataMember][Key] public int VendorResId { get; set; }

        [DataMember] public int? VendorId { get; set; }

        [DataMember] public string UploadLink { get; set; }

        [DataMember] public string CustomName { get; set; }

        [DataMember] public string TFileId { get; set; }

        [DataMember] public DateTime? EffectiveDate { get; set; }

        [DataMember] public DateTime? EndDate { get; set; }

        [DataMember] public DateTime? EventStartDate { get; set; }

        [DataMember] public DateTime? EventEndDate { get; set; }

        [DataMember] public bool IsActive { get; set; }

        [DataMember] public int? CreatedBy { get; set; }

        [DataMember] public DateTime? CreatedDate { get; set; }

        [DataMember]
        public List<TFiles> File { get; set; }



        public VendorResource()
        {
            File = new List<TFiles>();
        }

        public Vendors Vendor { get; set; }

        public string GetVendorName()
        {
            return (Vendor != null) ? Vendor.Name : "";
        }

    }
}