using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace HCF.BDO
{
    [DataContract]
    public class CmsEpMapping
    {
        [DataMember]
        [Key]
        public int CMSEPMappingId { get; set; }

        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public int CopDetailsId { get; set; }
        
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }
    }
}