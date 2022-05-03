using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class EPAssignee : BaseEntity
    {
        [DataMember]
        [Key]
        public int AssigneeId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public List<int> UserIds { get; set; }

        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public int? AssetsTypeId { get; set; }

        [DataMember]
        [NotMapped]
        public int? AssetId { get; set; }

        [DataMember]
        public int? DocTypeId { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public UserProfile UserProfile { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public EPDetails EPDetails { get; set; }

        [DataMember]
        [NotMapped]
        public string EpDetailIDs { get; set; }

        //[DataMember]
        //[NotMapped]
        //public string Assets_CSV { get; set; }

        //[DataMember]
        //[NotMapped]
        //public string Document_CSV { get; set; }

        //[DataMember]
        //[NotMapped]
        //public string UserName_CSV { get; set; }

        //[DataMember]
        //[NotMapped]
        //public string UserId_CSV { get; set; }

        [NotMapped]
        public StandardEps StandardEps { get; set; }

    }

    public class EPUsers
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public string EPDetailIds { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public bool Status { get; set; }
    }

    public class UsersEPCount
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public int EpCount { get; set; }
    }
}