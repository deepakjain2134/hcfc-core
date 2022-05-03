using HCF.BDO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace HCF.Web.ViewModels.Users
{
    public class UserAddViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(150)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(150)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required]       
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Phone #")]
        [StringLength(16)]
        [RegularExpression(@"^[(]?\d{3}[)]?[(\s)?.-]\d{3}[\s.-]\d{4}$", ErrorMessage = "Characters are not allowed.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Cell #")]
        [StringLength(16)]
        [RegularExpression(@"^[(]?\d{3}[)]?[(\s)?.-]\d{3}[\s.-]\d{4}$", ErrorMessage = "Characters are not allowed.")]
        public string CellNumber { get; set; }


        public BDO.Organization Organization { get; set; }

        public BDO.Vendors Vendor { get; set; }

        public Guid? Orgkey { get; set; }

        public int? VendorId { get; set; }


        public List<SelectListItem> DrpUserGroups { get; set; }

        public List<SelectListItem> DrpUserAdditionRole { get; set; }

        public List<SelectListItem> DrpDrawings { get; set; }

        public List<SelectListItem> DrpVendors { get; set; }

        public List<SelectListItem> DrpPermitTypes { get; set; }

        public int UserRegisterType { get; set; }

        public bool IsVendorUser { get; set; }

        [DataMember]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public bool IsPowerUser { get; set; }

        public string ResourceNumber { get; set; }

        //[Required(ErrorMessage ="Please select atleast one role group")]
        public long[] DrpUserGroupsIds { get; set; }

        // [Required(ErrorMessage = "Please select atleast one role")]
        public long[] DrpUserAdditionRoleIds { get; set; }

        public long[] DrpDrawingsIds { get; set; }

        public long[] PermitTypeIds { get; set; }

        public bool IsPwdChange { get; set; }

        public int UserId { get; set; }

        public List<UserSiteMapping> UserSites { get; set; }

        public string FileName { get; set; }

        public IFormFile UserImage { get; set; }

        public string FileContent { get; set; }

        public VendorOrganizations vendorOrg { get; set; }

        public DateTime? OrientationDate { get; set; }

        public int LoginCount { get; set; }
    }
}