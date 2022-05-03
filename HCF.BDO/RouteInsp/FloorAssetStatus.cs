using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HCF.BDO
{
    [DataContract]
    public class FloorAssetStatus
    {
        [DataMember]
        public int FloorId { get; set; }  

        [DataMember]
        public int TotalInspectedAssets { get; set; }

        [DataMember]
        public int TotalAssets { get; set; }
    }
}