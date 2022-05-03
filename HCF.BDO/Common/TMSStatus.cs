using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class WoStatus
    {
        [DataMember][Key]
        public int WoStatusId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string CRxCode { get; set; }

        [DataMember]
        public string ModuleCode { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public List<SubStatus> SubStatus { get; set; }

        [DataMember] public bool IsDefaultSelected { get; set; }


    }
}