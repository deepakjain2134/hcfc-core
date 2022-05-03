using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace HCF.BDO
{
    [DataContract]
    public class BuildingDetails
    {
        [DataMember]
        [Key]
        public int BuildingDetailId { get; set; }
        [DataMember]
        public int BuildingId { get; set; }
        [DataMember]
        public string PercentageSqrFt { get; set; }
        [DataMember]
        public string PercentageRenovated { get; set; }
        [DataMember]
        public string SquareFtRange { get; set; }
        [DataMember]
        public bool Sprinkled { get; set; }
        [DataMember]
        public string Beds { get; set; }
        [DataMember]
        public bool GovEnvSusp { get; set; }
        [DataMember]
        public string OpenSPFI { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }


    }
}