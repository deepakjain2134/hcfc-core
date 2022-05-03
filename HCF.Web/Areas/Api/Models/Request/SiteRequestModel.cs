using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Request
{
    [DataContract]
    public class SaveSiteRequestModel
    {
       
        [Required]
        [DataMember]
        public string Name { get; set; }

        [Required]
        [DataMember]
        public string SiteCode { get; set; }

        [Required]
        [DataMember]
        public bool IsActive { get; set; }
    }


    public class UpdateSiteRequestModel
    {
        [DataMember]
        public int SiteId { get; set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        [Required]
        [DataMember]
        public bool IsActive { get; set; }
    }
}
