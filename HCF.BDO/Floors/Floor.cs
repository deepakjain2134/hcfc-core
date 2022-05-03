using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class StopMaster
    {
        [DataMember]
        [Key]
        public int StopId { get; set; }

        [DataMember]
        [DisplayName("Stop Name")]
        public string StopName { get; set; }

        [DataMember]
        [DisplayName("Stop Code")]
        public string StopCode { get; set; }

        [DataMember]
        public string StopBarCode { get; set; }


        [DataMember]
        public int? FloorId { get; set; }

        [DataMember]
        public int? BuildingId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsInRoute { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public Floor Floor { get; set; }

        [DataMember]
        public List<TFloorAssets> TfloorAssets { get; set; }

        [DataMember]
        public List<RouteMaster> Routes { get; set; }

        public StopMaster()
        {
            TfloorAssets = new List<TFloorAssets>();
            Routes = new List<RouteMaster>();
        }
    }


    [DataContract]
    public class Floor
    {
        [DataMember]
        public int FloorId { get; set; }

        [DataMember]
        public Guid FloorPlanId { get; set; }

        [DataMember]
        [DisplayName("Floor Code")]
        //[Required]
        public string FloorCode { get; set; }

        [DataMember]
        //[Required]
        [DisplayName("Floor Name")]
        public string FloorName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        //[Required]
        public int BuildingId { get; set; }

        public string BuildingCode { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public string Version { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        [DisplayName("Floor Plan")]
        [DataMember]
        public string ImagePath { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FileName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FileContent { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        [DisplayName("Floor #")]
        //[Required]
        public string Alias { get; set; }

        [DataMember]
        public string AliasSequence { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public string ThumbImage { get; set; }

        [DataMember(EmitDefaultValue = false)]
        //[DisplayName("Review Date")]
        public DateTime? EffectiveDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public double EffectiveDateTimeSpan { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Assets> Assets { get; set; }

        [NotMapped]
        [DataMember(EmitDefaultValue = false)]
        public List<Wing> Wings { get; set; }

        [DataMember]
        public List<FloorPlan> FloorPlans { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Wing Wing { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Buildings Building { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile User { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<FloorShapes> FloorShapes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TFloorAssets> TFloorAssets { get; set; }

        [DataMember]
        public int FloorTypeId { get; set; }

        [NotMapped]
        [DataMember]
        [DisplayName("Floor Type")]
        public FloorTypes FloorType { get; set; }

        [DataMember]
        public List<FloorTypes> FloorPlanTypes { get; set; }

        [DataMember]
        public int? Sequence { get; set; }

        public List<RouteMaster> Routes { get; set; }

        public string OldFloorCode { get; set; }

        public string FloorLocation
        {
            get => SetLoction();
            set { }
        }

        public string ShortFloorLocation
        {
            get => SetShortLoction();
            set { }
        }

        private string SetLoction()
        {
            string location = string.Empty;
            if (BuildingId > 0 && Building != null && !string.IsNullOrEmpty(Alias))
                location = $"{Alias}, {Building.BuildingCode}";
            else
                location = (!string.IsNullOrEmpty(FloorName) && !string.IsNullOrEmpty(Building.BuildingName)) ? ($"{FloorName}, {Building.BuildingName}") : "NA";
            return location;
        }

        private string SetShortLoction()
        {
            var location = string.Empty;
            if (BuildingId > 0 && Building != null)
                location = $"{FloorCode}, {Building.BuildingCode}";
            return location;
        }
        public string FloorBuildingLocation
        {
            get => SetFloorBuildingLocation();
            set { }
        }

        private string SetFloorBuildingLocation()
        {
            string location = string.Empty;
            if (BuildingId > 0 && Building != null && !string.IsNullOrEmpty(Alias))
                location = $"{FloorName}, {Building.BuildingName}";
            else
                location = (!string.IsNullOrEmpty(FloorName) && !string.IsNullOrEmpty(Building.BuildingName)) ? ($"{FloorName}, {Building.BuildingName}") : "NA";
            return location;
        }

        public Floor()
        {
            FloorPlans = new List<FloorPlan>();
            Building = new Buildings();
            Wings = new List<Wing>();
           
        }
    }

}