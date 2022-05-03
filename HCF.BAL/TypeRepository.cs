//using HCF.BDO;
//using System;
//using System.Collections.Generic;

//namespace HCF.BAL
//{
//    public static class TypeRepository
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="type"></param>
//        /// <returns></returns>
//        public static int Save(Types type)
//        {
//            return DAL.TypeRepository.Save(type);
//        }



//        #region TaggedUsers

//        public static int SaveTaggedUser(TaggedMaster taggedMaster)
//        {
//            return DAL.TypeRepository.SaveTaggedUser(taggedMaster);
//        }
//        public static int RemoveTagID(string TagId, string activityID, int UserId, int Tagtype, string PermitNo)
//        {
//            return DAL.TypeRepository.RemoveTagID( TagId,  activityID, UserId, Tagtype, PermitNo);
//        }


//        //public static bool UpdateTagidInDeficiencies(string Activityid, string Tagid)
//        //{
//        //    return DAL.TypeRepository.UpdateTagidInDeficiencies(Activityid, Tagid);
//        //}

//        public static List<BDO.TaggedMaster> CheckExistingTag(string Tagid)
//        {
//            return DAL.TypeRepository.CheckExistingTag(Tagid);
//        }


//        public static List<TaggedMaster> GetTaggedList(int userId, Guid? tagId,int? tagtype)
//        {
//            return DAL.TypeRepository.GetTaggedList(userId, tagId, tagtype);
//        }

//        #endregion
//    }
//}
