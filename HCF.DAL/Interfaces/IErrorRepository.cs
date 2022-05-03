using System;

namespace HCF.DAL
{
    public interface IErrorRepository
    {      

        bool SaveError(Exception ex, string functionName);
     
    }
}