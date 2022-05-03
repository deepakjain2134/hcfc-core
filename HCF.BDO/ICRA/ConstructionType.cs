using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace HCF.BDO
{
    [DataContract]
    public class ConstructionType
    {
        [DataMember(EmitDefaultValue = false)][Key]
        public int ConstructionTypeId { get; set; }

        [DisplayName("Construction Type")]
        [DataMember(EmitDefaultValue = false)]
        public string TypeName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<ConstructionActivity> ConstructionActivity { get; set; }

    }

}