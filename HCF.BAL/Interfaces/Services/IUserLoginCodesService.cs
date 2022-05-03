using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.BAL.Interfaces.Services
{
   public interface IUserLoginCodesService
    {
        int SaveUserLoginCodes(int noOfCodes, Guid orgKey, int createdBy, string codes);
        List<UserLoginCodes> GetUserLoginCodes(Guid orgKey);
    }
}
