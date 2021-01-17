using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Application.Extensions
{
    public static class SwaggerExtension
    {
        private static readonly OpenApiInfo _swaggerOpenApiInfo = new OpenApiInfo { Title = "Guild-api", Version = "v1" };

        public static IServiceCollection BootstrapSwaggerConfig(this IServiceCollection services, IWebHostEnvironment env)
        {
            if (!env.IsProduction())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(_swaggerOpenApiInfo.Version, _swaggerOpenApiInfo);

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        Type = SecuritySchemeType.ApiKey
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                    c.IncludeXmlComments(xmlPath);
                }).AddSwaggerGenNewtonsoftSupport();
            }

            return services;
        }
    }
}
