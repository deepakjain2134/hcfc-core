using HCF.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class DeviceTypes:  EntityBase
    {
        [DataMember]
        [Key]
        public int DeviceTypeId { get; set; }

        [DataMember]
        public string Device { get; set; }
    }

}