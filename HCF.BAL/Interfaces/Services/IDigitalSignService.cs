using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IDigitalSignService
    {
        List<DigitalSignature> GetDigitalSign();
        List<DigitalSignature> GetUserSign(int userId);
        int Save(DigitalSignature newDigitalSignature);
        DigitalSignature SaveSign(DigitalSignature newDigitalSignature);
        bool UpdateDigitalSignature(DigitalSignature newDigitalSignature);
        int DeleteDigitalSign(int digSignatureId);
    }
}