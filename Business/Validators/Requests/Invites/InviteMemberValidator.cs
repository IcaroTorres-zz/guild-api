using Business.Commands.Invites;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using System.Linq;

namespace Business.Validators.Requests.Invites
{
  public class InviteMemberValidator : AbstractValidator<InviteMemberCommand>
  {
    public InviteMemberValidator(IMemberRepository memberRepository, IGuildRepository guildRepository)
    {
      RuleFor(x => x.Invite.MemberId)
          .NotEmpty()
          .MustAsync(async (memberId, _) => await memberRepository.ExistsWithIdAsync(memberId))
          .WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Member), x.Invite.MemberId));

      RuleFor(x => x.Invite.GuildId)
          .NotEmpty()
          .MustAsync(async (guildId, _) => await guildRepository.ExistsWithIdAsync(guildId))
          .WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Guild), x.Invite.GuildId));

      RuleFor(x => x.Invite)
          .MustAsync(async (x, _) => !(
              await guildRepository.ExistsAsync(y =>
                  y.Id.Equals(x.GuildId) &&
                  y.Members.Any(m => m.Id.Equals(x.MemberId))
              )))
          .WithMessage(x => string.Format(
              "{0} already in target {1} with key {2}", nameof(Member), nameof(Guild), x.GuildId));
    }
  }
}
