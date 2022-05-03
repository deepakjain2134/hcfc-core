using HCF.BDO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Response
{
    public class AssetViewModel
    {
              

        [DataMember]
        [Key]
        public int Id { get; set; }   
        [DataMember]
        [Display(Name = "Asset")]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "Asset Code")]
        public string Code { get; set; }

        [DataMember]
        [Display(Name = "TypeId")]
        public int TypeId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; } 

        
       
    }
}
