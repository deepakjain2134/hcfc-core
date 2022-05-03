using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace HCF.BDO
{
    [DataContract]
    public class InspStatus
    {
        [DataMember][Key]
        public int InspStatusId { get; set; }

        [DataMember]
        public int InspResultId { get; set; }

        [DataMember]
        public string StatusName { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; }

    }

}