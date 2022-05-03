using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCF.BDO
{
    [DataContract]
    public class Shift
    {
        [DataMember]
        public List<TExercises> Exercises { get; set; }

        [DataMember(EmitDefaultValue = false)][Key]
        public int ShiftId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember]
        public TimeSpan? StartTime { get; set; }

        [DataMember]
        public TimeSpan? EndTime { get; set; }

        [DataMember]
        public string StartTimeString
        { get; set; }

        [DataMember]
        public string EndTimeString
        { get; set; }


        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<FireDrillTypes> FireDrillTypes { get; set; }

    }
}