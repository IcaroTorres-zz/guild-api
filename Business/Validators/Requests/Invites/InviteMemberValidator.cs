using System.Linq;
using Business.Commands.Invites;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Requests.Invites
{
	public class InviteMemberValidator : AbstractValidator<InviteMemberCommand>
	{
		public InviteMemberValidator(IMemberRepository memberRepository, IGuildRepository guildRepository)
		{
			RuleFor(x => x.MemberId)
				.NotEmpty()
				.MustAsync(async (memberId, _) => await memberRepository.ExistsWithIdAsync(memberId))
				.WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Member), x.MemberId));

			RuleFor(x => x.GuildId)
				.NotEmpty()
				.MustAsync(async (guildId, _) => await guildRepository.ExistsWithIdAsync(guildId))
				.WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Guild), x.GuildId));

			RuleFor(x => x)
				.MustAsync(async (x, _) => !await guildRepository.ExistsAsync(y =>
					y.Id.Equals(x.GuildId) &&
					y.Members.Any(m => m.Id.Equals(x.MemberId))
				))
				.WithMessage(x => string.Format(
					"{0} already in target {1} with key {2}", nameof(Member), nameof(Guild), x.GuildId));
		}
	}
}