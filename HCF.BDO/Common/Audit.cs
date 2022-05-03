using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class Audit
    {
        [DataMember]
        [Key]
        public Guid AuditId { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string TableName { get; set; }

        [DataMember]
        public string PK { get; set; }

        [DataMember]
        public string FieldName { get; set; }

        [DataMember]
        public string OldValue { get; set; }

        [DataMember]
        public string NewValue { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime? UpdateDate { get; set; }

        [DataMember]
        public string UserName { get; set; }

    }
}