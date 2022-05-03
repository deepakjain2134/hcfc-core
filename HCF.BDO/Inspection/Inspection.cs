using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class Inspection
    {
        [DataMember][Key]
        public int InspectionId { get; set; }

        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public int? Status { get; set; }

        [DataMember]
        public string SubStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime StartDate { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public long StartDateTimeSpan
        //{
        //    get => DueDate == default(DateTime) ? 0 : Utility.Conversion.ConvertToTimeSpan(StartDate);
        //    set { }
        //}

        [DataMember(EmitDefaultValue = false)]
        public DateTime? DueDate { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public long DueDateTimeSpan
        //{
        //    get => DueDate == default(DateTime) ? 0 : Utility.Conversion.ConvertToTimeSpan(DueDate);
        //    set { }
        //}

        private DateTime? DeficiencyDate { get; set; }

        //[DataMember]
        //public long DeficiencyDateTimeSpan
        //{
        //    get => Utility.Conversion.ConvertToTimeSpan(DeficiencyDate);
        //    set { }
        //}

        [DataMember(EmitDefaultValue = false)]
        public DateTime? EndDate { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public long EndDateTimeSpan
        //{
        //    get => Utility.Conversion.ConvertToTimeSpan(EndDate);
        //    set { }
        //}

        [DataMember(EmitDefaultValue = false)]
        public DateTime CreatedDate { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public long CreatedDateTimeSpan
        //{
        //    get => CreatedDate == default(DateTime) ? 0 : Utility.Conversion.ConvertToTimeSpan(CreatedDate);
        //    set { }
        //}

        [DataMember]
        public int? IsAllDocumentUploaded { get; set; }

        //[DataMember]
        //public long InspectionDateTimeSpan
        //{
        //    get => CreatedDate == default(DateTime) ? 0 : Utility.Conversion.ConvertToTimeSpan(CreatedDate);
        //    set { }
        //}

        [DataMember]
        public bool IsCurrent { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IslmFail { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsRAFail { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsTrigger { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool? IsDeficiency { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public EPDetails EPDetails { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TInspectionActivity> TInspectionActivity { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<InspectionEPDocs> InspectionDocs { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TInspectionFiles> TInspectionFiles { get; set; }

        [DataMember]
        public bool IsDocRequired { get; set; }

        public DateTime? GraceDate { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public long GraceDateTimeSpan
        //{
        //    get => GraceDate == default(DateTime) ? 0 : Utility.Conversion.ConvertToTimeSpan(GraceDate);
        //    set { }
        //}

        [DataMember]
        public int? FrequencyId { get; set; }

        [DataMember]
        public int? DocStatus { get; set; }

        [DataMember]
        public int GraceDays { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public long LastUpdatedDateTimeSpan
        //{
        //    get => CreatedDate == default(DateTime) ? 0 : Utility.Conversion.ConvertToTimeSpan(LastUpdatedDate);
        //    set { }
        //}

        public int LastUpdatedBy { get; set; }
        public UserProfile UserProfile { get; set; }
        //public List<UserProfile> UserProfile { get; set; }
        public DateTime GetNextInsStartDate()
        {
            return this.DueDate.HasValue ? this.DueDate.Value.AddDays(-this.GraceDays) : DateTime.Now;
        }

        public DateTime GetNextInsEndDate()
        {
            return this.DueDate.HasValue ? this.DueDate.Value.AddDays(this.GraceDays) : DateTime.Now;
        }

        public bool IsInInspectionRange()
        {
            DateTime StartDate = this.GetNextInsStartDate();
            return DateTime.Now >= StartDate;
        }

        [DataMember]
        public int InspectionGroupId { get; set; }

        public DateTime? FixDueDate { get; set; }

        public int RecentActivityType { get; set; }

        [DataMember]
        public int? IsCurrentCycle { get; set; }

        [DataMember]
        public int? IsPreviousCycle { get; set; }

        [DataMember]
        public int? PreviousCycleInspectionId { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public List<Buildings> Campus { get; set; }

        

        [DataMember]
        public Guid TCycleId { get; set; }

        [DataMember] 
        public int SiteId { get; set; }
        public Inspection()
        {
            TInspectionActivity = new List<TInspectionActivity>();
            InspectionDocs = new List<InspectionEPDocs>();
            TInspectionFiles = new List<TInspectionFiles>();
            Campus = new List<Buildings>();
        }

        public Inspection(int inspectionId)
        {
            InspectionId = inspectionId;
        }
    }

    [DataContract]
    public class TInspectionActivity
    {
        public TInspectionActivity()
        {
            InspectionDocs = new List<InspectionEPDocs>();
            TInspectionFiles = new List<TInspectionFiles>();
            TInspectionDetail = new List<TInspectionDetail>();
            WorkOrders = new List<WorkOrder>();
            UserProfile = new UserProfile();
            StandardEps = new StandardEps();
            TaggedMaster = new List<TaggedMaster>();

        }

        [DataMember]
        public int? ReportFileId { get; set; }


        [DataMember]
        public TFiles ReportFile { get; set; }

        [DataMember]
        public Enums.RiskScore RiskScore { get; set; }


        [DataMember]
        public int? TinsDocId { get; set; }

        [DataMember]
        public DateTime? ScheduleDueDate { get; set; }

        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public int OpenWOCount { get; set; }

        [DataMember][Key]
        public int TInsActivityId { get; set; }

        [DataMember]
        public Guid ActivityId { get; set; }

        [DataMember]
        public int ActivityType { get; set; }

        [DataMember]
        public int? InspectionId { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? FloorAssetId { get; set; }

        [DataMember]
        public bool IsDeficiency { get; set; }

        [DataMember]
        public bool IsIlsm { get; set; }

        [DataMember]
        public int? RaScore { get; set; }

        [DataMember]
        public int IsWorkOrder { get; set; }

        [DataMember]
        public bool IsRAFail { get; set; }

        public DateTime? FixedDueDate { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? GraceDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        //[DataMember]
        //public long CreatedDateTimeSpan
        //{
        //    get => ActivityInspectionDate == null ? 0 : Utility.Conversion.ConvertToTimeSpan(ActivityInspectionDate);
        //    set { }
        //}

        //[DataMember]
        //public long DueDateTimeSpan
        //{
        //    get => DueDate == null ? 0 : Utility.Conversion.ConvertToTimeSpan(DueDate);
        //    set { }
        //}

        [DataMember(EmitDefaultValue = false)]
        public List<TInspectionDetail> TInspectionDetail { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile UserProfile { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TFloorAssets TFloorAssets { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TInspectionFiles> TInspectionFiles { get; set; }

        [DataMember]
        public bool IsSubmit { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public InspectionEPDocs InspectionEPDocs { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<InspectionEPDocs> InspectionDocs { get; set; }

        [DataMember]
        public List<WorkOrder> WorkOrders { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DocumentType DocumentType { get; set; }

        public DateTime? DtEffectiveDate { get; set; }

        public DateTime DtEffectiveDateUTC { get; set; }

        //[DataMember]
        //public long DtEffectiveDateTimeSpan
        //{
        //    get => DtEffectiveDate == null ? 0 : Utility.Conversion.ConvertToTimeSpan(DtEffectiveDate.Value);
        //    set { }
        //}

        [DataMember(EmitDefaultValue = false)]
        public int? DocTypeId { get; set; }

        [DataMember]
        public string ActivityInspStatus { get; set; }

        [DataMember]
        public int DueWithIn { get; set; }


        public DateTime? DeficiencyDate { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public long DeficiencyTimeSpan
        //{
        //    get => DeficiencyDate.HasValue ? Utility.Conversion.ConvertToTimeSpan(DeficiencyDate) : 0;
        //    set { }
        //}

        public DateTime? RaDate { get; set; }

        //[DataMember]
        //public long RaDateTimeSpan
        //{
        //    get => DeficiencyDate.HasValue ? Utility.Conversion.ConvertToTimeSpan(RaDate) : 0;
        //    set { }
        //}

        public DateTime? IlsmDate { get; set; }

        //[DataMember]
        //public long IlsmDateTimeSpan
        //{
        //    get => IlsmDate.HasValue ? Utility.Conversion.ConvertToTimeSpan(IlsmDate) : 0;
        //    set { }
        //}

        public DateTime? DRDate { get; set; }

        //[DataMember]
        //public long DRDateTimeSpan
        //{

        //    get => DRDate.HasValue ? Utility.Conversion.ConvertToTimeSpan(DRDate) : 0;
        //    set { }
        //}

        //[DataMember]
        //public int EstimatedTime { get; set; }

        [DataMember]
        public FrequencyMaster Frequency { get; set; }

        [DataMember]
        public string SubStatus { get; set; }

        public Inspection Tinspection { get; set; }

        //private string SetStepDeficiencyCode(int Status, int? DRTime, DateTime? DeficiencyDate)
        //{
        //    if (Status == 1)
        //        return Utility.Enums.InspectionSubStatus.NA.ToString();
        //    else if (Status == 0 && DRTime.Value == 0)
        //        return Utility.Enums.InspectionSubStatus.DE.ToString();
        //    else
        //    {
        //        DateTime currentTime = DateTime.UtcNow;
        //        DateTime RaTime = DeficiencyDate.Value.AddHours(DRTime.Value);
        //        if (currentTime > RaTime)
        //            return Utility.Enums.InspectionSubStatus.RA.ToString();
        //        else
        //            return Utility.Enums.InspectionSubStatus.DE.ToString();
        //    }
        //}

        [DataMember]
        public bool IsReInspection { get; set; }

        [DataMember]
        public int? FrequencyId { get; set; }

        [DataMember]
        public DateTime? ActivityInspectionDate { get; set; }

        [DataMember]
        public double InspectionDateTimeSpan { get; set; }

        //[DataMember]
        //public long ActivityInspectionDateTimeSpan
        //{
        //    get => ActivityInspectionDate.HasValue ? Utility.Conversion.ConvertToTimeSpan(ActivityInspectionDate) : 0;
        //    set { }
        //}

        [DataMember]
        public int GraceDays { get; set; }

        public DateTime GetNextInsStartDate()
        {
            if (this.DueDate.HasValue)
            {
                return this.DueDate.Value.AddDays(-this.GraceDays);
            }
            else
            {
                return DateTime.Now;
            }
        }

        public DateTime GetInsEndDate()
        {
            return this.DueDate.HasValue ? this.DueDate.Value.AddDays(this.GraceDays) : DateTime.Now;
        }

        [DataMember]
        public int InsType { get; set; }

        [DataMember]
        public string StepsId { get; set; }

        [DataMember]
        public StandardEps StandardEps { get; set; }

        [DataMember]
        public InspResult InspResult { get; set; }

        [DataMember]
        public InspStatus InspStatus { get; set; }

        [DataMember]
        public string InspStatusCode { get; set; }

        [DataMember]
        public int MaxDrTime { get; set; }

        [DataMember]
        public TIlsm Ilsm { get; set; }

        [DataMember]
        public List<EPDetails> EPDetails { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public int? DocInspectionId { get; set; }

        public int? IssueId { get; set; }

        public string Actiontaken { get; set; }

        public string Notify { get; set; }

        //public string ActiontakenEmail
        //{
        //    get => this.TaggedResources.Count > 0 ? string.Join(",", TaggedResources.Where(x => x.TaggedType == 1).Select(x => x.Email)) : string.Empty;
        //    set { }
        //}

        //public string NotifyEmail
        //{
        //    get => this.TaggedResources.Count > 0 ? string.Join(",", TaggedResources.Where(x => x.TaggedType == 0).Select(x => x.Email)) : string.Empty;
        //    set { }
        //}

        public List<TaggedMaster> TaggedMaster { get; set; }

    }


    [DataContract]
    public class InspectionEPDocs
    {

        public InspectionEPDocs()
        {
            UserProfile = new UserProfile();
        }

        [DataMember(EmitDefaultValue = false)][Key]
        public int TInsDocs { get; set; }

        [DataMember] public bool IsDeleted { get; set; }

        [DataMember]
        public Guid ActivityId { get; set; }

        [DataMember]
        public Guid TaggedId { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public int? AttachmentId { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public int? DocumentId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string DocumentName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? InspectionId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Path { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FilesContent { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [Display(Name = "Uploaded date")]
        public DateTime CreatedDate { get; set; }



        [DataMember(EmitDefaultValue = false)]
        public UserProfile UserProfile { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public double? EffectiveDate { get; set; }

        public DateTime? DtEffectiveDate { get; set; }
        public string Text_DtEffectiveDate { get; set; }
        public string Text_CreatedDate { get; set; }
        //[DataMember(EmitDefaultValue = false)]
        //public long DtEffectiveDateTimeSpan
        //{
        //    get => Utility.Conversion.ConvertToTimeSpan(DtEffectiveDate);
        //    set { }
        //}

        //[DataMember(EmitDefaultValue = false)]
        //public long CreatedDateTimeSpan
        //{
        //    get => Utility.Conversion.ConvertToTimeSpan(CreatedDate);
        //    set { }
        //}

        public DateTime? ExpiredDate { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public long ExpiredDateTimeSpan
        //{
        //    get => Utility.Conversion.ConvertToTimeSpan(ExpiredDate);
        //    set { }
        //}

        [DataMember]
        public int ActivityType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? DocTypeId { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public string Version { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public EPDetails EPDetail { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Assets Assets { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DocumentType DocumentType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Binders Binder { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? BinderId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? UploadDocTypeId { get; set; }

        public bool isShowNextDate => ShowNextDate();

        private bool ShowNextDate()
        {
            bool status = false;
            if (UploadDocTypeId.HasValue)
            {
                if (UploadDocTypeId.Value == (int)Enums.UploadDocTypes.Policy || UploadDocTypeId.Value == (int)Enums.UploadDocTypes.Report)
                    status = true;
            }
            return status;
        }

        public int DueWithInDays { get; set; }

        //public string DocReferenceId { get; set; }

        public bool IsRequiredDoc => SetRequiredDoc();

        private bool SetRequiredDoc()
        {
            var status = true;
            if (DocTypeId.HasValue)
            {
                if (DocTypeId.Value == (int)Enums.UploadDocTypes.Policy ||
                    DocTypeId.Value == (int)Enums.UploadDocTypes.Report ||
                    //  DocTypeId.Value == (int)Utility.Enums.UploadDocTypes.AssetsReport ||
                    DocTypeId.Value == (int)Enums.UploadDocTypes.MiscDocuments ||
                    DocTypeId.Value == (int)Enums.UploadDocTypes.SampleDocument)
                    status = false;
            }
            return status;
        }

        [DataMember(EmitDefaultValue = false)]
        public int? FileId { get; set; }
    }

    [DataContract]
    public class TInspectionDetail
    {
        [DataMember][Key]
        public int InspectionDetailId { get; set; }

        [DataMember]
        public Guid ActivityId { get; set; }

        [DataMember]
        public int MainGoalId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public MainGoal MainGoal { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<InspectionSteps> InspectionSteps { get; set; }

        [DataMember]
        public string StepsId { get; set; }

        public TInspectionDetail()
        {
            InspectionSteps = new List<InspectionSteps>();
        }

    }

    [DataContract]
    public class InspectionSteps
    {
        public InspectionSteps()
        {
            WorkOrders = new List<WorkOrder>();
        }

        [DataMember][Key]
        public int TInsStepsId { get; set; }

        [DataMember]
        public int InspectionDetailId { get; set; }

        [DataMember]
        public int StepsId { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public string Comments { get; set; }

        [DataMember]
        public int? RAScore { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FileContent { get; set; }

        [DataMember]
        public bool IsMarkDeficiencies { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? DRTime { get; set; }

        [DataMember]
        public int? IssueId { get; set; }

        [DataMember]
        public string DeficiencyStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Steps Steps { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<WorkOrder> WorkOrders { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<TInsStepsTask> TInsStepsTask { get; set; }

        public DateTime? DateCompleted { get; set; }

        public DateTime? DeficiencyDate { get; set; }

        [DataMember]
        public string DeficiencyCode { get; set; }

        [DataMember]
        public bool IsRA { get; set; }

        public DateTime? RaDate { get; set; }

        [DataMember]
        public string InputValue { get; set; }

        [DataMember]
        public int? AddedBy { get; set; }

        [DataMember]
        public DateTime? AddedDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        //[DataMember]
        //public long CreatedDateTimeSpan
        //{
        //    get => CreatedDate == null ? 0 : Utility.Conversion.ConvertToTimeSpan(CreatedDate);
        //    set { }
        //}

    }

    [DataContract]
    public class MarkPassAssets
    {
        [DataMember]
        public int FloorAssetId { get; set; }

        [DataMember]
        public int AssetsId { get; set; }

        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public int InspectionId { get; set; }

        [DataMember]
        public DateTime? InspectionDate { get; set; }

        [DataMember]
        public long InspectionDateTimeSpan { get; set; }

        [DataMember]
        public int? FileId { get; set; }

        [DataMember]
        public string FileContent { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public int InspectionGroupId { get; set; }



        [DataMember]
        public string AttachFiles { get; set; }


        // in case of save tms workorder assets as inspection
        [DataMember]
        public Enums.Frequency Frequency { get; set; }

        [DataMember]
        public int TmsAsssetId { get; set; }

        [DataMember]
        public int WorkOrderId { get; set; }

        [DataMember]
        public int? IsPreviousCycle { get; set; }

        [DataMember]
        public int? PreviousCycleInspectionId { get; set; }

        [DataMember]
        public Guid TCycleId { get; set; }
    }

    public class RiskManagement
    {
        public int RaCount { get; set; }
        public int IlsmCount { get; set; }
        public int DefecienciesCount { get; set; }
        public int Highcount { get; set; }
        public int LowCount { get; set; }
        public int ModerateCount { get; set; }
        public int IsssueCount { get; set; }
        public int RAEP { get; set; }
        public int DEEP { get; set; }
        public int ICRAPendingCount { get; set; }
        public int ICRAPermitAuthorizedByCount { get; set; }
        public int ICRAPermitRequestByCount { get; set; }
        public int RaUserCount { get; set; }
        public int ILUserCount { get; set; }
        public int DEUserCount { get; set; }
        public int IssueUserCount { get; set; }
        public int ICRACount { get; set; }
        public int AllPermitCount { get; set; }
        public int PCRACount { get; set; }
        public int ICRAPermitCount { get; set; }
        public int CRACount { get; set; }
        public int OneTimeFailsCount
        {
            get { return GetCount(1); }
            set { }
        }

        private int GetCount(int index)
        {
            int count = 0;

            var data = StandardEPs.GroupBy(x => x.EPDetailId).ToList();
            count = index > 2 ? data.Count(x => Enumerable.Count<StandardEps>(x) > 2) : data.Count(x => Enumerable.Count<StandardEps>(x) == index);

            return count;
        }

        public int TwoTimesFailsCount
        {
            get { return GetCount(2); }
            set { }
        }
        public int MoreThanTwoFailsCount
        {
            get { return GetCount(3); }
            set { }
        }

        public List<StandardEps> StandardEPs { get; set; }

        public RiskManagement()
        {
            StandardEPs = new List<StandardEps>();
        }

    }

    public class UserRecordsCount
    {
        public int TotalRounds { get; set; }

        public int TodayFireDrill { get; set; }

        public int PermitAuthorizedByCount { get; set; }

        public int PermitRequestByCount { get; set; }

        public int WoCount { get; set; }

        public int AssetsReportPending { get; set; }

        public int TotalEcEps { get; set; }

        public int TotalLsEps { get; set; }

        public int TotalEmEps { get; set; }

        public int TotalEps
        {
            get { return TotalEcEps + TotalEmEps + TotalLsEps; }
            set { }
        }

        public int TotalDueEps { get; set; }

        public int TotalFailedEps { get; set; }

        public int TotalInProgressEps { get; set; }

        public int TotalCompletedEps { get; set; }

        public int CRAPermitCount { get; set; }

        public int AllPermitPendingCount { get; set; }

        public int TaggedItem { get; set; }

        public List<StandardEps> StandardEps { get; set; }

        public List<TaggedMaster> TaggedMasters { get; set; }
    }



    public class calenderViewDashBoard
    {
        public List<TExercises> Exercises { get; set; }

        public List<Inspection> inspections { get; set; }

        public List<RoundScheduleDates> RoundSchedules { get; set; }

        public calenderViewDashBoard()
        {
            Exercises = new List<TExercises>();
            inspections = new List<Inspection>();
            RoundSchedules = new List<RoundScheduleDates>();

        }
    }
}