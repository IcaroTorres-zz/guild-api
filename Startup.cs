using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lumen.api.Context;
using lumen.api.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace lumen.api
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
                    .AddDbContext<LumenContext>(options => options.UseLazyLoadingProxies()
                                                                  .UseInMemoryDatabase("lumenInMemoryDB"));
            // your repositories and unit of work dependecy registration
            services.AddTransient<IGuildRepository, GuildRepository>(); 
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();                             
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

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
