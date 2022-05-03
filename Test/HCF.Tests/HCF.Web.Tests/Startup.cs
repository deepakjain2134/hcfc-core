using HCF.BAL.Services;
using HCF.BAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCF.BAL.Ioc;
using HCF.Utility.AppConfig;
using HCF.Utility.Configuration;
using HCF.Utility;
using HCF.Web.Base.Factory;
using HCF.Web.Base;
using Hcf.Api.Application;
using HCF.Utility.EmailSettings;
using Microsoft.AspNetCore.Http;
using Amazon.S3;
using HCF.DAL;

namespace HCF.Tests.HCF.Web.Tests
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;

        }

        public void ConfigureServices(IServiceCollection services)
        {

            
            services.AddSingleton<IServiceProviderProxy, HttpContextServiceProviderProxy>();

            services.AddTransient<IAppSetting, AppSetting>();
            //services.AddTransient<ILogger, Logger>();
            services.AddTransient<IHCFSession, SessionStore>();
            services.AddTransient<ICommonModelFactory, CommonModelFactory>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IHttpPostFactory, HttpPostFactory>();
            services.AddScoped<ICommonProvider, CommonProvider>();
            services.AddScoped<ICookieUtilFactory, CookieUtilFactory>();
            services.AddScoped<IApiCommon, ApiCommon>();
           // services.AddTransient<IMailService, MailService>();
            services.AddTransient<IEmailProcessor, EmailProcessor>();
            services.AddAWSService<IAmazonS3>();
            services.AddTransient<IFileUpload, AmazonFileUpload>();
            BAlServiceCollectionExtensions.AddRepositoryServices(services, _configuration);
            DalServiceCollectionExtensions.AddRepositoryServices(services, _configuration);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider sp)
        {
            // your logic goes here
        }

    }
}
