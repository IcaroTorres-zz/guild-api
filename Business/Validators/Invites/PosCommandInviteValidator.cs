using Business.Commands;
using Domain.Entities;
using FluentValidation;
using System.Linq;

namespace Business.Validators.Invites
{
    public class PosCommandInviteValidator : AbstractValidator<ApiResponse<Invite>>
    {
        public PosCommandInviteValidator()
        {
            RuleFor(x => x.Value.Member.Id).Equal(x => x.Value.MemberId);

            RuleFor(x => x.Value.Guild.Id).Equal(x => x.Value.GuildId);

            RuleFor(x => x)
                .Must(x => x.Value.Member.Memberships
                    .Any(ms => ms.MemberId == x.Value.MemberId && ms.GuildId == x.Value.GuildId))
                .When(x => x.Value.Status == InviteStatuses.Accepted &&
                           x.Value.Member != null &&
                           x.Value.Member.Memberships != null)
                .WithErrorCode(CommonValidations.ConflictCodeString)
                .WithMessage($"The {nameof(Member)} should have a related {nameof(Membership)}" +
                    $" representing an accepted invite.");
        }
    }
}
