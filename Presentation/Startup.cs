using Infrastructure.DependencyInjection;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.Middlewares;
using Presentation.Swagger;
using System.IO.Compression;

namespace Presentation
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
            services.AddHttpContextAccessor()
                .BootstrapSwaggerConfig(HostEnvironment)
                .BootstrapCacheService(Configuration)
                .BootstrapPersistenceServices(HostEnvironment, Configuration)
                .BootstrapIdentity(HostEnvironment, Configuration)
                .BootstrapPipelinesServices()
                .BootstrapHateoas()
                .BootstrapAutoMapper()
                .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
                .AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>())
                .AddControllers()
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
                .BootstrapValidators()
                .BootstrapNewtonsoftJson()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IdentityContext identityContext, ApiContext apiContext)
        {
            // for development purposes, migrate any database changes on startup (includes initial db creation)
            apiContext.Database.Migrate();
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
