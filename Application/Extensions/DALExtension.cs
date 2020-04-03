using Domain.Entities;
using Domain.Repositories;
using Domain.Unities;
using DAL.Context;
using DAL.Repositories;
using DAL.Unities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Application.Extensions
{
  public static class DALExtension
  {
    public static IServiceCollection BootstrapDALServices(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
    {
      return services
        // DbContext dependency registration
        .AddDbContext<ApiContext>(options =>
        {
          var sourcePath = Path.Combine(env.ContentRootPath, "../DAL", configuration["SqliteSettings:Source"]);
          options.UseSqlite($"Data Source={sourcePath}");
        })

        // Data access layer dependencies
        .AddScoped<IRepository<Guild>, Repository<Guild>>()
        .AddScoped<IRepository<Member>, Repository<Member>>()
        .AddScoped<IRepository<Invite>, Repository<Invite>>()
        .AddScoped<IRepository<Membership>, Repository<Membership>>()
        .AddScoped<IGuildRepository, GuildRepository>()
        .AddScoped<IMemberRepository, MemberRepository>()
        .AddScoped<IInviteRepository, InviteRepository>()
        .AddScoped<IUnitOfWork, UnitOfWork>();
    }
  }
}