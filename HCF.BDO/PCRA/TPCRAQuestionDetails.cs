using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace HCF.BDO
{
    [DataContract]
    public class TPCRAQuestionDetails
    {
        [DataMember][Key]
        public int TPCRAQuesDetailId { get; set; }

        [DataMember]
        public int TPCRAQuesId { get; set; }

        [DataMember]
        public int QuesPCRAId { get; set; }

        [DataMember]
        public int? PCRAOptionId { get; set; }         

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public int? QuesStatus { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public bool OptionStatus { get; set; }

        [DataMember]
        public QuestionPCRA QuestionPCRA { get; set; }

        //[DataMember]
        //public List<TPCRA_QuestionDetails> TPCRA_QuestionDetails { get; set; }




    }

}