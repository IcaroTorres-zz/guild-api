using Business.Commands.Members;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.Requests.Members
{
	public class CreateMemberValidator : AbstractValidator<CreateMemberCommand>
	{
		public CreateMemberValidator(IMemberRepository memberRepository)
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.MustAsync(async (name, cancellationToken) =>
					!await memberRepository.ExistsWithNameAsync(name, cancellationToken))
				.WithMessage(x => CommonValidationMessages.ForConflictWithKey(nameof(Member), x.Name));
		}
	}
}