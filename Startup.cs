using Domain.Services;
using Domain.Unities;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Services;
using DataAccess.Unities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using Application.Extensions;
using Application.Swagger;

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
                    .UseSqlite($"Data Source={AppHost.ContentRootPath}/{Configuration["SqliteSettings:Source"]}")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)))

                // Custom service layer dependecy registration
                .AddScoped<IGuildService, GuildService>()
                .AddScoped<IMemberService, MemberService>()
                .AddScoped<IInviteService, InviteService>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<NullEntityFactory>()

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
                .AddHateoasResources(options => options
                .AddLink<List<Guild>>("get-guilds")
                .AddLink<List<Guild>>("create-guild")

                .AddLink<Guild>("get-guild", e => new { id = e.Id })
                .AddLink<Guild>("get-members", e => new { guildId = e.Id })
                .AddLink<Guild>("update-guild", e => new { id = e.Id })
                .AddLink<Guild>("patch-guild", e => new { id = e.Id })
                .AddLink<Guild>("delete-guild", e => new { id = e.Id })

                .AddLink<List<Member>>("get-members")
                .AddLink<List<Member>>("create-member")
                .AddLink<List<Member>>("invite-member")

                .AddLink<Member>("get-member", e => new { id = e.Id })
                .AddLink<Member>("get-guild", e => new { id = e.GuildId })
                .AddLink<Member>("update-member", e => new { id = e.Id })
                .AddLink<Member>("patch-member", e => new { id = e.Id })
                .AddLink<Member>("promote-member", e => new { id = e.Id })
                .AddLink<Member>("demote-member", e => new { id = e.Id })
                .AddLink<Member>("leave-guild", e => new { id = e.Id })
                .AddLink<Member>("delete-member", e => new { id = e.Id })

                .AddLink<List<Invite>>("get-invites")
                .AddLink<List<Invite>>("invite-member")

                .AddLink<Invite>("get-invite", e => new { id = e.Id })
                .AddLink<Invite>("accept-invite", e => new { id = e.Id })
                .AddLink<Invite>("decline-invite", e => new { id = e.Id })
                .AddLink<Invite>("cancel-invite", e => new { id = e.Id })
                .AddLink<Invite>("delete-invite", e => new { id = e.Id })
                .AddLink<Invite>("get-guild", e => new { id = e.GuildId })
                .AddLink<Invite>("get-member", e => new { id = e.MemberId }))
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
