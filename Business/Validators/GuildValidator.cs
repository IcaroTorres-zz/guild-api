using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using System.Linq;

namespace Business.Validators
{
    public class GuildValidator : BaseValidator<Guild>
    {
        public GuildValidator(IGuildRepository repository)
        {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x)
                .Must(x => !repository.Exists(y => y.Id.Equals(x.Id)))
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"A {nameof(Guild)} with given {nameof(Guild.Id)} '{x.Id}' already exists.");

            RuleFor(x => x)
                .Must(x => !repository.Exists(y => y.Name.Equals(x.Name)))
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"A {nameof(Guild)} with given {nameof(Guild.Name)} '{x.Name}' already exists.");

            RuleFor(x => x)
                .Must(x => x.Members.All(m => m.GuildId.Equals(x.Id)))
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"Not all {nameof(Member)}s with '{nameof(Member.GuildId)}' matching '{x.Id}'.");

            RuleFor(x => x)
                .Must(x => x.Invites.All(m => m.GuildId.Equals(x.Id)))
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"Not all {nameof(Invite)}s with '{nameof(Member.GuildId)}' matching '{x.Id}'.");

            RuleFor(x => x.Members)
                .Must(x => x.Any(m => m.IsGuildMaster))
                .ForEach(memberRule => memberRule
                    .InjectValidator()
                    .NotEmpty()
                    .Must(x => !x.Disabled)
                    .Must(x => x.Guild.Invites.Any(i => i.MemberId == x.Id)))
                .WithErrorCode(_conflictCodeString)
                .When(x => x.Members.Any());

            RuleForEach(x => x.Invites)
                .InjectValidator()
                .NotEmpty()
                .Must(x => !x.Disabled)
                .WithErrorCode(_conflictCodeString)
                .Unless(x => x.Invites is null);
        }
    }
}
