//using HCF.Module.Core.Areas.Core.ViewModels;
//using HCF.Module.Core.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HCF.Module.Core.Areas.Core.Controllers
//{
//    [Area("Core")]
//    public class ManageController : Controller
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly SignInManager<User> _signInManager;

//        public ManageController(UserManager<User> userManager, SignInManager<User> signInManager)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//        }

//        [HttpGet("user/info")]
//        public async Task<IActionResult> UserInfo()
//        {
//            var user = await GetCurrentUserAsync();
//            var model = new UserInfoVm { Email = user.Email, FullName = user.FullName };
//            return View(model);
//        }


//        private Task<User> GetCurrentUserAsync()
//        {
//            return _userManager.GetUserAsync(HttpContext.User);
//        }
//    }
//}
