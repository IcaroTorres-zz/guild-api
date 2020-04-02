
using Business.Commands.Invites;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Invites
{
    public class CancelInviteValidator : AbstractValidator<CancelInviteCommand>
    {
        public CancelInviteValidator(IInviteRepository inviteRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .MustAsync(async (id, _) => await inviteRepository.ExistsWithIdAsync(id))
                .WithMessage(x => CommonValidations.ForConflictWithKey(nameof(Invite), x.Id));

            RuleFor(x => x.Invite.Status).IsInEnum().Equal(InviteStatuses.Pending);

            RuleFor(x => x.Invite.MemberId).NotEmpty();

            RuleFor(x => x.Invite.GuildId).NotEmpty();
        }
    }
}
