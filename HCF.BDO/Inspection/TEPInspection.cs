using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class TEPInspection
    {
        [DataMember][Key]
        public int TEPInsId { get; set; }

        [DataMember]
        public Guid? EPGuid { get; set; }

        [DataMember]
        public int? EPDetailId { get; set; }

        [DataMember]
        public DateTime? InspectionDate { get; set; }

        [DataMember]
        public DateTime? NextDueDate { get; set; }

        [DataMember]
        public int? FrequencyId { get; set; }

        [DataMember]
        public bool? IsCurrent { get; set; }

        [DataMember]
        public bool? IsDone { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

    }


    [DataContract]
    public class TInspectionDates
    {
        [DataMember][Key]
        public Guid TInspectionDateId { get; set; }

        [DataMember]
        public int? InspectionGroupId { get; set; }

        [DataMember]
        public int ActivityType { get; set; }

        [DataMember]
        public int? FloorAssetsId { get; set; }

        [DataMember]
        public int? EpDetailId { get; set; }

        [DataMember]
        public int? AssetId { get; set; }

        [DataMember]
        public int? DocTypeId { get; set; }

        [DataMember]
        public int? FrequencyId { get; set; }

        [DataMember]
        public DateTime? InspectionDate { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

    }

}