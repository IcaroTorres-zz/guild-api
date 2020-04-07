using Business.Commands.Guilds;
using Business.Commands.Invites;
using Business.Commands.Members;
using Business.Responses;
using Business.Validators.Requests.Members;
using Business.Validators.Requests.Guilds;
using Business.Validators.Requests.Invites;
using Business.Validators.Responses.Guilds;
using Business.Validators.Responses.Invites;
using Business.Validators.Responses.Members;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
	public static class FluentValidationsExtensions
	{
		public static IServiceCollection BootstrapValidators(this IServiceCollection services) => services
			// Members
			.AddTransient<IValidator<MemberFilterCommand>, MemberFilterValidator>()
			.AddTransient<IValidator<CreateMemberCommand>, CreateMemberValidator>()
			.AddTransient<IValidator<UpdateMemberCommand>, UpdateMemberValidator>()
			.AddTransient<IValidator<PromoteMemberCommand>, PromoteMemberValidator>()
			.AddTransient<IValidator<DemoteMemberCommand>, DemoteMemberValidator>()
			.AddTransient<IValidator<LeaveGuildCommand>, LeaveGuildValidator>()
			.AddTransient<IValidator<ApiResponse<Member>>, MemberValidator>()

			// Guilds
			.AddTransient<IValidator<GuildFilterCommand>, GuildFilterValidator>()
			.AddTransient<IValidator<CreateGuildCommand>, CreateGuildValidator>()
			.AddTransient<IValidator<UpdateGuildCommand>, UpdateGuildValidator>()
			.AddTransient<IValidator<ApiResponse<Guild>>, GuildValidator>()

			// Invites
			.AddTransient<IValidator<InviteFilterCommand>, InviteFilterValidator>()
			.AddTransient<IValidator<InviteMemberCommand>, InviteMemberValidator>()
			.AddTransient<IValidator<AcceptInviteCommand>, AcceptInviteValidator>()
			.AddTransient<IValidator<DeclineInviteCommand>, DeclineInviteValidator>()
			.AddTransient<IValidator<CancelInviteCommand>, CancelInviteValidator>()
			.AddTransient<IValidator<ApiResponse<Invite>>, InviteValidator>();
	}
}