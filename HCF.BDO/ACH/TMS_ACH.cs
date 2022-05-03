using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HCF.BDO.MaintenanceConnection
{
    public class ParentRef
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class ClassificationRef
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class DepartmentRef
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class AccountRef
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class RepairCenterRef
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class TypeDetails
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class ClassIndustryDetails
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class RiskAssessmentGroupDetails
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class PMCycleStartByDetails
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class TMS_Results
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public bool? IsLocation { get; set; }
        public bool? IsUp { get; set; }
        public decimal PurchaseCost { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal ReplacementCost { get; set; }
        public bool? InstructionsToWO { get; set; }
        public bool? IsMeter { get; set; }
        public decimal Meter1Reading { get; set; }
        public decimal Meter2Reading { get; set; }
        public string InternalAssetNum { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }
        public string Vicinity { get; set; }
        public string GenLedger { get; set; }
        public DateTime? WarrantyExpire { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public DateTime? PurchasedDate { get; set; }
        public string PurchaseOrder { get; set; }
        public DateTime? InstallDate { get; set; }
        public DateTime? StartupDate { get; set; }
        public DateTime? ReplaceDate { get; set; }
        public int? Life { get; set; }
        public DateTime? DisposalDate { get; set; }
        public string LicenseNumber { get; set; }
        public string Instructions { get; set; }
        public string InsuranceCarrier { get; set; }
        public string InsurancePolicy { get; set; }
        public string LeaseNumber { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string xcoordinate { get; set; }
        public string ycoordinate { get; set; }
        public string zcoordinate { get; set; }
        public string parcelid { get; set; }
        public string Technology { get; set; }
        public string TechnologyDesc { get; set; }
        public bool? Meter1TrackHistory { get; set; }
        public bool? Meter2TrackHistory { get; set; }
        public DateTime? LastMaintained { get; set; }
        public int? LastMaintained_WOPK { get; set; }
        public string Icon { get; set; }
        public string Photo { get; set; }
        public bool? DrawingUpdatesNeeded { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? RiskAssessmentRequired { get; set; }
        public DateTime? LastAssessed { get; set; }
        public bool? PMRequired { get; set; }
        public bool? PlanForImprovement { get; set; }
        public bool? HIPPARelated { get; set; }
        public bool? StatementOfConditions { get; set; }
        public bool? StatementOfConditionsCompliant { get; set; }
        public string County { get; set; }
        public DateTime? YearBuilt { get; set; }
        public string MajorRenovations { get; set; }
        public decimal? SquareFootage { get; set; }
        public int? NumberOfStories { get; set; }
        public string FloodZone { get; set; }
        public string QuakeZone { get; set; }
        public string Ext100Feet { get; set; }
        public int? OperatingUnits { get; set; }
        public double? EstimatedValue { get; set; }
        public bool? Meter1RollDown { get; set; }
        public bool? Meter2RollDown { get; set; }
        public string Meter1RollDownMethod { get; set; }
        public string Meter2RollDownMethod { get; set; }
        public decimal? Meter1ReadingLife { get; set; }
        public decimal? Meter2ReadingLife { get; set; }
        public bool? IsLinear { get; set; }
        public DateTime? PMCycleStartDate { get; set; }
        public int? PMCounter { get; set; }
        public string ModelNumber { get; set; }
        public string ModelNumberMFG { get; set; }
        public bool? ResourceForScheduling { get; set; }
        public int? RotatingRepairCenterPK { get; set; }
        public int? RotatingLocationPK { get; set; }
        public int? RotatingLaborPK { get; set; }
        public string RotatingBin { get; set; }
        public string GISLayer { get; set; }
        public string GISKeyValue { get; set; }
        public string GISTable { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string UDFChar1 { get; set; }
        public string UDFChar2 { get; set; }
        public string UDFChar3 { get; set; }
        public string UDFChar4 { get; set; }
        public string UDFChar5 { get; set; }
        public string UDFChar6 { get; set; }
        public string UDFChar7 { get; set; }
        public string UDFChar8 { get; set; }
        public string UDFChar9 { get; set; }
        public string UDFChar10 { get; set; }
        public DateTime? UDFDate1 { get; set; }
        public DateTime? UDFDate2 { get; set; }
        public DateTime? UDFDate3 { get; set; }
        public DateTime? UDFDate4 { get; set; }
        public DateTime? UDFDate5 { get; set; }
        public bool? UDFBit1 { get; set; }
        public bool? UDFBit2 { get; set; }
        public bool? UDFBit3 { get; set; }
        public bool? UDFBit4 { get; set; }
        public bool? UDFBit5 { get; set; }
        public int? RowVersionUserPK { get; set; }
        public string RowVersionAction { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string ResourceUri { get; set; }
        public Guid? AssetUUID { get; set; }
        public int? AssetLevel { get; set; }
        public ParentRef ParentRef { get; set; }
        public int ParentRefPK { get; set; }
        public string ParentRefID { get; set; }
        public string ParentRefName { get; set; }
        public ClassificationRef ClassificationRef { get; set; }
        public int ClassificationRefPK { get; set; }
        public string ClassificationRefID { get; set; }        
        public string ClassificationName { get; set; }
        public object OperatorRef { get; set; }
        public DepartmentRef DepartmentRef { get; set; }
        public object CustomerRef { get; set; }
        public AccountRef AccountRef { get; set; }
        public RepairCenterRef RepairCenterRef { get; set; }
        public object ShopRef { get; set; }
        public object VendorRef { get; set; }
        public object ManufacturerRef { get; set; }
        public object ResponsibilitySafetyRef { get; set; }
        public object ResponsibilityRepairRef { get; set; }
        public object ResponsibilityPMRef { get; set; }
        public object ServiceRepairRef { get; set; }
        public object ServicePMRef { get; set; }
        public object ContactRef { get; set; }
        public object ZoneRef { get; set; }
        public object StatusDetails { get; set; }
        public TypeDetails TypeDetails { get; set; }
        public object SystemDetails { get; set; }
        public object PriorityDetails { get; set; }
        public object TechnologyDetails { get; set; }
        public object Meter1UnitsDetails { get; set; }
        public object Meter2UnitsDetails { get; set; }
        public object PurchaseTypeDetails { get; set; }
        public ClassIndustryDetails ClassIndustryDetails { get; set; }
        public RiskAssessmentGroupDetails RiskAssessmentGroupDetails { get; set; }
        public object ConstructionCodeDetails { get; set; }
        public object ISOProtectionDetails { get; set; }
        public object AutoSprinklerDetails { get; set; }
        public object SmokeAlarmDetails { get; set; }
        public object HeatAlarmDetails { get; set; }
        public PMCycleStartByDetails PMCycleStartByDetails { get; set; }
        public object ModelLineDetails { get; set; }
        public object ModelSeriesDetails { get; set; }
        public object SystemPlatformDetails { get; set; }
    }

    public class TMS_Classifcation
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public ClassificationRef ClassificationRef { get; set; }
    }

    public class RootObject<T>
    {
        public List<T> Results { get; set; }
        public int Total { get; set; }
    }

    public class AuthStatusDetails
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class AuthStatusDetails2
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class AuthorizationStatus
    {
        public AuthStatusDetails2 AuthStatusDetails { get; set; }
        public int? AuthLevelsRequired { get; set; }
        public int? AuthStatusUserPK { get; set; }
        public string AuthStatusUserInitials { get; set; }
        public DateTime? AuthStatusDate { get; set; }
    }

    public class StatusDetails
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class PriorityDetails
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class AssetRef
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class ProcedureRef
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class RequesterRef
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class TakenByRef
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class TMS_WorkOrderRes
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Reason { get; set; }
        public DateTime? TargetDate { get; set; }
        public string Instructions { get; set; }
        public string LaborReport { get; set; }
        public int? RouteOrder { get; set; }
        public bool? HasWarranty { get; set; }
        public bool? IsShutdownRequired { get; set; }
        public bool? IsLockoutTagout { get; set; }
        public bool? IsChargeable { get; set; }
        public bool? IsFollowupWork { get; set; }
        public bool? IsFailedWorkOrder { get; set; }
        public bool? IsOpen { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsPartsReserved { get; set; }
        public bool? IsAssigned { get; set; }
        public int? GroupPK { get; set; }
        public int? FollowupFromWorkOrderPK { get; set; }
        public decimal? TargetHours { get; set; }
        public decimal? TargetDays { get; set; }
        public decimal? ActualHours { get; set; }
        public decimal? ResponseHours { get; set; }
        public decimal? Meter1Reading { get; set; }
        public decimal? Meter2Reading { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? RequestedDate { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime? OnHoldDate { get; set; }
        public decimal? CompletePercent { get; set; }
        public DateTime? CompleteDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? CanceledDate { get; set; }
        public DateTime? DeniedDate { get; set; }
        public DateTime? RespondedDate { get; set; }
        public DateTime? FinalizedDate { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
        public string UDFChar1 { get; set; }
        public string UDFChar2 { get; set; }
        public string UDFChar3 { get; set; }
        public string UDFChar4 { get; set; }
        public string UDFChar5 { get; set; }
        public string UDFChar6 { get; set; }
        public string UDFChar7 { get; set; }
        public string UDFChar8 { get; set; }
        public string UDFChar9 { get; set; }
        public string UDFChar10 { get; set; }
        public DateTime? UDFDate1 { get; set; }
        public DateTime? UDFDate2 { get; set; }
        public DateTime? UDFDate3 { get; set; }
        public DateTime? UDFDate4 { get; set; }
        public DateTime? UDFDate5 { get; set; }
        public bool? UDFBit1 { get; set; }
        public bool? UDFBit2 { get; set; }
        public bool? UDFBit3 { get; set; }
        public bool? UDFBit4 { get; set; }
        public bool? UDFBit5 { get; set; }
        public string LastModifiedAction { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string ResourceUri { get; set; }
        public Guid? WorkOrderUUID { get; set; }
        public AuthStatusDetails AuthStatusDetails { get; set; }
        public AuthorizationStatus AuthorizationStatus { get; set; }
        public TypeDetails TypeDetails { get; set; }
        public StatusDetails StatusDetails { get; set; }
        public CustomDetails SubStatusDetails { get; set; }
        public PriorityDetails PriorityDetails { get; set; }
        public CustomDetails ReferenceDetails { get; set; }
        public RepairCenterRef RepairCenterRef { get; set; }
        public AssetRef AssetRef { get; set; }
        public CustomRef AccountRef { get; set; }
        public ProcedureRef ProcedureRef { get; set; }
        public RequesterRef RequesterRef { get; set; }
        public string RequesterPhone { get; set; }
        public string RequesterEmail { get; set; }
        public string RequesterName { get; set; }
        public CustomRef StockRoomRef { get; set; }
        public CustomRef ToolRoomRef { get; set; }
        public CustomRef SupervisorRef { get; set; }
        public CustomRef ProjectRef { get; set; }
        public CustomRef ShopRef { get; set; }
        public CustomRef ShiftRef { get; set; }
        public CustomRef DepartmentRef { get; set; }
        public CustomRef CustomerRef { get; set; }
        public CustomRef ProblemRef { get; set; }
        public CustomRef FailureRef { get; set; }
        public CustomRef SolutionRef { get; set; }
        public CustomRef PMRef { get; set; }
        public CustomRef CategoryRef { get; set; }
        public TakenByRef TakenByRef { get; set; }
        public CustomRef ZoneRef { get; set; }
    }

    public class CustomRef
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }

        public string Uuid { get; set; }
    }

    public class CustomDetails
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    #region TMS Users

    public class TMS_UserRes
    {
        public int PK { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? Active { get; set; }

        public CustomDetails LaborTypeDetails { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }

    #endregion

    #region WorkOrder Assignment

    public class TMS_WorkOrderAssignment
    {
        public Int32  PK { get; set; }
        public Int32 WorkOrderPK { get; set; }

        public bool? IsAssigned { get; set; }
        public DateTime? AssignedDate { get; set; }

        public decimal? AssignedHours { get; set; }

        public string LaborID { get; set; }

        public Int32? LaborPK { get; set; }
        public bool? IsAssignedLead { get; set; }

        public bool? IsAssignedPDA { get; set; }
        public Int32?  WOlaborPK { get; set; }

        public decimal? CompletePercent { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool? IsActive { get; set; }

        public bool? IsAnytime { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public CustomRef LaborRef { get; set; }
        public string ResourceUri { get; set; }

    }

    #endregion

    #region WorkOrder Task

    [DataContract]
    public class TMS_WorkOrderTasks
    {
        [DataMember]
        public Int32  PK { get; set; }

        [DataMember]
        public Int32 WorkOrderPK { get; set; }

        [DataMember]
        public decimal? TaskNo { get; set; }

        [DataMember]
        public string TaskAction { get; set; }

        [DataMember]
        public bool? IsHeader { get; set; }

        [DataMember]
        public string MeasurementInitial { get; set; }

        [DataMember]
        public string Measurement { get; set; }

        [DataMember]
        public decimal? HoursEstimated { get; set; }

        [DataMember]
        public decimal? HoursActual { get; set; }

        [DataMember]
        public Int32? Rate { get; set; }

        [DataMember]
        public Int32? TaskPK { get; set; }

        [DataMember]
        public Int32? CraftPK { get; set; }

        [DataMember]
        public Int64? AssetSpecificationPK { get; set; }

        [DataMember]
        public Int32? SpecificationPK { get; set; }


        [DataMember]
        public Int32? AssetPK { get; set; }

        public Int32? ClassificationPK { get; set; }

        [DataMember]

        public Int32? ToolPK { get; set; }



        [DataMember]
        public bool? IsComplete { get; set; }

        [DataMember]
        public bool? IsFailed { get; set; }

        [DataMember]
        public bool? IsSpec { get; set; }

        [DataMember]
        public bool? IsMeter1 { get; set; }

        [DataMember]
        public bool? IsMeter2 { get; set; }

        [DataMember]
        public bool? NotApplicable { get; set; }

        [DataMember]
        public string Comments { get; set; }

        [DataMember]
        public string Initials { get; set; }

        [DataMember]
        public string Photo1 { get; set; }

        [DataMember]
        public bool? DeletePhoto1 { get; set; }

        [DataMember]
        public string Photo2 { get; set; }

        [DataMember]
        public bool? DeletePhoto2 { get; set; }

        [DataMember]
        public Int32? FollowupWorkOrderPK { get; set; }


        [DataMember]
        public DateTime? LastModifiedDate { get; set; }

    
        [DataMember]
        public string ResourceUri { get; set; }

    }

    #endregion

    #region Lookup Table & Vlaues 
    
    public class TMS_LookupTable
    {
        public string LookupTableID { get; set; }

        public string Description { get; set; }

        public int? CodeWidth { get; set; }

        public int? DescriptionWidth { get; set; }

        public bool? Internal { get; set; }

        public bool? Enabled { get; set; }

        public bool? SkipValidation { get; set; }

        public bool? CanModify { get; set; }

        public bool? NoCascadeUpdate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

    }
    
    public class TMS_LookupTableValues
    {
        public string LookupTableID { get; set; }

        public string CodeName { get; set; }

        public string CodeDesc { get; set; }

        public decimal? CodeValue { get; set; }

        public bool? SystemCode { get; set; }

        public bool? AvailableToRequester { get; set; }

        public DateTime? LastModifiedDate { get; set; }

    }


    #endregion

}