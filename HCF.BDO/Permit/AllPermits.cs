using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HCF.BDO
{
    [DataContract]
    public class AllPermits
    {
        [DataMember]
        public string  PermitId { get; set; }
        [DataMember]
        public string PermitNo { get; set; }
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string ProjectNo { get; set; }
        [DataMember]
        public int? PermitType { get; set; }

        [DataMember]
        public string ProjectNumber { get; set; }

        [DataMember]
        public int? RequestedBy { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public DateTime? ReportDate { get; set; }

        [DataMember]
        public int? Status { get; set; }
        [DataMember]
        public int? FormType { get; set; }
        [DataMember]
        public int TicraId { get; set; }
        [DataMember]
        public string CreatedDateDiff { get; set; }
        public int Version { get; set; }
    }
}