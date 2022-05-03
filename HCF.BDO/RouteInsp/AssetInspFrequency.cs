using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HCF.BDO
{
    [DataContract]
    public class AssetInspFrequency
    {
        [DataMember]
        [Key]
        public int Id { get; set; }

        [DataMember]
        public int AssetId { get; set; }

        [DataMember]
        public int FrequencyId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public Assets Asset { get; set; }

        [DataMember]
        public FrequencyMaster InspType { get; set; }

    }
}