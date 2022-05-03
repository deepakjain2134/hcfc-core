using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IDigitalSignRepository
    {
        int AddDigitalSignature(DigitalSignature newDigitalSignature);
        List<DigitalSignature> GetDigitalSignature(string digSignatureId = null);
        DigitalSignature GetDigitalSignature(int tRoundId);
        bool UpdateDigitalSignature(DigitalSignature newDigitalSignature);
         int DeleteDigitalSign(int digSignatureId);
    }
}