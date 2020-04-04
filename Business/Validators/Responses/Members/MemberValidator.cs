using System;
using System.Linq;
using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Responses.Members
{
	public class MemberValidator : AbstractValidator<ApiResponse<Member>>
	{
		public MemberValidator(IMemberRepository memberRepository, IGuildRepository guildRepository)
		{
			RuleFor(x => x.Value.Name).NotEmpty();

			RuleFor(x => x.Value)
				.Must(x => x.Memberships.All(m => m.MemberId.Equals(x.Id)))
				.WithErrorCode(CommonValidationMessages.ConflictCodeString)
				.WithMessage(x =>
					$"Not all {nameof(Membership)}s with '{nameof(Membership.MemberId)}' matching '{x.Value.Id}'.");

			RuleFor(x => x.Value.GuildId).NotEmpty().Unless(x => x.Value.Guild is null);

			RuleFor(x => x.Value.GuildId)
				.MustAsync(async (guildId, _) => await guildRepository.ExistsWithIdAsync(guildId.Value))
				.WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Guild), x.Value.GuildId.Value))
				.When(x => x.Value.GuildId.HasValue);

			RuleFor(x => x.Value.Guild.Id)
				.Equal(x => x.Value.GuildId.Value)
				.WithMessage(x =>
					$"{nameof(Member.GuildId)} and {nameof(Guild)}.{nameof(Member.Guild.Id)} not matching.")
				.Unless(x => x.Value.Guild is null);

			RuleFor(x => x)
				.Must(x => x.Value.Memberships.Any(ms
					=> ms.MemberId == x.Value.Id
					   && ms.GuildId == x.Value.GuildId
					   && ms.Until == null
					   && !ms.Disabled))
				.WithMessage(x =>
					$"{nameof(Member)} missing active {nameof(Membership)} for {nameof(Member.GuildId)} '{x.Value.GuildId}'.")
				.When(x => x.Value.Memberships.Any() && x.Value.GuildId.HasValue &&
				           !x.Value.GuildId.Equals(Guid.Empty));
		}
	}
}