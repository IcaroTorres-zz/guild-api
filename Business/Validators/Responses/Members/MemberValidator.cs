using System;
using System.Linq;
using Business.Responses;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Responses.Members
{
	public class MemberValidator : AbstractValidator<ApiResponse<Member>>
	{
		public MemberValidator(IGuildRepository guildRepository)
		{
			RuleFor(x => x.Data.Name).NotEmpty();

			RuleFor(x => x.Data)
				.Must(x => x.Memberships.All(m => m.MemberId.Equals(x.Id)))
				.WithMessage(x =>
					$"Not all {nameof(Membership)}s with '{nameof(Membership.MemberId)}' matching '{x.Data.Id}'.");

			RuleFor(x => x.Data.GuildId).NotEmpty().Unless(x => x.Data.Guild is null);

			RuleFor(x => x.Data.GuildId)
				.MustAsync(async (guildId, _) => await guildRepository.ExistsWithIdAsync(guildId.Value))
				.WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Guild), x.Data.GuildId.Value))
				.When(x => x.Data.GuildId.HasValue);

			RuleFor(x => x.Data.Guild.Id)
				.Equal(x => x.Data.GuildId.Value)
				.WithMessage(x =>
					$"{nameof(Member.GuildId)} and {nameof(Guild)}.{nameof(Member.Guild.Id)} not matching.")
				.Unless(x => x.Data.Guild is null);

			RuleFor(x => x)
				.Must(x => x.Data.Memberships.Any(ms
					=> ms.MemberId == x.Data.Id
					   && ms.GuildId == x.Data.GuildId
					   && ms.Until == null
					   && !ms.Disabled))
				.WithMessage(x =>
					$"{nameof(Member)} missing active {nameof(Membership)} for {nameof(Member.GuildId)} '{x.Data.GuildId}'.")
				.When(x => x.Data.Memberships.Any() && x.Data.GuildId.HasValue &&
				  !x.Data.GuildId.Equals(Guid.Empty));
		}
	}
}