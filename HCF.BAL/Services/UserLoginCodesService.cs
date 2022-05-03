using HCF.BAL.Interfaces.Services;
using HCF.BDO;

using HCF.DAL.Interfaces;
using System;
using System.Collections.Generic;


namespace HCF.BAL.Services
{
    public class UserLoginCodesService : IUserLoginCodesService
    {
        private readonly IUserLoginCodesRepository _repository;
        public UserLoginCodesService(IUserLoginCodesRepository repository)
        {
            _repository = repository;
        }

        public List<UserLoginCodes> GetUserLoginCodes(Guid orgKey)
        {
            return _repository.GetUserLoginCodes(orgKey);
        }

        public int SaveUserLoginCodes(int noOfCodes, Guid orgKey, int createdBy, string codes)
        {
            return _repository.SaveUserLoginCodes(noOfCodes, orgKey, createdBy, codes);
        }
    }
}
