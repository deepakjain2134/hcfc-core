using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HCF.BDO
{
    [DataContract]
    public class UserRoles
    {
        [DataMember][Key] public int UserRoleId { get; set; }

        [DataMember] public int? UserId { get; set; }

        [DataMember] public int? RoleId { get; set; }

        [DataMember] public bool? Status { get; set; }

        [Display(Name ="Role Name")]
        [DataMember]
        public string RoleName { get; set; }

        [Display(Name="Display Text")]
        [DataMember]
        public string DisplayText { get; set; }

        [DataMember]
        public int? Sequence { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int? ParentId { get; set; }

        [Display(Name ="Parent Name")]
        [DataMember]
        public string ParentName { get; set; }

        [DataMember]
        public bool IsChild { get; set; }

        [DataMember]
        public bool IsUserRole { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }
    }
}