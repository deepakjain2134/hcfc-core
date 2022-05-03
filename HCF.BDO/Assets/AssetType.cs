using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class AssetType
    {
        [DataMember]
        [Key]
        public int TypeId { get; set; }

        [DataMember]
        [Display(Name = "Asset Type")]
        public string Name { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreatedBy { get; set; }

        [DataMember]
        [Display(Name = "Asset Type Code")]
        public string AssetTypeCode { get; set; }
        //[DataMember]
        //public string AssetTypeCode1 { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile UserProfile { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Assets> Assets { get; set; }

        [DataMember]
        public bool IsInternal { get; set; }

        public AssetType()
        {
            Assets = new List<Assets>();

        }
    }
}