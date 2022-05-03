using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HCF.BDO
{
    [DataContract]
    public class Buildings
    {
        public Buildings()
        {
            Floor = new List<Floor>();
            BuildingDetails = new BuildingDetails();
            Shifts = new List<Shift>();
            DrawingTypes = new List<FloorTypes>();
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

        [DataMember]
        [DisplayName("CityId")]
        public int? CityId { get; set; }

        [DisplayName("StateId")]
        public int? StateId { get; set; }

        [DisplayName("CityName")]
        public string CityName { get; set; }

        [DisplayName("StateName")]
        public string StateName { get; set; }

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


        public DateTime CreateDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool SiteActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [DisplayName("Building Code")]
        //  [Required]
        public string BuildingCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Floor> Floor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Shift> Shifts { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<FireDrillTypes> FireDrillTypes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public BuildingType BuildingType { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public UserProfile User { get; set; }

        ///* Added on 03/04/2020 START*/
        //public List<CityMaster> Cities { get; set; }
        //public List<StateMaster> States { get; set; }
        ///* Added on 03/04/2020 END*/
        ///

        [DataMember(EmitDefaultValue = false)]
        public List<FloorTypes> DrawingTypes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public BuildingDetails BuildingDetails { get; set; }
        public bool IsHavingFiredrill { get; set; }

        public int? LocationGroupId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool? IsInspectionDone { get; set; }
        public int? EPDetailId { get; set; }

    }

}