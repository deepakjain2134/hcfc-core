using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HCF.BDO
{
    [DataContract]
    public class SearchFilter
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid FilterId { get; set; }
        
        [DataMember]
        public int SearchType { get; set; }

        [Display(Name = "Search Name")]
        [DataMember]
        public string FilterName { get; set; }

        [DataMember]
        public int OwnerID { get; set; }

        [DataMember]
        public bool IsSaveSearchData { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Asset #")]
        [DataMember(EmitDefaultValue = false)]
        public string DeviceNo { get; set; }

        [Display(Name = "Barcode #")]
        [DataMember(EmitDefaultValue = false)]
        public string SerialNo { get; set; }

        [Display(Name = "Building Name")]
        [DataMember(EmitDefaultValue = false)]
        public int? BuildingId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? AssetId { get; set; }

        public string SearchMode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TFloorAssets> TFloorAssets { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<EPDetails> EPDetails { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? StdescId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? EpDetailId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? ScoreId { get; set; }

        [DataMember]
        public List<SampleData> SampleData { get; set; }

        [DataMember]
        public string SearchTerm { get; set; }

        [DataMember]
        public string SearchField { get; set; }

        public SearchFilter() {
            TFloorAssets = new List<TFloorAssets>();
            EPDetails = new List<EPDetails>();
            SampleData = new List<SampleData>();
        }

    }


    public class SampleData
    {
        public int Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }

        public int Type { get; set; }

        public string IncidentNo { get; set; }

        public string BinderName { get; set; }

        public string AssetName { get; set; }

        public string AssetNo { get; set; }

        public string BarCodeNo { get; set; }
    }

}