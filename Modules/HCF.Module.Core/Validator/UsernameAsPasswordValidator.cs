using HCF.BDO;
using HCF.BDO.Users;
using HCF.Infrastructure.Data;
using HCF.Module.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HCF.Module.Core.Validator
{
    public class CustPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : UserProfile
    {
        private readonly IRepository<UsedPassword> _userPassword;
        public CustPasswordValidator(IRepository<UsedPassword> userPassword)
        {
            _userPassword = userPassword;
        }
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8 ||  password.Length > 128)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "MinimumPasswordLength",
                    Description = string.Format("Minimum Password Length Required is:{0}", 8)
                }));
            }

            string PasswordPattern = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";

            if (!Regex.IsMatch(password, PasswordPattern))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "MissingNumberAndSpcChar",
                    Description = "The Password must have at least one numeric and one special character"
                }));
            }

            if (string.Equals(user.UserName, password, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "UsernameAsPassword",
                    Description = "You cannot use your username as your password"
                }));
            }

            var hashpassword = manager.PasswordHasher.HashPassword(user, password);
            var isInLastPassword = IsPreviousPassword(user.Id, hashpassword);
           
            if (isInLastPassword)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "CurrentPasswordAsPassword",
                    Description = "Password must be not same as last 3 password."
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }

        private bool IsPreviousPassword(long UserID, string newPassword)
        {
            var userPasswords = _userPassword.Query().Where(x => x.UserID == UserID).OrderByDescending(up => up.CreatedDate).Take(3).ToList();
            if (userPasswords.Any(up => up.HashPassword == newPassword))
            {
                return true;
            }
            return false;
        }
    }
}
