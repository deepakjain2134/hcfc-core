using HCF.BDO;
using HCF.Infrastructure.Modules;
using HCF.Module.Core.Events;
using HCF.Module.Core.Services;
using HCF.Module.Localization.Events;
using HCF.Module.Localization.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace HCF.Module.Localization
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<INotificationHandler<UserSignedIn>, UserSignedInHandler>();
            serviceCollection.AddTransient<IContentLocalizationService, ContentLocalizationService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
