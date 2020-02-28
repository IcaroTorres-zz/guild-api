using Context;
using Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Unities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;

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
                    .UseLazyLoadingProxies()
                    .UseSqlite($"Data Source={AppHost.ContentRootPath}\\{Configuration["SqliteSettings:Source"]}")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)))

                // swagger
                .AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Guild.api", Version = "v1" }); })

                // Custom service layer dependecy registration
                .AddScoped<DbContext, ApiContext>()
                .AddScoped<IBaseService, BaseService>()
                .AddScoped<IGuildService, GuildService>()
                .AddScoped<IUnitOfWork, UnitOfWork>()

                // enabling UseLazyLoadingProxies, requires AddJsonOptions to handle navigation reference looping on json serialization
                .AddMvcCore()
                .AddApiExplorer()
                .AddJsonFormatters(options =>
                {
                    options.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.Formatting = Newtonsoft.Json.Formatting.Indented;
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
                    option.SwaggerEndpoint(swaggerOptions.UiEndpoint,
                                            swaggerOptions.Description);
               })
               .UseHttpsRedirection()
               .UseMvc();
        }
    }
}
