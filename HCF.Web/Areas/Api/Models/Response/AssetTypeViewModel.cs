using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace HCF.Web.Areas.Api.Models.Response
{
    public class AssetTypeViewModel
    {
        [DataMember]
        [Key]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "Type")]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "Asset Type Code")]
        public string Code { get; set; } 

        [DataMember]
        public bool IsActive { get; set; }
           
    }
}
