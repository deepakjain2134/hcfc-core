using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class TFSBPBuildingDetails
    {
        [DataMember]
        public int TFSBPBuildingDetailId { get; set; }

        [DataMember]
        public int TFSBPermitId { get; set; }

        [DataMember]
        public int? BuildingId { get; set; }

        [DataMember]
        public string BuildingName { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public Buildings Buildings { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public string SiteBuildingName
        {
            get => GetSiteBuildingName();
            set { }
        }

        [DataMember]
        public string TFireSystemId { get; set; }
        [DataMember]
        public List<FireSystemType> fireSystemTypes { get; set; }
        private string GetSiteBuildingName()
        {
            if (Buildings != null)
                return string.Format("{0} - {1} [{2}] ", Buildings.SiteCode, BuildingName, Buildings.BuildingCode);
            else
                return "";
        }

    }

}