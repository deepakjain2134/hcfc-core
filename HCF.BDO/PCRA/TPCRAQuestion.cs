using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using HCF.BDO.Enums;

namespace HCF.BDO
{
    [DataContract]
    public class TPCRAQuestion
    {
        [DataMember][Key]
        [DisplayName("PCRA #")]
        public int TPCRAQuesId { get; set; }
        [DataMember]
        [DisplayName("PCRA #")]
        public string PCRANumber { get; set; }
        [DataMember]
        [DisplayName("Project Name")]
        public int ProjectId { get; set; }

        [DataMember]
        public int? ParentTPCRAQuesId { get; set; }

        [DataMember]
        [DisplayName("Sign1 date")]
        public DateTime? Sign1Date { get; set; }

        [DataMember]
        [DisplayName("Sign2 date")]
        public DateTime? Sign2Date { get; set; }

        [DataMember]
        public int? Sign1Name { get; set; }
        [DataMember]
        public UserProfile Sign1User { get; set; }
        [DataMember]
        public int? Sign2Name { get; set; }
        [DataMember]
        public UserProfile Sign2User { get; set; }
        [DataMember]

        public int? Sign1SignatureId { get; set; }

        [DataMember]
        public int? Sign2SignatureId { get; set; }
        [DataMember]
        public UserProfile RequestByUser { get; set; }
        [DataMember]
        public UserProfile ApproveByUser { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSSign1Signature { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSSign2Signature { get; set; }

        //[DataMember]
        //public int? CreateId { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public int TPCRAQuesNumber { get; set; }

        [DataMember]
        public TIcraProject Project { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        [DataMember]
        public List<QuestionPCRA> QuestionPCRA { get; set; }

        [DataMember]
        public List<TPCRAQuestionDetails> TPCRAQuestionDetails { get; set; }
        //[DataMember]
        //public List<TDepartmentNearConstruction> TDepartmentNearConstruction { get; set; }
        [DataMember]
        public TIcraLog TIcraLog { get; set; }

        [DataMember]
        public int TicraId { get; set; }

        [DataMember]
        public int RiskAssessmentType { get; set; }

        [DataMember]
        public int? ContractorSignatureId { get; set; }
        [DataMember]
        public int? ContractorId { get; set; }
        [DataMember]
        public DateTime? ContractorSignatureDate { get; set; }
        [DataMember]
        public UserProfile ContractorUser { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSContractorSignature { get; set; }

        [DataMember]
        public int? FacilityId { get; set; }
        [DataMember]
        public int? FacilitySignatureId { get; set; }
        [DataMember]
        public DateTime? FacilitySignatureDate { get; set; }
        [DataMember]
        public UserProfile FacilityUser { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSFacilitySignature { get; set; }

        [DataMember]
        public int? InfectionControlSignatureId { get; set; }
        [DataMember]
        public int? InfectionControlId { get; set; }
        [DataMember]
        public DateTime? InfectionControlSignatureDate { get; set; }
        [DataMember]
        public UserProfile InfectionControlUser { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSInfectionControlSignature { get; set; }


        [DataMember]
        public int? SafetyId { get; set; }
        [DataMember]
        public int? SafetySignatureId { get; set; }
        [DataMember]
        public DateTime? SafetySignatureDate { get; set; }
        [DataMember]
        public UserProfile SafetyUser { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSSafetySignature { get; set; }

        public List<EnumModel> CheckBoxRiskAssesmentType { get; set; }

        [DataMember]
        public DateTime? DateSubmitted { get; set; }
        [DataMember]
        public string BuildingId { get; set; }
        [DataMember]
        public string BuildingName { get; set; }
        [DataMember]
        public string FloorName { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string WorkDescription { get; set; }

        [DataMember]
        public int? RequestedBy { get; set; }
        [DataMember]
        public int ApprovalStatus { get; set; }

        [DataMember]
        public string RejectionReason { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string TDrawingFields { get; set; }

        [DataMember]
        public List<DrawingpathwayFiles> DrawingAttachments { get; set; }
        public List<TPermitWorkFlowDetails> TPermitWorkFlowDetails { get; set; }


        [DataMember]
        public bool IsPCRA { get; set; }
        public TIcraProject TIcraProject { get; set; }
        public List<UserProfile> lstUserProfile { get; set; }
        public TPCRAQuestion()
        {
            TIcraLog = new TIcraLog();
            TPCRAQuestionDetails = new List<TPCRAQuestionDetails>();
            DrawingAttachments = new List<DrawingpathwayFiles>();
            TPermitWorkFlowDetails = new List<TPermitWorkFlowDetails>();
        }

    }
}