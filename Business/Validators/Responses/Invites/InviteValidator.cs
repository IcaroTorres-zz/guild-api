using System.Linq;
using Business.ResponseOutputs;
using Domain.Entities;
using FluentValidation;

namespace Business.Validators.Responses.Invites
{
	public class InviteValidator : AbstractValidator<ApiResponse<Invite>>
	{
		public InviteValidator()
		{
			RuleFor(x => x.Value.Member.Id).Equal(x => x.Value.MemberId);

			RuleFor(x => x.Value.Guild.Id).Equal(x => x.Value.GuildId);

			RuleFor(x => x)
				.Must(x =>
				{
					return x.Value.Member.Memberships.Any(ms =>
						ms.MemberId == x.Value.MemberId &&
						ms.GuildId == x.Value.GuildId);
				})
				.When(x => x.Value.Status == InviteStatuses.Accepted &&
				           x.Value.Member != null &&
				           x.Value.Member.Memberships != null)
				.WithErrorCode(CommonValidationMessages.ConflictCodeString)
				.WithMessage(
					$"The {nameof(Member)} should have a related {nameof(Membership)} representing an accepted invite.");
		}
	}
}