using System;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    public class Activity : BaseEntity
    {
        public Activity()
        {
            Id = Guid.NewGuid();        
        }

        [Key]
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public DateTime? Timestamp { get; set; }
        public DateTime EffectiveDate { get; set; }

        public int CreatedBy { get; set; }
        public bool IsMailed { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime? NotificationDate { get; set; }
    }



    public class ActivityData 
    {        

        [Key]
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public DateTime Timestamp { get; set; }
        public string rGroupName { get; set; }

        public string roundcategories { get; set; }

        public UserProfile user { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string roundGLocation { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DataLine { get; set; }

        public int roundGroupId { get; set; }
        public bool IsMailed { get; set; }
        public DateTime? NotificationDate { get; set; }

        public int RoundCatId { get; set; }
        public string frequency { get; set; }
        public string StartDate { get; set; }

        public bool IsEnabled { get; set; }
        public string EndDate { get; set; }
        public ActivityData()
        {           
            user = new UserProfile();
        }
    }
}