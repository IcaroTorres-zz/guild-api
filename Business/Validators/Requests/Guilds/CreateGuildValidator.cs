using Business.Commands.Guilds;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Requests.Guilds
{
	public class CreateGuildValidator : AbstractValidator<CreateGuildCommand>
	{
		public CreateGuildValidator(IGuildRepository guildRepository, IMemberRepository memberRepository)
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.MustAsync(async (name, cancellationToken) =>
					!await guildRepository.ExistsWithNameAsync(name, cancellationToken))
				.WithMessage(x => CommonValidationMessages.ForConflictWithKey(nameof(Guild), x.Name));

			RuleFor(x => x.MasterId)
				.NotEmpty()
				.WithMessage("No Member key reference was used to created this the Guild as guild master.")
				.MustAsync(async (masterId, cancellationToken) =>
					await memberRepository.ExistsWithIdAsync(masterId, cancellationToken))
				.WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Member), x.MasterId));
		}
	}
}