using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class SOCMatrix
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int SOCId { get; set; }

        [DataMember]
        public int ScoreTypeId { get; set; }

        [DataMember]
        public int Value { get; set; }

    }
}