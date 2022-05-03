using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class TInsReportDetails
    {
        [DataMember]
        public int ReportDetailId { get; set; }

        [DataMember]
        public int ReportId { get; set; }

        [DataMember]
        public Guid ActivityId { get; set; }

        [DataMember]
        public int? FloorAssetId { get; set; }

        [DataMember]
        public int InspectionId { get; set; }

        [DataMember]
        public int EpDetailId { get; set; }

        [DataMember]
        public int BinderId { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public TInspectionActivity TInspectionActivity { get; set; }
    }
}