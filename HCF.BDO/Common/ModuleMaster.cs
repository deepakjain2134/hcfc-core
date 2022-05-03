using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class ModuleMaster
    {

        public ModuleMaster()
        {
            Menus = new List<Menus>();
        }
        [DataMember][Key] public int ModuleId { get; set; }

        [DataMember] public string ModuleName { get; set; }

        [DataMember] public string ModuleCode { get; set; }

        [DataMember] public string MenuIds { get; set; }

        [DataMember] public bool IsActive { get; set; }

        [DataMember] public int CreatedBy { get; set; }

        [DataMember] public DateTime CreatedDate { get; set; }

        [DataMember] public int Sequence { get; set; }

        [DataMember] public bool OrgModuleStatus { get; set; }

        [DataMember] public List<Menus> Menus { get; set; }

        [DataMember] public int OrgServiceMode { get; set; }

        [DataMember] public DateTime? TrialStartDate { get; set; }

        [DataMember] public DateTime? TrialEndDate { get; set; }

        [DataMember]
        public int? RemainingTrialDays
        {
            get => GetRemainingTrialDays();
            set { }
        }

        private int GetRemainingTrialDays()
        {
            if (TrialEndDate.HasValue)
            {
                return (TrialEndDate.Value - System.DateTime.Today).Days;
            }
            return 0;
        }
    }

}