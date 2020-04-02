using Business.Commands.Invites;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Invites
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
                .WithMessage(x => CommonValidations.ForRecordNotFound(nameof(Invite), x.Invite.Id));

            RuleFor(x => x.Invite.Status).IsInEnum().Equal(InviteStatuses.Pending);

            RuleFor(x => x.Invite.MemberId)
                .NotEmpty()
                .MustAsync(async (memberId, _) => await memberRepository.ExistsWithIdAsync(memberId))
                .WithMessage(x => CommonValidations.ForRecordNotFound(nameof(Member), x.Invite.MemberId));

            RuleFor(x => x.Invite.GuildId)
                .NotEmpty()
                .MustAsync(async (guildId, _) => await guildRepository.ExistsWithIdAsync(guildId))
                .WithMessage(x => CommonValidations.ForRecordNotFound(nameof(Guild), x.Invite.GuildId));
        }
    }
}
