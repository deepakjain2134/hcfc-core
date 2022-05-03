using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class EpBinder
    {
        [DataMember]
        public int EPBinderId { get; set; }

        [DataMember]
        public int? EPDetailId { get; set; }

        [DataMember]
        public int? BinderId { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public Binders Binder { get; set; }

        [DataMember]
        public EPDetails EpDetails { get; set; }

    }
}