using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Response
{
    public class FloorAssetViewModel
    {

        [DataMember]

        public int AssetsId { get; set; }

        [DataMember]
        public int? FloorId { get; set; }

        [DataMember]
        public int TypeId { get; set; }

        [DataMember]
        public int SubTypeId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string AssetNo { get; set; }

        [DataMember]
        public string StatusCode { get; set; }

        [DataMember]
        public string SubStatusCode { get; set; }

        [DataMember]
        public string SerialNo { get; set; }

        [DataMember]
        public string NearBy { get; set; }

        //[DataMember]
        //public int? ManufactureId { get; set; }

        [DataMember]
        public string CMMSReference { get; set; }

        [DataMember]
        public TypesViewModel Type { get; set; }
        [DataMember]
        public SubType SubType { get; set; }

    }
    [DataContract]
    public class SubType
    {

        [DataMember]
        [Key]
        public int Id { get; set; }
        [DataMember]
        [Display(Name = "Asset")]
        public string Name { get; set; }

    }

    [DataContract]
    public class TypesViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

    }
}
