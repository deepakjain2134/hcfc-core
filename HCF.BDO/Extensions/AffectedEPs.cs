using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class AffectedEPs
    {
        [DataMember]
        [Key]
        public int Id { get; set; }

        //[DataMember]
        //public int? EPDetailId { get; set; }

        [DataMember]
        public int StepsId { get; set; }

        [DataMember]
        public int? AffectedEPDetailId { get; set; }

        //[DataMember]
        //public bool IsDeleted { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

    }
}