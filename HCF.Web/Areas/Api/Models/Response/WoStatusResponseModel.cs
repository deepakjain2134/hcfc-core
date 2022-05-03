using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Response
{
    [DataContract]
    public class StatusResponseModel
    {
        [DataMember]        
        public int StatusId { get; set; }

        [DataMember]       
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }

        //[DataMember]
        //public string ModuleCode { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}
