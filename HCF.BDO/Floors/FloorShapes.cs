using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace HCF.BDO
{
    [DataContract]
    public class FloorShapes
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int? FloorId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Data { get; set; }
        [DataMember]
        public string XMin { get; set; }
        [DataMember]
        public string XMax { get; set; }
        [DataMember]
        public string YMin { get; set; }
        [DataMember]
        public string YMax { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public int? ParentId { get; set; }
        [DataMember]
        public int? CreatedBy { get; set; }

        // [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public List<TFloorAssets> tFloorAssets { get; set; }

        [DataMember]
        public string FloorShapeCode { get; set; }
    }
}