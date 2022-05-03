using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Request
{
    [DataContract]
    public class SaveBuildingRequestModel
    {        
        [Required]
        [DataMember]       
        public string BuildingName { get; set; }

        [Required]
        [DataMember]        
        public string BuildingCode { get; set; }

        [Required]
        [DataMember]        
        public string SiteCode { get; set; }

        [Required]
        [DataMember]
        public bool IsActive { get; set; }
    }

    public class UpdateBuildingRequestModel
    {
        [Required]
        [DataMember]       
        public string BuildingName { get; set; }

        [Required]
        [DataMember]
        public bool IsActive { get; set; }
    }
}
