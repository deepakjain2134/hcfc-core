using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class Tips
    {
        [DataMember][Key]
        public int TipId { get; set; }

        [DataMember]
        public int? ParentTipId { get; set; }

        [DataMember]
        [Required]
        public string Title { get; set; }

        [DataMember]
       
        [Required]
        public string Description { get; set; }

        [DataMember]
        [Required]
        public string Source { get; set; }

        [DataMember]
        public string RouteUrl { get; set; }

        [DataMember]
        public string RouteUrlPath { get; set; }

        [DataMember]
        public string RouteName { get; set; }

        [DataMember]
        public string ContributorName { get; set; }

        [DataMember]
        public string ContributorOrg { get; set; }

        [DataMember]
        public string ContributorPosition { get; set; }

        [DataMember]
        public bool ShowContributorName { get; set; }

        [DataMember]
        public bool ShowContributorOrg { get; set; }

        [DataMember]
        public bool ShowContributorPosition { get; set; }

        [DataMember]
        public int IsApproved { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        [Required]
        [DataMember]
        public Enums.TipTypes TipType { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public int? UpdatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public DateTime? UpdatedDate { get; set; }

        [DataMember]
        public int? ClientNo { get; set; }

        [DataMember]
        public Menus Services { get; set; }

    }
}