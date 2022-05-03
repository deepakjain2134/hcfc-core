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
    public class SaveFloorRequestModel
    {
        [Required]
        [DataMember]
        public string FloorName { get; set; }

        [Required]
        [DataMember]
        public string FloorCode { get; set; }


        [Required]
        [DataMember]
        public int BuildingId { get; set; }

        [Required]
        [DataMember]
        public bool IsActive { get; set; }

    }

    public class UpdateFloorRequestModel
    {
        [Required]
        [DataMember]
        public string FloorName { get; set; }

        [Required]
        [DataMember]
        public bool IsActive { get; set; }

    }
}
