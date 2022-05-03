using System;
using System.Runtime.Serialization;
using  System.ComponentModel.DataAnnotations;


namespace HCF.BDO
{
    [DataContract]
    public class QuestionOptionPCRA
    {
        [DataMember][Key]
        public int? PCRAOptionId { get; set; }

        [DataMember]
        public int? QuesPCRAId { get; set; }

        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public bool IsReadOnly { get; set; }

        [DataMember]
        public string Type { get; set; }       

        [DataMember]
        public bool CanComment { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool OptionStatus { get; set; }

    }

}