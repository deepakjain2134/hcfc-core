using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.DAL.Interfaces
{
    public interface IRepositoryWrapper
    {
        IBuildVersionRepository BuildVersion { get; }
       
        Task SaveAsync();
    }
}
