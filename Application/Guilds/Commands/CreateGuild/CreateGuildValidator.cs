using Application.Common.Abstractions;
using Domain.Models;
using FluentValidation;
using System;
using System.Net;

namespace Application.Guilds.Commands.CreateGuild
{
    public class CreateGuildValidator : AbstractValidator<CreateGuildCommand>
    {
        public CreateGuildValidator(IGuildRepository guildRepository, IMemberRepository memberRepository)
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.LeaderId).NotEmpty();

            When(x => x.Name != string.Empty && x.LeaderId != Guid.Empty, () =>
            {
                RuleFor(x => x)
                    .MustAsync(async (x, ct) => !await guildRepository.ExistsWithNameAsync(x.Name, ct))
                    .WithMessage(x => $"Record already exists for guild with given name {x.Name}.")
                    .WithName(nameof(Member.Name))
                    .WithErrorCode(nameof(HttpStatusCode.Conflict))

                    .MustAsync((x, ct) => memberRepository.ExistsWithIdAsync(x.LeaderId, ct))
                    .WithMessage(x => $"Record not found for leader with given id {x.LeaderId}.")
                    .WithName(x => nameof(x.LeaderId))
                    .WithErrorCode(nameof(HttpStatusCode.NotFound));
            });
        }
    }
}