using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using HCF.BDO.Extensions;

namespace HCF.BDO
{
    [DataContract]
    public class ScheduledLogs
    {
        [DataMember][Key]
        public int Id { get; set; }

        //[DataMember]
        //[DisplayName("Effective Date")]
        //public DateTime? EffectiveDate { get; set; }

        //[DataMember]
        [DisplayName("Effective Time")]
        public TimeSpan? EffectiveTime { get; set; }

        public TimeSpan Effective_Time { get; set; }

        [DataMember]
        public string EfTime { get; set; }

        //[DataMember]
        //[DisplayName("Continue until date")]
        //public DateTime? UntilDate { get; set; }

        //[DataMember]
        [DisplayName("Continue until time")]
        public TimeSpan? UntilTime { get; set; }

        public TimeSpan Until_Time { get; set; }

        [DataMember]
        public string UtTime { get; set; }

        [DataMember]
        public int? BuildingId { get; set; }

        [DataMember]
        public int? FloorId { get; set; }

        [DataMember]
        public bool IsClosed { get; set; }

        [DataMember]
        public string Area { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        //[DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public Buildings Buildings { get; set; }

        [DataMember]
        public UserProfile UserProfile { get; set; }

        [DataMember]
        public int? TIlsmId { get; set; }

        [DataMember]
        public int? PermitNo { get; set; }

        [DataMember]
        public int? PermitType { get; set; }

        [DataMember]
        public string InitiatedBy { get; set; }

        [DataMember]
        public string PrintInitial { get; set; }

        [DataMember]
        public int? Signature1Id { get; set; }

        [DataMember]
        public string PrintFinal { get; set; }

        [DataMember]
        public int? Signature2Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSSign1Signature { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSSign2Signature { get; set; }

        [DataMember]
        public int ScheduledStatus
        {
            get => SetScheduledStatus(StartDate, Enddate);
            set { }
        }

        private int SetScheduledStatus(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue)  
                return startDate.Value > DateTime.UtcNow ? 0 : 2;           
            else
                return -1;

            //2 in progress
            //1 complete
            //0 pending 
        }

        public string AreaComment
        {
            get => SetAreaComment(Buildings, Comment, Area);
            set { }
        }

        private string SetAreaComment(Buildings Buildings, string Comment, string Area)
        {
            var areaComment = Buildings != null ? $"{Buildings.BuildingName}_{Area}_{Comment}" : $"{Area}_{Comment}";
            return areaComment;
        }

        [DataMember(EmitDefaultValue = false)]
        public int? ClosedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile ClosedByUser { get; set; }

        [DataMember]
        public DateTime? ClosedDate { get; set; }

        [DataMember]
        public long ClosedDateSpan
        {
            get => ClosedDate == null ? 0 : Conversion.ConvertToTimeSpan(ClosedDate);
            set { }
        }

        //[DataMember]
        [DisplayName("Effective Date")]
        public DateTime? StartDate { get; set; }


        [DataMember]
        public long StartDateTimeSpan { get; set; }

        //[DataMember]
        [DisplayName("Continue Until Date")]
        public DateTime? Enddate { get; set; }

        [DataMember]
        public long EnddateTimeSpan { get; set; }

        [DataMember]
        public FirewatchNotificationType FirewatchNotification { get; set; }

        [DataMember]
        public List<TFirewatchNotificationType> TFirewatchNotificationType { get; set; }

        public ScheduledLogs()
        {
            TFirewatchNotificationType = new List<TFirewatchNotificationType>();
            DSSign1Signature = new DigitalSignature();
            DSSign2Signature = new DigitalSignature();
        }
    }
}