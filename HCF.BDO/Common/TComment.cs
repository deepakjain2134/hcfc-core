using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HCF.BDO
{
    [DataContract]
    public class TComment
    {
        [DataMember(EmitDefaultValue = false)][System.ComponentModel.DataAnnotations.Key]
        public int TCommentId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? InspectionId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Guid? ActivityId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? TIlsmId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? StepId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Comment { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? AddedBy { get; set; }

        [DataMember]
        public UserProfile AddedByUserProfile { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? AddedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long AddedDateTimeSpan
        {
            get;set;
        }        

        [DataMember(EmitDefaultValue = false)]
        public int? CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsDeleted { get; set; }

        public TComment()
        {
            AddedByUserProfile = new UserProfile();
        }

    }

}