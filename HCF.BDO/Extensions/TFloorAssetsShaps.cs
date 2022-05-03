using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class TFloorAssetsShaps
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int? FloorAssetsId { get; set; }

        [DataMember]
        public int? ShapeId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        //  [DataMember]
        public DateTime? CreatedDate { get; set; }

    }
}