using Domain.Repositories;
using FluentValidation;
using System;

namespace Business.Usecases.Guilds.CreateGuild
{
    public class CreateGuildValidator : AbstractValidator<CreateGuildCommand>
    {
        public CreateGuildValidator(IGuildRepository guildRepository, IMemberRepository memberRepository)
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.MasterId).NotEmpty();

            When(x => x.Name != string.Empty && x.MasterId != Guid.Empty, () =>
            {
                RuleFor(x => x.Name)
                    .MustAsync(async (name, ct) => !await guildRepository.ExistsWithNameAsync(name, ct))
                    .WithMessage(x => $"Record already exists for guild with given name {x.Name}.");

                RuleFor(x => x.MasterId)
                    .MustAsync((masterId, ct) => memberRepository.ExistsWithIdAsync(masterId, ct))
                    .WithMessage(x => $"Record not found for master with given id {x.MasterId}.");
            });
        }
    }
}