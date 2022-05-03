using HCF.BDO;
using HCF.BDO.Users;
using HCF.DAL.Contexts;
using HCF.Infrastructure.Data;
using HCF.Module.Core.Data;
using HCF.Module.Core.Validator;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimplCommerce.Module.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.Core.Extensions
{

    public class SimplUserManager<TUser> : UserManager<TUser> where TUser : UserProfile
    {
        private readonly IMediator _mediator;
        private readonly IRepository<UsedPassword> _userPassword;

        public SimplUserManager(IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, ILogger<UserManager<TUser>> logger, IMediator mediator, IRepository<UsedPassword> userPassword
            )
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _mediator = mediator;
            _userPassword = userPassword;
        }

        public override async Task<IdentityResult> CreateAsync(TUser user)
        {
            var result = await base.CreateAsync(user);
            return result;
        }

        public async Task AddToUsedPasswordAsync(long userId, string userpassword)
        {
            var userPassword = new UsedPassword() { UserID = userId, HashPassword = userpassword };
            _userPassword.Add(userPassword);
            await _userPassword.SaveChangesAsync();
        }

        public override async Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword)
        {
            var result = await base.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
                await AddToUsedPasswordAsync(user.Id, user.PasswordHash);

            return result;
        }

        public override async Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword)
        {
            var result = await base.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
                await AddToUsedPasswordAsync(user.Id, user.PasswordHash);

            return result;
        }
    }

}
