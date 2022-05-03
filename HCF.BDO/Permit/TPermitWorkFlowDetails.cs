using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class TPermitWorkFlowDetails
    {
        public Int32 Id { get; set; }
        public Int32 FormSettingId { get; set; }

        public string  LabelType { get; set; }

        public Int32 Sequence { get; set; }

        public bool Required { get; set; }

        public string LabelText { get; set; }
        public string Comment { get; set; }
        public string PermitNumber { get; set; }
        public int? LabelValue { get; set; }
        public int? LabelSignId { get; set; }
        public int? PathId { get; set; }
        public DateTime? LabelSignDate { get; set; }
        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }
       
        public DigitalSignature DSPermitSignature { get; set; }
        public UserProfile UserDetail { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsNotifyEmail { get; set; }

        public PermitSetting PermitSetting { get; set; }

    }
}