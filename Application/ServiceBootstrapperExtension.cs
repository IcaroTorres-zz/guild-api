using DataAccess.Context;
using Services;
using DataAccess.Unities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Models.NullEntities;

namespace Application.Extensions
{
    public static class ServiceBootstrapperExtension
    {
        public static IServiceCollection BootstrapServicesRegistration(this IServiceCollection services, IHostingEnvironment appHost, IConfiguration configuration)
        {
            // DbContext dependency registration
            return services
                .AddEntityFrameworkSqlite()
                .AddDbContext<ApiContext>(options => options
                    .UseSqlite($"Data Source={appHost.ContentRootPath}/{configuration["SqliteSettings:Source"]}")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)))

                // Custom service layer dependecy registration
                .AddScoped<IGuildService, GuildService>()
                .AddScoped<IMemberService, MemberService>()
                .AddScoped<IInviteService, InviteService>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<NullEntityFactory>();
        }
    }
}