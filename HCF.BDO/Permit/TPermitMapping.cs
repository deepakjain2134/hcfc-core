using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HCF.BDO
{
    [DataContract]
    public class TPermitMapping
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int PermitType1 { get; set; }

        [DataMember]
        public int PermitType2 { get; set; }

        [DataMember]
        public string PermitId1 { get; set; }

        [DataMember]
        public string PermitId2 { get; set; }

        [DataMember]
        public string PermitNumber { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

    }
}