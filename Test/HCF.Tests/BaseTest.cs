using HCF.BAL;
using HCF.BAL.Services;
using HCF.BDO;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Tests
{
    public abstract class BaseTest
    {
        static BaseTest()
        {
            //var services = new ServiceCollection();
           // services.AddTransient<IUserService, UserService>();
        }
    }
}
