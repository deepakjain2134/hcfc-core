using HCF.BAL;
using HCF.BDO;
using HCF.Module.Core.Services;
using HCF.Module.Core.Areas.Core.Controllers;
using Microsoft.AspNetCore.Identity;
using HCF.Module.Core.Extensions;

namespace HCF.Module.ExternalAuth.Areas.ExternalAuth.Controllers
{
    //[Area("ExternalAuth")]
    public class MicrosoftAuthController : BasePublicController
    {
        private readonly IUserLoginService _userloginService;
        private readonly IOrganizationService _organizationService;
        private readonly IUserManagerService _userManagerService;
        private readonly UserManager<UserProfile> _userManager;
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly IWorkContext _workCOntext;

        public MicrosoftAuthController(
             IUserLoginService userloginService,
             IOrganizationService organizationService,
             IUserManagerService userManagerService,
             UserManager<UserProfile> userManager,
             SignInManager<UserProfile> signInManager,
             IWorkContext workCOntext
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userloginService = userloginService;
            _organizationService = organizationService;
            _userManagerService = userManagerService;
            _workCOntext = workCOntext;
        }

        //[AllowAnonymous]
        //[HttpGet("microsoft/auth/sucess/{email}")]
        //public async Task<IActionResult> MicrosoftAuthSucess(string email)
        //{
        //    var result = await _userManager.FindByEmailAsync(email);
        //    if (result != null && result.Id > 0 && result.IsActive)
        //    {
        //        SuccessMessage = "Logged in successfully.";
        //        var loginInfo = await _userManagerService.GetBrowserInfo(HttpContext);
        //        loginInfo.RefreshToken = Guid.NewGuid();
        //        loginInfo.OrgKey = result.Orgkey;
        //        loginInfo.UserId = result.Id;
        //        loginInfo.LogOnDate = DateTime.UtcNow;
        //        loginInfo.IsLogOn = true;
        //        loginInfo.UserName = result.Email;
        //        loginInfo.LastActivityDateTime = DateTime.Now;
        //        loginInfo.LoginProvider = LoginProvider.Microsoft.ToString();
        //        loginInfo.DeviceTypeId = 3;
        //        // save details in user login with login type                   
        //        await _userloginService.Save(loginInfo);
        //        await _signInManager.SignInAsync(result, true, null);
        //        if (!string.IsNullOrEmpty(result.Orgkey.ToString()) && result.Orgkey != default(Guid))
        //        {
        //            var org = await _organizationService.GetOrganizationAsync(result.Orgkey);
        //            await _workCOntext.SetCurrentOrg(org);
        //            return RedirectToRoute("setClient", new
        //            {
        //                orgkey = result.Orgkey,
        //                userId = result.UserId,
        //                refreshtoken = loginInfo.RefreshToken
        //            });
        //        }
        //    }
        //    return RedirectToAction("LogOff", "Auth", new { strmsg = email, message = AccountMessageId.EmailNotFound });
        //}

    }
}
