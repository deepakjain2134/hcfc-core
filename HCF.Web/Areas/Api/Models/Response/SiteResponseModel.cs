using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Response
{
    [DataContract]
    public class SiteResponseModel
    {
        [DataMember]
        public int SiteId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string SiteCode { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}
