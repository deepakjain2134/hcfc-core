using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Response
{
    [DataContract]
    public class UserResponseModel
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        //[DataMember]
        //public string UserName { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        //[DataMember]
        //public string Name
        //{
        //    get => (FirstName ?? "") + " " + (LastName ?? "");
        //    set { }
        //}

        [DataMember]
        public string ResourceNumber { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}
