using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class ActivityType
    {
        [Key]
        [DataMember(EmitDefaultValue = false)]
        public int ActivityTypeId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }
    }
}