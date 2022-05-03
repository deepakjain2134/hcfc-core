using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class FirewatchNotificationType
    {
        [DataMember][Key] public int ID { get; set; }

        [DataMember] public string Name { get; set; }

        [DataMember] public bool? IsActive { get; set; }

        [DataMember] public int? Sequence { get; set; }

        //[DataMember]
        //public TFirewatchNotificationType TFirewatchNotificationType { get; set; }


    }
}