using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class EPScheduler
    {
        [DataMember]
        [Key]
        public int SchedulerId { get; set; }

        [DataMember]
        public int EpDetailld { get; set; }

        [DataMember]
        public int? Day { get; set; }

        [DataMember]
        public int? Month { get; set; }

        [DataMember]
        public int? Year { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public DateTime InspectionDate { get; set; }

        [DataMember]
        public DateTime DueDate { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

    }

}