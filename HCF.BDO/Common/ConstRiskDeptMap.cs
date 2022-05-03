using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace HCF.BDO
{
    [DataContract]
    public class ConstRiskDeptMap
    {
        [DataMember][Key] public int Id { get; set; }

        [DataMember] public int? ConstructionRiskId { get; set; }

        [DataMember] public int? ConstructionRiskAreaId { get; set; }

        [DataMember] public bool? IsActive { get; set; }

        [DataMember] public int? CreatedBy { get; set; }

        [DataMember] public DateTime? CreatedDate { get; set; }

    }

}