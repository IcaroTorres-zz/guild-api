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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // enable acces Current HttpContext in class within dependency injection container
            services.AddHttpContextAccessor()
                    // if < .NET Core 2.2 use this
                    //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                    // enabling UseLazyLoadingProxies, requires AddJsonOptions to handle navigation reference looping on json serialization
                    .AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddJsonOptions(options => options.SerializerSettings
                                                      .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // your context dependency registration
            services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<ApiContext>(options => options.UseLazyLoadingProxies()
                                                                .UseInMemoryDatabase($"InMemory{nameof(ApiContext)}"))

                    .AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Guild.api", Version = "v1" }); })

                    // your custom service layer dependecy registration
                    .AddTransient<DbContext, ApiContext>()
                    .AddTransient<IService<ApiContext>, Service<ApiContext>>()
                    .AddTransient<IGuildService, GuildService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IService<ApiContext> service)
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
