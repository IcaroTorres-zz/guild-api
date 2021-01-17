using Domain.Repositories;
using FluentValidation;
using System;

namespace Business.Usecases.Members.ChangeMemberName
{
    public class ChangeMemberNameValidator : AbstractValidator<ChangeMemberNameCommand>
    {
        public ChangeMemberNameValidator(IMemberRepository memberRepository)
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name).NotEmpty();

            When(x => x.Id != Guid.Empty && x.Name.Length != 0, () =>
            {
                RuleFor(x => x.Id)
                    .MustAsync((id, ct) => memberRepository.ExistsWithIdAsync(id, ct))
                    .WithMessage(x => $"Record not found for member with given id {x.Id}.");

                RuleFor(x => x)
                    .MustAsync((x, ct) => memberRepository.CanChangeNameAsync(x.Id, x.Name, ct))
                    .WithMessage(x => $"Record already exists for member with given name {x.Name}.")
                    .WithName(x => nameof(x.Name));
            });
        }
    }
}