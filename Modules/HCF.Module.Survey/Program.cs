using BlazorClient.Security;
using Blazored.LocalStorage;
using HCF.Module.Survey.Auth;
using HCF.Module.Survey.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HCF.Module.Survey
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddHttpClient("api")
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { "https://localhost:5002" },
                            scopes: new[] { "weatherapi" });

                    return handler;
                });

            //builder.Services.AddTransient(sp => new HttpClient
            //{
            //    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            //})
            //    .AddBlazoredLocalStorage();

            builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("api")).AddBlazoredLocalStorage();

            //builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationProvider>();
           //// builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazoredLocalStorage();
           builder.Services.AddScoped<IAccountService, AccountService>();
           // builder.Services.AddScoped<ITokenManagerService, TokenManagerService>();


            builder.Services
                .AddOidcAuthentication(options =>
                {
                    builder.Configuration.Bind("oidc", options.ProviderOptions);
                    options.UserOptions.RoleClaim = "role";
                })
                .AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

            await builder.Build().RunAsync();
        }
    }
}
