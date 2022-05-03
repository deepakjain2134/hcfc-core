using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;



namespace HCF.BDO
{
    [DataContract]
    public class ICRAMatixPrecautions
    {
        [DataMember][Key]
        public int Id { get; set; }

        [DataMember]
        public int ConstructionClassId { get; set; }

        [DataMember]
        public int ConstructionRiskId { get; set; }

        [DataMember]
        public int ConstructionTypeId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int Version { get; set; }


        [DataMember]
        public ConstructionClass ConstructionClass { get; set; }

        [DataMember]
        public ConstructionType ConstructionType { get; set; }

        [DataMember]
        public ConstructionRisk ConstructionRisk { get; set; }

    }

}