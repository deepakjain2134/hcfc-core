using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data, string message = null)
        {
            Success = true;
            Message = message;
            Result = data;
        }
        public Response(string message)
        {
            Success = false;
            Message = message;
        }
        public bool Success { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Message { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<string> Errors { get; set; }
        public T Result { get; set; }
    }


    [DataContract]
    public class HttpResponseMessage
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Message { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Result Result { get; set; }

        [DataMember]
        public int PageCount { get; set; }

        [DataMember]
        public int PageIndex { get; set; }

        [DataMember]
        public int TotalResult { get; set; }

        public HttpResponseMessage()
        {
            Result = new Result();
        }

    }

    [DataContract]
    public class Result
    {

        [DataMember(EmitDefaultValue = false)]
        public List<Status> Status { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Score> Score { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<NotificationCategory> NotificationCategories { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<NotificationEvent> NotificationEvents { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Manufactures> Manufactures { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<InspResult> InspResults { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<InspStatus> InspStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Actions> Actions { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Cause> Cause { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Problem> Problem { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Item> Item { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<WoStatus> WoStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public WoStatus WOStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<SubStatus> SubStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public SubStatus SubStatusObject { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<UserProfile> Users { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Menus> Menus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<EPsDocument> EpsDocuments { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile User { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Department> Departments { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Steps> Steps { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<News> News { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<NotificationEmails> NotificationEmails { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public NotificationEmails NotificationEmail { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Organization UserDashBoard { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<RolesInGroups> Roles { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Roles> Role { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Buildings Building { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Buildings> Buildings { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<AssetType> AssetType { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public AssetType AssetTypeObject { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TFloorAssets TFloorAsset { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TFloorAssets> TFloorAssets { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<DashboardDetails> DasboardData { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Assets Asset { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Assets> Assets { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public EPDetails EPDetail { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<EPDetails> EPDetails { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Documents> Documents { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<EPSteps> EPDocument { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<List<EPSteps>> EPDocumentList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Standard Standard { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public StandardEps StandardEp { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Standard> Standards { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<InspectionEPDocs> InspectionDocs { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Inspection Inspection { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TComment TComment { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Inspection> Inspections { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TInspectionDetail> TInspectionDetails { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Organization Organization { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Organization> Organizations { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<UserGroup> UserGroups { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Vendors> Vendors { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Vendors Vendor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TRounds TRound { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TRounds> Rounds { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<RoundCategory> RoundCategorys { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TRounds Round { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Certificates> UserCertificates { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<AffectedEPs> AffectedEPs { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int DefeciencyScore { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Floor Floor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Floor> Floors { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public FloorShapes FloorShape { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<FrequencyMaster> FrequencyMaster { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public EPFrequency EPFrequency { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<MainGoal> MainGoals { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Binders> Binders { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TExercises> Exercises { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TExercises Exercise { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<BuildingType> BuildingTypes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TInspectionActivity> tInspectionActivites { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TInspectionFiles TInspectionFiles { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TInspectionActivity tInspectionActivity { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<QuarterMaster> QuarterMasterSettings { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public QuarterMaster QuarterMasterSetting { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Binders Binder { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public RiskManagement risk { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public RoundsQuestionnaires RoundsQuestionnaires { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public WorkOrder WorkOrder { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<WorkOrder> WorkOrders { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<DownTime> DownTimes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Site> Sites { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Site Site { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<SiteType> SiteType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<CityMaster> CityMaster { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<StateMaster> StateMaster { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Priority> Priorities { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Category> Categories { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<WoStatus> TMSStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Types> Types { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Skills> Skills { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TInspectionReport> TInspectionReports { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TInspectionReport TInspectionReport { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public FloorTypes FloorType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<FloorTypes> FloorTypes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DocumentType DocumentType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DocumentMaster DocumentMaster { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<DocumentType> DocumentTypes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TIlsm> TIlsms { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TIlsm TIlsm { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<FireDrillCategory> FireDrillCategory { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<FireDrillTypes> FireDrillTypes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string result { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public List<BuildVersion> BuildVersions { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<NotificationMapping> NotificationMapping { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<NotificationCategory> NotificationCategory { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<ScheduledLogs> ScheduledLogs { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ScheduledLogs ScheduledLog { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TimeSlots> timeSlots { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<FireWatchLog> FireWatchLogs { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<FloorAssetStatus> FloorAssetStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<StopMaster> Stops { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public StopMaster Stop { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<RouteMaster> Routes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public RouteMaster Route { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<StopsRouteMapping> StopsRouteMapping { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ConstructionType ConstructionType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ConstructionActivity ConstructionActivity { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ConstructionRisk ConstructionRisk { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ConstructionClass ConstructionClass { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ConstructionClassActivity ConstructionClassActivity { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ICRASteps ICRASteps { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TIcraLog TIcraLog { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TIcraLog> TIcraLogs { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ICRARiskArea ICRARiskArea { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<List<TimeSlots>> lsttimeSlots { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<List<FireWatchLog>> lstFireWatchLogs { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<UserRecordsCount> UserRecordsCounts { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public UserRecordsCount UserRecordsCount { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<AssetInspFrequency> AssetInspFrequency { get; set; }      

        [DataMember(EmitDefaultValue = false)]
        public TFloorAssets TFloorAssetItem { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TFiles> TFiles { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<RoundGroup> RoundGroup { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<RoundScheduleDates> RoundScheduleDates { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Certificates certificates { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<AllPermits> AllPermits { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<EPDetails> PoliciesNeedingReview { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<EPDetails> OtherEPsNeedingReview { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<BinderFolders> BinderFolders { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public FloorPlan floorPlan { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public AllPermitReport AllPermitReport { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public WorkOrderfiles WorkOrderfiles { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TICRAReports TICRAReports { get; set; }
        

    }

    public class Files
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Extension { get; set; }

        public string MessageId { get; set; }

    }
}