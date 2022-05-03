using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class ICRASteps
    {
        [DataMember(EmitDefaultValue = false)][Key]
        public int ICRAStepId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Alias { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Steps { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? ParentICRAStepId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? Sequence { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? CreatedDate { get; set; }

    }

}