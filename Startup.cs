using Context;
using Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                // enable access to Current HttpContext
                .AddHttpContextAccessor()

                // DbContext dependency registration
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<ApiContext>(options => options.UseLazyLoadingProxies().UseInMemoryDatabase($"InMemory{nameof(ApiContext)}"))
                .AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Guild.api", Version = "v1" }); })

                // Custom service layer dependecy registration
                .AddScoped<DbContext, ApiContext>()
                .AddScoped<IBaseService, BaseService>()
                .AddScoped<IGuildService, GuildService>()

                // enabling UseLazyLoadingProxies, requires AddJsonOptions to handle navigation reference looping on json serialization
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => 
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IBaseService service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebApiExceptionHandler(service);
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; })
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
