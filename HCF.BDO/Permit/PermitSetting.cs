using System.Collections.Generic;
using System.Runtime.Serialization;


namespace HCF.BDO
{
    [DataContract]
    public class PermitSetting
    {
        [DataMember]


        public int Id { get; set; }
        public string LabelType { get; set; }
        public int Sequence { get; set; }
        public bool Required { get; set; }
        public bool IsSendMail { get; set; }
        public int IsSendMailValue { get; set; }
        public int RequiredType { get; set; }
        public string LabelText { get; set; }
        public bool IsDeleted { get; set; }
        public int PermitType { get; set; }
        public int ControlType { get; set; }
        public int CreatedBy { get; set; }

        public int? Mode { get; set; }
        [DataMember]
        public string NotificationEmail { get; set; }

        [DataMember]
        public string NotificationCCEmail { get; set; }
        public int? PathId { get; set; }
        public string StepCode { get; set; }
        public string AdditionalDescription { get; set; }
        public bool? IsAssetDeviceChange { get; set; }
    

       
    }
}