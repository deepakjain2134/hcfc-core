using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models
{
    public class MenuApp
    {
        [DataMember(EmitDefaultValue = false)]
        [Key]
        public int Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int Type { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }



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
        public string PageUrl { get; set; }

        [DataMember]
        public int ParentId { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }
    }




}
