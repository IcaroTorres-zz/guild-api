using Business.Commands;
using Domain.Entities;
using FluentValidation;
using System.Linq;

namespace Business.Validators.Guilds
{
    public class PosCommandGuildValidator : AbstractValidator<ApiResponse<Guild>>
    {
        public PosCommandGuildValidator()
        {
            RuleFor(x => x.Value.Id).NotEmpty();

            RuleFor(x => x)
                .Must(x => !x.Value.Members.Except(x.Value.Invites.Select(i => i.Member).Distinct()).Any())
                .WithMessage($"Not all members matching members from related guild invites.");

            RuleFor(x => x.Value.Members)
                .NotEmpty()
                .Must(x => x.Any(m => m.IsGuildMaster))
                .When(x => x.Value.Members.Any());

            RuleFor(x => x.Value.Invites)
                .NotEmpty()
                .Must(invites => invites.Any(invites => invites.Status == InviteStatuses.Accepted))
                .When(x => x.Value.Invites.Any());
        }
    }
}
