using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class Types
    {
        [DataMember][System.ComponentModel.DataAnnotations.Key]
        public int TypeId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ModuleCode { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

    }
}