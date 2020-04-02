using Business.Commands;
using Business.Commands.Guilds;
using Business.Commands.Invites;
using Business.Commands.Members;
using Business.Handlers;
using Business.Handlers.Guilds;
using Business.Handlers.Invites;
using Business.Handlers.Members;
using Domain.Entities;
using Domain.Repositories;
using Domain.Unities;
using DAL.Context;
using DAL.Repositories;
using DAL.Unities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

namespace Application.Extensions
{
    public static class ServiceBootstrapperExtension
    {
        public static IServiceCollection BootstrapServicesRegistration(this IServiceCollection services,
            IWebHostEnvironment appHost,
            IConfiguration configuration)
        {
            return services
                // DbContext dependency registration
                .AddDbContext<ApiContext>(options =>
                {
                    var sourcePath =
                        Path.Combine(appHost.ContentRootPath, "../Infrastructure", configuration["SqliteSettings:Source"]);

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
                .AddScoped<IUnitOfWork, UnitOfWork>()

                // mediatR dependency injection
                .AddMediatR(typeof(CreateGuildHandler).GetTypeInfo().Assembly)

                // mediatR open types pipeline behaviors
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))

                // mediatR handlers
                .AddTransient<IPipelineBehavior<MemberFilterCommand, ApiResponse<Pagination<Member>>>, MemberFilterHandler>()
                .AddTransient<IPipelineBehavior<GuildFilterCommand, ApiResponse<Pagination<Guild>>>, GuildFilterHandler>()
                .AddTransient<IPipelineBehavior<InviteFilterCommand, ApiResponse<Pagination<Invite>>>, InviteFilterHandler>()

                .AddTransient<IPipelineBehavior<CreateMemberCommand, ApiResponse<Member>>, CreateMemberHandler>()
                .AddTransient<IPipelineBehavior<CreateGuildCommand, ApiResponse<Guild>>, CreateGuildHandler>()
                .AddTransient<IPipelineBehavior<InviteMemberCommand, ApiResponse<Invite>>, InviteMemberHandler>()

                .AddTransient<IPipelineBehavior<UpdateMemberCommand, ApiResponse<Member>>, UpdateMemberHandler>()
                .AddTransient<IPipelineBehavior<UpdateGuildCommand, ApiResponse<Guild>>, UpdateGuildHandler>()

                .AddTransient<IPipelineBehavior<PromoteMemberCommand, ApiResponse<Member>>, PromoteMemberHandler>()
                .AddTransient<IPipelineBehavior<DemoteMemberCommand, ApiResponse<Member>>, DemoteMemberHandler>()
                .AddTransient<IPipelineBehavior<LeaveGuildCommand, ApiResponse<Member>>, LeaveGuildHandler>()

                .AddTransient<IPipelineBehavior<AcceptInviteCommand, ApiResponse<Invite>>, AcceptInviteHandler>()
                .AddTransient<IPipelineBehavior<DeclineInviteCommand, ApiResponse<Invite>>, DeclineInviteHandler>()
                .AddTransient<IPipelineBehavior<CancelInviteCommand, ApiResponse<Invite>>, CancelInviteHandler>();
        }
    }
}