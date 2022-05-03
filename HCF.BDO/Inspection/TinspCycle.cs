using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace HCF.BDO
{
    [DataContract]
    public class TinspCycle
    {
        [DataMember][Key] public Guid InspCycleId { get; set; }

        [DataMember] public int EpDetailId { get; set; }

        [DataMember] public DateTime StartDate { get; set; }

        [DataMember] public bool IsDeleted { get; set; }
        [DataMember] public DateTime EndDate { get; set; }

        [DataMember] public int FrequencyId { get; set; }

        [DataMember] public int? InspectionId { get; set; }

        [DataMember] public DateTime CreatedDate { get; set; }

        [DataMember] public int CreatedBy { get; set; }

    }

}