using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using System;
using System.Linq;

namespace Business.Validators
{
    public class InviteValidator : BaseValidator<Invite>
    {
        public InviteValidator(IInviteRepository repository)
        {
            RuleFor(x => x)
                .Must(x => !repository.Exists(y => y.Id.Equals(x.Id)))
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"A {nameof(Guild)} with given {nameof(Guild.Id)} '{x.Id}' already exists.");

            RuleFor(x => x.Status).IsInEnum()
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"A {nameof(Guild)} with given {nameof(Guild.Id)} '{x.Id}' already exists.");

            RuleFor(x => x)
                .Must(x => x.Member.Memberships.Any(ms => ms.MemberId == x.MemberId && ms.GuildId == x.GuildId))
                .When(x => x.Status == InviteStatuses.Accepted && x.Member != null && x.Member.Memberships != null)
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"A {nameof(Guild)} with given {nameof(Guild.Id)} '{x.Id}' already exists.");

            RuleFor(x => x.Member).InjectValidator();

            RuleFor(x => x.Guild).InjectValidator();

            RuleFor(x => x.Member.Id)
                .Equal(x => x.MemberId)
                .NotEqual(Guid.Empty)
                .Unless(x => x.Member is null);

            RuleFor(x => x.Guild.Id)
                .Equal(x => x.GuildId)
                .NotEqual(Guid.Empty)
                .Unless(x => x.Guild is null);
        }
    }
}
