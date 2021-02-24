using Application.Common.Abstractions;
using Domain.Common;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using System.Net;

namespace Application.Invites.Commands.CancelInvite
{
    public class CancelInviteValidator : AbstractValidator<CancelInviteCommand>
    {
        public CancelInviteValidator(IInviteRepository inviteRepository)
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
                    .WithMessage("Invite must be on pending status to be canceled.")
                    .WithName(nameof(invite.Status))
                    .WithErrorCode(nameof(HttpStatusCode.UnprocessableEntity));
            });
        }
    }
}