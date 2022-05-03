//using HCF.BDO;
//using HCF.Utility;
//using System.Collections.Generic;
//using System.Linq;

//namespace HCF.BAL
//{
//    public static class DigitalSignRepository
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newDigitalSignature"></param>
//        /// <returns></returns>
//        public static int Save(DigitalSignature newDigitalSignature)
//        {
//            return DAL.DigitalSignRepository.AddDigitalSignature(newDigitalSignature);
//        }

//        public static List<DigitalSignature> GetUserSign(int userId)
//        {
//            return DAL.DigitalSignRepository.GetDigitalSignature().Where(x => x.UserId == userId).ToList();
//        }

//        public static bool UpdateDigitalSignature(DigitalSignature newDigitalSignature)
//        {
//            return DAL.DigitalSignRepository.UpdateDigitalSignature(newDigitalSignature);
//        }

//        public static DigitalSignature SaveSign(DigitalSignature newDigitalSignature)
//        {
//            if (!string.IsNullOrEmpty(newDigitalSignature.FileName) && !string.IsNullOrEmpty(newDigitalSignature.FileContent))
//                newDigitalSignature.FilePath = AmazonFileUpload.SaveFileUsingContent(newDigitalSignature.FileContent, newDigitalSignature.FileName, "DigitalSignPath", "ExerciseDigSign").FilePath;

//            newDigitalSignature.DigSignatureId = Save(newDigitalSignature);
//            return newDigitalSignature;
//        }

//        public static List<DigitalSignature> GetDigitalSign()
//        {
//            return DAL.DigitalSignRepository.GetDigitalSignature().ToList();
//        }
//    }
//}
