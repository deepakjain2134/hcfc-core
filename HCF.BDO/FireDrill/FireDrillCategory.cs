using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class FireDrillCategory
    {
        [DataMember]
        [Key]
        public int FiredrillCatId { get; set; }

        [Required]
        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public List<FireDrillQuestionnaires> FireDrillQuestionnaires { get; set; }

        [DataMember]
        public bool IsCommonCat { get; set; }

        [DataMember]
        public string Applicable { get; set; }

    }

}