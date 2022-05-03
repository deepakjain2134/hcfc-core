using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class TRoundsQuestionnaires
    {
        [DataMember][Key]
        public Guid RoundInspId { get; set; }

        [DataMember]
        public int TRQuesId { get; set; }

        [DataMember]
        public int TRoundId { get; set; }

        [DataMember]
        public int RoundId { get; set; }

        [DataMember]
        public int RouQuesId { get; set; }

        [DataMember]
        public int? IssueId { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public Enums.RiskScore RiskType { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public RoundsQuestionnaires RoundsQuestionnaires { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileContent { get; set; }
    }
}