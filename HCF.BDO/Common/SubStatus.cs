using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class SubStatus
    {
        [DataMember][Key]
        public int SubStatusId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int WoStatusId { get; set; }

        [DataMember]
        public string StatusCode { get; set; }
       
    }
}