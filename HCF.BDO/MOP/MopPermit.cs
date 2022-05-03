using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class TMOP
    {
        [DataMember]
        public int? PermitAuthorizedSignatureId { get; set; }
        [DataMember]
        public DigitalSignature DSSign1Signature { get; set; }
        [DataMember]
        public DigitalSignature DSSign2Signature { get; set; }
        public TIcraProject Project { get; set; }
        [DataMember]
        public string PermitNo { get; set; }

        [DataMember]
        public string AttachFiles { get; set; }
        [DataMember]
        public string AttachDrawingFiles { get; set; }
        [DataMember]
        public string ProjectName
        {
            get => GetProjectName();
            set { }
        }

        private string GetProjectName()
        {
            if (Project != null)
                return string.Format("{0} ({1})", Project.ProjectName, Project.ProjectNumber);
            else
                return "";
        }

        [DataMember][Key] public int TmopId { get; set; }

        [DataMember] public int? ProjectId { get; set; }

        [DataMember] public DateTime? Date { get; set; }

        [DataMember] public DateTime? StartDate { get; set; }

        [DataMember] public string StartTime { get; set; }

        [DataMember] public DateTime? EndDate { get; set; }

        [DataMember] public string EndTime { get; set; }

        [DataMember] public string Contractor { get; set; }

        [DataMember] public string Supervisor { get; set; }

        [DataMember] public string BuildingId { get; set; }
        [DataMember] public string BuildingName { get; set; }

        [DataMember] public string FloorId { get; set; }
        [DataMember] public string FloorName { get; set; }

        [DataMember] public string Zones { get; set; }

        [DataMember] public string Description { get; set; }

        [DataMember] public string ProcedureSteps { get; set; }

        [DataMember] public string RequiredFollowUp { get; set; }

        [DataMember] public string NotificationsNecessary { get; set; }

        [DataMember] public string AdditionalComments { get; set; }
        [DataMember] public string ApproverName { get; set; }
        [DataMember] public string RejectReason { get; set; }
        [DataMember] public int? CreatedBy { get; set; }

        [DataMember] public int Status { get; set; }

        [DataMember] public int? ApproveBy { get; set; }

        [DataMember] public DateTime? ApproveDate { get; set; }

        [DataMember] public int? ApproverSignatureId { get; set; }

        [DataMember] public bool HasAttachment { get; set; }
        [DataMember] public int? RequestBy { get; set; }
        [DataMember] public int? RequesterSignatureId { get; set; }
        [DataMember] public string EmailAddress { get; set; }
        [DataMember]
        public string TFileIds { get; set; }

        [DataMember]
        public string TDrawingFields { get; set; }
        [DataMember] public DateTime? CreatedDate { get; set; }

        [DataMember] public int? UpdatedBy { get; set; }

        [DataMember] public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public UserProfile ApprovedByUser { get; set; }

        [DataMember]
        public UserProfile RequestByUser { get; set; }

        public List<TProjectContactList> ProjectContactList { get; set; }

        public List<TSystemImpactArea> SystemImpactArea { get; set; }

        public List<TMopAdditionalForms> AdditionalForms { get; set; }

        public List<TMopAdditionalForms> SystemsImpacted { get; set; }
       
        [DataMember]
        public List<TFiles> Attachments { get; set; }
        [DataMember]
        public List<DrawingpathwayFiles> DrawingAttachments { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        public TIcraProject TIcraProject { get; set; }
        public TMOP()
        {
            ProjectContactList = new List<TProjectContactList>();
            SystemImpactArea = new List<TSystemImpactArea>();
            AdditionalForms = new List<TMopAdditionalForms>();
            Attachments = new List<TFiles>();
            DrawingAttachments = new List<DrawingpathwayFiles>();
        }
    }

    [DataContract]
    public class MopAdditionalForms
    {
        [DataMember] public int Id { get; set; }

        [DataMember] public string FormName { get; set; }

        [DataMember] public string Description { get; set; }
        [DataMember] public string AdditionalType { get; set; }

        [DataMember] public bool IsActive { get; set; }

        [DataMember] public int? CreatedBy { get; set; }

        [DataMember] public DateTime? CreatedDate { get; set; }

        [DataMember] public int? UpdatedBy { get; set; }

        [DataMember] public DateTime? UpdatedDate { get; set; }

        [DataMember]
        public int Type { get; set; }

    }

    [DataContract]
    public class TMopAdditionalForms
    {
        [DataMember] public int Id { get; set; }

        [DataMember] public int? TMopId { get; set; }

        [DataMember] public int? FormId { get; set; }

        [DataMember] public int? HasResponded { get; set; }

        [DataMember] public bool RespondStatus { get; set; }
        [DataMember] public string  AdditionalType { get; set; }

        [DataMember] public int? CreatedBy { get; set; }

        [DataMember] public DateTime CreatedDate { get; set; }

        [DataMember] public int? UpdatedBy { get; set; }

        [DataMember] public DateTime? UpdatedDate { get; set; }
        [DataMember] public int? HasCompleted { get; set; }
        [DataMember] public string PermitId { get; set; }
        [DataMember] public string PermitNo { get; set; }
        

        [DataMember]
        public MopAdditionalForms AdditionalForms { get; set; }

    }

    [DataContract]
    public class TSystemImpactArea
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string AreaName { get; set; }

        [DataMember]
        public int? Sequence { get; set; }

        [DataMember]
        public int? TmopId { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public int? UpdatedBy { get; set; }

        [DataMember]
        public DateTime? UpdatedDate { get; set; }

    }

    [DataContract]
    public class TProjectContactList
    {
        [DataMember] public int Id { get; set; }

        [DataMember] public int? TmopId { get; set; }

        [DataMember] public string Name { get; set; }

        [DataMember] public string Phone { get; set; }

        [DataMember] public string EmailAddress { get; set; }

        [DataMember] public int? Sequence { get; set; }

        [DataMember] public int? CreatedBy { get; set; }

        [DataMember] public DateTime? CreatedDate { get; set; }

        [DataMember] public int? UpdatedBy { get; set; }

        [DataMember] public DateTime? UpdatedDate { get; set; }

    } 
}