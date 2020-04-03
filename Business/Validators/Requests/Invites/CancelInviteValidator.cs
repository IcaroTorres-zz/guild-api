
using Business.Commands.Invites;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Requests.Invites
{
  public class CancelInviteValidator : AbstractValidator<CancelInviteCommand>
  {
    public CancelInviteValidator(IInviteRepository inviteRepository)
    {
      RuleFor(x => x.Id)
        .NotEmpty()
        .MustAsync(async (id, _) => await inviteRepository.ExistsWithIdAsync(id))
        .WithMessage(x => CommonValidationMessages.ForConflictWithKey(nameof(Invite), x.Id));

      RuleFor(x => x.Invite)
        .Must(x => x != null && x != new NullInvite())
        .WithMessage("Invite was null or empty.");

      RuleFor(x => x.Invite.Status).IsInEnum().Equal(InviteStatuses.Pending);

      RuleFor(x => x.Invite.MemberId).NotEmpty();

      RuleFor(x => x.Invite.GuildId).NotEmpty();
    }
  }
}
