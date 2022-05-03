using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HCF.BDO
{
    public class BinderSearch
    {
        [DataMember]
        public string DocumentName { get; set; }

        [DataMember]
        public string Name { get; set; }


        [DataMember]
        public int BinderId { get; set; }
        [DataMember]
        public int type { get; set; }

        [DataMember]
        public string EP { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }


        [DataMember]
        public string BinderName { get; set; }
    }
}