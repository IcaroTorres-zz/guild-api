using Domain.Enums;
using Domain.Models;
using Domain.Models.Nulls;
using Domain.Repositories;
using FluentValidation;

namespace Business.Usecases.Invites.DenyInvite
{
    public class DenyInviteValidator : AbstractValidator<DenyInviteCommand>
    {
        public DenyInviteValidator(IInviteRepository inviteRepository)
        {
            RuleFor(x => x.Id).NotEmpty().DependentRules(() =>
            {
                Invite invite = Invite.Null;

                RuleFor(x => x.Id)
                    .MustAsync(async (id, ct) =>
                    {
                        invite = await inviteRepository.GetByIdAsync(id, readOnly: true, ct);
                        return !(invite is INullObject);
                    })
                    .WithMessage(x => $"Record not found for invite with given id {x.Id}.")
                    .Must(_ => invite.Status == InviteStatuses.Pending)
                    .WithMessage("Invite must be on pending status to be denied.")
                    .WithName(nameof(invite.Status));
            });
        }
    }
}