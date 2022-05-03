using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BDO
{
    [DataContract]
    public class UserLoginCodes
    {
        [DataMember] 
        public int LoginCodeId { get; set; }

        [DataMember] 
        public Guid OrgKey { get; set; }

        [DataMember] 
        public string Code { get; set; }

        [DataMember] 
        public bool IsUsed { get; set; }

        [DataMember] 
        public int CreatedBy { get; set; }

        [DataMember] 
        public DateTime CreatedDate { get; set; }

    }
}
