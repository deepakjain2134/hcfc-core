using AutoMapper;
using HCF.BAL;
using HCF.BDO;
using HCF.Module.Core.Extensions;
using HCF.Module.Core.Services;
using HCF.Utility;
using HCF.Utility.AppConfig;
using HCF.Web.Areas.Api.Filters;
using HCF.Web.Areas.Api.Models;
using HCF.Web.Areas.Api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace HCF.Web.Areas.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Account")]
    public class AccountApiController : ApiController
    {

        #region ctor
        private readonly ILogger<AccountApiController> _loggingService;
        private readonly IUserService _userService;
        private readonly SimplSignInManager<UserProfile> _signInManager;
        private readonly IUserManagerService _userManagerService;
        private readonly SimplUserManager<UserProfile> _userManager;
        private readonly IUserLoginService _userLoginService;
        private readonly IOrganizationService _organizationService;
        private readonly IWorkContext _workContext;
        private readonly IHCFSession _session;
        private readonly IAppSetting _appSetting;
        private readonly IMapper _mapper;

        public AccountApiController(ILogger<AccountApiController> loggingService, IUserService userService, SimplSignInManager<UserProfile> signInManager,
            IUserManagerService userManagerService, SimplUserManager<UserProfile> userManager, IUserLoginService userLoginService, IOrganizationService organizationService,
            IWorkContext workContext, IHCFSession session, IAppSetting appSetting, IMapper mapper)
        {
            _loggingService = loggingService;
            _userService = userService;
            _signInManager = signInManager;
            _userManagerService = userManagerService;
            _userManager = userManager;
            _userLoginService = userLoginService;
            _organizationService = organizationService;
            _workContext = workContext;
            _session = session;
            _appSetting = appSetting;
            _mapper = mapper;
        }

        #endregion

        #region API

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [IsAnonymous]
        [Route("AuthenticateUser/")]
        public ActionResult UserLogin(UserProfile newUser, string deviceToken, string deviceType)
        {
            const string version = "";
            var userLoginIfo = new UserLogin();
            var res = _userService.AuthenticateUser(newUser, deviceToken, Convert.ToInt32(deviceType), version, "", userLoginIfo);
            var _objHttpResponseMessage = new HCF.BDO.HttpResponseMessage();
            if (res.UserId > 0)
            {
                if (res.IsActive)
                {
                    _objHttpResponseMessage.Success = true;
                    _objHttpResponseMessage.Result = new Result { User = res };
                }
                else
                    _objHttpResponseMessage.Message = ConstMessage.User_InActive;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.Invalid_UserName_and_Password;
                _objHttpResponseMessage.Success = false;
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// Get the logon token using username and password 
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="deviceToken"></param>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [IsAnonymous]
        [Route("AuthenticateUsers/")]
        public async Task<IActionResult> APIUserLogin(HCF.Web.Areas.Api.Models.Request.UserLoginRequestModel newUser)
        {
            var _objHttpResponseMessage = new HttpResponseMessageApp();
            const string version = "";
            var userLoginIfo = new UserLogin();
            var user = await _userManager.FindByEmailAsync(newUser.UserName);
            if (user != null)
            {
                var resultAsync = await _signInManager.PasswordSignInAsync(newUser.UserName, newUser.Password, false, true);
                if (resultAsync.Succeeded)
                {
                    var loginInfo = new UserLogin();
                    user.UserId = Convert.ToInt32(user.Id);
                    loginInfo.RefreshToken = Guid.NewGuid();
                    loginInfo.UserId = user.UserId;
                    loginInfo.OrgKey = user.Orgkey;
                    loginInfo.DeviceId = "API-Call";
                    loginInfo.BuildVersion = _appSetting.BuildVersion;
                    loginInfo.DeviceTypeId = 3; //(!string.IsNullOrEmpty(deviceType) ? Convert.ToInt32(deviceType) : 0);
                    await _userLoginService.Save(loginInfo);

                    var org = await _organizationService.GetOrganizationAsync(user.Orgkey);

                    _session.Set(SessionKey.currentorg, org);
                    _session.Set(SessionKey.OrgName, org.DbName);
                    _session.Set(SessionKey.ClientDBName, org.DbName);
                    _session.Set(SessionKey.ClientNo, org.ClientNo);
                    var currentUser = _userService.GetUser(Convert.ToInt32(user.UserId));
                    LoginResponseModel loginViewModel = _mapper.Map<LoginResponseModel>(currentUser);
                    loginViewModel.LogOnToken = currentUser.UserLogin.FirstOrDefault(x => x.IsCurrent).RefreshToken.ToString();
                    if (loginViewModel.UserId > 0)
                    {
                        var data = new Response<LoginResponseModel>(loginViewModel);
                        return Ok(data);
                    }
                    else
                    {
                        var data = new Response<Int32>(loginViewModel.UserId, ConstMessage.Inactive_User_login_Msg);
                        return Ok(data);
                    }
                }
                return Ok(new Response<ConstMessage>(ConstMessage.Invalid_UserName_and_Password));
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Email_Not_Found));
        }

        #endregion

        #region Mobile APP API

        //[HttpPost]
        //[IsAnonymous]
        //[Route("app/AuthenticateUser/")]
        //public ActionResult AppUserLogin(UserProfile newUser, string deviceToken, string deviceType)
        //{
        //    const string version = "";
        //    var userLoginIfo = new UserLogin();
        //    var res = _userService.AuthenticateUser(newUser, deviceToken, Convert.ToInt32(deviceType), version, "", userLoginIfo);
        //    var _objHttpResponseMessage = new HttpResponseMessageApp();
        //    if (res.UserId > 0)
        //    {
        //        var ress = JsonConvert.DeserializeObject<UserProfileApp>(JsonConvert.SerializeObject(res));

        //        if (ress.IsActive)
        //        {
        //            _objHttpResponseMessage.Success = true;
        //            _objHttpResponseMessage.Result = new ResultApp { User = ress };
        //        }
        //        else
        //            _objHttpResponseMessage.Message = ConstMessage.User_InActive;
        //    }
        //    else
        //    {
        //        _objHttpResponseMessage.Message = ConstMessage.Invalid_UserName_and_Password;
        //        _objHttpResponseMessage.Success = false;
        //    }
        //    return Ok(_objHttpResponseMessage);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="deviceToken"></param>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [IsAnonymous]
        [Route("app/AuthenticateUser/")]
        public async Task<IActionResult> AppUserLogin(HCF.Web.Areas.Api.Models.Request.UserLoginRequestModel newUser, string deviceToken, string deviceType)
        {
            var _objHttpResponseMessage = new HttpResponseMessageApp();
            const string version = "";
            var userLoginIfo = new UserLogin();
            var user = await _userManager.FindByEmailAsync(newUser.UserName);
            if (user != null)
            {
                var resultAsync = await _signInManager.PasswordSignInAsync(newUser.UserName, newUser.Password, false, true);
                if (resultAsync.Succeeded)
                {
                    var loginInfo = new UserLogin();
                    user.UserId = Convert.ToInt32(user.Id);
                    loginInfo.RefreshToken = Guid.NewGuid();
                    loginInfo.UserId = user.UserId;
                    loginInfo.OrgKey = user.Orgkey;
                    loginInfo.DeviceId = deviceToken;
                    loginInfo.BuildVersion = _appSetting.BuildVersion;
                    loginInfo.DeviceTypeId = (!string.IsNullOrEmpty(deviceType) ? Convert.ToInt32(deviceType) : 0);
                    await _userLoginService.Save(loginInfo);

                    var org = await _organizationService.GetOrganizationAsync(user.Orgkey);

                    _session.Set(SessionKey.currentorg, org);
                    _session.Set(SessionKey.OrgName, org.DbName);
                    _session.Set(SessionKey.ClientDBName, org.DbName);
                    _session.Set(SessionKey.ClientNo, org.ClientNo);
                    var currentUser = _userService.GetUser(Convert.ToInt32(user.UserId));


                    if (!currentUser.IsActive)
                    {
                        _objHttpResponseMessage.Message = ConstMessage.Inactive_User_login_Msg;
                    }
                    else if (currentUser.IsVendorUser && currentUser.Vendor != null && !currentUser.Vendor.IsActive)
                    {
                        _objHttpResponseMessage.Message = ConstMessage.InActive_vendor_user;
                    }
                    else
                    {
                        currentUser.Organizations = new List<Organization>();
                        currentUser.Organizations = _userService.GetUserOrgsByUserId(user.UserId);
                        currentUser.TotalOrgCount = currentUser.Organizations.Count();

                        var ress = JsonConvert.DeserializeObject<UserProfileApp>(JsonConvert.SerializeObject(currentUser));
                        _objHttpResponseMessage.Success = true;
                        _objHttpResponseMessage.Result = new ResultApp { User = ress };
                    }
                }
                else
                {
                    _objHttpResponseMessage.Message = ConstMessage.Invalid_UserName_and_Password;
                }
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.Invalid_UserName_and_Password;
            }
            return Ok(_objHttpResponseMessage);
        }




        #endregion
    }
}
