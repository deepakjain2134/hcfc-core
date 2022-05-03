using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hcf.Api.Application;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Utility.AppConfig;
using HCF.Web.Areas.Api.Filters;
using HCF.Web.Areas.Api.Models.Request;
using HCF.Web.Areas.Api.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api.Controllers
{

    [Route("api/user")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "User")]
    public class UserApiController : ApiController
    {
        private readonly ILoggingService _loggingService;
        private static IUserService _userService;
        private static IEmailService _emailService;
        private readonly IApiCommon _apiCommon;
        private readonly IOrganizationService _organizationService;
        private readonly IRolesService _rolesService;
        private readonly IEpService _epService;
        private readonly IGoalMasterService _goalMasterService;
        private readonly ITransactionService _transactionService;
        private readonly IAppSetting _appSetting;
        private readonly ICommonProvider _commonProvider;
        private readonly IMapper _mapper;

        private readonly UserManager<UserProfile> _userManager;
        private readonly SignInManager<UserProfile> _signInManager;


        public UserApiController(UserManager<UserProfile> userManager, SignInManager<UserProfile> signInManager, IAppSetting appSetting, IEpService epService, IRolesService rolesService, IOrganizationService organizationService,
            IApiCommon apiCommon, ILoggingService loggingService, IUserService userService, IEmailService emailService, IGoalMasterService goalMasterService,
            ITransactionService transactionService, ICommonProvider commonProvider, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSetting = appSetting;
            _epService = epService;
            _rolesService = rolesService;
            _organizationService = organizationService;
            _apiCommon = apiCommon;
            _loggingService = loggingService;
            _userService = userService; _emailService = emailService;
            _goalMasterService = goalMasterService;
            _transactionService = transactionService;
            _commonProvider = commonProvider;
            _mapper = mapper;
        }

        #region EPAssignee

        /// <summary>
        /// </summary>
        /// <param name="epAssignee"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public ActionResult AddEpAssignee(List<EPAssignee> epAssignee)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (epAssignee != null)
            {
                List<int> userIDs = epAssignee.Select(x => x.UserId).ToList();
                if (epAssignee.Count > 0)
                {
                    EPAssignee newEPAssignee = new();
                    newEPAssignee = epAssignee[0];
                    newEPAssignee.UserIds = userIDs;
                    _goalMasterService.AddEpAssignees(newEPAssignee);

                }
                //foreach (var item in epAssignee)
                //{
                //    GoalMaster.AddEpAssignees(item);
                //}
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = ConstMessage.EP_Assign_Successfully;
            }
            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region User

        /// <summary>
        ///     CreateUser
        /// </summary>ForGotPassword
        /// <param name="newUser"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public ActionResult CreateUser(UserProfile newUser)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var userId = _userService.CreateUser(newUser, "");
            if (userId > 0)
            {
                newUser.UserId = userId;
                if (!string.IsNullOrEmpty(newUser.FileContent))
                {
                    newUser.FilePath = _apiCommon.SaveFile(newUser.FileContent, newUser.FileName, "UserImage",
                        newUser.UserId.ToString()).FilePath;

                }
                _userService.UpdateUser(newUser, "");
                var token = Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(newUser.Email))
                    token = _userService.PasswordResetQueue(newUser.Email, 'E', newUser.Email, token, Convert.ToString(BDO.Enums.passwordStatus.create));
                _emailService.SendMailOnUserRegistration(newUser, token, "");

                _objHttpResponseMessage.Result = new Result { User = newUser };
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = ConstMessage.Insert_Profile_Success;
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.User_Already_Exist;
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// </summary>
        /// <param name="userCertificates"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addcertificates")]
        public ActionResult AddCertificates(List<Certificates> userCertificates)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var result = 0;
            if (userCertificates != null)
            {
                if (userCertificates.Count > 0)
                    result = SaveCertificates(userCertificates, "UserCertificatePath");
                if (result == -1)
                {
                    _objHttpResponseMessage.Success = false;
                    _objHttpResponseMessage.Result = new Result { UserCertificates = userCertificates };
                    _objHttpResponseMessage.Message = "Certificate name already exist";
                }
                else
                {
                    _objHttpResponseMessage.Success = true;
                    _objHttpResponseMessage.Result = new Result { UserCertificates = userCertificates };
                    _objHttpResponseMessage.Message = ConstMessage.Certificate_Added_Successfully;
                }

            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private int SaveCertificates(List<Certificates> userCertificates, string certificateType)
        {
            var result = 0;
            foreach (var item in userCertificates)
                if (!string.IsNullOrEmpty(item.FileName))
                {
                    if (item.FileContent != null)
                        item.Path = _apiCommon.SaveFile(item.FileContent, item.FileName, certificateType, item.UserId.ToString())
                            .FilePath;
                    item.ValidUpTo = item.ValidUpToTimeSpan == 0 ? item.ValidUpTo : Conversion.ConvertToDateTime(item.ValidUpToTimeSpan);
                    item.VendorId = item.VendorId;
                    item.UserId = item.UserId;
                    result = _userService.AddCertificates(item);
                }
            return result;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="deviceToken"></param>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public ActionResult AuthenticateUser(UserProfile newUser, string deviceToken, string deviceType, string version, string osname, string ipaddress, string city, string country)
        {
            var userLoginIfo = new UserLogin();
            var res = _userService.AuthenticateUser(newUser, deviceToken, Convert.ToInt32(deviceType), version, "", userLoginIfo);
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (res.UserId > 0)
            {
                if (res.IsActive)
                {
                    if (res.Vendor != null && res.Vendor.IsActive == false)
                    {
                        _objHttpResponseMessage.Success = false;
                        _objHttpResponseMessage.Message = ConstMessage.InActive_vendor;
                        //return _objHttpResponseMessage;
                    }

                    _objHttpResponseMessage.Success = true;
                    _objHttpResponseMessage.Result = new Result { User = res };
                }
                else
                {
                    _objHttpResponseMessage.Message = ConstMessage.User_InActive;
                }
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.Invalid_UserName_and_Password;
                _objHttpResponseMessage.Success = false;
            }
            return Ok(_objHttpResponseMessage);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<ActionResult> GetRefreshtoken(string userId, string refreshToken)
        {
            var res = await _userService.GetRefreshtoken(userId, refreshToken);

            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Success = true,
                Result = new Result { User = res }
            };

            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        ///     GetUsers
        /// </summary>
        /// <returns></returns>

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetUsers")]
        public ActionResult GetUsers()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var userdetails = _userService.GetUsers();
            if (userdetails != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Users = userdetails };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ///
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetUser/{userId}")]
        public ActionResult GetUser(string userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var userdetail = _userService.GetUserList(Convert.ToInt32(userId));
            if (userdetail.UserId > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { User = userdetail };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        /// 

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("UpdateUser")]
        public ActionResult UpdateUser(UserProfile newUser)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (!string.IsNullOrEmpty(newUser.FileContent))
                newUser.FilePath = _apiCommon.SaveFile(newUser.FileContent, newUser.FileName, "UserImage",
                    newUser.UserId.ToString()).FilePath;

            var status = _userService.UpdateUser(newUser, "");
            if (!string.IsNullOrEmpty(newUser.Password) && status)
            {
                _userService.UpdatePassword(newUser.UserName, newUser.Password, newUser.CreatedBy, newUser.IsPwdChange);
                if (!string.IsNullOrEmpty(newUser.Email))
                    _emailService.SendMailOnUserChangePassword(newUser);
            }
            if (status)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { User = newUser };
                _objHttpResponseMessage.Message = ConstMessage.Update_Profile_Success;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.Error;
                _objHttpResponseMessage.Success = false;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("UpdatePassword")]
        public ActionResult UpdatePassword(UserUpdatePassword objUserUpdatePassword)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            bool isInLastPassword = _userService.IsInLastPasswords(objUserUpdatePassword.UserId, objUserUpdatePassword.NewPassword);
            if (isInLastPassword)
            {
                _objHttpResponseMessage.Message = "Password must be not same as last 3 password.";
                _objHttpResponseMessage.Success = false;
                return Ok(_objHttpResponseMessage);
            }
            var status = _userService.UpdatePassword(objUserUpdatePassword.UserId, objUserUpdatePassword.Password, objUserUpdatePassword.NewPassword);
            if (status)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = ConstMessage.Password_Change_Success;
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Current_Password_NotMatch_With_Password;
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetUserDashBoard/{userId}")]
        public ActionResult GetUserDashBoard(string userId)
        {
            var response = new HttpResponseMessage();
            if (!string.IsNullOrEmpty(userId))
            {
                var organization = _organizationService.GetUserDashBoard(Convert.ToInt32(userId));
                response.Result = new Result();
                response.Success = true;
                response.Result.UserDashBoard = organization;
            }
            return Ok(response);
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// 
        //[HttpGet]
        //[Route("GetDashBoarddata/{userId}")]
        //public ActionResult GetDashBoarddata(string userId)
        //{
        //    var _objHttpResponseMessage = new HttpResponseMessage();
        //    var obj = _userService.GetDashBoardData(Convert.ToInt32(userId));
        //    if (obj.Count > 0)
        //    {
        //        _objHttpResponseMessage.Result = new Result();
        //        _objHttpResponseMessage.Success = true;
        //        _objHttpResponseMessage.Result.DasboardData = obj;
        //    }
        //    else
        //    {
        //        _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
        //    }
        //    return Ok(_objHttpResponseMessage);
        //}


        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("GetDashBoarddataView/{userId}")]
        public ActionResult GetDashBoarddataView(Request objRequest, string userId)
        {
            //Request objRequest = new Request() { Mode = "A" };
            var _objHttpResponseMessage = new HttpResponseMessage();
            var data = _epService.GetDashBoardEp(objRequest, Convert.ToInt32(userId));
            //var data = await EpRepository.GetDashBoardEp(objRequest, Convert.ToInt32(userId));
            var obj = data.Result.EPDetails;
            if (obj.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result() { EPDetails = obj };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }



        /// <summary>
        ///     GetComplianceDashBoarddata
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// 
        //[HttpGet]
        //[Route("GetComplianceDashBoarddata/{userId}")]
        //public ActionResult GetComplianceDashBoarddata(string userId)
        //{
        //    var _objHttpResponseMessage = new HttpResponseMessage();
        //    var obj = _userService.GetDashBoardData(Convert.ToInt32(userId)).Where(x => x.IsDeficiency).ToList();
        //    //obj = obj.Where(x => x.LastInspectionDate != "" && x.LastInspectionDate != null && x.Status != "2").ToList();
        //    if (obj.Count > 0)
        //    {
        //        _objHttpResponseMessage.Result = new Result();
        //        _objHttpResponseMessage.Success = true;
        //        _objHttpResponseMessage.Result.DasboardData = obj;
        //    }
        //    else
        //    {
        //        _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
        //    }
        //    return Ok(_objHttpResponseMessage);
        //}

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// 

        //[HttpGet]
        //[Route("GetRecentActivity/{userId}")]
        //public ActionResult GetRecentActivity(string userId)
        //{
        //    var _objHttpResponseMessage = new HttpResponseMessage();
        //    var obj = _transactionService.GetRecentActivity(Convert.ToInt32(userId));
        //    //obj = obj.Where(x => x.LastInspectionDate != "" && x.LastInspectionDate != null && x.Status != "2").ToList();
        //    if (obj.Count > 0)
        //    {
        //        _objHttpResponseMessage.Result = new Result();
        //        _objHttpResponseMessage.Success = true;
        //        _objHttpResponseMessage.Result.EPDetails = obj;
        //    }
        //    else
        //    {
        //        _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
        //    }
        //    return Ok(_objHttpResponseMessage);
        //}

        /// <summary>
        ///     ForgotPassword
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// 

        //[Route("{userName:alpha}")]
        [Route("ForgotPassword/{userName}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [IsAnonymous]
        public async Task<ActionResult> ForgotPassword(string userName)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var existingUser = _userService.GetAllClientUsers().FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower());
            if (existingUser != null && existingUser.UserId > 0 && existingUser.IsActive)
            {
                var user = await _userManager.FindByEmailAsync(userName);
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Auth", new { userId = existingUser.UserId, code = code }, protocol: HttpContext.Request.Scheme);


                string path = _commonProvider.GetContentRootPath("wwwroot");
                FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/CantAccessAccount.htm"));
                var emailBody = System.IO.File.ReadAllText(fileInfo.FullName);
                _objHttpResponseMessage.Result = new Result
                {
                    Id = _userService.SendRecoveryMail(userName, emailBody, callbackUrl)
                };
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = _objHttpResponseMessage.Result.Id == 1 ? ConstMessage.Mail_Sent_Successfully : ConstMessage.Contact_Admin;
            }
            else if (existingUser != null && existingUser.UserId > 0 && !existingUser.IsActive)
                _objHttpResponseMessage.Message = ConstMessage.Inactive_User_login_Msg;
            else if (existingUser == null)
                _objHttpResponseMessage.Message = ConstMessage.Forgot_Password_User_Not_Found;

            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        ///     GetUserGroup
        /// </summary>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetUserGroup")]
        public ActionResult GetUserGroup()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var userGroups = _userService.GetUserGroups();
            if (userGroups.Any())
            {
                _objHttpResponseMessage.Result = new Result { UserGroups = userGroups };
                _objHttpResponseMessage.Success = true;
                return Ok(_objHttpResponseMessage);
            }
            _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("LogOut")]
        public ActionResult LogOut(UserLogin loginUser)
        {
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Success = _userService.LogOut(loginUser),
                Message = ConstMessage.LoggedOutSucess
            };
            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        /// Returns a list of users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("users")]
        public ActionResult Users()
        {
            var users = _userService.GetUsers();
            List<UserResponseModel> usersViewModel = _mapper.Map<List<UserResponseModel>>(users);
            if (usersViewModel.Count > 0)
            {
                var data = new Response<List<UserResponseModel>>(usersViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }


        /// <summary>
        /// Returns specified users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("users/{resoureNo}")]
        public ActionResult Users(string resoureNo)
        {
            var userdetails = _userService.GetUsers().FirstOrDefault(x => x.ResourceNumber == resoureNo);
            UserResponseModel usersViewModel = _mapper.Map<UserResponseModel>(userdetails);
            if (usersViewModel != null && usersViewModel.UserId > 0)
            {
                var data = new Response<UserResponseModel>(usersViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }

        /// <summary>
        /// Creates users with the details specified in the request body
        /// </summary>
        /// <param name="RequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("users")]
        public ActionResult AddUser(SaveUserRequestModel RequestModel)
        {
            UserProfile newUser = _mapper.Map<UserProfile>(RequestModel);
            newUser.UserName = RequestModel.Email;
            var userId = _userService.CreateUser(newUser, "");
            if (userId > 0)
            {
                newUser.UserId = userId;
                var token = Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(newUser.Email))
                    token = _userService.PasswordResetQueue(newUser.Email, 'E', newUser.Email, token, Convert.ToString(BDO.Enums.passwordStatus.create));
                //_emailService.SendMailOnUserRegistration(newUser, token, "");
                var data = new Response<Int32>(newUser.UserId, ConstMessage.Saved_Successfully);
                return Ok(data);

            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        }

        /// <summary>
        /// Updates users with the details specified in the request body
        /// </summary>
        /// <param name="RequestModel"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("users/{resoureNo}")]
        public ActionResult UpdateUser(UpdateUserRequestModel RequestModel, string resoureNo)
        {
            var userdetails = _userService.GetUsers().FirstOrDefault(x => x.ResourceNumber == resoureNo);
            UserProfile newUser = _mapper.Map<UserProfile>(RequestModel);
            newUser.UserId = userdetails.UserId;
            var status = _userService.UpdateUser(newUser, "");
            if (status)
            {
                var data = new Response<Int32>(newUser.UserId, ConstMessage.Success_Updated);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        }

        #endregion

        #region  Vendor

        /// <summary>
        ///     GetVendors
        /// </summary>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetVendors")]
        public ActionResult GetVendors()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //var vendors = _vendorService.GetVendorsUser();
            //if (vendors.Count > 0)
            //{
            //    _objHttpResponseMessage.Success = true;
            //    _objHttpResponseMessage.Result = new Result { Vendors = vendors };
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            //}
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        ///     Addvendor
        /// </summary>
        /// <param name="newVendor"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("Addvendor")]
        public ActionResult Addvendor(Vendors newVendor)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //var vendorid = VendorRepository.AddVendors(newVendor,true,0,Guid.NewGuid(),"");
            //if (vendorid > 0)
            //{
            //    newVendor.VendorId = vendorid;

            //    if (newVendor.VendorCertificates != null)
            //    {
            //        if (newVendor.VendorCertificates.Count > 0)
            //        {
            //            foreach (var item in newVendor.VendorCertificates)
            //            {
            //                item.VendorId = newVendor.VendorId;
            //                item.CreatedBy = Convert.ToInt32(newVendor.CreatedBy);
            //            }
            //            SaveCertificates(newVendor.VendorCertificates, "VendorCertificatePath");
            //        }
            //    }
            //    _objHttpResponseMessage.Result = new Result();
            //    newVendor.VendorId = vendorid;
            //    _objHttpResponseMessage.Success = true;
            //    _objHttpResponseMessage.Result.Vendor = newVendor;
            //}
            //else
            //{
            //    _objHttpResponseMessage.Success = false;
            //    _objHttpResponseMessage.Message = ConstMessage.Vendor_Already_Exist;
            //}
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// </summary>
        /// <param name="newVendor"></param>
        /// <returns></returns>
        /// 

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("UpdateVendor")]
        public ActionResult UpdateVendor(Vendors newVendor)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //var status = VendorRepository.UpdateVendor(newVendor,"");
            //if (newVendor.VendorCertificates != null)
            //{
            //    VendorRepository.DeleteCertificates(newVendor.VendorId);
            //    if (newVendor.VendorCertificates.Count > 0)
            //        SaveCertificates(newVendor.VendorCertificates, "VendorCertificatePath");
            //}
            //if (status)
            //{
            //    _objHttpResponseMessage.Success = true;
            //    _objHttpResponseMessage.Result = new Result { Vendor = newVendor };
            //    _objHttpResponseMessage.Message = ConstMessage.Update_vendor_Success;
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = ConstMessage.Error;
            //    _objHttpResponseMessage.Success = false;
            //}
            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region Roles

        /// <summary>
        ///     GetUserGroupPermission
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetUserGroupPermission/{userGroupId}")]
        public ActionResult GetUserGroupPermission(string userGroupId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var res = _rolesService.GetUserGroupPermission(Convert.ToInt32(userGroupId));
            if (res != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Roles = res };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        ///     GetPermissionMatrix
        /// </summary>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetPermissionMatrix")]
        public ActionResult GetPermissionMatrix()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var res = _rolesService.GetPermissionMatrix();
            if (res != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Roles = res };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
            }
            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        ///     GetPermissionMatrix
        /// </summary>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetPermissionMatrixByRoles")]
        public ActionResult GetPermissionMatrixByRoles()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var res = _rolesService.GetPermissionMatrixByRoles();
            if (res != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Role = res };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("UpdatePermissionMatrix")]
        public ActionResult UpdatePermissionMatrix(RolesInGroups newRolesInGroups)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var status = _rolesService.UpdatePermissionMatrix(newRolesInGroups);
            if (status)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Role = _rolesService.GetPermissionMatrixByRoles() };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
            }
            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region User Menu

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetUserMenu/{userId}/{menuId}")]
        public ActionResult GetUserMenu(string userId, string menuId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<Menus> menus = _organizationService.GetUserDashBoard(Convert.ToInt32(userId), Convert.ToInt32(menuId)).Services.ToList();
            if (menus.Count > 0)
                _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result
            {
                Menus = menus
            };
            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region User Record Count

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetUserRecordsCount/{userId}")]
        public ActionResult GetUserRecordsCount(string userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var UserRecordsCount = _userService.GetUserRecords(Convert.ToInt32(userId));
            if (UserRecordsCount.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    UserRecordsCounts = UserRecordsCount
                };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("AddCertificate")]
        public ActionResult AddCertificate(Certificates certificates)//, string strJson = null
        {
            var _objHttpResponseMessage = new HttpResponseMessage();

            if (!string.IsNullOrEmpty(certificates.FileContent))
            {
                string fileName = CommonUtility.CreateFileName(certificates.FileName);
                certificates.Path = _apiCommon.SaveFile(certificates.FileContent, fileName, "Certificate", null).FilePath;
            }

            int certificateid = _userService.AddCertificates(certificates);
            if (certificateid > 0)
            {
                _objHttpResponseMessage.Message = ConstMessage.Certificate_Added_Successfully;
                _objHttpResponseMessage.Success = true;
            }
            else { _objHttpResponseMessage.Message = ConstMessage.Error; _objHttpResponseMessage.Success = false; }

            _objHttpResponseMessage.Result = new Result { certificates = certificates };
            return Ok(_objHttpResponseMessage);
        }

        #endregion
    }
}