using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class FloorAssetsInspection
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public int FloorAssetId { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        //[DataMember]
        public DateTime InspectionDate { get; set; }

        [DataMember]
        public EPDetails Epdetails { get; set; }

        [DataMember]
        public Guid? ActivityId { get; set; }

        [DataMember]
        public TInspectionActivity TInspectionActivity { get; set; }

        [DataMember]
        public long InspectionDateTimeSpan { get; set; }
        //{
        //    get
        //    {
        //        return this.InspectionDate != null  ? this.InspectionDateTimeSpan : HCF.Utility.Conversion.ConvertToTimeSpan(this.InspectionDate);
        //    }
        //    set { }
        //}
    }
}