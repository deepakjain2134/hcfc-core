using Hangfire;
using HCF.Infrastructure.Modules;
using HCF.Utility.Configuration;
using HCF.Web.Framework.Infrastructure.Extensions;
using HCF.Web.Framework.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace HCF.Web.Infrastructure.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        public static void StartEngine(this IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider sp,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider
            )
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            app.UseCors(options => options.AllowAnyOrigin());
            app.UseRewriter(new RewriteOptions()
                .AddRedirectToHttps()
                .AddRedirectToNonWww()
            );

            app.UseDeveloperExceptionPage();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/error/500");
                app.UseHsts();
            }

            //app.UseWhen(
            //    context => !context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase),
            //    a => a.UseStatusCodePagesWithReExecute("/error/ErrorWithCode/{0}")
            //);
            //app.UseOutputCaching();
            //app.UseMiniProfiler();
            app.UsePageNotFound();
            app.UseDefaultFiles();

            ServiceLocator.Initialize(sp.GetService<IServiceProviderProxy>());
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24 * 365;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + durationInSeconds;
                }
            });

            app.UseCookiePolicy();
           // app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict });
            app.UseRouting();
           
           
           
            app.UseAuthentication();
            //app.UseAuthorization();
           
            app.UseSession();
            app.UserPasswordValidatorsMiddleware();

            app.UseCustomizedIdentity();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseHcfEndpoints();
            app.ErrorValidatorsMiddleware();

            //will create hangfire dashboard
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            //RecurringJob.AddOrUpdate(
            //   "run every 5am",
            //   () => sp.GetService<IRoundRecuringJobService>().SetArchiveRound()
            //   , "* 13 * * *");
             

            //AssignJobs();
            var moduleInitializers = app.ApplicationServices.GetServices<IModuleInitializer>();
            foreach (var moduleInitializer in moduleInitializers)
            {
                moduleInitializer.Configure(app, env);
            }

            //app.UseMiddleware<TenantValidatorsMiddleware>();


            #region Swagger
            app.UseSwagger(options =>
            {                
                options.PreSerializeFilters.Add((swagger, req) =>
                {
                    swagger.Servers = new List<OpenApiServer>() { new OpenApiServer() { Url = $"https://{req.Host}" } };
                });
            });

          

            app.UseSwaggerUI(options =>
            {
                foreach (var desc in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.ApiVersion.ToString());
                    options.DefaultModelsExpandDepth(-1);
                    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    options.RoutePrefix = "apis/docs";
                    options.InjectJavascript("/swagger-ui-custom.js");
                }
            });

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        public static void UsePageNotFound(this IApplicationBuilder application)
        {
            application.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    string originalPath = context.Request.Path.Value;
                    context.Items["originalPath"] = originalPath;
                    context.Request.Path = "/error/404";
                    await next();
                }
            });
        }

        public static IApplicationBuilder UseCustomizedIdentity(this IApplicationBuilder app)
        {
            app.UseIdentityServer();
            app.UseWhen(
                context => context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase),
                a => a.Use(async (context, next) =>
                {
                    if (!context.User.Identity.IsAuthenticated)
                    {
                        var principal = new ClaimsPrincipal();

                        var bearerAuthResult = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
                        if (bearerAuthResult?.Principal != null)
                        {
                            principal.AddIdentities(bearerAuthResult.Principal.Identities);
                        }
                        context.User = principal;    
                    }
                    await next();
                }));

            app.UseAuthorization();
            return app;
        }

        public static void UseHcfEndpoints(this IApplicationBuilder application)
        {
            //Execute the endpoint selected by the routing middleware
            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "areas",
                     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                RouteCollectionsProvider.EndpointRouteBuilder(endpoints);
            });
        }

        public static IApplicationBuilder UserPasswordValidatorsMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserValidatorsMiddleware>();           
            return app;
        }

        public static IApplicationBuilder ErrorValidatorsMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            return app;
        }
    }
}
