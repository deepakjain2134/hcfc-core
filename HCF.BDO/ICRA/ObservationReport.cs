using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using static System.String;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class ICRAObsReportCheckPoints
    {
        [DataMember]
        [Key]
        public int ICRAReportPointId { get; set; }

        [DataMember]
        public string CheckPoints { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

    }



    [DataContract]
    public class TICRAReports
    {
        [DataMember]
        [Key]
        public int TICRAReportId { get; set; }

        [DataMember]
        public string PermitNumber { get; set; }

        [DataMember]
        public DateTime ReportDate { get; set; }

        [DataMember]
        public string ReportName { get; set; }


        [DataMember]
        public int? TICRAId { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string ContractorSign { get; set; }

        [DataMember]
        public int? ContractorId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile ContractorUser { get; set; }

        [DataMember]
        public int? ContractorSignId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSContractorSignId { get; set; }

        [DataMember]
        public DateTime? ContractorSignDate { get; set; }

        [DataMember]
        public string ContractorSignTime { get; set; }

        [DataMember]
        public string ObserverSign { get; set; }

        [DataMember]
        public int? ObserverId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile ObserverUser { get; set; }

        [DataMember]
        public int? ObserverSignId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSObserverSignId { get; set; }

        [DataMember]
        public DateTime? ObserverSignDate { get; set; }

        [DataMember]
        public string ObserverSignTime { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        public string ProjectIds { get; set; }

        [DataMember]
        public List<TICRAReportsCheckPoints> CheckPoints { get; set; }

        public int ReportStatus => CheckStatus();

        public string ReportNameText => IsNullOrEmpty(ReportName) ? "NA" : ReportName;

        private int CheckStatus()
        {
            int status = 2;
            if (!IsNullOrEmpty(ObserverSign) && !IsNullOrEmpty(ContractorSign))
            {
                status = 1;
            }
            else if (IsNullOrEmpty(ObserverSign) && IsNullOrEmpty(ContractorSign))
            {
                status = 0;
            }
            return status;
        }

        public List<TICRAReports> RelatedReports { get; set; }

        public TICRAReports ProjectReport { get; set; }

        public List<TIcraProject> Projects { get; set; }

        public string ProjectNames { get; set; }

        public string ProjectNos { get; set; }

        public string ProjectLocations { get; set; }

        public string ProjectContractors { get; set; }


        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string ReportPath { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FilesContent { get; set; }

        public int? TIlsmId { get; set; }

        public TICRAReports()
        {
            CheckPoints = new List<TICRAReportsCheckPoints>();
            DSContractorSignId = new DigitalSignature();
            DSObserverSignId = new DigitalSignature();
        }

    }


    [DataContract]
    public class TICRAReportsCheckPoints
    {
        [DataMember] 
        [Key]
        public int Id { get; set; }

        [DataMember]
        public int TICRAReportId { get; set; }

        [DataMember] public int? ICRAReportPointId { get; set; }

        [DataMember] public int? Status { get; set; }

        [DataMember] public bool? IsActive { get; set; }

        [DataMember] public string CheckPointText { get; set; }

        [DataMember]
        public ICRAObsReportCheckPoints CheckPoints { get; set; }

    }

}