using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using HCF.BDO.Extensions;

namespace HCF.BDO
{
    [DataContract]
    public class Steps
    {
        public Steps()
        {
            IlsmStepsMapping = new List<IlsmStepsMapping>();
            StepsComments = new List<TComment>();
        }

        [DataMember]
        public string GoalUId { get; set; }        

        [DataMember][Key]
        public int StepsId { get; set; }

        [DataMember]
        public int MainGoalId { get; set; }

        [DataMember]
        public int? Sequence { get; set; }

        [DataMember]
        public string Step { get; set; }

        [DataMember]
        public string Comments { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FileContent { get; set; }

        [DataMember]
        public long CreatedBy { get; set; }

        [DataMember]
        public bool IsRA { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int? StepType { get; set; }

        [DataMember]
        public string InputValue { get; set; }

        [DataMember]
        public bool? Ilsm { get; set; }

        [DataMember]
        public int RAScore { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public int Ischecked { get; set; }

        [DataMember]
        public int? FileId { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        [DataMember]
        public bool IsMarkDefeciencies { get; set; }

        public DateTime? CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long CreatedDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(CreatedDate);
            set { }
        }

        [DataMember]
        public string DeficiencyStatus { get; set; }

        [DataMember]
        public string DeficiencyCode { get; set; }

        [DataMember]
        public int? DRTime { get; set; }

        [DataMember]
        public List<InspectionSteps> inspectionsteps { get; set; }

        [DataMember]
        public bool IsIlsmLink { get; set; }

        [DataMember]
        public int TrigEpCount { get; set; }


        [DataMember]
        public int? AddedBy { get; set; }

        [DataMember]
        public DateTime? AddedDate { get; set; }


        [DataMember]
        public List<IlsmStepsMapping> IlsmStepsMapping { get; set; }      

        [DataMember]
        public List<TComment> StepsComments { get; set; }

        [DataMember]
        public List<TInspectionFiles> TInspectionFiles { get; set; }

        public MainGoal MainGoal { get; set; }

    }

    [DataContract]
    public class IlsmStepsMapping
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int StepsId { get; set; }

        [DataMember]
        public int ILsmStepsId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}