using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class RolesInGroups
    {
        [DataMember][Key]
        public int ID { get; set; }

        [DataMember]
        public int? RoleId { get; set; }

        [DataMember]
        public int? GroupId { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public UserGroup UserGroup { get; set; }

        [DataMember]
        public Roles Roles { get; set; }

        //[DataMember]
        //public Roles DisplayText { get; set; }
    }
}