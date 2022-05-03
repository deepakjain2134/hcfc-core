using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.Core.Services
{
    public interface IPdfConverter
    {
        byte[] Convert(string htmlContent);
    }

}
