using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace HCF.BDO
{
    [DataContract]
    public class UserGroup
    {
        [DataMember][Key]
        public int GroupId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember]
        [Display(Name = "Is Read Only")]
        public bool IsReadOnly { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Roles> Permissions { get; set; }

        public UserGroup()
        {
            Permissions = new HashSet<Roles>();
        }
    }
}