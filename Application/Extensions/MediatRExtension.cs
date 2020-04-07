using System.Reflection;
using Business.Behaviors;
using Business.Commands.Guilds;
using Business.Commands.Invites;
using Business.Commands.Members;
using Business.Handlers;
using Business.Handlers.Guilds;
using Business.Handlers.Invites;
using Business.Handlers.Members;
using Business.Responses;
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

				// mediatR pre-request open-type pipeline behaviors
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>))
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>))

				// mediatR handlers
				.AddTransient<IRequestHandler<MemberFilterCommand, ApiResponse<Pagination<Member>>>,
					MemberFilterHandler>()
				.AddTransient<IRequestHandler<GuildFilterCommand, ApiResponse<Pagination<Guild>>>, GuildFilterHandler>()
				.AddTransient<IRequestHandler<InviteFilterCommand, ApiResponse<Pagination<Invite>>>,
					InviteFilterHandler>()
				.AddTransient<IRequestHandler<CreateMemberCommand, ApiResponse<Member>>, CreateMemberHandler>()
				.AddTransient<IRequestHandler<CreateGuildCommand, ApiResponse<Guild>>, CreateGuildHandler>()
				.AddTransient<IRequestHandler<InviteMemberCommand, ApiResponse<Invite>>, InviteMemberHandler>()
				.AddTransient<IRequestHandler<UpdateMemberCommand, ApiResponse<Member>>, UpdateMemberHandler>()
				.AddTransient<IRequestHandler<UpdateGuildCommand, ApiResponse<Guild>>, UpdateGuildHandler>()
				.AddTransient<IRequestHandler<PromoteMemberCommand, ApiResponse<Member>>, PromoteMemberHandler>()
				.AddTransient<IRequestHandler<DemoteMemberCommand, ApiResponse<Member>>, DemoteMemberHandler>()
				.AddTransient<IRequestHandler<LeaveGuildCommand, ApiResponse<Member>>, LeaveGuildHandler>()
				.AddTransient<IRequestHandler<AcceptInviteCommand, ApiResponse<Invite>>, AcceptInviteHandler>()
				.AddTransient<IRequestHandler<DeclineInviteCommand, ApiResponse<Invite>>, DeclineInviteHandler>()
				.AddTransient<IRequestHandler<CancelInviteCommand, ApiResponse<Invite>>, CancelInviteHandler>()
				
				// mediatR pos-request open-type pipeline behaviors
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(ResponseValidationBehavior<,>));
		}
	}
}