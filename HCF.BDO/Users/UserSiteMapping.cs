using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class UserSiteMapping :BaseEntity
    {
        [DataMember]
        [Key]
        public int Id { get; set; }

        [DataMember]
        public int? UserId { get; set; }

        [DataMember]
        public int? VendorId { get; set; }

        [DataMember]
        public int SiteId { get; set; }

        [DataMember]
        public int AssignedType { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public virtual Site Site { get; set; }

        public string DrawingIds { get; set; }

        [DataMember]
        public virtual UserProfile UserProfile { get; set; }


    }
}