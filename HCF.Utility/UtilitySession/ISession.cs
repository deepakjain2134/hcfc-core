using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Utility
{
  public  interface IHCFSession
    {
        T Get<T>(string key);

        void Set<T>(string key, T entry);

        string ClientDbName
        {
            get;           
        }

        string ClientNo
        {
            get;
        }

        Exception Exception
        {
            get;
        }

        string IsExistTagDeficiency
        {
            get;
        }

        string IsGuestUser
        {
            get;
        }
        string combinevaluearr
        {
            get;
        }

        void clear();
    }
}
