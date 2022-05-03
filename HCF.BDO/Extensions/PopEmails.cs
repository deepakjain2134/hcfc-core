using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class PopEmails
    {
        [DataMember]
        public int Id { get; set; }

        public string DbName { get; set; }


        [Display(Name = "Email")]
        [DataMember]
        public string EmailId { get; set; }

        [DataMember]
        public string Password { get; set; }

        [Display(Name = "Pop Server")]
        [DataMember]
        public string PopServer { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [Display(Name = "Status")]
        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int? ClientNo { get; set; }

        [DataMember]
        public int Port { get; set; }

        [Display(Name = "Use SSL")]
        [DataMember]
        public bool UseSSL { get; set; }
    }
}