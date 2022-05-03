using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class Skills
    {
        [DataMember]
        [Key]
        public int Id { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}