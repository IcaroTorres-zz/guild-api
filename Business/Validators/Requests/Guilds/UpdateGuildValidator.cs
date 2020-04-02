using Business.Commands.Guilds;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using System.Linq;

namespace Business.Validators.Requests.Guilds
{
  public class UpdateGuildCommandValidator : AbstractValidator<UpdateGuildCommand>
  {
    public UpdateGuildCommandValidator(IGuildRepository guildRepository, IMemberRepository memberRepository)
    {
      RuleFor(x => x.Id)
          .NotEmpty()
          .MustAsync(async (id, _) => await guildRepository.ExistsWithIdAsync(id))
          .WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Guild), x.Id));

      RuleFor(x => x.Name)
          .NotEmpty()
          .MustAsync(async (name, _) => !await memberRepository.ExistsWithNameAsync(name))
          .WithMessage(x => CommonValidationMessages.ForConflictWithKey(nameof(Guild), x.Name))
          .Unless(x => x.Id.Equals(memberRepository.Query().SingleOrDefault(y => y.Name.Equals(x.Name)).Id));

      RuleFor(x => x.MasterId)
          .NotEmpty()
          .MustAsync(async (masterId, _) => await memberRepository.ExistsWithIdAsync(masterId))
          .WithMessage("Guild must have a valid member as guild master.");

      RuleFor(x => x)
          .MustAsync(async (x, _) => (await memberRepository.GetByIdAsync(x.MasterId)).GuildId.Equals(x.Id))
          .WithMessage("Guild Master must be a Member of target Guild.");
    }
  }
}
