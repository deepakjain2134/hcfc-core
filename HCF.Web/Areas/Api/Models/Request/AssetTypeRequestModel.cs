using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HCF.Web.Areas.Api.Models.Request
{
    public class AssetTypeRequestModel
    {
       
        [DataMember]
        [Display(Name = "Asset Type")]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "Asset Type Code")]
        public string Code { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        //public int CreatedBy { get; set; }

      
    }
}
