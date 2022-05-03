using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.ComponentModel;


namespace HCF.BDO
{
    [DataContract]
    public sealed class News : BaseEntity,IValidatableObject
    {
        [DataMember][Key]
        public int Id { get; set; }

        [DataMember]     
        public string Title { get; set; }

        [DataMember]   
        public string Short { get; set; }

        [DataMember]
        
        public string Description { get; set; }

        [DataMember]       
        public bool Published { get; set; }

        [DataMember]
        public bool IsReleaseNotes { get; set; }

        [DataMember(EmitDefaultValue = false)]      
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DataMember(EmitDefaultValue = false)]   
        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedOn { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile UserProfile { get; set; }

        [DataMember]
        public bool IsExpired => CheckIsIsExpired(StartDate, EndDate);

        private bool CheckIsIsExpired(DateTime? startDate, DateTime? endDate)
        {            
            var dateToCheck = DateTime.UtcNow;
            var date = dateToCheck.Date;
            return !Convert.ToBoolean(date >= startDate && date <= endDate);
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return new ValidationResult("");
            }
        }
    }

}