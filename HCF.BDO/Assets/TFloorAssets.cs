using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;


namespace HCF.BDO
{
    [DataContract]
    public class TFloorAssets
    {

        [DataMember]
        public int FETypeId { get; set; }

        [DataMember]
        public int EpDetailId { get; set; }

        [DataMember]
        public string SubStatus { get; set; }
        [DataMember]
        public FireExtinguisherTypes FireExtinguisherType { get; set; }

        [DataMember]
        [Key]
        public int FloorAssetsId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? AssetId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string SerialNo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        //[Required]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? FloorId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? BuildingID { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string AreaCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Xcoordinate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Ycoordinate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsDeleted { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ZoomLevel { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Notes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? InstallationDate { get; set; }

        public double InstallationDateTimespan { get; set; }

        [Display(Name = "Asset #")]
        [DataMember(EmitDefaultValue = false)]
        public string DeviceNo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int InspectionCount { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int EPCount { get; set; }

        [Display(Name = "Barcode #")]
        [DataMember(EmitDefaultValue = false)]
        public string BarCodeNo
        {
            get => SerialNo;
            set { }
        }

        [Display(Name = "Asset #")]
        [DataMember(EmitDefaultValue = false)]
        public string AssetNo
        {
            get => DeviceNo;
            set { }
        }

        [Display(Name = "Nearby")]
        [DataMember]
        public string NearBy { get; set; }

        [Display(Name = "TMS Reference")]
        [DataMember(EmitDefaultValue = false)]
        public string TmsReference { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        public DateTime? DateCreated { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        public DateTime? DateUpdated { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BuildingCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string StatusCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string LocationCode { get; set; }

        public string AssetCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Assets Assets { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public UserProfile UserProfile { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Wing Wing { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Floor Floor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<FloorAssetsInspection> FloorAssetsInspection { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TInspectionActivity> TInspectionActivity { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TInspectionFiles> InspectionFiles { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TFloorAssetsLocations> TFloorAssetsLocations { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<EPDetails> EPDetails { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<UserProfile> Assignee { get; set; }

        [DataMember]
        public int IsInspReady { get; set; }

        [DataMember]
        public bool OnFloorPlan { get; set; }

        [DataMember]
        public int? AscId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public AssetsSubCategory AssetSubCategory { get; set; }

        [DataMember]
        [NotMapped]
        public List<WorkOrder> WorkOrders { get; set; }


        [Display(Name = "Tag/BarCode")]
        [DataMember(EmitDefaultValue = false)]
        public string TagNo
        {
            get => SerialNo;
            set { }
        }

        [Display(Name = "Model")]
        [DataMember(EmitDefaultValue = false)]
        public string Model { get; set; }

        [DataMember]
        public int? Source { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int InspectionGroupId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public InspectionGroup InspectionGroup { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int DueWithInDays { get; set; }

        [DataMember]
        public int StopId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public StopMaster Stop { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public AssetsSubCategory DeviceType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? ManufactureId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Manufactures Make { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<RouteMaster> Routes { get; set; }

        [DataMember]
        public string Rating { get; set; }

        [DataMember]
        public DateTime? FirstInspectionDate { get; set; }

        [DataMember]
        public List<Schedules> Schedules { get; set; }

        public string GetMake()
        {
            return Make != null ? Make.ManufactureName : "NA";
        }

        public string GetLocation()
        {
            return Floor != null ? Floor.FloorLocation : "NA";
        }

        public string GetShortLocation()
        {
            return Floor != null ? Floor.ShortFloorLocation : "NA";
        }
        public string InspStatus { get; set; }

        public TFloorAssets()
        {
            EPDetails = new List<EPDetails>();
            WorkOrders = new List<WorkOrder>();
            Stop = new StopMaster();
            Routes = new List<RouteMaster>();
            FireExtinguisherType = new FireExtinguisherTypes();
            Floor = new Floor();
        }
        [DataMember]
        [Display(Name = "Wall Rating")]
        public string WallRating { get; set; }
        [DataMember]
        [Display(Name = "Door Rating")]
        public string DoorRating { get; set; }
        [DataMember]
        [Display(Name = "Frame Rating")]
        public string FrameRating { get; set; }

        [DataMember]
        public string BuildingName { get; set; }

        [DataMember]
        public string FloorName { get; set; }

        [DataMember]
        public string BuildingTypeId { get; set; }

        [DataMember]
        public string SiteCode { get; set; }

        [DataMember]
        public string SiteName { get; set; }

        [DataMember]
        public string AssetDataset { get; set; }
        [DataMember]
        public int AssetTypeId { get; set; }
        [DataMember]
        public string AssetCategoryName { get; set; }
        [DataMember]
        public string LocationName { get; set; }

        [DataMember]
        public int OpenFailSetpsCount { get; set; }

        [DataMember]
        public int OpenWorkOrdersCount { get; set; }

        [DataMember]
        public int OpenIlsmsCount { get; set; }


        [DataMember]
        public bool IsRouteInsp { get; set; }

        [DataMember]
        public int? AreaId { get; set; }

        [DataMember]
        public EPDetails EPDetail { get; set; }

        [DataMember]
        public bool IsTrackingAsset { get; set; }

        [Display(Name = "Asset Change Device Type")]
        [DataMember]
        public int? AssetChangeDeviceType { get; set; }
        [DataMember]
        public int? Path { get; set; }
    }


    [DataContract]
    public class Manufactures
    {
        [DataMember]
        [Key]
        public int ManufactureId { get; set; }

        [DataMember]
        [Display(Name = "Manufacture Name")]
        public string ManufactureName { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreatedBy { get; set; }

        [NotMapped]
        public UserProfile UserProfile { get; set; }

    }

    [DataContract]
    public class AssetsSubCategory
    {
        [DataMember]
        [Key]
        public int AscId { get; set; }

        [DataMember]
        public string AscName { get; set; }

        [DataMember]
        public int AssetId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CustomAscName(string feType)
        {
            return !string.IsNullOrEmpty(feType) ? $"{feType}{AscName}" : AscName;
        }

        [DataMember]
        public List<FireExtinguisherTypes> FireExtinguisherTypes { get; set; }

    }

}