using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{

    [DataContract]
    public class EpAssets
    {
        [DataMember]
        public int MappingId { get; set; }

        [DataMember]
        public int? EPDetailId { get; set; }

        [DataMember]
        public int? AssetId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        //[DataMember]
        //public int? GoalId { get; set; }

        //[DataMember]
        //public int? DoctypeId { get; set; }       

        //[DataMember]
        //public int? Type { get; set; }        

        //[DataMember]
        //public string Version { get; set; }

        [DataMember(EmitDefaultValue=false)]
        public EPDetails EPDetail { get; set; }
    }
}