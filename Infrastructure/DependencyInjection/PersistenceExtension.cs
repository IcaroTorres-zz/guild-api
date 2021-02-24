using Application.Common.Abstractions;
using Domain.Models;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Infrastructure.DependencyInjection
{
    public static class PersistenceExtension
    {
        public static IServiceCollection BootstrapPersistenceServices(this IServiceCollection services, IWebHostEnvironment env,
            IConfiguration configuration)
        {
            return services
                // DbContext dependency registration
                .AddDbContext<ApiContext>(options =>
                {
                    var sourcePath = Path.Combine(env.ContentRootPath, "../Infrastructure/Persistence", configuration["SQLiteSettings:Source"]);
                    options.UseSqlite($"Data Source={sourcePath}");
                    if (!env.IsProduction()) options.EnableSensitiveDataLogging();
                })

                // Data access layer dependencies
                .AddScoped<IRepository<Guild>, NullObjectProxyRepository<Guild, Persistence.Entities.Guild>>()
                .AddScoped<IRepository<Member>, NullObjectProxyRepository<Member, Persistence.Entities.Member>>()
                .AddScoped<IRepository<Invite>, NullObjectProxyRepository<Invite, Persistence.Entities.Invite>>()
                .AddScoped<IRepository<Membership>, NullObjectProxyRepository<Membership, Persistence.Entities.Membership>>()
                .AddScoped<IGuildRepository, GuildRepository>()
                .AddScoped<IMemberRepository, MemberRepository>()
                .AddScoped<IInviteRepository, InviteRepository>()
                .AddScoped<IMembershipRepository, MembershipRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}