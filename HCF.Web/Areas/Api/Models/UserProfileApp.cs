using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models
{
    public class UserProfileApp
    {
        [DataMember]
        [Key]
        public int UserId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [DataMember]
        public Guid Orgkey { get; set; }



        [DataMember]
        public bool IsCRxUser { get; set; }








        [DataMember(EmitDefaultValue = false)]
        public string Email { get; set; }



        [DataMember(EmitDefaultValue = false)]
        public string FileName { get; set; }




        [DataMember(EmitDefaultValue = false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public string FullName
        {
            get => (FirstName ?? string.Empty) + " " + (!string.IsNullOrEmpty(LastName) ? LastName.Substring(0, 1) : string.Empty);
            set { }
        }






        [DataMember]
        public bool IsInternalUser { get; set; }





        [DataMember]
        public string FilePath { get; set; }



        [DataMember]
        public bool IsSystemUser { get; set; }

        [DataMember]
        public Guid UserProfileId { get; set; }

        [DataMember]
        public bool IsVerified { get; set; }



        [DataMember]
        public bool IsPwdChange { get; set; }

        [DataMember]
        [NotMapped]
        public int TotalOrgCount { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public IEnumerable<OrganizationApp> Organizations { get; set; }



        [DataMember(EmitDefaultValue = false)]
        public IEnumerable<UserLoginApp> UserLogin { get; set; }

        [DataMember]
        [Display(Name = "Name")]
        public bool IsActive { get; set; }






        public UserProfileApp()
        {

            UserLogin = new List<UserLoginApp>();


        }




    }


    public class UserDetaileApp
    {
        [DataMember]
        [Key]
        public int UserId { get; set; }

        [DataMember]
        public string Name
        { get; set; }
        [DataMember]
        public string FullName
          { get; set; }

    }
        [DataContract]
    public class UserLoginApp
    {
        [DataMember(EmitDefaultValue = false)]
        [Key]
        public int UserLogOnId { get; set; }
        [DataMember]
        public Guid? RefreshToken { get; set; }
    }



    [DataContract]
    public class OrganizationApp
    {


        public string Code { get; set; }



        [DataMember(EmitDefaultValue = false)]
        [Key]
        public int OrganizationId { get; set; }

        [DataMember]
        public int ClientNo { get; set; }

        [DataMember]
        public Guid Orgkey { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string City { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string State { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Country { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Zip { get; set; }


        [DataMember]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }



        [DataMember(EmitDefaultValue = false)]
        public UserProfileApp User { get; set; }





        [DataMember]
        public string DashBoadImagePath { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FilesContent { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public string LogoPath { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string LogoName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string LogoBase64 { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CRxInboxMailId { get; set; }


        public string LogoUrl => GetLogoUrlPath();

        private string GetLogoUrlPath()
        {
            var imageBasepath = "";//AppSettings.CommonFileBasePath;
            return !string.IsNullOrEmpty(LogoPath) ? $"{imageBasepath}{LogoPath.Replace("~/", string.Empty)}" : string.Empty;
        }

        [DataMember(EmitDefaultValue = false)]
        public string DbName { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public Guid? ParentOrgKey { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Address
        {
            get => (City + ", ") + (State + " ") + Zip + " ";
            set { }
        }






        public DateTime? LastWoTmsSync { get; set; }







    }

}