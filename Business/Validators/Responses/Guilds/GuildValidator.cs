using System.Linq;
using Business.ResponseOutputs;
using Domain.Entities;
using FluentValidation;

namespace Business.Validators.Responses.Guilds
{
	public class GuildValidator : AbstractValidator<ApiResponse<Guild>>
	{
		public GuildValidator()
		{
			RuleFor(x => x.Value.Id).NotEmpty();

			RuleFor(x => x)
				.Must(x =>
				{
					var idsFromCurrentMembers = x.Value.Members.Select(m => m.Id);
					var idsFromInvitedMembers = x.Value.Invites.Select(i => i.MemberId).Distinct();
					return idsFromCurrentMembers.Intersect(idsFromInvitedMembers) == idsFromCurrentMembers;
				})
				.WithMessage("Guild having members with no related invite record.");

			RuleFor(x => x.Value.Members)
				.NotEmpty()
				.Must(x => x.Any(m => m.IsGuildMaster))
				.When(x => x.Value.Members.Any());

			RuleFor(x => x.Value.Invites)
				.NotEmpty()
				.Must(invites => invites.Any(invites => invites.Status == InviteStatuses.Accepted))
				.When(x => x.Value.Invites.Any())
				.WithMessage("No matching accepted invite was found ");
		}
	}
}