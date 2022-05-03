using HCF.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class Standard : EntityBase
    {
        [Key]
        [DataMember(EmitDefaultValue = false)]        
        public int? StDescID { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Display(Name = "Category")]
        [ForeignKey(nameof(Category))]
        public int? CategoryId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Display(Name = "TJC Standard")]
        public string TJCStandard { get; set; }

        [Display(Name = "TJC Standard")]
        public string CmsStandard { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string DisplayDescription { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Display(Name = "TJC Description")]
        public string TJCDescription { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CmsStdDescription { get; set; }

        public string NFPAStandard { get; set; }

        public bool IsActive { get; set; }

        public string Version { get; set; }

        public DateTime CreateDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        //[ForeignKey(nameof(UserProfile))]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Category Category { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public virtual UserProfile UserProfile { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<EPDetails> EPDetails { get; set; }

        [NotMapped]
        [DataMember(EmitDefaultValue = false)]
        public List<InspectionGroup> Inspectiongroup { get; set; }
    }

}