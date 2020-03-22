using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Application.Extensions;
using Application.Swagger;
using Application.Middlewares;
using Application.Cache;
using Application.Hateoas;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment appHost)
        {
            Configuration = configuration;
            AppHost = appHost;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment AppHost { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                // enable access to Current HttpContext
                .AddHttpContextAccessor()

                // IoC registration of implemented application services
                .BootstrapServicesRegistration(AppHost, Configuration)

                // swagger
                .AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Guild.api", Version = "v1" }); })

                // Enabling distributed Redis Cache
                .AddRedisResponseCacheService(Configuration)

                // enabling Mvc framework services and resources
                .AddMvcCore()
                .AddApiExplorer()
                .AddJsonFormatters(options =>
                {
                    options.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.Formatting = Newtonsoft.Json.Formatting.Indented;
                })

                // custom hateoas resouces options for JsonHateoasFormatter
                .EnableHateoasOutput()

                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
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

            // exception handling as Internal server error output
            app.UseMiddleware(typeof(ExceptionHandlingMiddleware))

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
