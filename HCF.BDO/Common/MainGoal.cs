using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using HCF.BDO.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCF.BDO
{
    [DataContract]
    public class MainGoal
    {

        public MainGoal()
        {

            Steps = new List<Steps>();
            epDetails = new List<EPDetails>();
        }

        [DataMember]
        public string GoalUId { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        [DataMember(EmitDefaultValue = false)][Key]
        public int MainGoalId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int ActivityType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? EPDetailId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Goal { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Document { get; set; }

        [DataMember]
        public int? AssetId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int DoctypeId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? Type { get; set; }

        [DataMember]
        public int? FrequencyId { get; set; }
        //[DataMember]
        //public bool IsEPGoal { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int? InspectionDetailId { get; set; }

        public DateTime CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long CreatedDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(CreatedDate);
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        public List<Steps> Steps { get; set; }

        [DataMember]
        public Assets Asset { get; set; }

        [DataMember]
        public DocumentType DocumentType { get; set; }

        [DataMember]
        public int? FloorAssetId { get; set; }

        [DataMember]
        public int? ClientNo { get; set; }

        [NotMapped]
        [DataMember(EmitDefaultValue = false)]
        public List<EPDetails> epDetails { get; set; }

    }
}