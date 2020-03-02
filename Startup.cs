using Abstractions.Services;
using Abstractions.Unities;
using Cache;
using Context;
using Hateoas;
using Implementations.Entities;
using Implementations.Services;
using Implementations.Unities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

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

                // DbContext dependency registration
                .AddEntityFrameworkSqlite()
                .AddDbContext<ApiContext>(options => options
                    .UseSqlite($"Data Source={AppHost.ContentRootPath}\\{Configuration["SqliteSettings:Source"]}")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)))

                // Custom service layer dependecy registration
                .AddScoped<IBaseService, BaseService>()
                .AddScoped<IGuildService, GuildService>()
                .AddScoped<IUnitOfWork, UnitOfWork>()

                // swagger
                .AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Guild.api", Version = "v1" }); })

                // Ativando o uso de cache via Redis
                .AddRedisResponseCacheService(Configuration)

                // enabling UseLazyLoadingProxies, requires AddJsonOptions to handle navigation reference looping on json serialization
                .AddMvcCore()
                .AddApiExplorer()
                .AddJsonFormatters(options =>
                {
                    options.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.Formatting = Newtonsoft.Json.Formatting.Indented;
                })

                // custom hateoas resouces options for JsonHateoasFormatter
                .AddHateoasResources(options =>
                {
                    options.AddLink<List<Guild>>("create-guild");
                    options.AddLink<Guild>("get-guilds");
                    options.AddLink<Guild>("get-guild", e => new { id = e.Id });
                    options.AddLink<Guild>("update-guild", e => new { id = e.Id });
                    options.AddLink<Guild>("patch-guild", e => new { id = e.Id });
                    options.AddLink<Guild>("delete-enterprise", e => new { id = e.Id });
                })
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
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseWebApiExceptionHandler()
               .UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; })
               .UseSwaggerUI(option =>
               {
                   option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
               })
               .UseHttpsRedirection()
               .UseMvc();
        }
    }
}
