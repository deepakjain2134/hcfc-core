﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace HCF.BDO
{
    [DataContract]
    public class Category
    {
        [DataMember]
        [Key]
        public int CategoryId { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Version { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Code { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ModuleCode { get; set; }
    }

}