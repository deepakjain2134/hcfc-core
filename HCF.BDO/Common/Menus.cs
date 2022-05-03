using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCF.BDO
{
    [DataContract]
    public class Menus
    {
        [DataMember(EmitDefaultValue = false)][Key]
        public int Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int Type { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool Status { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Alias { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ImagePath { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int ChildCount { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string PageUrl { get; set; }

        [DataMember]
        public int ParentId { get; set; }


        [DataMember]
        public string FilesContent { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public int Seq { get; set; }

        [DataMember]
        public List<OrgServices> usermenu { get; set; }

        [DataMember]
        [NotMapped]
        public SelectList MainMenu { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

    }

}