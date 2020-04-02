using Application.Cache;
using Application.Extensions;
using Application.Hateoas;
using Application.Middlewares;
using Application.Swagger;
using Business.Validators.Guilds;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

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
            services
                // enable access to Current HttpContext
                .AddHttpContextAccessor()

                // swagger
                .AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Guild-api", Version = "v1" }))

                // Enabling distributed Redis Cache
                .AddRedisResponseCacheService(Configuration)

                // IoC registration of implemented application services
                .BootstrapServicesRegistration(HostEnvironment, Configuration)

                // enabling Mvc framework services and resources
                .AddMvcCore(options => options.EnableEndpointRouting = false)

                // enabling validations

                .AddFluentValidation(fv =>
                {
                    fv.ImplicitlyValidateChildProperties = true;
                    fv.RegisterValidatorsFromAssemblyContaining<CreateGuildCommandValidator>();
                })

                // Default framework order
                .AddFormatterMappings()
                .AddCacheTagHelper()
                .AddDataAnnotations()
                .AddApiExplorer()

                // new integration with newtonsoft json net for net core 3.0 +
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                })

                // custom hateoas resouces options for JsonHateoasFormatter
                .EnableHateoasOutput()

                .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName.ToLowerInvariant() == "development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var swaggerOptions = new MySwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app
                // exception handling as Internal server error output
                .UseMiddleware(typeof(ExceptionHandlingMiddleware))

                // swagger
                .UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; })
                .UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
                })

                // redirection and mvc resources
                .UseHttpsRedirection()
                .UseMvc();
        }
    }
}
