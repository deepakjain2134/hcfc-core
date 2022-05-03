using HCF.BDO.Extensions;
using HCF.BDO.Users;
using HCF.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class UserProfile : IdentityUser<long>, IEntityWithTypedId<long>, IExtendableObject
    {
        [Column("UserId")]
        public override long Id { get; set; }

        [DataMember]
        [NotMapped]
        public int UserId { get; set; }

        [DataMember]
        public Guid Orgkey { get; set; }

        [DataMember]
        [Display(Name = "Name")]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsCRxUser { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Password { get; set; }


        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public string UserGroupIds { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public long CreatedDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(CreatedDate);
            set { }
        }

        [DataMember]
        //[NotMapped]
        public override string Email { get; set; }

        [DataMember]
        //[NotMapped]
        public override string PhoneNumber { get; set; }

        [DataMember]
        public override string UserName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Guid Salt { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public bool IsLoginEnable { get; set; }

        [NotMapped]
        public string IsLoginEnableText { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FileName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public string FileContent { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Roles> Roles { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public List<Roles> AdditionalRoles
        {
            get => GetAdditionalRoles();
            set { }
        }

        private List<Roles> GetAdditionalRoles()
        {
            if (Roles == null)
                return new List<Roles>();

            return Roles.Where(x => (x.IsAdditionalRole != null && x.IsAdditionalRole == 1)).ToList();
        }

        [DataMember(EmitDefaultValue = false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public string FullName
        {
            get => (FirstName ?? string.Empty) + " " + (!string.IsNullOrEmpty(LastName) ? LastName.Substring(0, 1) : string.Empty);
            //set { }
        }

        [NotMapped]
        public bool IsVendorUser
        {
            get => (VendorId.HasValue && VendorId.Value > 0);
            //set { }
        }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public string Name
        {
            get => (FirstName ?? "") + " " + (LastName ?? "");
            set { }
        }

        [DataMember]
        public bool IsInternalUser { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? VendorId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Vendors Vendor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Display(Name = "Cell #")]
        public string CellNo { get; set; }

        [DataMember]
        public bool IsSystemUser { get; set; }

        [DataMember]
        [Column("UserProfileId")]
        public Guid UserGuid { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public string StatusCode { get; set; }

        [DataMember]
        [NotMapped]
        public bool IsVerified { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [DisplayName("Skill Code")]
        [NotMapped]
        public string SkillCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public Skills Skills { get; set; }

        [DataMember]
        [NotMapped]
        public int? ResourceId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [DisplayName("Resource #")]
        [NotMapped]
        public string ResourceNumber { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public string TypeCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public int EpsCount { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public int DocsCount { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public int PolicyCount { get; set; }


        [DataMember]
        public bool IsPwdChange { get; set; }

        [DataMember]
        [NotMapped]
        public int TotalOrgCount { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public IEnumerable<Organization> Organizations { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<UserGroup> UserGroups { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public IEnumerable<UserLogin> UserLogin { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Certificates> UserCertificates { get; set; }

        [DataMember]
        [NotMapped]
        public bool IsPowerUser { get; set; }

        [DataMember]
        [NotMapped]
        public int UserStatusCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public string UserAdditionalRoleIds { get; set; }

        public DateTime? LastUpdatedPasswordDate { get; set; }
        public bool IsUserRole(Enums.UserRole userRole)
        {
            if (!string.IsNullOrEmpty(UserAdditionalRoleIds))
            {
                var split = UserAdditionalRoleIds.Split(',');
                foreach (var item in split)
                {
                    if (!string.IsNullOrEmpty(item) && userRole == (Enums.UserRole)Convert.ToUInt32(item))
                        return true;

                }
            }
            return false;
        }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public bool IsPowerUserOrSystemAdmin
        {
            get => (IsPowerUser || IsSystemUser);
            //set { }
        }


        public string GetVendorName()
        {
            return (Vendor != null && Vendor.VendorId > 0) ? Vendor.Name : string.Empty;
        }

        public List<UserViewHistory> UserViewHistory { get; set; }

        [NotMapped]
        public string LastViewUrl { get; set; }

        public bool HasPermission(string requiredPermission)
        {
            bool bFound = false;
            foreach (var role in this.Roles)
            {
                if (role.RoleName == requiredPermission)
                    bFound = true;

                if (bFound)
                    break;
            }
            return bFound;
        }

        public bool HasRoles(string roles)
        {
            bool bFound = false;
            string[] _roles = roles.ToLower().Split(',');
            foreach (var role in this.Roles)
            {

                bFound = _roles.Contains(role.RoleName.ToLower());
                if (bFound)
                    return bFound;

            }
            return bFound;
        }

        //public bool HasRole(string role)
        //{
        //    return (Roles.Where(p => p.RoleName == role).ToList().Count > 0);
        //}

        [DataMember]
        [NotMapped]
        public bool ISExistsRoundSchedules { get; set; }
        //[DataMember]
        //[NotMapped]
        //public List<UserProfile> allusers { get; set; }       

        [NotMapped]
        public virtual IList<UsedPassword> UserUsedPassword { get; set; }

        [NotMapped]
        public DateTime? OrientationDate { get; set; }
        public UserProfile()
        {
            AdditionalRoles = new List<Roles>();
            Roles = new List<Roles>();
            UserLogin = new List<UserLogin>();
            UserCertificates = new List<Certificates>();
            UserViewHistory = new List<UserViewHistory>();
            UserGroups = new List<UserGroup>();
            //allusers = new List<UserProfile>();

        }
        public UserProfile(int userId, string firstName, string lastName, string email)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;

        }

        [NotMapped]
        public long[] DrawingIds { get; set; }

        [NotMapped]
        public long[] PermitTypeIds { get; set; }

        [NotMapped]
        public List<EPDetails> EPDetails { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public bool IsChangePasswordRequired { get; set; }

        [DataMember]
        [NotMapped]
        public int? ClientNo { get; set; }

        public string ExtensionData { get; set; }
    }

    public class UserUpdatePassword
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string NewPassword { get; set; }

    }

    [DataContract]
    public class UserViewHistory
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string PageUrl { get; set; }

        [DataMember]
        public DateTime CreatedDateTime { get; set; }

    }

}