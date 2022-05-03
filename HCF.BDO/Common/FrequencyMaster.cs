using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class FrequencyMaster
    {       
        [DataMember(EmitDefaultValue = false)][Key]
        public int FrequencyId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string DisplayName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int Days { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int Version { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Type { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int Value { get; set; }

        [DataMember]
        public bool IsActive { get; set; }       

        [DataMember(EmitDefaultValue = false)]
        public int GracePeriod { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreatedBy { get; set; }
        public int AssetId { get; set; }
    }

}