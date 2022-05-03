using HCF.BDO;
using HCF.Infrastructure.Modules;
using HCF.Module.Core.Extensions;
using HCF.Module.Core.Factory;
using HCF.Module.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace HCF.Module.Core
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped<IWorkContext, WorkContext>();
            serviceCollection.AddScoped<INavigationSession, NavigationMngSession>();
            serviceCollection.AddScoped<IMediator, Mediator>();
            serviceCollection.AddScoped<IUserManagerService, UserManagerService>();
            serviceCollection.AddScoped<IUserLoginService, UserLoginService>();
            serviceCollection.AddScoped<IUserClaimsPrincipalFactory<UserProfile>, CustomClaimsFactory>();
            serviceCollection.AddScoped<IClaimsTransformation, ClaimsTransformation>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
