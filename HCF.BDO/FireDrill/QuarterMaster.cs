using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCF.BDO
{
    [DataContract]
    public class QuarterMaster
    {
        [DataMember][Key]
        public int? QuarterId { get; set; }

        [DataMember]
        public int? BuildingTypeId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? QuarterNo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string QuarterName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? TotalPlanned { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? TotalUnAnnounced { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? TotalDone { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? TotalUnAnnouncedDone { get; set; }

        [DataMember]
        public int? Year { get; set; }

        //[DataMember]
        public DateTime? StartDate { get; set; }

        //[DataMember]
        public long StartDateTimeSpan { get; set; }

        //[DataMember]
        public DateTime? EndDate { get; set; }

        //[DataMember]
        public long EndDateTimeSpan { get; set; }

        [DataMember]
        public List<Buildings> Buildings { get; set; }

        [DataMember]
        public List<TExercises> Exercises { get; set; }

        public List<FireDrillTypes> FireDrillTypes { get; set; }

        public QuarterMaster()
        {
            FireDrillTypes=new List<FireDrillTypes>();
            Buildings=new List<Buildings>();
            

        }
    }

    [DataContract]
    public class FireDrillTypes
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Sequence { get; set; }

        [DataMember]
        public string FireDrillType { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int TotalPerShift { get; set; }

        [DataMember] public bool IsAdded { get; set; }
        [NotMapped]
        [DataMember] public List<Shift> Shifts { get; set; }

    }

}