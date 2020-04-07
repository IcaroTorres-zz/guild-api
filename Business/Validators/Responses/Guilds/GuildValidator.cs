using System;
using System.Linq;
using Business.Responses;
using Domain.Entities;
using Domain.Entities.Nulls;
using FluentValidation;

namespace Business.Validators.Responses.Guilds
{
	public class GuildValidator : AbstractValidator<ApiResponse<Guild>>
	{
		public GuildValidator()
		{
			RuleFor(x => x.Data).NotEmpty().NotEqual(new NullGuild());
			RuleFor(x => x.Data.Id).NotEmpty().NotEqual(Guid.Empty);

			RuleFor(x => x)
				.Must(x =>
				{
					var members = x.Data.Members;
					var inviteMembers = x.Data.Invites.Select(i => i.Member).Distinct();
					return members.Intersect(inviteMembers).Count() == members.Count();
				}).WithMessage("Guild having Members with no related Invite record.");

			RuleFor(x => x.Data.Members)
				.Must(x => x.Any(m => m.IsGuildMaster))
				.When(x => x.Data.Members.Any());

			RuleFor(x => x.Data.Invites)
				.Must(invites => invites.Any(i => i.Status.Equals(InviteStatuses.Accepted)))
				.When(x => x.Data.Invites.Any())
				.WithMessage("No matching accepted Invite was found.");
		}
	}
}