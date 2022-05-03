using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HCF.Web.Areas.Api.Models.Request
{
    public class AssetRequestModel
    {
        
        //[DataMember]
        //[Key]
        //public int AssetId { get; set; }

        [DataMember]
        public int TypeId { get; set; }

        [DataMember]
        [Display(Name = "Asset")]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        //[DataMember]
        //public string IconPath { get; set; }
        //[DataMember]
        //public int CreatedBy { get; set; }

        [DataMember]
        [Display(Name = "Asset Code")]
        public string Code { get; set; }

    }
}
