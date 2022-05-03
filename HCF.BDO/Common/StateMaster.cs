using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HCF.Infrastructure.Models;

namespace HCF.BDO
{
    public class StateMaster : EntityBase
    {
        [DataMember][Key]
        public int StateId { get; set; }

        [DataMember]
        public string StateName { get; set; }

        [DataMember]
        public string StateCode { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }
       
        [DataMember]
        public DateTime? CreatedDate { get; set; }      
       
       
    }
}