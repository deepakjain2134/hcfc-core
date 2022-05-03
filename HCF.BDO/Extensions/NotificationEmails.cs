using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class NotificationEmails
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int NotificationMappingId { get; set; }

        [DataMember]

        public int NotificationCatId { get; set; }

        [DataMember]
        [ForeignKey("NotificationCatId")]
        public NotificationCategory NotifyCategory { get; set; }

        [DataMember]
        public string NotificationEmail { get; set; }

        [DataMember]
        public string NotificationCCEmail { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public int? SiteId { get; set; }

        [DataMember]
        public int? BuildingId { get; set; }

        [DataMember]
        public int? FloorId { get; set; }

        [DataMember]
        public NotificationEvent NotificationEvent { get; set; }

        [DataMember]
        public int? NotificationEventId { get; set; }        

        [DataMember]
        public int? NotificationTypeId { get; set; }
       

    }   
}