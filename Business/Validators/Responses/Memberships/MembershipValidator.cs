using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Responses.Memberships
{
  public class MembershipValidator : AbstractValidator<Membership>
  {
    public MembershipValidator(IGuildRepository guildRepository, IMemberRepository memberRepository)
    {
      RuleFor(x => x.Since)
          .NotEmpty()
          .WithErrorCode(CommonValidationMessages.ConflictCodeString)
          .WithMessage($"{nameof(Membership)} must have a {nameof(Membership.Since)} start date.");

      RuleFor(x => x.MemberId)
          .NotEmpty()
          .MustAsync(async (x, _) => await memberRepository.ExistsAsync(y => y.Id.Equals(x)))
          .WithErrorCode(CommonValidationMessages.ConflictCodeString)
          .WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Member), x.MemberId));

      RuleFor(x => x.GuildId)
          .NotEmpty()
          .MustAsync(async (x, _) => await guildRepository.ExistsAsync(y => y.Id.Equals(x)))
          .WithErrorCode(CommonValidationMessages.ConflictCodeString)
          .WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Guild), x.GuildId));

      RuleFor(x => x.Member.Id).Equal(x => x.MemberId).Unless(x => x.Member is null);

      RuleFor(x => x.Guild.Id).Equal(x => x.GuildId).Unless(x => x.Guild is null);
    }
  }
}
