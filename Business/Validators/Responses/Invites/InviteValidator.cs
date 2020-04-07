using System.Linq;
using Business.Responses;
using Domain.Entities;
using FluentValidation;

namespace Business.Validators.Responses.Invites
{
	public class InviteValidator : AbstractValidator<ApiResponse<Invite>>
	{
		public InviteValidator()
		{
			RuleFor(x => x.Data.Member.Id).Equal(x => x.Data.MemberId);

			RuleFor(x => x.Data.Guild.Id).Equal(x => x.Data.GuildId);

			RuleFor(x => x)
				.Must(x =>
				{
					return x.Data.Member.Memberships.Any(ms =>
						ms.MemberId == x.Data.MemberId &&
						ms.GuildId == x.Data.GuildId);
				})
				.When(x => x.Data.Status.Equals(InviteStatuses.Accepted) &&
				           x.Data.Member != null &&
				           x.Data.Member.Memberships != null)
				.WithErrorCode(CommonValidationMessages.ConflictCodeString)
				.WithMessage(
					$"The {nameof(Member)} should have a related {nameof(Membership)} representing an accepted invite.");
		}
	}
}