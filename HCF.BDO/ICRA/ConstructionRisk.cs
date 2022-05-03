using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCF.BDO
{
    [DataContract]
    public class ConstructionRisk
    {
        [DataMember(EmitDefaultValue = false)]
        [Key]
        public int ConstructionRiskId { get; set; }

        [DisplayName("Construction Risk")]
        [DataMember(EmitDefaultValue = false)]
        public string RiskName { get; set; }

        [DisplayName("Group Name")]
        [DataMember(EmitDefaultValue = false)]
        public string GroupName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        [NotMapped]
        public List<ICRARiskArea> RiskArea { get; set; }

        public ConstructionRisk()
        {
            RiskArea = new List<ICRARiskArea>();
        }

    }

    [DataContract]
    public class ICRARiskArea
    {
        [DataMember]
        [Key]
        public int RiskAreaId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int ApprovalStatus { get; set; }

        [DataMember]
        public bool IsActive { get; set; }


        [DataMember]
        public bool IsSendEmail { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public bool IsEmpty { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public String RiskType { get; set; }

        [DataMember]
        public int RiskTypeID { get; set; }

        [DataMember]
        public int ConstructionRiskAreaId { get; set; }
        [DataMember]
        public int FireRiskType { get; set; }

        public List<ConstructionRisk> lstconstructionrisk { get; set; }
    }
}