using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace HCF.Web.Framework.Infrastructure.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerConfigureOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        public SwaggerConfigureOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in _provider.ApiVersionDescriptions)
            {
               
                options.SwaggerDoc(desc.GroupName, new OpenApiInfo
                {
                    Title = "CRx by HCFCompliance",
                    Version = desc.ApiVersion.ToString(),
                    Description = "To integrate using CRx REST APIs, these APIs are available: Users, Sites, Buildings, Floors, Assets, Work Orders (Work Requests) and work-order related master records like status/ sub-status. Using these APIs, you can add and update records in CRx system. We offer REST API so you can call it from any platform. Access is using a login token in the header and you can easily get that using authentication API and you need to pass the username and password in authentication API . You can also test these APIs using Postman and any other tool." + "\r\n"
                    + Environment.NewLine + "CRx has taken an approach which can significantly reduce your integration effort. Certain master data can be given to CRx team ahead of integration, CRx team will populate that on CRx end. That way you could use your own Priority codes, Work Request Status and sub status codes etc in the GET PUT POST requests and you need not map your data elements to CRx data elements for many of API calls. A detailed document is available upon request.",
                    Contact = new OpenApiContact
                    {
                        Name = "CRx by HCFCompliance",
                        Email = "crxsupport@hcfcompliance.com",
                        Url = new Uri("https://www.hcfcompliance.com/contact-us/"),
                    },
                   

                    
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "logintoken",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your token in this format {your token here} to access this API",
                });
                options.TagActionsBy(api => new[] { api.GroupName });
                options.DocInclusionPredicate((name, api) => true);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            }
        }




    }
}
