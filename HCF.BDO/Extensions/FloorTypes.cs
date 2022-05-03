using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class FloorTypes
    {
        [DataMember] [System.ComponentModel.DataAnnotations.Key]
        public int FloorTypeId { get; set; }

        [DataMember]
        public string FloorType { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        public string IconPath { get; set; }

        [DataMember(EmitDefaultValue=false)]
        public List<Floor> Floors { get; set; }

        [DataMember]
        public List<FloorPlan> FloorPlans { get; set; }

        public FloorTypes()
        {
            FloorPlans = new List<FloorPlan>();           
        }
    }
}