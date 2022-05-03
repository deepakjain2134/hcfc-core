using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class Binders
    {
        [DataMember]
        [Key]
        public int BinderId { get; set; }

        [DataMember]
        public int? ClientNo { get; set; }

        [DataMember]
        public string BinderCode { get; set; }

        [DataMember]
        [Required]
        [DisplayName("Binder Name")]
        public string BinderName { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FileName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FilePath { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FileContent { get; set; }

        [DataMember]
        public bool IsActive { get; set; }      

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public int? ParentBinderId { get; set; }

        [DataMember]
        public int IsPrimary { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        [NotMapped]
        public List<EPDetails> EpDetails { get; set; }

        [DataMember]
        [NotMapped]
        public List<StandardEps> StandardEps { get; set; }

        [DataMember]
        [NotMapped]
        public List<EPsDocument> EPdocument { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<EpBinder> EpBinders { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Binders> SubBinders { get; set; }

        public Binders()
        {            
            EpBinders = new List<EpBinder>();
            EpDetails = new List<EPDetails>();
            StandardEps = new List<StandardEps>();
        }
    }
}