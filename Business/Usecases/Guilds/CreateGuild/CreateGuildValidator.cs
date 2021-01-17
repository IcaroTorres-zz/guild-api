using Domain.Repositories;
using FluentValidation;
using System;
using System.Net;

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
                RuleFor(x => x)
                    .MustAsync(async (x, ct) => !await guildRepository.ExistsWithNameAsync(x.Name, ct))
                    .WithMessage(x => $"Record already exists for guild with given name {x.Name}.")
                    .WithName(x => nameof(x.Name))
                    .WithErrorCode(nameof(HttpStatusCode.Conflict))

                    .MustAsync((x, ct) => memberRepository.ExistsWithIdAsync(x.MasterId, ct))
                    .WithMessage(x => $"Record not found for master with given id {x.MasterId}.")
                    .WithName(x => nameof(x.MasterId))
                    .WithErrorCode(nameof(HttpStatusCode.NotFound));
            });
        }
    }
}