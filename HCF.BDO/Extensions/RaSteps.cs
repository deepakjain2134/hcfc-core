using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class RaSteps
    {
        [DataMember]
        public int RaStepId { get; set; }

        [DataMember]
        public int StepId { get; set; }

        [DataMember]
        public int RaScore { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public bool? IsCurrent { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Steps Steps { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile UserProfile { get; set; }

    }
}