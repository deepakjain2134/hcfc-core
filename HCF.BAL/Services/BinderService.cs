using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;

namespace HCF.BAL
{
    public  class BinderService : IBinderService
    {
        private readonly IBinderRepository _binderRepository;
        public BinderService(IBinderRepository binderRepository)
        {
            _binderRepository = binderRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binderId"></param>
        /// <returns></returns>
        public  List<EPsDocument> GetBinderDocument(int? binderId, string Year,int? DocTypeId)
        {
            return _binderRepository.GetBinderDocument(binderId,Year,DocTypeId);
        }

        public  Binders GetEpBinderDocument(int? binderId)
        {
            return _binderRepository.GetEPBinderDocument(binderId);
        }
    }
}
