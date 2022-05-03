using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HCF.Web.Models
{

    public class LoginModel
    {
        public LoginModel()
        {
            //UserOrganization = new List<BDO.Organization>();
        }

        [Required]
        [Display(Name = "User name")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public int? dType { get; set; }

        public string dToken { get; set; }

        //public string ip { get; set; }

        public string LoginMode { get; set; }

        //public List<BDO.Organization> UserOrganization { get; set; }
    }

    public class RecentLoginModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string FilePath { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public System.DateTime LoginDate { get; set; }
        public string PassCode { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public bool IsPassCodeLogin { get; set; }

    }

    public class CRxForumLoginModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public int Status { get; set; }

        public string EmailAddress { get; set; }

        public string RefreshToken { get; set; }
    }
}
