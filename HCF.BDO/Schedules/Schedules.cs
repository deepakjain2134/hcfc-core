using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class Schedules
    {
        [DataMember] [Key]
        public int ScheduleId { get; set; }

        [DataMember]
        public string ReferenceName { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public UserProfile CreatedByUser { get; set; }

        [DataMember]
        public List<ScheduleFrequency> ScheduleFrequency { get; set; }

        public IEnumerable<ScheduleEPAssets> ScheduleEPAssets { get; set; }

        public bool IsCustomDate { get; set; }

        public IEnumerable<StandardEps> StandardEP { get; set; }

        public IEnumerable<Assets> Assets { get; set; }

       

    }

    [DataContract]
    public class ScheduleFrequency
    {
        [DataMember][Key]
        public int ScheduleFreqId { get; set; }

        [DataMember]
        public int ScheduleId { get; set; }

        [DataMember]
        public int FrequencyId { get; set; }

        [DataMember]
        public int GraceDays { get; set; }

        [DataMember]
        public bool IsCustomDate { get; set; }

        [DataMember]
        public int? YearNo { get; set; }

        [DataMember]
        public int? MonthNo { get; set; }

        [DataMember]
        public int? WeekNo { get; set; }

        [DataMember]
        public int? WeekDays { get; set; }

        [DataMember]
        public TimeSpan StartTime { get; set; }

        [DataMember]
        public TimeSpan EndTime { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public FrequencyMaster Frequency { get; set; }

        public DateTime StartScheduleDate { get; set; }

        public DateTime LastScheduleDate { get; set; }

        public DateTime? LastActivityInspectionDate { get; set; }

        //public DateTime CurrentScheduleDate
        //{
        //    get
        //    {
        //        var CurrentScheduleDate = new DateTime();
        //        try
        //        {
        //            var weekday = WeekDays == null ? 1 : WeekDays.Value;
        //            var weekno = WeekNo == null ? 0 : WeekNo.Value;
        //            var monthno = MonthNo == null ? 0 : MonthNo.Value;
        //            var ScheduleStartDate = LastActivityInspectionDate == null ? StartScheduleDate : LastActivityInspectionDate.Value;
        //            CurrentScheduleDate = Utility.Common.GetNextInspScheduleDateNew(ScheduleStartDate, (Frequency.Type,Frequency.Value) ,ScheduleStartDate.Day, weekday, weekno, monthno, IsCustomDate, LastActivityInspectionDate != null);
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        return CurrentScheduleDate;
        //    }
        //}


        public DateTime StartDate { get; set; }

        public DateTime CurrentScheduleDate => GetNextDate(0);


        public DateTime NextScheduleDate
        {
            get => GetNextDate(1);
            set { }
        }

        private DateTime GetNextDate(int sequence)
        {
            return DateTime.Today;
            //if (IsCustomDate)
            //{
            //    var weekday = WeekDays ?? 1;
            //    var weekno = WeekNo ?? 1;
            //    var monthno = MonthNo ?? 0;
            //    return Utility.ScheduleDateTime.GetScheduleCustomDate(FrequencyId, StartDate, monthno, weekno, weekday, sequence);
            //}
            //else
            //    return .ScheduleDateTime.GetScheduleFixedDate(FrequencyId, StartDate, sequence);
        }
    }

    [DataContract]
    public class ScheduleEPAssets
    {
        [DataMember][Key]
        public int ScheduleItemId { get; set; }

        [DataMember]
        public int ScheduleId { get; set; }

        [DataMember]
        public int ActiviityType { get; set; }

        [DataMember]
        public int? EPDetailId { get; set; }

        [DataMember]
        public int? FloorAssetId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}