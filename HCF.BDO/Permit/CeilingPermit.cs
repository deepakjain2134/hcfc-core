using HCF.BDO.Enums;
using HCF.BDO.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class CeilingPermit : PermitBase
    {
        [DataMember]
        public int CeilingPermitId { get; set; }
        
        //[DataMember]
        //public string PermitNo { get; set; }
       
        //[DataMember]
        //public string Email { get; set; }
       
        [DataMember(EmitDefaultValue = false)]
        public DateTime? EffectiveDate { get; set; }

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public int? Pages { get; set; }

        [DataMember]
        public string WorkArea { get; set; }

        [DataMember]
        public string Scope { get; set; }

        //[DataMember]
        //public int ProjectId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? StartDate { get; set; }

        [DataMember]
        public TimeSpan? StartTime { get; set; }

        //[DataMember]
        public string Stime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? CompletionDate { get; set; }

        [DataMember]
        public TimeSpan? EndTime { get; set; }

        public string Etime { get; set; }

        [DataMember]
        public string Responsible { get; set; }

        [DataMember]
        public string TypeofSealant { get; set; }

        [DataMember]
        public bool ULApproved { get; set; }

        [DataMember]
        public int? Requestedby { get; set; }

        [DataMember]
        public UserProfile RequestedbyUser { get; set; }

        [DataMember]
        public string RequestedDept { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string PhoneNo { get; set; }

        [DataMember]
        public DateTime? RequestedDate { get; set; }

        [DataMember]
        public int? AdditionalItems { get; set; }

        [DataMember]
        public string ApproverId { get; set; }

        [DataMember]
        public int? ApproversignId { get; set; }


        [DataMember]
        public int? IsPenetrationStructure { get; set; }

        [DataMember]
        public int? IsPenetrationsVerified { get; set; }

        [DataMember]
        public int? RequesterSignId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSRequesterSign { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public DateTime? ApprovedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSApproverSign { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public bool HasICRA { get; set; }

        [DataMember]
        public bool IsVerified { get; set; }

        [DataMember]
        public int? FinalInspectionBy { get; set; }

        [DataMember]
        public UserProfile FinalInspectionByUser { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? InspectionDate { get; set; }

        //[DataMember]
        //public DateTime CreatedDate { get; set; }

        //[DataMember]
        //public int CreatedBy { get; set; }

        public List<CeilingPermitDetail> CeilingPermitDetail { get; set; }

        public List<TCeilingPermitDetail> TCeilingPermitDetail { get; set; }
        [DataMember]
        public DigitalSignature Approversign { get; set; }
        //[DataMember]
        //public TIcraProject Project { get; set; }
        public long CreatedDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(CreatedDate);
            set { }
        }

        //[DataMember]
        //public string Reason { get; set; }

        [DataMember]
        public List<TFiles> Attachments { get; set; }
        [DataMember]
        public string AttachDrawingFiles { get; set; }
        [DataMember]
        public string TFileIds { get; set; }
        [DataMember]
        public string TDrawingFields { get; set; }
        [DataMember]
        public string TDrawingViewerId { get; set; }

        [DataMember]
        public string AttachFiles { get; set; }

        [DataMember] public string BuildingId { get; set; }
        [DataMember] public string BuildingName { get; set; }

        [DataMember] public string FloorId { get; set; }
        [DataMember] public string FloorName { get; set; }

        [DataMember] public string Zones { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string TempPermitNumber { get; set; }
        [DataMember]
        public List<DrawingpathwayFiles> DrawingAttachments { get; set; }

        [DataMember]
        public List<TaggedMaster> TaggedMaster { get; set; }

        public bool IsTaggingEnabled { get; set; }

        public TIcraProject TIcraProject { get; set; }
        public List<TPermitWorkFlowDetails> TPermitWorkFlowDetails { get; set; }


        public override PermitType GetPermitType()
        {
            return PermitType.CeilingPermit;
        }

        public CeilingPermit()
        {
            Attachments = new List<TFiles>();
            TCeilingPermitDetail = new List<TCeilingPermitDetail>();
            TPermitWorkFlowDetails = new List<TPermitWorkFlowDetails>();
            DrawingAttachments = new List<DrawingpathwayFiles>();
            TaggedMaster = new List<TaggedMaster>();
        }

    }

    [DataContract]
    public class CeilingPermitDetail
    {
        [DataMember]
        public int CeilingPermitDetailId { get; set; }

        [DataMember]
        public int CeilingPermitId { get; set; }

        [DataMember]
        public DateTime? RevisedDate { get; set; }

        [DataMember]
        public DateTime? ReviewedDate { get; set; }

        [DataMember]
        public int? PermitReviewBy { get; set; }

        [DataMember]
        public int? PermitReviewBySignId { get; set; }

        [DataMember]
        public int? UpdatedBy { get; set; }

        [DataMember]
        public DateTime? UpdatedDate { get; set; }

    }

    [DataContract]
    public class TPermitLinkForms
    {
        [DataMember]
        public int TFormId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool? IsCPActive { get; set; }

        [DataMember]
        public bool? IsLSDActive { get; set; }

        [DataMember]
        public int? Type { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }



    }
}