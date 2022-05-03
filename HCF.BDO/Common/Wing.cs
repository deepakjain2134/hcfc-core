using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HCF.BDO
{
    [DataContract]   
    public class Wing
    {
        [DataMember]
        [Key]
        public int WingId { get; set; }

        //[DataMember]
        //public int PropertyId { get; set; }

        [DataMember]
        public int BuildingId { get; set; }

        [DataMember]
        public int FloorId { get; set; }

        [DataMember]
        [Required]
        [DisplayName("Wing Name")]
        public string WingName { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        [DisplayName("Status ")]
        public bool IsActive { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }       

        //[DataMember(EmitDefaultValue = false)]
        //public Buildings Building { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Floor Floor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile UserProfile { get; set; }
       
    }

}