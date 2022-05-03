using Amazon.S3;
using Hangfire;
using Hcf.Api.Application;
using HCF.BDO;
using HCF.Infrastructure.Data;
using HCF.Infrastructure.Modules;
using HCF.Module.Core.Data;
using HCF.Module.Core.Extensions;
using HCF.Module.Core.Models;
using HCF.Utility;
using HCF.Utility.AppConfig;
using HCF.Utility.Configuration;
using HCF.Utility.EmailSettings;
using HCF.Utility.Enum;
using HCF.Web.Areas.Api.Filters;
using HCF.Web.Base;
using HCF.Web.Base.Factory;
using HCF.Web.Filters;
using HCF.Web.IdentityServer;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using SimplCommerce.Module.Core.Extensions;
using StackExchange.Profiling.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using HCF.Module.Core.Validator;
using System.Security.Cryptography.X509Certificates;
using HCF.Module.ExternalAuth.Areas.ExternalAuth.Events;
using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using HCF.Web.Framework.Infrastructure.Extensions;

namespace HCF.Web.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly IModuleConfigurationManager _modulesConfig = new ModuleConfigurationManager();

        public static void ConfigureApplicationServices(this IServiceCollection services,
              IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            services.AddModules();


           // services.Configure<CookiePolicyOptions>(options =>
           // {
             //   options.CheckConsentNeeded = context => false;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
         //  });



            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US")
                    };
                });

            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            services.AddCors(options =>
             {
                 options.AddPolicy(name: MyAllowSpecificOrigins,
                                   builder =>
                                   {
                                       builder.WithOrigins("https://localhost:5001"
                                                           )
                                          .AllowCredentials()
                                         .AllowAnyMethod()
                                         .AllowAnyHeader();
                                   });
             });



            services.AddHttpClient("api", client =>
            {
                client.BaseAddress = new Uri(configuration["AppSeettings:CommonApiUrl"]);
            });

            services.AddDbContextPool<HcfDatabaseContext>(options =>
                 options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                     b => b.MigrationsAssembly("HCF.Web")));

            services.AddHttpContextAccessor();
            services.AddSingleton<IServiceProviderProxy, HttpContextServiceProviderProxy>();

            services.AddCustomizedIdentity(configuration); //Identity 
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IRepositoryWithTypedId<,>), typeof(RepositoryWithTypedId<,>));

            services.AddScoped<IAppSetting, AppSetting>();
            services.AddScoped<IHCFSession, SessionStore>();
            services.AddScoped<ICommonModelFactory, CommonModelFactory>();
            services.AddScoped<IHttpPostFactory, HttpPostFactory>();
            services.AddScoped<ICommonProvider, CommonProvider>();
            services.AddScoped<ICookieUtilFactory, CookieUtilFactory>();
            services.AddScoped<IApiCommon, ApiCommon>();
            //services.AddTransient<IMailService, MailService>();
            services.AddTransient<IEmailProcessor, EmailProcessor>();


            //services.AddTransient<ITenantService, TenantService>();
            //services.Configure<TenantSettings>(configuration.GetSection(nameof(TenantSettings)));


            services.AddAutoMapper(typeof(Startup));
            // emails services
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<CadViewerSettings>(configuration.GetSection("CadViewerSettings"));

            services.AddAddMiniProfiler();
            // end email services          

            #region API versioning

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            #endregion

            #region Swagger

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
            services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(xmlFilePath);               
            });

          

            #endregion

            BAL.Ioc.BAlServiceCollectionExtensions.AddRepositoryServices(services, configuration);

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddHttpSession();
            services.AddDistributedMemoryCache();
            //services.AddAuthentication(configuration);
            services.AddAuthorization();
            services.AddBrowserDetection();

            services.AddMvcService(HCF.Infrastructure.GlobalConfiguration.Modules);
            //services.Configure<RazorViewEngineOptions>(
            //    options => { options.ViewLocationExpanders.Add(new ThemeableViewLocationExpander()); });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddOutputCaching();
            // filters
            services.AddScoped<BaseActionAsyncActionFilter>();
            services.AddScoped<ActionWebApiFilter>();

            //amazon relates services
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.AddScoped<IFileUpload, AmazonFileUpload>();
            string connection = configuration.GetConnectionString("HCFConnection").Replace("{0}", "HCF");
            services.AddHangfire(x => x.UseSqlServerStorage(connection));
            services.AddHangfireServer();

            foreach (var module in HCF.Infrastructure.GlobalConfiguration.Modules)
            {
                var moduleInitializerType = module.Assembly.GetTypes()
                   .FirstOrDefault(t => typeof(IModuleInitializer).IsAssignableFrom(t));
                if ((moduleInitializerType != null) && (moduleInitializerType != typeof(IModuleInitializer)))
                {
                    var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
                    services.AddSingleton(typeof(IModuleInitializer), moduleInitializer);
                    moduleInitializer.ConfigureServices(services, configuration);
                }
            }
            services.AddScoped<ServiceFactory>(p => p.GetService);
            services.AddScoped<IMediator, Mediator>();
        }

        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void AddAddMiniProfiler(this IServiceCollection services)
        {
            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
                (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);
                options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
                options.TrackConnectionOpenClose = true;
                options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;
                options.EnableMvcFilterProfiling = true;
                options.EnableMvcViewProfiling = true;


                options.IgnoredPaths.Add("/lib");
                options.IgnoredPaths.Add("/css");
                options.IgnoredPaths.Add("/js");
                options.IgnoredPaths.Add("/bundle");

            });

        }

        public static void AddAntiForgery(this IServiceCollection services)
        {
            //override cookie name
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = $"{HcfCookieDefaults.Prefix}{HcfCookieDefaults.AntiforgeryCookie}";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
        }

        public static IServiceCollection AddModules(this IServiceCollection services)
        {
            foreach (var module in _modulesConfig.GetModules())
            {
                if (!module.IsBundledWithHost)
                {
                    TryLoadModuleAssembly(module.Id, module);
                    if (module.Assembly == null)
                    {
                        throw new AppException($"Cannot find main assembly for module {module.Id}");
                    }
                }
                else
                {
                    module.Assembly = Assembly.Load(new AssemblyName(module.Id));
                }

                HCF.Infrastructure.GlobalConfiguration.Modules.Add(module);
            }

            return services;
        }

        private static void TryLoadModuleAssembly(string moduleFolderPath, ModuleInfo module)
        {
            const string binariesFolderName = "bin";
            var binariesFolderPath = Path.Combine(moduleFolderPath, binariesFolderName);
            var binariesFolder = new DirectoryInfo(binariesFolderPath);

            if (Directory.Exists(binariesFolderPath))
            {
                foreach (var file in binariesFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
                {
                    Assembly assembly;
                    try
                    {
                        assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                    }
                    catch (FileLoadException)
                    {
                        // Get loaded assembly. This assembly might be loaded
                        assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));

                        if (assembly == null)
                        {
                            throw;
                        }

                        string loadedAssemblyVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
                        string tryToLoadAssemblyVersion = FileVersionInfo.GetVersionInfo(file.FullName).FileVersion;

                        // Or log the exception somewhere and don't add the module to list so that it will not be initialized
                        if (tryToLoadAssemblyVersion != loadedAssemblyVersion)
                        {
                            throw new AppException($"Cannot load {file.FullName} {tryToLoadAssemblyVersion} because {assembly.Location} {loadedAssemblyVersion} has been loaded");
                        }
                    }

                    if (Path.GetFileNameWithoutExtension(assembly.ManifestModule.Name) == module.Id)
                    {
                        module.Assembly = assembly;
                    }
                }
            }
        }

        public static void AddHttpSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = $"{HcfCookieDefaults.Prefix}{HcfCookieDefaults.SessionCookie}";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
        }

        public static IMvcBuilder AddMvcService(this IServiceCollection services, IList<ModuleInfo> modules)
        {
            var mvcBuilder = services.AddControllersWithViews();
            mvcBuilder.AddViewLocalization();
            mvcBuilder.AddRazorRuntimeCompilation();

            mvcBuilder.AddViewLocalization();
            services.AddControllers().AddNewtonsoftJson(options =>
             {
                 options.UseMemberCasing();
             });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest).AddSessionStateTempDataProvider();
            mvcBuilder.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            foreach (var module in modules.Where(x => !x.IsBundledWithHost))
            {
                AddApplicationPart(mvcBuilder, module.Assembly);
            }

            return mvcBuilder;
        }

        private static void AddApplicationPart(IMvcBuilder mvcBuilder, Assembly assembly)
        {
            var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
            foreach (var part in partFactory.GetApplicationParts(assembly))
            {
                mvcBuilder.PartManager.ApplicationParts.Add(part);
            }

            var relatedAssemblies = RelatedAssemblyAttribute.GetRelatedAssemblies(assembly, throwOnError: false);
            foreach (var relatedAssembly in relatedAssemblies)
            {
                partFactory = ApplicationPartFactory.GetApplicationPartFactory(relatedAssembly);
                foreach (var part in partFactory.GetApplicationParts(relatedAssembly))
                {
                    mvcBuilder.PartManager.ApplicationParts.Add(part);
                }
            }
        }

        private static Task HandleRemoteLoginFailure(RemoteFailureContext ctx)
        {
            ctx.Response.Redirect("/logOff");
            ctx.HandleResponse();
            return Task.CompletedTask;
        }

        public static IServiceCollection AddCustomizedIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIdentity<UserProfile, Role>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequiredUniqueChars = 1;
                    options.User.RequireUniqueEmail = true;
                    options.ClaimsIdentity.UserNameClaimType = JwtRegisteredClaimNames.Sub;
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(20 * 365);
                    options.Lockout.AllowedForNewUsers = false;

                })
                .AddRoleStore<SimplRoleStore>()
                .AddUserStore<SimplUserStore>()
                .AddSignInManager<SimplSignInManager<UserProfile>>()
                .AddUserManager<SimplUserManager<UserProfile>>()
                .AddDefaultTokenProviders()
                .AddPasswordValidator<CustPasswordValidator<UserProfile>>();

            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(24));

            var rsaCertificate = new X509Certificate2(Path.Combine(HCF.Infrastructure.GlobalConfiguration.ContentRootPath, "rsaCert.pfx"), "1234");

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                 .AddInMemoryIdentityResources(IdentityServerConfig.Ids)
                 .AddInMemoryApiResources(IdentityServerConfig.Apis)
                 .AddInMemoryClients(IdentityServerConfig.Clients)
                 .AddAspNetIdentity<UserProfile>()
                 .AddProfileService<SimplProfileService>()
                 .AddSigningCredential(rsaCertificate);

            var authenticationBuilder = services.AddAuthentication(AuthenticationDefaults.AuthenticationScheme);


            authenticationBuilder.AddCookie(AuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = $"{HcfCookieDefaults.Prefix}{HcfCookieDefaults.AuthenticationCookie}";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.LoginPath = AuthenticationDefaults.LoginPath;
                options.LogoutPath = AuthenticationDefaults.LogoutPath;
                options.AccessDeniedPath = AuthenticationDefaults.AccessDeniedPath;
                options.EventsType = typeof(CustomCookieAuthenticationEvents);
                options.Cookie.SameSite = SameSiteMode.Lax;

            })

                .AddLocalApi(JwtBearerDefaults.AuthenticationScheme, option =>
                {
                    option.ExpectedScope = "api.crxweb";
                })
            .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"));


            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = AuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = "oidc";
            //})
            //.AddCookie(AuthenticationDefaults.AuthenticationScheme)
            //.AddOpenIdConnect("oidc", options =>
            //{
            //    options.Authority = "https://localhost:44325";
            //    options.ClientId = "crxweb";
            //    options.ClientSecret = "secret";
            //    options.ResponseType = "code";
            //    options.SaveTokens = true;
            //    options.Scope.Add("email");
            //    options.Events = new OpenIdConnectEvents
            //    {
            //        OnTokenValidated = context =>
            //        {
            //            var idToken = context.SecurityToken;
            //            string userIdentifier = idToken.Subject;
            //            return Task.CompletedTask;
            //        },
            //        OnAuthenticationFailed = context =>
            //        {
            //            context.Response.Redirect("/Home/Error");
            //            context.HandleResponse(); // Suppress the exception
            //            return Task.CompletedTask;
            //        },
            //    };
            //}).AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"));



            services.AddScoped<CustomCookieAuthenticationEvents>();

            services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = ctx =>
                    {
                        //var claims = new List<Claim>
                        // {
                        //        new Claim(ClaimTypes.Role, "Admin")
                        // };
                        //var appIdentity = new ClaimsIdentity(claims);
                        //ctx.Principal.AddIdentity(appIdentity);
                        string userEmail = ctx.Principal.Identity.Name;

                        var returnpath = configuration["AzureAd:CallbackPath"]; //"microsoft/auth/sucess/"
                        ctx.Response.Redirect(returnpath + userEmail);
                        ctx.HandleResponse();
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.Redirect("/logout");
                        context.HandleResponse(); // Suppress the exception
                        return Task.CompletedTask;
                    },
                    OnRemoteFailure = context =>
                    {
                        context.Response.Redirect("/logout");
                        context.HandleResponse(); // Suppress the exception
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddRazorPages()
                 .AddMicrosoftIdentityUI();

            services.ConfigureApplicationCookie(x =>
            {
                x.LoginPath = new PathString("/login");
                x.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase) && context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    }

                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
                x.Events.OnRedirectToAccessDenied = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase) && context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Task.CompletedTask;
                    }

                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            });
            return services;
        }

    }
}
