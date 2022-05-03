namespace HCF.BDO
{
    using System.ComponentModel.DataAnnotations;

    public class PopEmail
    {
        [Key]
        public int Id { get; set; }

        public string EmailId { get; set; }

        public string Password { get; set; }

        public string PopServer { get; set; }

        public string FilePath { get; set; }

        public bool IsActive { get; set; }

        public string DbName { get; set; }

        public string ClientNo { get; set; }

        public int Port { get; set; }

        public bool UseSSL { get; set; }
    }


    public class EmailFiles
    {
        public string fileName { get; set; }
        public string filePath { get; set; }
        public string mainId { get; set; }
    }
}