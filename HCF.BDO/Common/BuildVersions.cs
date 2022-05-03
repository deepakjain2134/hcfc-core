using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using HCF.BDO.Extensions;

namespace HCF.BDO
{
    [DataContract]
    public class BuildVersion : BaseEntity
    {
        [DataMember]
        [Key]
        public int Id { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsMajorRelease { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }
               
        public DateTime ReleaseDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long ReleaseDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(ReleaseDate);
            set { }
        }

         [DataMember]
        public string BasePath {

            get => "";// AppSettings.CommonApiUrl;
            set { }
        }

        [DataMember]
        public string BuildPath
        {

            get => SetBuildPath(BasePath,FilePath);
            set { }

        }

        private string SetBuildPath(string basePath, string filePath)
        {
            var result = $"{basePath}{filePath.Replace("~/", string.Empty)}";
            Uri.TryCreate(result, UriKind.Absolute, out var newUrl);
            return newUrl.ToString();
        }
    }

}