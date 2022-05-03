using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class BuildingType
    {
        [DataMember]
        [Key]
        public int BuildingTypeId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Buildings> Buildings { get; set; }

    }
}