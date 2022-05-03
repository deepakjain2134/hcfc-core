using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class FireDrillQuestionnaires
    {
        [DataMember]
        [Key]
        public int FireDrillQuesId { get; set; }

        [DataMember]
        public int? FireDrillCatId { get; set; }

        [DataMember]
        public string Questionnaries { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public FireDrillCategory FireDrillCategory { get; set; }


        [DataMember]
        public bool IsCommQues { get; set; }

        [DataMember]
        public string Applicable { get; set; }

    }

}