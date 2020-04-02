using Business.Commands.Members;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Requests.Members
{
  public class PromoteMemberValidator : AbstractValidator<PromoteMemberCommand>
  {
    public PromoteMemberValidator(IMemberRepository memberRepository, IGuildRepository guildRepository)
    {
      // record for given key must exist 
      RuleFor(x => x.Id)
          .NotEmpty()
          .MustAsync(async (id, _) => await memberRepository.ExistsWithIdAsync(id))
          .WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Member), x.Id));

      // can only promote members not being guild masters
      RuleFor(x => x.Member.IsGuildMaster)
          .Equal(false)
          .WithMessage("Member is already a Guild Master and can not be promoted.");

      // member must have a guildId foreignkey reference
      RuleFor(x => x.Member.GuildId)
          .NotEmpty()
          .WithMessage("Missing a guild key reference. Members out of a guild can not be promoted.");

      // member must have a guild to be demoted
      RuleFor(x => x.Member.Guild)
          .NotEmpty()
          .NotEqual(new NullGuild())
          .WithMessage("Members out of a guild can not be promoted.");
    }
  }
}