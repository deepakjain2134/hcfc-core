using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Request
{
    public class SaveFloorAssetRequestModel
    {
        //[DataMember]

        //public int FloorAssetsId { get; set; }        

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string AssetNo { get; set; }

        [DataMember]
        public string SubTypeCode { get; set; }

        [DataMember]
        public string StatusCode { get; set; }


        [DataMember]
        public string SerialNo { get; set; }

        [DataMember]
        public string NearBy { get; set; }

        [DataMember]
        public string BuildingCode { get; set; }

        [DataMember]
        public string FloorCode { get; set; }


        [DataMember]
        public string CMMSReference { get; set; }

    }

    [DataContract]
    public class UpdateFloorAssetRequestModel
    {

        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        public string NearBy { get; set; }

        [DataMember]
        public string StatusCode { get; set; }

        [DataMember]
        public string SubStatusCode { get; set; }


    }
}
