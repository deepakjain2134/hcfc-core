using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class ConstructionWorkType
    {
        [DataMember][Key]
        public int Id { get;set;}

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get;set;}

        [DataMember]
        public bool? IsActive  { get;set;}

        [DataMember]
        public int? IsChecked { get; set; }

        [DataMember]
        public int? Sequence { get;set;}
    }
}