using HCF.BDO;
using HCF.DAL;
using HCF.Utility;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL
{
    public  class DigitalSignService :IDigitalSignService
    {
        private readonly IDigitalSignRepository _digitalSignRepository;
        private readonly IFileUpload _fileUpload;
        public DigitalSignService(IDigitalSignRepository digitalSignRepository, IFileUpload fileUpload)
        {
            _digitalSignRepository = digitalSignRepository;
            _fileUpload = fileUpload;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newDigitalSignature"></param>
        /// <returns></returns>
        public  int Save(DigitalSignature newDigitalSignature)
        {
            return _digitalSignRepository.AddDigitalSignature(newDigitalSignature);
        }

        public  List<DigitalSignature> GetUserSign(int userId)
        {
            return _digitalSignRepository.GetDigitalSignature().Where(x => x.UserId == userId).ToList();
        }

        public  bool UpdateDigitalSignature(DigitalSignature newDigitalSignature)
        {
            return _digitalSignRepository.UpdateDigitalSignature(newDigitalSignature);
        }

        public DigitalSignature SaveSign(DigitalSignature newDigitalSignature)
        {
            if (!string.IsNullOrEmpty(newDigitalSignature.FileName) && !string.IsNullOrEmpty(newDigitalSignature.FileContent))
                newDigitalSignature.FilePath = _fileUpload.SaveFileUsingContent(newDigitalSignature.FileContent, newDigitalSignature.FileName,
                    "DigitalSignPath", "ExerciseDigSign").FilePath;

            newDigitalSignature.DigSignatureId = Save(newDigitalSignature);
            return newDigitalSignature;
        }

        public  List<DigitalSignature> GetDigitalSign()
        {
            return _digitalSignRepository.GetDigitalSignature().ToList();
        }

      public  int DeleteDigitalSign(int digSignatureId)
        {
            return _digitalSignRepository.DeleteDigitalSign(digSignatureId);
        }
    }
}
