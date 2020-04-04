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
				.MustAsync(async (name, _) => !await memberRepository.ExistsWithNameAsync(name))
				.WithMessage(x => CommonValidationMessages.ForConflictWithKey(nameof(Member), x.Name));
		}
	}
}