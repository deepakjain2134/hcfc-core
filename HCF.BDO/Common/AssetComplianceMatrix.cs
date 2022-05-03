using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HCF.BDO
{
    [DataContract]
    public class AssetComplianceMatrix
    {
        [DataMember]
        public int BuildingId { get; set; }

        [DataMember]
        public int AssetId { get; set; }

        [DataMember]
        public int EPdetailId { get; set; }

        [DataMember]
        public int AssetCount { get; set; }

        [DataMember]
        public int InspectedAsset { get; set; }

        [DataMember]
        public StandardEps StandardEps { get; set; }

        public AssetComplianceMatrix()
        {
            StandardEps = new StandardEps();
        }
    }

}