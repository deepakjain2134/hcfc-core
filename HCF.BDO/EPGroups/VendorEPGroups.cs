using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class VendorEPGroups
    {
        [DataMember]
        public int VendorEPGroupsId { get; set; }

        [DataMember]
        public int EPGroupId { get; set; }

        [DataMember]
        public int VendorId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}