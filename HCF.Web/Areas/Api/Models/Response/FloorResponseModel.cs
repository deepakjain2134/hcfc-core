using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Response
{
    [DataContract]
    public class FloorResponseModel
    {

        [DataMember]
        public int FloorId { get; set; }

        [DataMember]       
        public string FloorCode { get; set; }

        [DataMember]     
        public string FloorName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int BuildingId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Alias { get; set; }

    }
}
