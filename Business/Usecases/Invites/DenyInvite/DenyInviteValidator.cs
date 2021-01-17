using Domain.Enums;
using Domain.Models;
using Domain.Models.Nulls;
using Domain.Repositories;
using FluentValidation;
using System.Net;

namespace Business.Usecases.Invites.DenyInvite
{
    public class DenyInviteValidator : AbstractValidator<DenyInviteCommand>
    {
        public DenyInviteValidator(IInviteRepository inviteRepository)
        {
            RuleFor(x => x.Id).NotEmpty().DependentRules(() =>
            {
                Invite invite = Invite.Null;

                RuleFor(x => x)
                    .MustAsync(async (x, ct) =>
                    {
                        invite = await inviteRepository.GetByIdAsync(x.Id, readOnly: true, ct);
                        return !(invite is INullObject);
                    })
                    .WithMessage(x => $"Record not found for invite with given id {x.Id}.")
                    .WithName(x => nameof(x.Id))
                    .WithErrorCode(nameof(HttpStatusCode.NotFound))

                    .Must(_ => invite.Status == InviteStatuses.Pending)
                    .WithMessage("Invite must be on pending status to be denied.")
                    .WithName(nameof(invite.Status))
                    .WithErrorCode(nameof(HttpStatusCode.UnprocessableEntity));
            });
        }
    }
}