using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class CopDetails
    {
        [DataMember]
        public int CopDetailsId { get; set; }

        [DataMember]
        public int CopStdescId { get; set; }

        [DataMember]
        public string RequirementName { get; set; }

        [DataMember]
        public string CopText { get; set; }

        [DataMember]
        public string EPTextID { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]

        public CopStdesc CopStdesc { get; set; }

        [DataMember]
        [NotMapped]
        public List<EPDetails> EPdetails { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CoPRequirement
        {
            get => getCoPRequirement();
            set { }
        }

        private string getCoPRequirement()
        {
            if (CopStdesc != null && !string.IsNullOrEmpty(CopStdesc.CopTitle))
            {
                return string.Format("{0}: {1}", CopStdesc.CopTitle.Split(':')[1], CopText);
            }
            return string.Empty;
        }
    }

}