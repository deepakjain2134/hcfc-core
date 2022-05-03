using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class TExercises
    {

        [DataMember]
        public string NearBy { get; set; }

        [DataMember]
        [Key]
        public int TExerciseId { get; set; }

        [DataMember]
        public int ShiftId { get; set; }

        [DataMember]
        public int? BuildingId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int QuarterId { get; set; }

        // [DataMember]
        public TimeSpan? Time { get; set; }

        [DataMember]
        public int? Status { get; set; }

        [DataMember]
        public bool Announced { get; set; }

        //[DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        //[DataMember]
        public UserProfile CreatedByUser { get; set; }

        public DateTime CreatedDate { get; set; }

        [DataMember]
        public DateTime? Date { get; set; }

        [DataMember]
        //public long DateTimeSpan { get; set; }

        public long DateTimeSpan
        {
            get; set;
        }



        [DataMember]
        public string StartTime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TExerciseFiles> TExerciseFiles { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TExerciseQuestionnaires> TExerciseQuestionnaires { get; set; }

        [DataMember]
        public List<DigitalSignature> DigitalSignature { get; set; }

        [DataMember]
        public string SignIds { get; set; }

        [DataMember]
        public QuarterMaster QuarterMaster { get; set; }

        [DataMember]
        public Buildings Building { get; set; }


        [DataMember]
        public int DrillType
        {
            get; set;
        }

        [DataMember]
        public FireDrillTypes FireDrillType { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string EducationComment { get; set; }

        [DataMember]
        public string CritiquesComment { get; set; }

        [DataMember]
        public string Observers { get; set; }

        [DataMember]
        public string ConductedBy { get; set; }

        [DataMember]
        public List<TExerciseActions> TExerciseActions { get; set; }

        [DataMember]
        public DateTime? CritiqueDate { get; set; }

        [DataMember]
        public long CritiqueDateTimeSpan { get; set; }

        [DataMember]
        public DateTime? EducationDate { get; set; }

        [DataMember]
        public long EducationDateTimeSpan { get; set; }

        [DataMember]
        public List<DigitalSignature> CritiqueDigitalSignature { get; set; }

        [DataMember]
        public string CritiqueSignIds { get; set; }

        [DataMember]
        public List<DigitalSignature> EducationDigitalSignature { get; set; }

        [DataMember]
        public string EducationSignIds { get; set; }

        [DataMember]
        public int FireDrillMode { get; set; }

        [DataMember]
        public Shift Shift { get; set; }

        [DataMember] public bool IsAdditional { get; set; }

        [DataMember]
        public int? Year { get; set; }

        [DataMember]
        public int QuarterNo { get; set; }

        [DataMember]
        public bool IsAudible { get; set; }

        [DataMember]
        public int FiredrillDocStatus { get; set; }

        [DataMember]
        public int? LocationGroupId { get; set; }

        [DataMember]
        public string LocationGroupBuildingIds { get; set; }

        public TExercises()
        {
            DigitalSignature = new List<DigitalSignature>();
            CritiqueDigitalSignature = new List<DigitalSignature>();
            EducationDigitalSignature = new List<DigitalSignature>();
            TExerciseQuestionnaires = new List<TExerciseQuestionnaires>();
            TExerciseFiles = new List<TExerciseFiles>();
            TExerciseActions = new List<TExerciseActions>();
        }
    }


    [DataContract]
    public class TExerciseQuestionnaires
    {
        [DataMember]
        [Key]
        public int TExerciseQuestId { get; set; }

        [DataMember]
        public int FireDrillQuesId { get; set; }

        [DataMember]
        public int TExerciseId { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public FireDrillQuestionnaires FireDrillQuestionnaires { get; set; }

        [DataMember]
        public string Comments { get; set; }

        [DataMember] public int? Ratings { get; set; }

    }


    [DataContract]
    public class TExerciseActions
    {
        [DataMember]
        [Key]
        public int TExerciseActionId { get; set; }

        [DataMember]
        public int TExerciseId { get; set; }

        [DataMember]
        public string Issue { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

    }

}