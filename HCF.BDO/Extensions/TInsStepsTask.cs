using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class TInsStepsTask
    {
        [DataMember][System.ComponentModel.DataAnnotations.Key]
        public int TinsStepTaskId { get; set; }

        [DataMember]
        public int TInsStepsId { get; set; }

        [DataMember]
        public int? StepsId { get; set; }

        [DataMember]
        public Guid ActivityId { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        [DisplayName("Status")]
        public string DeficiencyStatus { get; set; }

        [DataMember]
        public int? Sequence { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        [DisplayName("Updated Date")]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        [DisplayName("Code")]
        public string DeficiencyCode { get; set; }

    }
}