using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class EpDocuments
    {
        [DataMember]
        [Key]
        public int EpDocumentId { get; set; }

        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public int DocTypeId { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        //[DataMember]
        public DateTime CreatedDate { get; set; }

        //[DataMember]
        public DateTime? DateEffective { get; set; }

        [DataMember]
        public DocumentType DocumentType { get; set; }

        [DataMember]
        public StandardEps StandardEPs { get; set; }

    }
}