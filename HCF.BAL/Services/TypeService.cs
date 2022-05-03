using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;

namespace HCF.BAL
{
    public  class TypeService : ITypeService
    {
        private readonly ITypeRepository _typeRepository;
        public TypeService(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public  int Save(Types type)
        {
            return _typeRepository.Save(type);
        }



        #region TaggedUsers

        public  int SaveTaggedUser(TaggedMaster taggedMaster)
        {
            return _typeRepository.SaveTaggedUser(taggedMaster);
        }
        public  int RemoveTagID(string TagId, string activityID, int UserId, int Tagtype, string PermitNo)
        {
            return _typeRepository.RemoveTagID( TagId,  activityID, UserId, Tagtype, PermitNo);
        }
     
        public  List<TaggedMaster> CheckExistingTag(string Tagid)
        {
            return _typeRepository.CheckExistingTag(Tagid);
        }

        public  List<TaggedMaster> GetTaggedList(int userId, Guid? tagId,int? tagtype)
        {
            return _typeRepository.GetTaggedList(userId, tagId, tagtype);
        }

        #endregion
    }
}
