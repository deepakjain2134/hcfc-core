using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class TSchedule
    {
        [DataMember][Key]
        public int TScheduleId { get; set; }

        [DataMember]
        public int ScheduleId { get; set; }

        [DataMember]
        public int ScheduleFreqId { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public Schedules Schedule { get; set; }

        public List<TScheduleInspection> ScheduleInspection { get; set; }
        
    }

    [DataContract]
    public class TScheduleInspection
    {
        [DataMember]
        public int TScheduleInspId { get; set; }

        [DataMember]
        public int TScheduleId { get; set; }

        [DataMember]
        public int? ActivittyId { get; set; }

        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public int FloorAssetsId { get; set; }

        [DataMember]
        public bool IsCompleted { get; set; }

        [DataMember]
        public DateTime? InspectionDate { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public TInspectionActivity InspectionActivity { get; set; }


    }

}