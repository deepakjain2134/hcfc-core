using HCF.BDO;
using System.Collections.Generic;
using System.Security.Principal;

namespace HCF.Web.Filters
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; }

        public bool IsInRole(string role)
        {           
            return true;
        }

        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }

        public int UserId { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string FullName { get; set; }
        public Group Group { get; set; }
    }

    public class CustomPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }     

        public Group Group { get; set; }
    }

    public class Group
    {
        public Group()
        {
            roles = new List<RolesInGroups>();
            userRoles = new List<Roles>();
        }
        public int groupId { get; set; }
        public string groupName { get; set; }
        public List<RolesInGroups> roles { get; set; }
        public List<Roles> userRoles { get; set; }
    }
}