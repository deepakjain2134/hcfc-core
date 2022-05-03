using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class FireWatchLog
    {
        [DataMember]
        [System.ComponentModel.DataAnnotations.Key]
        public int FWLogId { get; set; }

        [DataMember]
        public DateTime? LogDate { get; set; }

        [DataMember]
        public long LogDateTimeSpan { get; set; }

        [DataMember]
        public TimeSpan? LogTime { get; set; }

        [DataMember]
        public TimeSpan? StartTime { get; set; }

        [DataMember]
        public string CompleteTime { get; set; }

        [DataMember]
        public TimeSpan? FinishTime { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string Area { get; set; }

        [DataMember]
        public string InspectorName { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public UserProfile UserProfile { get; set; }

        [DataMember]
        public string LogDateText { get; set; }
        
        [DataMember]
        public DateTime? RoundInspDate { get; set; }

        [DataMember]
        public long RoundInspDateTimeSpan
        {
            get;set;
        }
    }

    [DataContract]
    public class TimeSlots
    {
        [DataMember]
        public int Sequence { get; set; }

        [DataMember]
        public DateTime Start { get; set; }

        [DataMember]
        public long StartTimeSpan { get; set; }

        [DataMember]
        public DateTime End { get; set; }

        [DataMember]
        public long EndTimeSpan { get; set; }

        [DataMember]
        public bool isCurrent { get; set; }

        [DataMember]
        public bool isNext { get; set; }

        [DataMember]
        public string DateText { get; set; }

        [DataMember]
        public string StartTimeText { get; set; }
        
        [DataMember]
        public string EndTimeText { get; set; }

        [DataMember]
        public DateTime UTCStartTime { get; set; }

        [DataMember]
        public DateTime UTCEndTime { get; set; }


        [DataMember]
        public List<FireWatchLog> FireWatchLog { get; set; }
    }
}