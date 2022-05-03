using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using  System.ComponentModel.DataAnnotations;


namespace HCF.BDO
{
    [DataContract]
    public class TDepartmentNearConstruction
    {
        [DataMember][Key]
        public int Id { get; set; }
        [DataMember]
        public int QuesPCRAId { get; set; }
        [DataMember]
        public string Above { get; set; }
        [DataMember]
        public string below { get; set; }
        [DataMember]
        public string North { get; set; }
        [DataMember]
        public string South { get; set; }
        [DataMember]
        public string East { get; set; }
        [DataMember]
        public string West { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }
}