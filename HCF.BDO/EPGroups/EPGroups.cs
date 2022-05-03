using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class EPGroups
    {
        [DataMember]
        public int EPGroupId { get; set; }

        [DataMember]
        public int? EPDetailId { get; set; }


        [DataMember]
        [DisplayName("EPGroup Name")]
        public string EPGroupName { get; set; }

        [DataMember]
        [DisplayName("Status")]
        public bool IsActive { get; set; }

        [DataMember]
        public List<StandardEps> StandardEps { get; set; }



    }
}