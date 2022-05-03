using HCF.BDO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace HCF.Web.Areas.Api.Models.Response
{
    [DataContract]
    public class LoginResponseModel
    {
        [DataMember]
        public int UserId { get; set; }

        //[DataMember]
        //public string UserName { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //[NotMapped]
        //public IEnumerable<UserLogin> UserLogin { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string LogOnToken { get; set; }
    }
}
