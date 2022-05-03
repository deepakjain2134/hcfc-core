using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace HCF.BDO
{
    [DataContract]
    public class DrawingpathwayFiles
    {
        [DataMember][System.ComponentModel.DataAnnotations.Key]
        public int ID { get; set; }


        [DataMember]
        public string FloorPlanId { get; set; }

        [DataMember]
        public string FloorName { get; set; }

        [DataMember]
        public string BuildingName { get; set; }

        [DataMember]
        public int BuildingID { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public string SiteCode { get; set; }
        [DataMember]
        public string BuildingCode { get; set; }
        [DataMember]
        public string FloorCode { get; set; }

        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string FullFileName { get; set; }
    }
    
}