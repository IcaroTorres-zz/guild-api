using Business.Commands.Guilds;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Guilds
{
    public class CreateGuildCommandValidator : AbstractValidator<CreateGuildCommand>
    {
        public CreateGuildCommandValidator(IGuildRepository guildRepository, IMemberRepository memberRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MustAsync(async (name, _) => !await guildRepository.ExistsWithNameAsync(name))
                .WithMessage(x => CommonValidations.ForConflictWithKey(nameof(Guild), x.Name));

            RuleFor(x => x.MasterId)
                .NotEmpty()
                .MustAsync(async (masterId, _) => await memberRepository.ExistsWithIdAsync(masterId))
                .WithMessage("No member was used to created this guild as guildmaster.");
        }
    }
}
