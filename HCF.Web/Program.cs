using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;


namespace HCF.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            using var host = WebHost.CreateDefaultBuilder(args)
                        .CaptureStartupErrors(true)
                        .ConfigureAppConfiguration((hostingcontext, config) =>
                        {
                            config
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env}.json", optional: true)
                            .AddEnvironmentVariables();
                        })
                        .UseSetting("detailedErrors", "true")
                        .UseStartup<Startup>()
                        .Build();

            //start the program, a task will be completed when the host starts
            await host.StartAsync();

            // a task will be completed when shutdown is triggered
            await host.WaitForShutdownAsync();

        }
    }
}