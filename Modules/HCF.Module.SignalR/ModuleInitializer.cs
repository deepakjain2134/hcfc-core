using HCF.Infrastructure.Modules;
using HCF.Module.SignalR.Hubs;
using HCF.Module.SignalR.RealTime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace HCF.Module.SingnalR
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSignalR();
            serviceCollection.AddSingleton<IOnlineClientManager, OnlineClientManager>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {            
            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<CommonHub>("/signalr");
                });
        }
    }
}
