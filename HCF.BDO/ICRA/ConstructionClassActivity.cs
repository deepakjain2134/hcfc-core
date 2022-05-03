using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace HCF.BDO
{
    [DataContract]
    public class ConstructionClassActivity
    {
        [DataMember]
        [Key]
        public int ConstClassActivityId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? ConstClassId { get; set; }

        [DisplayName("Class Category")]
        [DataMember(EmitDefaultValue = false)]
        public int ConstClassCatId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Activity { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ConstructionClassCategrory ConstructionClassCategrory { get; set; }

    }

    public class ConstructionClassCategrory
    {
        [DataMember]
        [Key]
        public int ConstClassCatId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Categrory { get; set; }
    }
}