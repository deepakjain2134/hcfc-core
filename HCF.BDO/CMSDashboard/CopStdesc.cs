using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class CopStdesc
    {
        [DataMember]
        public int CopStdescId { get; set; }

        [DataMember]
        public string CopName { get; set; }

        [DataMember]
        public string CopTitle { get; set; }

        [DataMember]
        public string TagCode { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

    }
}