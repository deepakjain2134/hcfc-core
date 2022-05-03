using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class LocationGroupDetails
    {
        [DataMember] public int LocationGroupDetailId { get; set; }

        [DataMember] public int LocationGroupId { get; set; }

        [DataMember] public int BuildingId { get; set; }

        [DataMember] public int? FloorId { get; set; }

        [DataMember] public bool IsActive { get; set; }

        [DataMember] public int CreatedBy { get; set; }

        [DataMember] public DateTime CreatedDate { get; set; }

        [DataMember] public DateTime? UpdatedDate { get; set; }

        [DataMember] public Buildings Buildings { get; set; }

    }
}
