using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models
{
    public class BuildingsApp
    {
        public BuildingsApp()
        {
            Floor = new List<FloorApp>();

        }



        [DataMember]
        [Key]
        public int BuildingId { get; set; }

        [DataMember]
        public int BuildingTypeId { get; set; }

        [DataMember]
        [DisplayName("Building Name")]
        public string BuildingName { get; set; }

        [DataMember]
        [DisplayName("Site Code")]
        public string SiteCode { get; set; }
        [DataMember]
        public int SiteId { get; set; }
        [DataMember]
        [DisplayName("Site Name")]
        public string SiteName { get; set; }

        [DataMember]
        public string Address { get; set; }


        [DisplayName("CityName")]
        public string CityName { get; set; }



        [DisplayName("StateCode")]
        public string StateCode { get; set; }

        [DisplayName("Location")]
        public string Location
        {
            get => SetLocation();
            set { }
        }

        //public CityMaster? City { get; set; }

        private string SetLocation()
        {
            if (!string.IsNullOrEmpty(CityName) && !string.IsNullOrEmpty(StateCode))
            {
                return string.Format("{0},{1}", CityName, StateCode);
            }
            else
                return string.Empty;
        }

        [DataMember]
        [DisplayName("Building User")]
        public int BuildingAssignUserId { get; set; }

        [DisplayName("Building User")]
        public string BuildingAssignUser { get; set; }




        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool SiteActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [DisplayName("Building Code")]
        [Required]
        public string BuildingCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<FloorApp> Floor { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public BuildingTypeApp BuildingType { get; set; }

    }


    [DataContract]
    public class BuildingTypeApp
    {
        [DataMember]
        [Key]
        public int BuildingTypeId { get; set; }

        [DataMember]
        public string Name { get; set; }

    }


    [DataContract]
    public class FloorApp
    {
        [DataMember]
        public int FloorId { get; set; }

        [DataMember]
        public Guid FloorPlanId { get; set; }

        [DataMember]
        [DisplayName("Floor Code")]
        [Required]
        public string FloorCode { get; set; }

        [DataMember]
        [Required]
        [DisplayName("Floor Name")]
        public string FloorName { get; set; }



        //[DataMember(EmitDefaultValue = false)]
        //public string Version { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        [DisplayName("Floor Plan")]
        [DataMember]
        public string ImagePath { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FileName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FileContent { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        [DisplayName("Floor #")]
        [Required]
        public string Alias { get; set; }

        [DataMember]
        public string AliasSequence { get; set; }

        [DataMember]
        public int FloorTypeId { get; set; }

        [DataMember]
        [DisplayName("Floor Type")]
        public FloorTypesApp FloorType { get; set; }
    }


    public class FloorTypesApp
    {
        [DataMember]
        [System.ComponentModel.DataAnnotations.Key]
        public int FloorTypeId { get; set; }

        [DataMember]
        public string FloorType { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        public string IconPath { get; set; }


    }


}