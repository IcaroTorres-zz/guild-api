using System.Reflection;
using Business.Commands.Guilds;
using Business.Commands.Invites;
using Business.Commands.Members;
using Business.Handlers;
using Business.Handlers.Guilds;
using Business.Handlers.Invites;
using Business.Handlers.Members;
using Business.ResponseOutputs;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
	public static class BusinessExtension
	{
		public static IServiceCollection BootstrapPipelinesServices(this IServiceCollection services)
		{
			return services
				// mediatR dependency injection
				.AddMediatR(typeof(CreateGuildHandler).GetTypeInfo().Assembly)

				// mediatR open types pipeline behaviors
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>))
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(ResponseValidationBehavior<,>))

				// mediatR handlers
				.AddTransient<IPipelineBehavior<MemberFilterCommand, ApiResponse<Pagination<Member>>>,
					MemberFilterHandler>()
				.AddTransient<IPipelineBehavior<GuildFilterCommand, ApiResponse<Pagination<Guild>>>, GuildFilterHandler
				>()
				.AddTransient<IPipelineBehavior<InviteFilterCommand, ApiResponse<Pagination<Invite>>>,
					InviteFilterHandler>()
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