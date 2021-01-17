using Application.Extensions;
using Application.Identity;
using Application.MapperProfiles;
using Application.Middlewares;
using Application.Swagger;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Persistence.Context;
using Persistence.MapperProfiles;
using System.IO.Compression;

namespace Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services
                .BootstrapSwaggerConfig(HostEnvironment)
                .BootstrapCacheService(Configuration)
                .BootstrapPersistenceServices(HostEnvironment, Configuration)
                .BootstrapIdentity(HostEnvironment, Configuration)
                .BootstrapPipelinesServices()
                .BootstrapHateoas()
                .AddAutoMapper(cfg => cfg.AddExpressionMapping(), typeof(DomainToApplicationProfile), typeof(NullObjectToDataFilterProfile))
                .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
                .AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>())
                .AddControllers()
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
                .BootstrapValidators()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IdentityContext identityContext, ApiContext apiContext)
        {
            // for development purposes, migrate any database changes on startup (includes initial db creation)
            //apiContext.Database.Migrate();
            identityContext.Database.Migrate();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            else app.UseHsts();

            var swaggerOptions = new MySwaggerOptions();
            Configuration.GetSection(nameof(swaggerOptions)).Bind(swaggerOptions);

            app.UseMiddleware(typeof(ExceptionMiddleware));

            if (!env.IsProduction())
            {
                app.UseSwagger(option => option.RouteTemplate = swaggerOptions.JsonRoute)
                   .UseSwaggerUI(option => option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description));
            }

            app.UseHttpsRedirection()
               .UseRouting()
               .UseAuthentication()
               .UseAuthorization()
               .UseEndpoints(e => e.MapControllers());
        }
    }
}
