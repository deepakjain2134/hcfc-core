using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Request
{
    [DataContract]
    public class SaveSubstatusRequestModel
    {
        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        [Required]
        public string Code { get; set; }

        [DataMember]
        [Required]
        public bool IsActive { get; set; }

        [DataMember]
        [Required]
        public int StatusId { get; set; }
    }


    public class UpdateSubstatusRequestModel
    {
        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        [Required]
        public bool IsActive { get; set; }

    }
}
