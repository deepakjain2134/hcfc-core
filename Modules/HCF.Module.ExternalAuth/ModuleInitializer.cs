
using HCF.Infrastructure.Modules;
using HCF.Module.ExternalAuth.Areas.ExternalAuth.Events;
using HCF.Utility.Enum;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HCF.Module.ExternalAuth
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //var authenticationBuilder = serviceCollection.AddAuthentication(AuthenticationDefaults.AuthenticationScheme);
                

            //authenticationBuilder.AddCookie(AuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.Cookie.Name = $"{HcfCookieDefaults.Prefix}{HcfCookieDefaults.AuthenticationCookie}";
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //    options.LoginPath = AuthenticationDefaults.LoginPath;
            //    options.LogoutPath = AuthenticationDefaults.LogoutPath;
            //    options.AccessDeniedPath = AuthenticationDefaults.AccessDeniedPath;
            //    options.EventsType = typeof(CustomCookieAuthenticationEvents);
            //    options.Cookie.SameSite = SameSiteMode.Lax;

            //})
            //.AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"))
            //;

            //serviceCollection.AddScoped<CustomCookieAuthenticationEvents>();         

            //serviceCollection.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
            //{
            //    options.Events = new OpenIdConnectEvents
            //    {
            //        OnTokenValidated = ctx =>
            //        {
            //            //var claims = new List<Claim>
            //            // {
            //            //        new Claim(ClaimTypes.Role, "Admin")
            //            // };
            //            //var appIdentity = new ClaimsIdentity(claims);
            //            //ctx.Principal.AddIdentity(appIdentity);
            //            string userEmail = ctx.Principal.Identity.Name;

            //            var returnpath = configuration["AzureAd:CallbackPath"]; //"microsoft/auth/sucess/"
            //            ctx.Response.Redirect(returnpath + userEmail);
            //            ctx.HandleResponse();
            //            return Task.CompletedTask;
            //        },
            //        OnAuthenticationFailed = context =>
            //        {
            //            context.Response.Redirect("/logout");
            //            context.HandleResponse(); // Suppress the exception
            //            return Task.CompletedTask;
            //        },
            //        OnRemoteFailure = context =>
            //        {
            //            context.Response.Redirect("/logout");
            //            context.HandleResponse(); // Suppress the exception
            //            return Task.CompletedTask;
            //        }
            //    };
            //});

            //serviceCollection.AddRazorPages()
            //     .AddMicrosoftIdentityUI();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }            
    }
}
