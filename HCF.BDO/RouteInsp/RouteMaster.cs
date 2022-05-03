using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class RouteMaster
    {
        [DataMember][Key]
        public int RouteId { get; set; }

        [DataMember]
        public int? FloorId { get; set; }

        [DataMember]
        public int? BuildingID { get; set; }

        [DataMember]
        [DisplayName("Route #")]
        public string RouteNo { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsDefault { get; set; }        

        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public Floor Floor { get; set; }

        [DataMember]
        public List<StopsRouteMapping> StopsRouteMapping { get; set; }

        [DataMember]
        public int? LocationId { get; set; }

        [DataMember]
        public int? StopId { get; set; }

        [DataMember]
        public StopMaster Stop { get; set; }

        [DataMember]
        public int AssetCounts { get; set; }

        [DataMember]
        public int StopCount { get; set; }

        public List<Assets> Assets { get; set; }

        public RouteMaster()
        {
            Assets = new List<Assets>();
        }
    }


    [DataContract]
    public class StopsRouteMapping
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int? StopId { get; set; }

        [DataMember]
        public int? RouteId { get; set; }

        [DataMember]
        public int? Sequence { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public RouteMaster Route { get; set; }

        [DataMember]
        public StopMaster Stops { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

    }
}