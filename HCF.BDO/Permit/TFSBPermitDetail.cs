using System;
using System.Runtime.Serialization;


namespace HCF.BDO
{
    [DataContract]
    public class TFSBPermitDetail
    {
        [DataMember]
        public int TFSBPermitDetailId { get; set; }

        [DataMember]
        public int? TFSBPermitId { get; set; }

        [DataMember]
        public int? FSBPFormId { get; set; }

        [DataMember]
        public bool RespondStatus { get; set; }

        [DataMember]
        public string TimeinbyPass { get; set; }        

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public FSBPForms FSBPForms { get; set; }

    }
}