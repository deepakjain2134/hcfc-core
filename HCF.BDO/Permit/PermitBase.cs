using HCF.BDO.Enums;
using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public abstract class PermitBase
    {
        [DataMember]
        public string PermitNo { get; set; }

        [DataMember]
        public int ProjectId { get; set; }

        [DataMember]
        public TIcraProject Project { get; set; }

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
                return string.Empty;
        }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        public abstract PermitType GetPermitType();

        [DataMember]
        public string Reason { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Company { get; set; }

        [DataMember]
        public DateTime? ApprovedDate { get; set; }

        public PermitEmailModel GetPermitEmailModel(PermitType permitType, string requesterUser, DateTime? requestedDate, DateTime? approvedDate,
            int approvalStatus, string approvedByUser, string isRequestEdited,string company)
        {
            var objPermitEmailModel = new PermitEmailModel
            {
                PermitType = permitType,
                PermitNo = !string.IsNullOrEmpty(PermitNo)? PermitNo : string.Empty,
                ProjectName = Project != null ? Project.ProjectName : string.Empty,
                ProjectNo = Project.ProjectNumber,
                Requestor = requesterUser,
                RequestedDate = requestedDate,
                ApprovedDate = approvedDate,
                Approver = approvedByUser,
                ApprovalStatus = approvalStatus,
                Reason = Reason,
                RequesterEmail = Email,
                Company = company,
                IsRequestEdited = isRequestEdited,
                EventId = approvalStatus == 1 ? Convert.ToInt32(Enums.NotificationEvent.OnApproved)
                : Convert.ToInt32(Enums.NotificationEvent.OnSubmit)
            };
            return objPermitEmailModel;
        }
    }
}
