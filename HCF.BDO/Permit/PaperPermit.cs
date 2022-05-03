using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class PaperPermit
    {
        [DataMember]
        public int PermitId { get; set; }
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
        [DataMember] public string Contractor { get; set; }
        public string TFileIds { get; set; }
        [DataMember] public int? CreatedBy { get; set; }
        [DataMember] public DateTime? CreatedDate { get; set; }

        [DataMember] public int? UpdatedBy { get; set; }

        [DataMember] public DateTime? UpdatedDate { get; set; }
        public List<TFiles> Attachments { get; set; }
        [DataMember]
        public string AttachFiles { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

    }
}