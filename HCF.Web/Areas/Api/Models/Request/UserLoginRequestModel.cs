using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Request
{
    [DataContract]
    public class UserLoginRequestModel
    {        

        [DataMember(EmitDefaultValue = false)]
        public string Password { get; set; }

        [DataMember]
        public string UserName { get; set; }
    }
}
