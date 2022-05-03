using HCF.BDO.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class WorkOrder
    {
        [DataMember(EmitDefaultValue = false)][Key]
        public int IssueId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<RoundIssueWorkOrderCategory> RoundIssueWorkOrderCategory { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? ParentIssueId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsDeleted { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int KeyID { get; set; }

        //[Required]
        [DisplayName("Account Code")]
        [DataMember(EmitDefaultValue = false)]
        public string AccountCode { get; set; }

        [DisplayName("Account Description")]
        [DataMember(EmitDefaultValue = false)]
        public string AccountDescription { get; set; }

        [DisplayName("Action Code")]
        [DataMember(EmitDefaultValue = false)]
        public string ActionCode { get; set; }

        [DisplayName("Action Description")]
        [DataMember(EmitDefaultValue = false)]
        public string ActionDescription { get; set; }

        [DisplayName("Asset GroupNumber")]
        [DataMember(EmitDefaultValue = false)]
        public string AssetGroupNumber { get; set; }

        [DisplayName("Asset Number")]
        [DataMember(EmitDefaultValue = false)]
        public string AssetNumber { get; set; }


        [DisplayName("Building Code")]
        [DataMember(EmitDefaultValue = false)]
        public string BuildingCode { get; set; }

        [DisplayName("Building Name")]
        [DataMember(EmitDefaultValue = false)]
        public string BuildingName { get; set; }

        [DisplayName("Cause Code")]
        [DataMember(EmitDefaultValue = false)]
        public string CauseCode { get; set; }

        [DisplayName("Cause Description")]
        [DataMember(EmitDefaultValue = false)]
        public string CauseDescription { get; set; }

        [DisplayName("Completion Comments")]
        [DataMember(EmitDefaultValue = false)]
        public string CompletionComments { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        public DateTime? DateAvailable { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? DateCreated { get; set; }

        [DataMember(EmitDefaultValue = false)]
      
        public TIlsm Ilsm { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public int? TilsmId { get; set; }      


        [DataMember(EmitDefaultValue = false)]
        public long DateCreatedTimeSpan
        {
            get => DateCreated.HasValue ? Conversion.ConvertToTimeSpan(DateCreated.Value) : 0;
            set { }
        }

        //[DataMember(EmitDefaultValue = false)]
        public DateTime? DateNeeded { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? DateUpdated { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long DateUpdatedDateTimeSpan
        {
            get => DateUpdated.HasValue ? Conversion.ConvertToTimeSpan(DateUpdated.Value) : 0;
            set { }
        }

        [Required]
        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        [DisplayName("Item Code")]
        [DataMember(EmitDefaultValue = false)]
        public string ItemCode { get; set; }

        [DisplayName("Item Description")]
        [DataMember(EmitDefaultValue = false)]
        public string ItemDescription { get; set; }

        [DisplayName("Location Code")]
        [DataMember(EmitDefaultValue = false)]
        public string LocationCode { get; set; }

        [DisplayName("Location Description")]
        [DataMember(EmitDefaultValue = false)]
        public string LocationDescription { get; set; }

        [DisplayName("Meter Reading")]
        [DataMember(EmitDefaultValue = false)]
        public decimal? MeterReading { get; set; }

        //[Required]
        [DisplayName("Priority Code")]
        [DataMember(EmitDefaultValue = false)]
        public string PriorityCode { get; set; }

        [DisplayName("Priority Description")]
        [DataMember(EmitDefaultValue = false)]
        public string PriorityDescription { get; set; }

        [DisplayName("Problem Code")]
        [DataMember(EmitDefaultValue = false)]
        public string ProblemCode { get; set; }

        [DisplayName("Problem Description")]
        [DataMember(EmitDefaultValue = false)]
        public string ProblemDescription { get; set; }

        [DisplayName("Reference Number")]
        [DataMember(EmitDefaultValue = false)]
        public string ReferenceNumber { get; set; }

        [DisplayName("Requester Comments")]
        [DataMember(EmitDefaultValue = false)]
        public string RequesterComments { get; set; }

        [DisplayName("Requester Email")]
        [DataMember(EmitDefaultValue = false)]
        public string RequesterEmail { get; set; }

        [DisplayName("Requester Name")]
        [DataMember(EmitDefaultValue = false)]
        public string RequesterName { get; set; }

        [DisplayName("Requester Pager")]
        [DataMember(EmitDefaultValue = false)]
        public string RequesterPager { get; set; }

        [DisplayName("Requester Phone")]
        [DataMember(EmitDefaultValue = false)]
        public string RequesterPhone { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int SegmentID { get; set; }

        [DisplayName("Shop Code")]
        [DataMember(EmitDefaultValue = false)]
        public string ShopCode { get; set; }

        [DisplayName("Shop Description")]
        [DataMember(EmitDefaultValue = false)]
        public string ShopDescription { get; set; }

        //[Required]
        [DisplayName("Site Code")]
        [DataMember(EmitDefaultValue = false)]
        public string SiteCode { get; set; }

        [DisplayName("Site Name")]
        [DataMember(EmitDefaultValue = false)]
        public string SiteName { get; set; }

        //[Required]
        [DisplayName("Skill Code")]
        [DataMember(EmitDefaultValue = false)]
        public string SkillCode { get; set; }

        [DisplayName("Skill Description")]
        [DataMember(EmitDefaultValue = false)]
        public string SkillDescription { get; set; }

        [Required]
        [DisplayName("Status")]
        [DataMember(EmitDefaultValue = false)]
        public string StatusCode { get; set; }

        [DisplayName("Status Description")]
        [DataMember(EmitDefaultValue = false)]
        public string StatusDescription { get; set; }

        //[Required]
        [DisplayName("SubStatus Code")]
        [DataMember(EmitDefaultValue = false)]
        public string SubStatusCode { get; set; }

        [DisplayName("SubStatus Description")]
        [DataMember(EmitDefaultValue = false)]
        public string SubStatusDescription { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal? TotalCost { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal? TotalHours { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal? TotalMaterialCharges { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal? TotalTimeCharges { get; set; }

        //[Required]
        [DisplayName("Type Code")]
        [DataMember(EmitDefaultValue = false)]
        public string TypeCode { get; set; }

        [DisplayName("Type Description")]
        [DataMember(EmitDefaultValue = false)]
        public string TypeDescription { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        public DateTime? WeekScheduled { get; set; }

        [DataMember]
        public int? WorkOrderId { get; set; }

        [DataMember]
        [DisplayName("WorkOrder #")]
        public string WorkOrderNumber { get; set; }

        [DataMember]
        public string TempWorkOrderNumber { get; set; }

        [DataMember]
        [DisplayName]
        public string SourceWoId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Xcoordinate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Ycoordinate { get; set; }

        [DisplayName("Deficiency Code")]
        [DataMember(EmitDefaultValue = false)]
        public string DeficiencyCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Guid? ActivityId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? EpDetailId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public EPDetails EPDetails { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public UserProfile UserProfile { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TInspectionActivity TInspectionActivity { get; set; }

        [DataMember]
        public List<WorkOrderFloorAssets> WorkOrderFloorAssets { get; set; }

        public DateTime? CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public double? CloseDateTimeSpan { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long CreatedDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(CreatedDate);
            set { }
        }

       

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<UserProfile> Resources { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TFloorAssets> FloorAssets { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<WorkOrderfiles> WorkOrderFiles { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ZoomLevel { get; set; }

        [DataMember]
        public int TimetoResolve { get; set; }

        [DataMember]
        public bool IsDeficiency { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? FloorId { get; set; }

        //[DataMember]
        public DateTime? DeficiencyTime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public double? DeficiencyTimeSpan { get; set; }

        [DataMember]
        [NotMapped]
        public List<InspectionSteps> InspectionSteps { get; set; }

        [DataMember]
        public Floor Floor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public double? RemainingMin
        {
            get => CalcRemainingMin();
            set { }

        }

        [DataMember]
        public string AttachFiles { get; set; }
        private double? CalcRemainingMin()
        {
            if (DeficiencyTime.HasValue)
            {
                TimeSpan time = DeficiencyTime.Value.Subtract(DateTime.UtcNow);
                return time.TotalMinutes;
            }
            else
            {
                return 0;
            }
        }

        [DataMember]
        public DateTime? DateCompleted { get; set; }

        [DataMember]
        public long DateCompletedDateTimeSpan { get; set; }
        //public long DateCompletedDateTimeSpan
        //{
        //    get
        //    {
        //        return DateCompletedDateTimeSpan > 0 ? DateCompletedDateTimeSpan : DateCompleted.HasValue ? Utility.Conversion.ConvertToTimeSpan(DateCompleted.Value) : 0;
        //    }
        //    set { }
        //}

        [DataMember]
        public Guid? FloorPlanId { get; set; }

        [DataMember]
        public bool IsIlsm { get; set; }

        [DataMember]
        public string Data { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? UpdateDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long UpdateDateTimeSpan
        {
            get => UpdateDate.HasValue ? Conversion.ConvertToTimeSpan(UpdateDate.Value) : 0;
            set { }
        }


        //[DataMember(EmitDefaultValue = false)]
        //public DateTime? DateNeeded { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long DateNeededTimeSpan
        {
            get => DateNeeded.HasValue ? Conversion.ConvertToTimeSpan(DateNeeded.Value) : 0;
            set { }
        }

        //[DataMember(EmitDefaultValue = false)]
        //public DateTime? DateAvailable { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long DateAvailableTimeSpan
        {
            get => DateAvailable.HasValue ? Conversion.ConvertToTimeSpan(DateAvailable.Value) : 0;
            set { }
        }

        //[DataMember(EmitDefaultValue = false)]
        //public DateTime? WeekScheduled { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long WeekScheduledTimeSpan
        {
            get => WeekScheduled.HasValue ? Conversion.ConvertToTimeSpan(WeekScheduled.Value) : 0;
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        public int? UpdatedBy { get; set; }

        //[DataMember]
        //public string CreateDate { get; set; }


        [DataMember]
        public int ActivityType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string TemplateName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Mode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Group { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CategoryId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CategoryName { get; set; }

        [DataMember]
        public string CRxCode { get; set; }

        [DataMember]
        public List<TRoundWorkOrders> TRoundWorkOrders { get; set; }

        [DataMember]
        [DisplayName("Target Date (Due By)")]
        public DateTime? TargetDate { get; set; }

        [DataMember]
        public long TargetDateTimeSpan { get; set; }

        public Enums.Frequency Frequency { get; set; }

        public WorkOrder()
        {
            InspectionSteps = new List<InspectionSteps>();
            Resources = new List<UserProfile>();
            FloorAssets = new List<TFloorAssets>();
            TRoundWorkOrders = new List<TRoundWorkOrders>();
            TaggedResource = new List<TaggedResource>();
        }

        [DataMember]
        public List<TaggedResource> TaggedResource { get; set; }

        public string status { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FileContent { get; set; }

        [DataMember]
        public string FileName { get; set; }

        public string ActivitiesIds { get; set; }
        [DataMember]
        public int TRoundId { get; set; }

        public RoundCategory RoundCategory { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsTimeToResolve
        {
            get => SetIsTimeToResolve();
            set { }
        }
        private bool SetIsTimeToResolve()
        {          
            if ((!string.IsNullOrEmpty(DeficiencyCode) && !string.IsNullOrEmpty(StatusCode)) &&
                (DeficiencyCode.ToLower() == Enums.WODeficiencyCode.FD.ToString().ToLower()
                  || DeficiencyCode.ToLower() == Enums.WODeficiencyCode.FI.ToString().ToLower())
                         && (StatusCode != Enums.StatusCode.CMPLT.ToString() && StatusCode != Enums.StatusCode.CANCL.ToString()))
            {
                return true;
            }
            return false;
        }

    }

    [DataContract]
    public class WorkOrderFloorAssets
    {
        [DataMember][Key]
        public int ID { get; set; }

        [DataMember]
        public int? IssueId { get; set; }

        [DataMember]
        public int? TmsAssetId { get; set; }

        [DataMember]
        public TInspectionActivity TinspectionActivity { get; set; }

        [DataMember]
        public int? FloorAssetId { get; set; }

        [DataMember]
        public Guid? ActivityId { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }
    }


    public class FloorAssetsWorkOrderCount
    {
        [DataMember]
        public int FloorAssetId { get; set; }
        [DataMember]
        public int TotalWO { get; set; }
        [DataMember]
        public int OpenWO { get; set; }
    }

    public class RoundIssueWorkOrderCategory
    {
        [DataMember]
        public int RoundCatId { get; set; }
        [DataMember]
        public int RouQuesId { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public int IssueId { get; set; }
        public int RoundGroupId { get; set; }
        [DataMember]
        public string RoundGroupName { get; set; }
    }
}