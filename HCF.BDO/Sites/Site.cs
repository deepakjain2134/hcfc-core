using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class Site : BaseEntity
    {
        [DataMember]
        [Key]
        public int SiteId { get; set; }

        [DataMember]
        public int SiteTypeId { get; set; }

        [Display(Name = "Site Name")]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string SiteName { get; set; }

        [Display(Name = "Site Type")]
        [DataMember]
        public string SiteTypeName { get; set; }

        [Display(Name = "Site Code")]
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }
        //[DataMember]
        //public string SiteDescription { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsInternal { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public int BuildingCount { get; set; }

        [DataMember]
        public List<Buildings> Buildings { get; set; }

        [DataMember]
        public int? CityId { get; set; }

        [DataMember]
        public int? StateId { get; set; }
        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public string StateName { get; set; }
        [DataMember]
        public string Address { get; set; }
        public Site()
        {
            Buildings = new List<Buildings>();
        }

    }
}