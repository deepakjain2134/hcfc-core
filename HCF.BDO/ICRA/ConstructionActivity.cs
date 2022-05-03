using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class ConstructionActivity
    {
        [DataMember(EmitDefaultValue = false)][Key]
        public int ConstActivityId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? ConstructionTypeId { get; set; }

        [DisplayName("Construction Activity")]
        [DataMember(EmitDefaultValue = false)]
        public string Activity { get; set; }

        [DisplayName("Status")]
        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? CreatedDate { get; set; }

    }
}