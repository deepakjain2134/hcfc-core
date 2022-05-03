using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HCF.Infrastructure.Models;

namespace HCF.BDO
{
    [DataContract]
    public class UserLogin : EntityBase
    {
        public UserLogin()
        {
            LogOnDate = DateTime.UtcNow;
            LastActivityDateTime = DateTime.UtcNow;
            IsLogOn = true;
            IsCurrent = true;            
        }

        [NotMapped]
        public override long Id { get; protected set; }

        [Key]
        [DataMember]
        public long UserLoginId { get; set; }

        public string DeviceType { get; set; }

        [DataMember]
        //[ForeignKey(nameof(UserProfile))]
        public long UserId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string DeviceId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [ForeignKey(nameof(DeviceTypes))]
        public int DeviceTypeId { get; set; }

        [DataMember]
        public bool IsLogOn { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        [DataMember]
        public Guid? RefreshToken { get; set; }

        [DataMember]
        public DateTime LogOnDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? LogOffDate { get; set; }

        [DataMember]
        public string BuildVersion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ip { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string city { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string country_name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string organisation { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string OsName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BrowserName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime LastActivityDateTime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string LastLoginURL { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsNewDevice { get; set; }

        public string LoginProvider { get; set; }

        public Guid? OrgKey { get; set; }

        public string UserName { get; set; }       

        public virtual DeviceTypes DeviceTypes { get; set; }

        [DataMember]
        [NotMapped]
        public string DbName { get; set; }

        [DataMember]
        [NotMapped]
        public int ClientNo { get; set; }

    }
}