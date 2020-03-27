using Business.Services;
using DataAccess.Context;
using DataAccess.Unities;
using Domain.Services;
using Domain.Unities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ServiceBootstrapperExtension
    {
        public static IServiceCollection BootstrapServicesRegistration(this IServiceCollection services, IHostingEnvironment appHost, IConfiguration configuration)
        {
            return services
                // DbContext dependency registration
                .AddEntityFrameworkSqlite()
                .AddDbContext<ApiContext>(options => options
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .UseSqlite($"Data Source={appHost.ContentRootPath}/{configuration["SqliteSettings:Source"]}"))

                // Custom service layer dependecy registration
                .AddScoped<IGuildService, GuildService>()
                .AddScoped<IMemberService, MemberService>()
                .AddScoped<IInviteService, InviteService>()
                .AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}