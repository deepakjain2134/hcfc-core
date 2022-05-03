using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCF.BDO
{
    public class TaggedMaster
    {
        [DataMember][Key]
        public int Id { get; set; }

        [DataMember]
        public Guid TaggedId { get; set; }


        [DataMember]
        public string ActionTaken { get; set; }


        [DataMember]
        public string Notify { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public bool IsRequired { get; set; }

        [DataMember]
        public Guid? ActivityId { get; set; }


        [DataMember]
        public string PermitNo { get; set; }

        public string selectedDeficiencies { get; set; }

        [DataMember]
        public int TaggedType { get; set; }

        [DataMember]
        public int MainTaggedType { get; set; }

        [DataMember]
        public int Createdby { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public string AssetName { get; set; }

        [DataMember]
        public string AssetNo { get; set; }

        [DataMember]
        public string ActionName { get; set; }

        [DataMember]
        public string NotifyName { get; set; }

        [DataMember]
        public string CreatedName { get; set; }

        [DataMember]
        [NotMapped]
        public UserProfile CreatedByUser { get; set; }


        [DataMember]
        public string ActionToRemoveUser { get; set; }

        [DataMember]
        public string NotifyToRemoveUser { get; set; }

        [NotMapped]
        public List<UserProfile> allUsers { get; set; }

        public List<TaggedResource> Resource { get; set; }

        public string ActiontakenEmail
        {
            get => this.Resource.Count > 0 ? string.Join(",", Resource.Where(x => x.TaggedType == 1 && x.TaggedId == TaggedId).Select(x => x.Email)) : string.Empty;
            set { }
        }

        public string NotifyEmail
        {
            get => this.Resource.Count > 0 ? string.Join(",", Resource.Where(x => x.TaggedType == 0 && x.TaggedId == TaggedId).Select(x => x.Email)) : string.Empty;
            set { }
        }

        public int? IssueId { get; set; }
        public TaggedMaster()
        {
            Resource = new List<TaggedResource>();
            TaggedId = Guid.NewGuid();
        }

    }


    [DataContract]
    public class TaggedResource
    {
        [DataMember] public int TaggedResourceId { get; set; }

        [DataMember] public Guid TaggedId { get; set; }

        [DataMember] public string Email { get; set; }

        [DataMember] public int? UserId { get; set; }

        [DataMember] public int? VendorId { get; set; }

        [DataMember] public bool IsActive { get; set; }

        [DataMember] public int TaggedType { get; set; }

        [DataMember] public Guid? ActivityId { get; set; }

        [DataMember] public UserProfile TaggedUser { get; set; }

        [DataMember]  public int? IssueId { get; set; }

    }

}