namespace HCF.Utility.FileUpload
{
    public class UploadedFiles
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Extension { get; set; }
        public string MD5Digest { get; set; }
        public string ETag { get; set; }
    }
}
