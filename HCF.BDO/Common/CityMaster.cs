using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Web;
using HCF.Infrastructure.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCF.BDO
{
    public class CityMaster : EntityBase
    {
        [DataMember]
        [Key]
        public int CityId { get; set; }

        [DataMember]
        public string CityName { get; set; }
       
        [DataMember]
        public string CityCode { get; set; }

        [DataMember]
        [ForeignKey(nameof(StateMaster))]
        public int StateId { get; set; }
       
        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int Zipcode { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }
        
        [DataMember]
        public DateTime? CreatedDate { get; set; }

        public virtual StateMaster StateMaster { get; set; }
    }
}