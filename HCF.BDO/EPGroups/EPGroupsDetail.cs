using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class EPGroupsDetail
    {
        [DataMember]
        public int EPGroupDetailId { get; set; }

        [DataMember]
        public int EPGroupId { get; set; }

        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}