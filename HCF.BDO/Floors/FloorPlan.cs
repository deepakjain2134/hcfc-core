using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using HCF.BDO.Enums;

namespace HCF.BDO
{
    [DataContract]
    public class FloorPlan
    {
        [DataMember]
        public int FloorPlanTypeId { get; set; }

        [DataMember]
        public FloorTypes DrawingType { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember][Key]
        public Guid? FloorPlanId { get; set; }

        [DataMember]
        public int FloorId { get; set; }

        [DataMember]
        public string FloorName { get; set; }

        [DataMember]
        public string ImagePath { get; set; }

        [DataMember]
        public string FileContent { get; set; }


        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        public DateTime EffectiveDate { get; set; }

        [DataMember]
        public double EffectiveDateTimeSpan { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public long CreatedDateTimeSpan
        //{
        //    get => Utility.Conversion.ConvertToTimeSpan(CreatedDate);
        //    set { }
        //}

        [DataMember]
        public UserProfile CreatedByUser { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TFloorAssets> TfloorAssets { get; set; }

        [DataMember]
        public Floor Floor { get; set; }


        [DataMember]
        public string ThumbImagePath { get; set; }

        [DataMember]
        public int BuildingId { get; set; }

        //[DataMember]
        //public bool IsDefault { get; set; }

    }


    [DataContract]
    public class DrawingViewer
    {
        [DataMember]
        public string SourceFilePath { get; set; }
        [DataMember]
        public int FloorId { get; set; }

        [DataMember]
        public Guid FloorPlanId { get; set; }

        [DataMember]
        public int ModifiedBy { get; set; }

        [DataMember]
        public int ViewerMode { get; set; }

        [DataMember]
        public FloorPlan FloorPlan { get; set; }

        [DataMember]
        public string RedlineObject { get; set; }

        [DataMember]
        public string ImageObject { get; set; }

        [DataMember]
        public string PermitNo { get; set; }

        [DataMember]
        public string SpeceObject { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public DrawingObjectType DrawingObjectType { get; set; }
    }
}