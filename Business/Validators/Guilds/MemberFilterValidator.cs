using Business.Commands.Guilds;
using FluentValidation;

namespace Business.Validators.Guilds
{
    public class GuildFilterValidator : AbstractValidator<GuildFilterCommand>
    {
        public GuildFilterValidator()
        {
            RuleFor(x => x.Count).GreaterThan(0);
        }
    }
}
