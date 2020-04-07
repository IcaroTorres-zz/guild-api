using Business.Commands.Invites;
using Domain.Entities;
using Domain.Entities.Nulls;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Requests.Invites
{
	public class AcceptInviteValidator : AbstractValidator<AcceptInviteCommand>
	{
		public AcceptInviteValidator(IInviteRepository inviteRepository,
			IMemberRepository memberRepository,
			IGuildRepository guildRepository)
		{
			RuleFor(x => x.Id)
				.NotEmpty()
				.MustAsync(async (id, _) => await inviteRepository.ExistsWithIdAsync(id))
				.WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Invite), x.Invite.Id));

			RuleFor(x => x.Invite)
				.NotEmpty().NotEqual(new NullInvite())
				.WithMessage("Invite was null or empty.");

			RuleFor(x => x.Invite.Status).IsInEnum().Equal(InviteStatuses.Pending);

			RuleFor(x => x.Invite.MemberId)
				.NotEmpty()
				.MustAsync(async (memberId, _) => await memberRepository.ExistsWithIdAsync(memberId))
				.WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Member), x.Invite.MemberId));

			RuleFor(x => x.Invite.GuildId)
				.NotEmpty()
				.MustAsync(async (guildId, _) => await guildRepository.ExistsWithIdAsync(guildId))
				.WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Guild), x.Invite.GuildId));
		}
	}
}