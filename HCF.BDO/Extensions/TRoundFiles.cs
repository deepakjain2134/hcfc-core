using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class TRoundFiles
    {
        [DataMember]
        public int TRoundDocId { get; set; }


        [DataMember]
        public int? TRoundId { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string Extension { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public UserProfile UserProfile { get; set; }
    }
}