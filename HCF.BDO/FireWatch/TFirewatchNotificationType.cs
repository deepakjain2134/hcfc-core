using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class TFirewatchNotificationType
    {
        [DataMember][Key] public int ID { get; set; }

        public int FirewatchNotificationTypeId { get; set; }

       // [DataMember] public int? FirewatchID { get; set; }

        [DataMember] public int? ScheduledLogID { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string FirewatchNotificationName { get; set; } 
        [DataMember] public bool? IsActive { get; set; }
        [DataMember]
        [Display(Name = "Initial Notification Date")]
        public DateTime? InitNotificationDate { get; set; }

        [DataMember]
        [Display(Name = "End Notification Date")]
        public DateTime? EndNotificationDate { get; set; }

        [DataMember]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [Display(Name ="Initial Notification Time")]
        public string InitNotificationTime { get; set; }

        [DataMember]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Notification Time")]
        public string EndNotificationTime { get; set; }


        public TimeSpan? InitTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }      

    }
}