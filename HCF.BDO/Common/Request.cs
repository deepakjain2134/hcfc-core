using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public Guid? RefreshToken { get; set; }

        [DataMember]
        public int? PageIndex { get; set; }

        [DataMember]
        public int? PageSize { get; set; }

        [DataMember]
        public string SortOrder { get; set; }

        [DataMember]
        public string OrderbySort { get; set; }

        [DataMember]
        public string Mode { get; set; }

        [DataMember]
        public int? UserId { get; set; }

        [DataMember]
        public string SearchcBy { get; set; }

        [DataMember]
        public int FloorAssetId { get; set; }

        [DataMember]
        public int EpDetailId { get; set; }

        [DataMember]
        public string ActivityId { get; set; }

        [DataMember]
        public string WhereCondition { get; set; }

        [DataMember]
        public int? FilterByUserUserId { get; set; }

    }
}