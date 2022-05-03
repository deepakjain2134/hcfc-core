using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class Roles
    {
        [Key]
        [DataMember]
        public int RoleId { get; set; }

        [DataMember]
        public string RoleName { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string DisplayText { get; set; }

        [DataMember]
        public List<RolesInGroups> RolesInGroups { get; set; }

        [DataMember]
        public bool IsUserRole { get; set; }

        [DataMember]
        public int IsAdditionalRole { get; set; }

        [DataMember]
        public int? ParentId { get; set; }

        [DataMember]
        public bool IsChild { get; set; }

        [DataMember]
        public bool Status { get; set; }

    }    
}