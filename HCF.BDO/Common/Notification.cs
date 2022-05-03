using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCF.BDO
{
    [DataContract]
    public class NotificationCategory
    {
        [DataMember]
        [Key]
        public int NotificationCatId { get; set; }

        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public string CategoryDescription { get; set; }

        [DataMember]
        public string EnumValue { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<NotificationMapping> NotificationMappings { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [ForeignKey("Id")]
        public NotificationEmails NotifyEmails { get; set; }
       
        public NotificationCategory()
        {
            NotificationMappings = new List<NotificationMapping>();
        }

    }
    
    [DataContract]
    public class NotificationEvent
    {
        [DataMember]
        public int NotificationEventId { get; set; }

        [DataMember]
        public string EventName { get; set; }

        [DataMember]
        public string EventDescription { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

    }

    [DataContract]
    public class NotificationMapping
    {
        [DataMember]
        public int NotificationMappingId { get; set; }

        [DataMember]
        public int? NotificationCatId { get; set; }

        [DataMember]
        public int? NotificationEventId { get; set; }

        public bool BoolStatus
        {
            get => Status == 1;
            set => Status = value ? 1 : 0;
        }

        [DataMember]
        public int Status { get; set; }        

        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        [NotMapped]
        public NotificationCategory NotificationCategory { get; set; }

        [DataMember]
        [NotMapped]
        public NotificationEvent NotificationEvent { get; set; }

        [NotMapped]
        public NotificationEmails NotificationEmails { get; set; }
    }
}