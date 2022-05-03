using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    using System.ComponentModel.DataAnnotations;

    [DataContract]
    public class QuestionnaireTypes
    {
        [DataMember][Key] public int QuestionnaireTypeId { get; set; }

        [DataMember] public string Type { get; set; }

    }

    [DataContract]
    public class Questionnaire
    {
        [DataMember][Key]
        public int QuestionnaireId { get; set; }

        [DataMember]
        public int Sequence { get; set; }

        [DataMember]
        public int QuestionnaireTypeId { get; set; }

        [DataMember]
        public string Goal { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsHide { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public List<QuestionnaireSteps> QuestionnaireSteps { get; set; }
        public QuestionnaireSteps QuestionnaireStep { get; set; }

        public List<QuestionnaireOptions> QuestionnaireOptions { get; set; }

        public QuestionnaireTypes QuestionnaireType { get; set; }
        public string QuestHeaderTypeIds { get; set; }

        public Questionnaire()
        {
            QuestionnaireSteps = new List<QuestionnaireSteps>();
        }
    }

    [DataContract]
    public class QuestionnaireSteps
    {
        [DataMember]
        public int QuestionnaireStepId { get; set; }

        [DataMember]
        public int? QuestionnaireId { get; set; }

        [DataMember]
        public string Step { get; set; }

        [DataMember]
        public bool IsActive { get; set; }


        [DataMember]
        public bool IsDeleted { get; set; }

        //[DataMember]
        //public string RecommendedValue { get; set; }       

        [DataMember]
        public int CreatedBy { get; set; }

        //[DataMember]
        //public int? MinValue { get; set; }


        //[DataMember]
        //public int? MaxValue { get; set; }

        //[DataMember]
        //public string Format { get; set; }

        //[DataMember]
        //public string FormatName { get; set; }


        //[DataMember]
        //public string RecommendedText
        //{
        //    get
        //    {
        //        return SetRecommendedText(MinValue, MaxValue, RecommendedValue, Format);
        //    }
        //}

        //private string SetRecommendedText(int? minValue, int? maxValue, string recommendedValue, string format)
        //{
        //    if (!string.IsNullOrEmpty(recommendedValue))
        //        return recommendedValue;
        //    else if (minValue.HasValue && maxValue.HasValue)
        //        return string.Format("{0}-{1} {2}", minValue.Value, maxValue.Value, format);
        //    else if (minValue.HasValue)
        //        return string.Format("≤{0} {1}", minValue.Value, format);
        //    else if (maxValue.HasValue)
        //        return string.Format("≥{0} {1}", maxValue.Value, format);
        //    else
        //        return "NA";
        //}
        [DataMember]
        public string StepCode { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        public List<TPMLogSteps> PMLogSteps { get; set; }


        //public string InputValueHeader { get; set; }

        //public int? QuestionnaireHeaderTypeId { get; set; }

        [DataMember]
        public List<QuestionnaireStepDetail> QuestionnaireStepDetail { get; set; }

        public int Sequence { get; set; }

        public QuestionnaireSteps()
        {
            PMLogSteps = new List<TPMLogSteps>();
            QuestionnaireStepDetail = new List<QuestionnaireStepDetail>();
        }

    }


    [DataContract]
    public class QuestionnaireStepDetail
    {
        [DataMember]
        public int QuestionnaireStepDetailId { get; set; }

        [DataMember]
        public int QuestionnaireStepId { get; set; }

        [DataMember]
        public int? QuestionnaireId { get; set; }

        [DataMember]
        public string RecommendedValue { get; set; }

        [DataMember]
        public int MinValue { get; set; }

        [DataMember]
        public int MaxValue { get; set; }

        [DataMember]
        public string Format { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public string InputValueHeader { get; set; }

        [DataMember]
        public string InputValueHeaderText { get; set; }

        [DataMember]
        public int? QuestionnaireHeaderTypeId { get; set; }

        [DataMember]
        public string RecommendedText => SetRecommendedText(MinValue, MaxValue, RecommendedValue, Format);

        [DataMember]
        public int SequenceId { get; set; }

        [DataMember]
        public int InputType { get; set; }
        private static string SetRecommendedText(int minValue, int maxValue, string recommendedValue, string format)
        {
            if (!string.IsNullOrEmpty(recommendedValue))
                return recommendedValue;
            else if (minValue > 0 && maxValue > 0)
                return $"{minValue} - {maxValue} {format}";
            else if (minValue > 0)
                return $"≤{minValue} {format}";
            else if (maxValue > 0)
                return $"≥{maxValue} {format}";
            else
                return "N/A";
        }

    }




    [DataContract]
    public class QuestionnaireOptions
    {
        [DataMember]
        public int QuestionnaireHeaderTypeId { get; set; }

        [DataMember]
        public int QuestionnaireId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public QuestionnaireHeaderTypes QuestionnaireHeaderType { get; set; }
    }

    [DataContract]
    public class QuestionnaireHeaderTypes
    {
        [DataMember] public int QuestionnaireHeaderTypeId { get; set; }

        [DataMember] public string Name { get; set; }

        [DataMember] public bool IsActive { get; set; }

        [DataMember] public int OptionType { get; set; }

    }



    #region TBDO

    [DataContract]
    public class TPMLogs
    {
        [DataMember]
        public int PMLogId { get; set; }

        [DataMember]
        public int BuildingId { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public TimeSpan Time { get; set; }

        [DataMember]
        public string POTime { get; set; }

        [DataMember]
        public string Comments { get; set; }

        [DataMember]
        public string ActionTaken { get; set; }

        [DataMember]
        public int ReviewedBy { get; set; }

        [DataMember]
        public UserProfile ReviewedByUser { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        public List<Questionnaire> Questionnaires { get; set; }

        public UserProfile CreatedByUser { get; set; }

        public int CreatedBy { get; set; }

        public List<TPMLogSteps> LogSteps { get; set; }

        public int ParentLogId { get; set; }

        public bool IsCurrent { get; set; }

        public int Status { get; set; }

        [DataMember] public Buildings Building { get; set; }

        public TPMLogs()
        {
            Questionnaires = new List<Questionnaire>();
        }

    }

    [DataContract]
    public class TPMLogSteps
    {
        [DataMember]
        public int PMLogStepId { get; set; }

        [DataMember]
        public int? PMLogId { get; set; }

        [DataMember]
        public int? QuestionnaireHeaderTypeId { get; set; }

        [DataMember]
        public int? QuestionnaireStepId { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Comments { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

    }


    #endregion

    public enum Formats
    {
        HOURS,
        VOLTS,
        PSI,
        INCHES,
        Bottles,
        F,
    }
}