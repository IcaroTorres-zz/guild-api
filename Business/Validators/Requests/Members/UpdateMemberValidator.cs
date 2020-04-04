using System;
using System.Linq;
using Business.Commands.Members;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Requests.Members
{
	public class UpdateMemberValidator : AbstractValidator<UpdateMemberCommand>
	{
		public UpdateMemberValidator(IMemberRepository memberRepository, IGuildRepository guildRepository)
		{
			RuleFor(x => x.Id)
				.NotEmpty()
				.MustAsync(async (id, _) => await memberRepository.ExistsWithIdAsync(id))
				.WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Member), x.Id));

			RuleFor(x => x.Name)
				.NotEmpty()
				.MustAsync(async (name, _) => !await memberRepository.ExistsWithNameAsync(name))
				.WithMessage(x => CommonValidationMessages.ForConflictWithKey(nameof(Member), x.Name))
				.Unless(x => x.Id.Equals(memberRepository.Query().SingleOrDefault(y => y.Name.Equals(x.Name)).Id));

			RuleFor(x => x.GuildId)
				.MustAsync(async (guildId, _) => await guildRepository.ExistsWithIdAsync(guildId))
				.WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Guild), x.GuildId))
				.When(x => x.GuildId != Guid.Empty && x.GuildId != null);
		}
	}
}