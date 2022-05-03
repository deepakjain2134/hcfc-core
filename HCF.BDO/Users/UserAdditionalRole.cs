using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace HCF.BDO
{
    public class UserAdditionalRole
    {
        [DataMember][Key]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? CreatedBy { get; set; }

        [DataMember]
        public bool IsReadOnly { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}