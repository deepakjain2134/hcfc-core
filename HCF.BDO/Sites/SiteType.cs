using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace HCF.BDO
{
    public class SiteType
    {
        [DataMember][Key]
        public int SiteTypeId { get; set; }
        [DataMember]
        public string SiteTypeName { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int? CreatedBy { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
    }
}