using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class LocationGroups
    {
        [DataMember] public int LocationGroupId { get; set; }

        [DataMember] public string Name { get; set; }

        [DataMember] public int? Type { get; set; }

        [DataMember] public string Description { get; set; }

        [DataMember] public bool IsActive { get; set; }

        [DataMember] public int CreatedBy { get; set; }

        [DataMember] public DateTime CreatedDate { get; set; }

        [DataMember] public DateTime? UpdatedDate { get; set; }

        [DataMember] public int BuildingCount { get; set; }

        [DataMember] public List<LocationGroupDetails> LocationGroupDetails { get; set; }

    }
}
