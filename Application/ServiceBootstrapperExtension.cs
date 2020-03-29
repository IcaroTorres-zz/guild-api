using Business.Validators;
using Business.Services;
using DataAccess.Context;
using DataAccess.Repositories;
using DataAccess.Unities;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Domain.Unities;
using FluentValidation;
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

                // Data access layer dependencies
                .AddScoped<IRepository<Guild>, Repository<Guild>>()
                .AddScoped<IRepository<Member>, Repository<Member>>()
                .AddScoped<IRepository<Invite>, Repository<Invite>>()
                .AddScoped<IGuildRepository, GuildRepository>()
                .AddScoped<IMemberRepository, MemberRepository>()
                .AddScoped<IInviteRepository, InviteRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>()

                // Custom service layer dependecy registration
                .AddScoped<IGuildService, GuildService>()
                .AddScoped<IMemberService, MemberService>()
                .AddScoped<IInviteService, InviteService>()

                // Domain ModelFactory and validations
                .AddScoped<ModelFactory>()
                .AddScoped<IValidator<Guild>, GuildValidator>()
                .AddScoped<IValidator<Member>, MemberValidator>()
                .AddScoped<IValidator<Invite>, InviteValidator>()
                .AddScoped<IValidator<Membership>, MembershipValidator>();
        }
    }
}