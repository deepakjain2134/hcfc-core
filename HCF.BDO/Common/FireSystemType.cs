using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class FireSystemType
    {
        [DataMember]
        [Key]
        public int ID { get; set; }


        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public string SiteId { get; set; }

        [DataMember]
        public int? CheckVal { get; set; }

        [DataMember]
        public bool IsDelete { get; set; }
    }
}