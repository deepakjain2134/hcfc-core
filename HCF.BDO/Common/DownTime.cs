using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class DownTime
    {
        [DataMember]
        public int AssetId { get; set; }

        [DataMember]
        public string AssetNumber { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int DownTimeId { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public double TotalHours { get; set; }

        [DataMember]
        public int WorkOrderId { get; set; }

        [DataMember]
        public int WorkOrderNumber { get; set; }
    }
}