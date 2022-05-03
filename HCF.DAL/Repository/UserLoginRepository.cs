using HCF.BDO;
using HCF.DAL.Contexts;
using HCF.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.DAL.Repository
{
    public class UserLoginRepository : IUserLoginRepository
    {
        // private readonly DbSet<UserLogin> _userLogins;
        public UserLoginRepository()
        {
            //_userLogins = dbContext.Set<UserLogin>();
        }

        public Task<UserLogin> GetUserLogin(Guid refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserLogin>> GetUserLogins(int userId)
        {
            throw new NotImplementedException();
        }

        //public async Task<IEnumerable<UserLogin>> GetUserLogins(int userId)
        //{
        //    throw MethodNotFound(); ;// MissingMethodException();
        //    //return await _userLogins.Where(x => x.UserId == userId).ToListAsync();
        //}

        //public async Task<UserLogin> GetUserLogin(Guid refreshToken)
        //{
        //    throw MissingMethodException();
        //    // return await _userLogins.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        //}
    }
}
