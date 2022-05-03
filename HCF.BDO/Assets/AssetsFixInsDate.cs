using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class AssetsFixInsDate
    {
        [DataMember]
        [Key]
        public Guid AssetsFixInsId { get; set; }

        [DataMember]
        public int? InspectionGroupId { get; set; }

        [DataMember]
        public int? ActivityType { get; set; }

        [DataMember]
        public int? EpDetailId { get; set; }

        [DataMember]
        public int? FloorAssetsId { get; set; }

        [DataMember]
        public int? AssetId { get; set; }

        [DataMember]
        public int? DocTypeId { get; set; }

        [DataMember]
        public int? FrequencyId { get; set; }

        [DataMember]
        public DateTime? InspectionDate { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

    }
}