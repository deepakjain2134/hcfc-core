using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace HCF.BDO
{
    [DataContract]
    public class ConstructionClass
    {
        [DataMember(EmitDefaultValue = false)][Key]
        public int ConstructionClassId { get; set; }

        [DisplayName("Construction Class")]
        [DataMember(EmitDefaultValue = false)]
        public string ClassName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<ConstructionClassActivity> ConstructionClassActivity { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int Version { get; set; }


    }

}