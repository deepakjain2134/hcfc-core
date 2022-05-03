using HCF.BAL;
using HCF.BDO;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class CommonuserCustomddlViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        public CommonuserCustomddlViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string type)
        {
            var users = new List<UserProfile>();
            if (UserSession.CurrentOrg.IsTmsWo)
                users = _userService.GetUsers().Where(x => x.ResourceNumber != null && x.IsActive).OrderBy(x => x.FullName).ToList();
            else
                users = _userService.GetUsers().Where(x => x.IsActive).OrderBy(x => x.FullName).ToList();

            ViewBag.type = type;
            return await Task.FromResult(View(users));
        }
    }
}
