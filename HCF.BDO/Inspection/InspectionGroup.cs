using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class InspectionGroup
    {
        [DataMember][Key]
        public int InspectionGroupId { get; set; }

        [DataMember]
        [DisplayName("Schedule")]
        public string GroupName { get; set; }

        [DataMember]
        [DisplayName("Inspection Date")]
        public DateTime? InspectionDate { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

    }

}