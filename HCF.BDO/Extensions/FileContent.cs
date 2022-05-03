using System.Runtime.Serialization;

namespace HCF.BDO
{
    public class FileContents
    {
        [DataMember(EmitDefaultValue = false)]
        public string FileName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FilePath { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FileContent { get; set; }
    }  

}