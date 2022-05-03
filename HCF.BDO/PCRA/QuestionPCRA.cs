using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace HCF.BDO
{
    [DataContract]
    public class QuestionPCRA
    {
        [DataMember][Key]
        public int QuesPCRAId { get; set; }

        [DataMember]
        public string Questions { get; set; }

        [DataMember]
        public bool CanComment { get; set; }

        [DataMember]
        public bool IsOption { get; set; }

        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public string Sign1date { get; set; }
        [DataMember]
        public string Sign2date { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        public TIcraProject Project { get; set; }

        public List<QuestionOptionPCRA> QuestionOptionPCRAs { get; set; }    

        public QuestionPCRA()
        {
            QuestionOptionPCRAs = new List<QuestionOptionPCRA>();
        }
    }

}