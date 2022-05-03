using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL.Interfaces
{
   public interface IUserLoginCodesRepository
    {
        int SaveUserLoginCodes(int noOfCodes,Guid orgKey, int createdBy,string codes);
        List<UserLoginCodes> GetUserLoginCodes(Guid orgKey);
    }
}