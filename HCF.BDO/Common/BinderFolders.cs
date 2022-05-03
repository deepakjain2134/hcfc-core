using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace HCF.BDO
{
    [DataContract]
    public class BinderFolders
    {
        [DataMember][Key] public int FolderId { get; set; }

        [DataMember] public string FolderName { get; set; }

        [DataMember] public string CommonName { get; set; }

        [DataMember] public bool IsActive { get; set; }

        [DataMember] public bool IsDeleted { get; set; }

        [DataMember] public bool IsPublic { get; set; }

        [DataMember] public int? TFileId { get; set; }

        [DataMember] public long FileSize { get; set; }

        [DataMember] public TFiles File { get; set; }

        [DataMember] public int? ParentFolderId { get; set; }

        [DataMember] public DateTime CreatedDate { get; set; }

        [DataMember] public DateTime? ValidUpTo { get; set; }

        [DataMember] public int CreatedBy { get; set; }

        [DataMember] public UserProfile CreatedByUserProfile { get; set; }

        [DataMember] public List<BinderFolders> ChildBinderFolders { get; set; }

        [DataMember]
        public bool IsSurveyPrepBinder { get; set; }

        [DataMember]
        public bool IsFolder
        {
            get { return TFileId.HasValue ? false : true; }
            set { }
        }
        public BinderFolders()
        {
            ChildBinderFolders = new List<BinderFolders>();
            File = new TFiles();
        }
    }

}