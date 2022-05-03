using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{

    [DataContract]
    public class RoundCategory
    {
        // [DataMember]
        public string FrequencyIds { get; set; }

        [DataMember]
        [Key]
        public int RoundCatId { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        // [DataMember]
        public int? AssetId { get; set; }

        // [DataMember]
        public int CreatedBy { get; set; }

        // [DataMember]
        public DateTime CreatedDate { get; set; }

        // [DataMember]
        public Enums.Frequency FrequencyId { get; set; }

        // [DataMember]
        public string Date { get; set; }

        // [DataMember]
        public bool IsCommonCat { get; set; }

        //[DataMember]
        public string Applicable { get; set; }

        [DataMember]
        public List<RoundsQuestionnaires> RoundItems { get; set; }

        public RoundCategory()
        {
            RoundItems = new List<RoundsQuestionnaires>();
        }
    }

}