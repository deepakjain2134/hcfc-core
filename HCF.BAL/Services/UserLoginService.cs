//using HCF.BAL.Interfaces.Services;
//using HCF.BDO;
//using HCF.DAL.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace HCF.BAL.Services
//{
//    public class UserLoginService : IUserLoginService
//    {

//        private readonly IUserLoginRepository _userLoginRepository;

//        public UserLoginService(IUserLoginRepository userLoginRepository)
//        {
//            _userLoginRepository = userLoginRepository;
//        }

//        public async Task<IEnumerable<UserLogin>> GetUserLogins(int userId)
//        {
//            return await _userLoginRepository.GetUserLogins(userId);
//        }

//        public async Task<bool> LogoutUserAsync(Guid refreshToken, string url)
//        {
//            var isExistingUserLogin = await _userLoginRepository.GetUserLogin(refreshToken);
//            if (isExistingUserLogin != null)
//            {
//                isExistingUserLogin.LogOffDate = DateTime.UtcNow;
//                isExistingUserLogin.IsLogOn = false;
//                isExistingUserLogin.LastLoginURL = url;
//               // await _userLoginRepository.UpdateAsync(isExistingUserLogin);
//            }
//            return true;
//        }

//        public async Task<bool> Save(UserLogin userlogin)
//        {
//            //await _userLoginRepository.AddAsync(userlogin);
//            return true;
//        }
//    }
//}
