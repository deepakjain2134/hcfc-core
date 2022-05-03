using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class AssetsMapping
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int? AssetId { get; set; }

        [DataMember]
        public int? UserId { get; set; }

        [DataMember]
        public int? VendorId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

    }
}