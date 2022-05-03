using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class OrgServices
    {
        [DataMember] public int ClientNo { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember][Key] public int ClientMenuMapID { get; set; }

        [DataMember] public int? ModuleId { get; set; }

        [DataMember] public int? MenuID { get; set; }

        [DataMember] public int OrganizationKey { get; set; }

        [DataMember] public bool Status { get; set; }

        [DataMember] public int Type { get; set; }

        [DataMember] public int Createdby { get; set; }

        [DataMember] public DateTime Createddate { get; set; }

        [DataMember] public int ServiceMode { get; set; }

        [DataMember] public DateTime? TrialStartDate { get; set; }

        [DataMember] public DateTime? TrialEndDate { get; set; }

       

    }

}