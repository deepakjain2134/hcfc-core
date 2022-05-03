//using HCF.Module.Core.Areas.Core.ViewModels.Account;
//using HCF.Module.Core.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Localization;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HCF.Module.Core.Areas.Core.Controllers
//{
//    [Area("Core")]
//    [Authorize]
//    public class AccountController : Controller
//    {
//        private readonly UserManager<UserPro> _userManager;
//        private readonly SignInManager<User> _signInManager;
//        private readonly IStringLocalizer _localizer;
//        //private readonly IRazorViewRenderer _viewRender;
//        //private readonly IEmailSender _emailSender;
//        //private readonly ISmsSender _smsSender;
//        private readonly ILogger _logger;

//        public AccountController(
//            UserManager<User> userManager,
//            SignInManager<User> signInManager,
//            IStringLocalizerFactory stringLocalizerFactory,
//            //IRazorViewRenderer viewRender,
//            //IEmailSender emailSender,
//            //ISmsSender smsSender,
//            ILoggerFactory loggerFactory)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _localizer = stringLocalizerFactory.Create(null);
//            //_viewRender = viewRender;
//            //_emailSender = emailSender;
//            //_smsSender = smsSender;
//            _logger = loggerFactory.CreateLogger<AccountController>();
//        }


//        [HttpGet]
//        [AllowAnonymous]
//        [Route("account/login")]
//        public IActionResult Login(string returnUrl = null)
//        {
//            ViewData["ReturnUrl"] = returnUrl;
//            return View();
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        [Route("account/login")]
//        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
//        {
//            ViewData["ReturnUrl"] = returnUrl;
//            if (ModelState.IsValid)
//            {
//                // This doesn't count login failures towards account lockout
//                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
//                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
//                if (result.Succeeded)
//                {
//                    _logger.LogInformation(1, "User logged in.");
//                    return RedirectToLocal(returnUrl);
//                }
//                //if (result.RequiresTwoFactor)
//                //{
//                //    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
//                //}
//                if (result.IsLockedOut)
//                {
//                    _logger.LogWarning(2, "User account locked out.");
//                    return View("Lockout");
//                }
//                else
//                {
//                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
//                    return View(model);
//                }
//            }

//            // If we got this far, something failed, redisplay form
//            return View(model);
//        }

//        private IActionResult RedirectToLocal(string returnUrl)
//        {
//            if (Url.IsLocalUrl(returnUrl))
//            {
//                return Redirect(returnUrl);
//            }
//            else
//            {
//                return RedirectToAction(nameof(AccountController.Login), "Account");
//            }
//        }
//    }
//}
