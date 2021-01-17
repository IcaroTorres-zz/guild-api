using Domain.Enums;
using Domain.Models;
using Domain.Models.Nulls;
using Domain.Repositories;
using FluentValidation;

namespace Business.Usecases.Invites.AcceptInvite
{
    public class AcceptInviteValidator : AbstractValidator<AcceptInviteCommand>
    {
        public AcceptInviteValidator(IInviteRepository inviteRepository)
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
                    .WithMessage("Invite must be on pending status to be accepted.")
                    .WithName(nameof(invite.Status))
                    .Must(_ => !(invite.Member is INullObject))
                    .WithMessage(_ => $"Record not found for invited member with given id {invite.MemberId}.")
                    .WithName(nameof(Invite.Member))
                    .Must(_ => !(invite.Guild is INullObject))
                    .WithMessage(_ => $"Record not found for inviting guild with given id {invite.GuildId}.")
                    .WithName(nameof(Invite.Guild));
            });
        }
    }
}