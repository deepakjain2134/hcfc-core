using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Request
{
    [DataContract]
    public class SaveStatusRequestModel
    {        
        [Required]
        [DataMember]
        public string Name { get; set; }

        [Required]
        [DataMember]
        public string Code { get; set; }

        [Required]
        [DataMember]
        public bool IsActive { get; set; }
    }


    public class UpdateStatusRequestModel
    {
        [Required]
        [DataMember]
        public string Name { get; set; }       
       

        [Required]
        [DataMember]
        public bool IsActive { get; set; }
    }
}
