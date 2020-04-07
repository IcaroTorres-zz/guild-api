using System.Linq;
using Business.Commands.Guilds;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Requests.Guilds
{
	public class UpdateGuildValidator : AbstractValidator<UpdateGuildCommand>
	{
		public UpdateGuildValidator(IGuildRepository guildRepository, IMemberRepository memberRepository)
		{
			RuleFor(x => x.Id)
				.NotEmpty()
				.MustAsync(async (id, cancellationToken) => await guildRepository.ExistsWithIdAsync(id, cancellationToken))
				.WithMessage(x => CommonValidationMessages.ForRecordNotFound(nameof(Guild), x.Id));

			RuleFor(x => x.Name)
				.NotEmpty()
				.MustAsync(async (name, cancellationToken) =>
					!await memberRepository.ExistsWithNameAsync(name, cancellationToken))
				.WithMessage(x => CommonValidationMessages.ForConflictWithKey(nameof(Guild), x.Name))
				.UnlessAsync(async (x, cancellationToken) =>
				{
					var member = await memberRepository.GetByNameAsync(x.Name, true, cancellationToken);
					return x.Id.Equals(member.Id);
				});

			RuleFor(x => x.MasterId)
				.NotEmpty()
				.MustAsync(async (masterId, cancellationToken) =>
					await memberRepository.ExistsWithIdAsync(masterId, cancellationToken))
				.WithMessage("Member chosen for Guild Master not found.");

			RuleFor(x => x)
				.MustAsync(async (x, cancellationToken) =>
					(await memberRepository.GetByIdAsync(x.MasterId, true, cancellationToken))
					.GuildId.Equals(x.Id))
				.WithMessage("Member chosen for Guild Master must be a Member of target Guild.");
		}
	}
}