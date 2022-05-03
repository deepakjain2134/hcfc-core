using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class EPFrequency
    {
        [DataMember]
        public int EpFrequencyId { get; set; }

        [DataMember]
        public int EpDetailId { get; set; }                    

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int? UdFrequencyId
        {
            get => FrequencyId;
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        public FrequencyMaster UdFrequency
        {
            get => Frequency;
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        public int FrequencyId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public FrequencyMaster Frequency { get; set; }
                
        public int? RecFrequencyId { get; set; }
        
        public FrequencyMaster RecFrequency { get; set; }

        [DataMember]
        public int TjcFrequencyId { get; set; }

        [DataMember]
        public FrequencyMaster TjcFrequency { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}