using HCF.BDO.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class TFireSystemByPassPermit : PermitBase
    {
        [DataMember]
        public int TFSBPermitId { get; set; }

        //[DataMember]
        //public int? ProjectId { get; set; }

        //[DataMember]
        //public TIcraProject Project { get; set; }
        
        //[DataMember]
        //public string PermitNo { get; set; }      
        

        [DataMember]
        public DateTime? RequestedDate { get; set; }

        [DataMember]
        public DateTime? ScheduledDate { get; set; }

        [DataMember]
        public int? RequestorBy { get; set; }

        [DataMember]
        public UserProfile RequestorByUser { get; set; }

        [DataMember]
        public string PhoneNo { get; set; }

        //[DataMember]
        //public string Company { get; set; }

        //[DataMember]
        //public string Email { get; set; }

        [DataMember]
        public string OnSiteContact { get; set; }

        [DataMember]
        public string OnSitePhone { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }

        [DataMember]
        public TimeSpan? StartTime { get; set; }

        //[DataMember]
        public string Stime { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public TimeSpan? EndTime { get; set; }       

        public string Etime { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsSystemReprogrammingRequired { get; set; }

        [DataMember]
        public int ApprovalStatus { get; set; }

        //[DataMember]
        //public DateTime? ApprovedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSFSBPApproverSign { get; set; }

        [DataMember]
        public int? ApprovedBy { get; set; }

        [DataMember]
        public int? ApproverSignId { get; set; }

        [DataMember]
        public UserProfile ApprovedByUser { get; set; }

        //[DataMember]
        //public int CreatedBy { get; set; }

        //[DataMember]
        //public DateTime CreatedDate { get; set; }

        [DataMember]
        public string DevicePointsAffected { get; set; }

        [DataMember]
        public string DepartmentZonesAffected { get; set; }

        [DataMember]
        public List<TFSBPermitDetail> ILSMRequiredChecklist { get; set; }

        [DataMember]
        public List<TFSBPermitDetail> BypassEngineeringActions { get; set; }

        [DataMember]
        public List<TFSBPermitDetail> BypassReturnEngineeringActions { get; set; }

        [DataMember]
        public List<TFSBPBuildingDetails> TFSBPBuildingDetails { get; set; }


        [DataMember]
        public DateTime? BypassEngActionDate { get; set; }

        [DataMember]
        public TimeSpan? BypassEngActionTime { get; set; }

        public string ByEngActionTime { get; set; }

        [DataMember]
        public int? BypassEngActionSignId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSBypassEngActionSign { get; set; }

        [DataMember]
        public DateTime? BypassReturnEngActionDate { get; set; }

        [DataMember]
        public TimeSpan? BypassReturnEngActionTime { get; set; }

        public string ByReturnEngActionTime { get; set; }

        [DataMember]
        public int? BypassReturnEngActionSignId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSBypassReturnEngActionSign { get; set; }

        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string AttachFiles { get; set; }
        [DataMember]
        public string TDrawingFields { get; set; }
        [DataMember]
        public string TFileIds { get; set; }
        [DataMember]
        public List<DrawingpathwayFiles> DrawingAttachments { get; set; }
        public List<TFiles> Attachments { get; set; }
        public TIcraProject TIcraProject { get; set; }

        public override PermitType GetPermitType()
        {
            return PermitType.FireSystemBypassPermit;
        }

        public TFireSystemByPassPermit()
        {
            BypassEngineeringActions = new List<TFSBPermitDetail>();
            BypassReturnEngineeringActions = new List<TFSBPermitDetail>();
            ILSMRequiredChecklist = new List<TFSBPermitDetail>();
            Project = new TIcraProject();
            RequestorByUser = new UserProfile();
            DrawingAttachments = new List<DrawingpathwayFiles>();
            Attachments = new List<TFiles>();
        }
    }


    [DataContract]
    public class FSBPForms
    {
        [DataMember]
        public int FSBPFormId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public int? Type { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

    }


}