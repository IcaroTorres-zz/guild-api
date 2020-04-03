using Business.Commands.Members;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using System;
using System.Linq;

namespace Business.Validators.Requests.Members
{
  public class UpdateMemberValidator : AbstractValidator<UpdateMemberCommand>
  {
    public UpdateMemberValidator(IMemberRepository memberRepository, IGuildRepository guildRepository)
    {
      // record for given key must exist 
      RuleFor(x => x.Id)
          .NotEmpty()
          .MustAsync(async (id, _) => await memberRepository.ExistsWithIdAsync(id))
          .WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Member), x.Id));

      // Name can not match a record unless if it self 
      RuleFor(x => x.Name)
          .NotEmpty()
          .MustAsync(async (name, _) => !await memberRepository.ExistsWithNameAsync(name))
          .WithMessage(x => CommonValidationMessages.ForConflictWithKey(nameof(Member), x.Name))
          .Unless(x => x.Id.Equals(memberRepository.Query().SingleOrDefault(y => y.Name.Equals(x.Name)).Id));

      // guild record must exists if provided
      RuleFor(x => x.GuildId)
          .MustAsync(async (guildId, _) => await guildRepository.ExistsWithIdAsync(guildId))
          .WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Guild), x.GuildId))
          .When(x => x.GuildId != Guid.Empty && x.GuildId != null);
    }
  }
}
