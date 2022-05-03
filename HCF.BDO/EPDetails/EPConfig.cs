using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class EpConfig
    {
        [DataMember] [Key]public int Id { get; set; }

        [DataMember] public int ClientNo { get; set; }

        [DataMember] public int EPDetailId { get; set; }

        [DataMember] public bool? IsApplicable { get; set; }

        [DataMember] public bool? IsActiveEp { get; set; }

        [DataMember] public DateTime? InspectionDate { get; set; }

        [DataMember] public int? InspectionGroupId { get; set; }

        [DataMember] public int? CreatedBy { get; set; }

    }
}