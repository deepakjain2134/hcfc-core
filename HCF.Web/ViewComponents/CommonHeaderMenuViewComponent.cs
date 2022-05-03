using HCF.BAL;
using HCF.BDO;
using HCF.Module.Core.Extensions;
using HCF.Utility;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class CommonHeaderMenuViewComponent : ViewComponent
    {
        private readonly IOrganizationService _organizationService;
        private readonly IWorkContext _workContext;
        public CommonHeaderMenuViewComponent(IOrganizationService organizationService, IWorkContext workContext)
        {
            _organizationService = organizationService;
            _workContext = workContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var currentOrg = UserSession.CurrentOrg;
            var currentUser = UserSession.CurrentUser;

            var org = new Organization();
            //if (currentOrg != null && currentOrg.Orgkey != Guid.Empty)
            org = _organizationService.GetUserDashBoard(currentUser.UserId, 0);

            var lists = from BDO.Enums.MenuPermitAliasType e in Enum.GetValues(typeof(BDO.Enums.MenuPermitAliasType))
                        select new
                        {
                            Value = (int)e,
                            Text = e.GetDescription()
                        };

            if (!currentUser.IsPowerUser)
            {
                int permitparentid = Convert.ToInt32(org.Services.Where(x => x.Alias.Trim().ToLower() == "inspection_icra" && x.IsActive)
                    .Select(x => x.Id).FirstOrDefault());
                if (permitparentid > 0)
                {
                    foreach (var permit in org.Services)
                    {
                        if (permit.ParentId == permitparentid && permit.Alias.Trim().ToLower() != "icraproject_index" && permit.Alias.Trim().ToLower()
                            != "permit_paperpermit" && permit.Alias.Trim().ToLower() != "icra_icraprojectpermit" && permit.Alias.Trim().ToLower() != "permit_getallpermits")
                        {
                            int permittype = Convert.ToInt32(lists.Where(x => x.Text.ToLower() == permit.Name.ToLower()).Select(x => x.Value).FirstOrDefault());
                            bool Isfalse = _organizationService.IsPermitActiveByUserID(currentUser.UserId, permittype);
                            if (!Isfalse)
                            {
                                org.Services = org.Services.Where(x => x.Id != permit.Id).ToList();
                            }
                        }
                    }
                }
            }
            return await Task.FromResult(View(org));
        }
    }
}
