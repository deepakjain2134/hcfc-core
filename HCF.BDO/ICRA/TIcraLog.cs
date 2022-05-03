using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using HCF.BDO.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCF.BDO
{
    [DataContract]
    public class TIcraLog
    {
        [DataMember(EmitDefaultValue = false)][Key]
        public int TicraId { get; set; }

        [DisplayName("Project Name")]
        [DataMember(EmitDefaultValue = false)]
        public string ProjectName { get; set; }

        [DisplayName("Infection Preventionist")]
        [DataMember(EmitDefaultValue = false)]
        public string InfectionPreventionist { get; set; }
        [DataMember]
        public string AttachFiles { get; set; }
        [DisplayName("Permit #")]
        [DataMember(EmitDefaultValue = false)]
        public string ProjectNo
        {
            get => GetProjectName();
            set { }
        }

        private string GetProjectName()
        {
            return Project != null ? Project.ProjectName : "";
        }

        [DisplayName("Permit #")]
        [DataMember]
        public string PermitNo { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        public List<TFiles> Attachments { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int? FloorId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Scope { get; set; }

        [DisplayName("Start Date")]
        [DataMember(EmitDefaultValue = false)]
        public DateTime? StartDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long StartDateTimeSpan
        {
            get => StartDate.HasValue ? Conversion.ConvertToTimeSpan(StartDate.Value) : 0;
            set { }
        }


        [DisplayName("Completion Date")]
        [DataMember(EmitDefaultValue = false)]
        public DateTime? CompletionDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long CompletionDateTimeSpan
        {
            get => CompletionDate.HasValue ? Conversion.ConvertToTimeSpan(CompletionDate.Value) : 0;
            set { }
        }


        [DisplayName("ICRA")]
        [DataMember]
        public bool IsICRA { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? ConstructionTypeId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? ConstructionRiskId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? ConstructionClassId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Comment { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long CreatedDateTimeSpan
        {
            get => CreatedDate.HasValue ? Conversion.ConvertToTimeSpan(CreatedDate.Value) : 0;
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        public List<TIcraSteps> TIcraSteps { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Floor Floor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ConstructionType ConstructionType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ConstructionRisk ConstructionRisk { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ConstructionClass ConstructionClass { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Location { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ProjectCoordinator { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string EstimatedDuration { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Contractor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Supervisor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Telephone { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [DisplayName("Permit Request By")]
        public int PermitRequestBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile PermitRequestByUser { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? RequestDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ReasonRejection { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long RequestDateTimeSpan
        {
            get => RequestDate.HasValue ? Conversion.ConvertToTimeSpan(RequestDate.Value) : 0;
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        //[DisplayName("Permit Authorized By")]
        [DisplayName("Reviewer 2")]
        public int? PermitAuthorizedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile PermitAuthorizedByUser { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSPermitAuthorizedBy { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSPermitRequestBy { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public DateTime? AuthorizedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long AuthorizedDateTimeSpan
        {
            get => AuthorizedDate.HasValue ? Conversion.ConvertToTimeSpan(AuthorizedDate.Value) : 0;
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        public List<TICRAFiles> TICRAFiles { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? PermitRequestBySignId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? PermitAuthorizedBySignId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string UnitBelow { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string UnitAbove { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Lateral { get; set; }

        [DisplayName("Lateral")]
        [DataMember(EmitDefaultValue = false)]
        public string Secondl { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Behind { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Front { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string RiskAreaId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TICRAAreasNearBy> AreasSurroundings { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsNotifyEmailPermitReq { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsNotifyEmailPermitAuth { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? Date1 { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long Date1TimeSpan
        {
            get => Date1.HasValue ? Conversion.ConvertToTimeSpan(Date1.Value) : 0;
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? Date2 { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long Date2TimeSpan
        {
            get => Date2.HasValue ? Conversion.ConvertToTimeSpan(Date2.Value) : 0;
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        public string Initial1 { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Initial2 { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? ClassDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long ClassDateTimeSpan
        {
            get => ClassDate.HasValue ? Conversion.ConvertToTimeSpan(ClassDate.Value) : 0;
            set { }
        }


        [DataMember(EmitDefaultValue = false)]
        public string ClassInitial { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Audit> IcraHistory { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<ConstructionType> ConstructionTypes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<ConstructionRisk> ConstructionRisks { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<ConstructionClass> ConstructionClasses { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<ICRAMatixPrecautions> ICRAMatixPrecautions { get; set; }

        [DataMember]
        public TICRAReports ObservtionReport { get; set; }


        [DataMember]
        public List<TICRAReports> ObservtionReports { get; set; }


        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public DateTime? ClosedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long ClosedDateTimeSpan
        {
            get => ClosedDate.HasValue ? Conversion.ConvertToTimeSpan(ClosedDate.Value) : 0;
            set { }
        }


        [DataMember]
        public string ActivityLists { get; set; }


        public DateTime? Class3Date { get; set; }

        public DateTime? Class4Date { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Class3Initial { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Class4Initial { get; set; }

        public int TIlsmId { get; set; }

        public int ProjectId { get; set; }

        public TIcraProject Project { get; set; }

        [NotMapped]
        public List<TIlsm> ILSM { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSPermitReviewerBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [DisplayName("Reviewer 1")]
        public int? PermitReviewerBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? PermitReviewerBySignId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsNotifyEmailPermitReviewer { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile PermitReviewerByUser { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string LinkICRA { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string CeilingPermitId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string TDrawingFields { get; set; }

        [DisplayName("Version")]
        [DataMember]
        public int Version { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<DrawingpathwayFiles> DrawingAttachments { get; set; }
        public TIcraProject TIcraProject { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TPermitWorkFlowDetails> TPermitWorkFlowDetails { get; set; }
        public TIcraLog()
        {
            AreasSurroundings = new List<TICRAAreasNearBy>();
            TICRAFiles = new List<TICRAFiles>();
            TIcraSteps = new List<TIcraSteps>();
            ObservtionReports = new List<TICRAReports>();
            ILSM = new List<TIlsm>();
            DrawingAttachments = new List<DrawingpathwayFiles>();
            TPermitWorkFlowDetails = new List<TPermitWorkFlowDetails>();
        }

    }
    public class AllPermitReport
    {
        public List<PermitCountDetail> PermitCountDetail { get; set; }
        public Int64 AllPermitCount { get; set; }
        public Int64 ProjectCount { get; set; }

    }
    public class PermitCountDetail
    {
        
        public Int32 Status { get; set; }
        public Int64 PermitCount { get; set; }
        public Int64 PermitType { get; set; }
    }
    [DataContract]
    public class TIcraSteps
    {
        [DataMember(EmitDefaultValue = false)][Key]
        public int TIrcaStepId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? TicraId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int ICRAStepId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Comment { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ICRASteps Step { get; set; }

    }


    [DataContract]
    public class TICRAFiles
    {
        [DataMember(EmitDefaultValue = false)][Key]
        public int TICRAFileId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TicraId { get; set; }        

        [DataMember(EmitDefaultValue = false)]
        public string FileName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FilePath { get; set; }

        [DataMember(EmitDefaultValue = false)]       
        public string Name
        {
            get => (!string.IsNullOrEmpty(FileName) ? FileName.Split('.')[0] : string.Empty);
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        public string FileContent { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Comment { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsDeleted { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime UploadedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int UploadedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int AttachmentID { get; set; }

        [DataMember]
        public bool IsReport { get; set; }

        [DataMember]
        public UserProfile UserProfile { get; set; }

    }


    [DataContract]
    public class TICRAAreasNearBy
    {
        [DataMember(EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TicraId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? ConstructionRiskId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ConstructionRisk ConstructionRisk { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int AreasSurroundingId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string RiskAreaIds { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string RiskAreaIdNames { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Enums.AreasSurrounding AreasSurrounding { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Comment { get; set; }

    }
}