using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Response
{
    [DataContract]
    public class BuildingResponseModel
    {
        [DataMember]
        public int BuildingId { get; set; }

        [DataMember]       
        public string BuildingName { get; set; }

        [DataMember]
        public string BuildingCode { get; set; }

        [DataMember]      
        public string SiteCode { get; set; }
    }
}
