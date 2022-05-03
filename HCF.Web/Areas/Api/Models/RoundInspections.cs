using HCF.BDO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models
{

    public class RoundInspections
    {
        [DataMember]
        [Key] public string RoundName { get; set; }
        [DataMember]
        public string AdditionalUserIds { get; set; }
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string RoundScheduleId { get; set; }
        [DataMember]
        public int RoundGroupId { get; set; }
        [DataMember]
        public int RoundType { get; set; }
        [DataMember]
        public DateTime RoundDate { get; set; }

        [DataMember]
        public int FloorId { get; set; }

        [DataMember]
        public int Status { get; set; }
        [DataMember]

        public List<TRoundInspections> Inspections { get; set; }
        public RoundInspections()
        {

            Inspections = new List<TRoundInspections>();
        }
    }
}