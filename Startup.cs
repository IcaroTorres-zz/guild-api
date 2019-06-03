using api.Context;
using api.Repositories;
using api.Services;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // enabling UseLazyLoadingProxies, requires AddJsonOptions to handle navigation reference looping on json serialization
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddJsonOptions(options => options.SerializerSettings
                                                      .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // your context dependency registration
            services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<ApiContext>(options => options.UseLazyLoadingProxies()
                                                                .UseInMemoryDatabase("ApiInMemoryDB"));

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Guild.api", Version = "v1" }); });

            // your repositories and unit of work dependecy registration
            services.AddTransient<IGuildRepository, GuildRepository>(); 
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();                             
            // your custom service layer dependecy registration
            services.AddTransient<IGuildService, GuildService>();                           
        }    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseWebApiExceptionHandler(_UoW);
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UiEndpoint,
                                                                swaggerOptions.Description); });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
