using HCF.BDO;
using HCF.Infrastructure.Data;
using HCF.Infrastructure.Modules;
using HCF.Module.Core.Data;
using HCF.Module.LuceneSearch.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HCF.Module.LuceneSearch
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<IGoLucene, GoLucene>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


        }
    }
}