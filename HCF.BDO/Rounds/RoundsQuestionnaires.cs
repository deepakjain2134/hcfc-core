using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class RoundsQuestionnaires
    {
        [DataMember][Key]
        public int RouQuesId { get; set; }

        [DataMember]
        public bool IsOneTimeStep { get; set; }

        [DataMember]
        public int RoundCatId { get; set; }

        [DataMember]
        public string RoundStep { get; set; }

        [DataMember]
        public string RiskStepCode { get; set; }

        [DataMember]
        public Enums.RiskScore RiskType { get; set; }

        [DataMember]
        public bool IsShared { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int? ParentRouQuesId { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public bool IsCommonRoundQues { get; set; }


        [DataMember]
        public string Applicable { get; set; }

        [DataMember]
        public string ShortDescription { get; set; }

        [DataMember]
        public int? AdditionalRoundType { get; set; }

        [DataMember]
        public int IssueId { get; set; }
        [DataMember]
        public bool IsWODone { get; set; }

        [DataMember]
        public string CategoryName { get; set; }
    }
}