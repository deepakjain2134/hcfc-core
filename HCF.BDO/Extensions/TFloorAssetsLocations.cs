using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class TFloorAssetsLocations
    {
        [DataMember][System.ComponentModel.DataAnnotations.Key]
        public int TFloorAssetsLocationId { get; set; }

        [DataMember]
        public int FloorAssetsId { get; set; }

        [DataMember]
        public bool IsCurrentLocation { get; set; }

        [DataMember]
        public string Xcoordinate { get; set; }

        [DataMember]
        public string Ycoordinate { get; set; }

        [DataMember]
        public string ZoomLevel { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? ShapeId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? FloorId { get; set; }

        public DateTime CreatedDate { get; set; }

        [DataMember]
        public bool OnFloorPlan { get; set; }

        [DataMember]
        public DateTime? RemovedDate { get; set; }

        [DataMember]
        public string AreaCode { get; set; }

    }
}