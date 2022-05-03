using HCF.BDO.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace HCF.BDO
{
    [DataContract]
    public class PermitEmailModel
    {
       

        public PermitType PermitType { get; set; }


        [DataMember]
        public string PermitNo { get; set; }

        [DataMember]
        public string ProjectName { get; set; }

        [DataMember]
        public string ProjectNo { get; set; }

        [DataMember]
        public string Reason { get; set; }

        [DataMember]
        public string PermitUrl { get; set; }

        [DataMember]
        public string RequesterEmail { get; set; }

        [DataMember]
        public DateTime? RequestedDate { get; set; }

        [DataMember]
        public string Requestor { get; set; }

        [DataMember]
        public int ApprovalStatus { get; set; }

        [DataMember]
        public DateTime? ApprovedDate { get; set; }

        [DataMember]
        public string Approver { get; set; }
        [DataMember]
        public string Company { get; set; }

        [DataMember]
        public string Reviewer { get; set; }
        [DataMember]
        public string ReviewerEmail { get; set; }
        [DataMember]
        public int? EventId { get; set; }
        [DataMember]
        public bool? SendMailToApprover { get; set; }

        [DataMember]
        public bool? SendMailToNextPhase{ get; set; }

        [DataMember]
        public string IsRequestEdited { get; set; }

        [DataMember]
        public string AdditionalNextLevelToEmails { get; set; }

        [DataMember]
        public string AdditionalNextLevelCCEmails { get; set; }

    }
}