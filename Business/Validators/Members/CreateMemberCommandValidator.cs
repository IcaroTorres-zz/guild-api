using Business.Commands.Members;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Business.Validators.PreCommands
{
    public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
    {
        public CreateMemberCommandValidator(IMemberRepository memberRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MustAsync(async (name, _) => !await memberRepository.ExistsWithNameAsync(name))
                .WithMessage(x => CommonValidations.ForConflictWithKey(nameof(Member), x.Name));
        }
    }
}
