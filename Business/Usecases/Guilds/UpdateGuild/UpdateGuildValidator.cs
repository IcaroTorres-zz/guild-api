﻿using Domain.Repositories;
using FluentValidation;
using System;
using System.Net;

namespace Business.Usecases.Guilds.UpdateGuild
{
    public class UpdateGuildValidator : AbstractValidator<UpdateGuildCommand>
    {
        public UpdateGuildValidator(IGuildRepository guildRepository, IMemberRepository memberRepository)
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.MasterId).NotEmpty();

            When(x => x.Name != string.Empty && x.Id != Guid.Empty && x.MasterId != Guid.Empty, () =>
            {
                RuleFor(x => x)
                    .MustAsync((x, ct) => guildRepository.ExistsWithIdAsync(x.Id, ct))
                    .WithMessage(x => $"Record not found for guild with given id {x.Id}.")
                    .WithName(x => nameof(x.Id))
                    .WithErrorCode(nameof(HttpStatusCode.NotFound))

                    .MustAsync((x, ct) => memberRepository.ExistsWithIdAsync(x.MasterId, ct))
                    .WithMessage(x => $"Record not found for member with given id {x.MasterId}.")
                    .WithName(x => nameof(x.MasterId))
                    .WithErrorCode(nameof(HttpStatusCode.NotFound))

                    .MustAsync((x, ct) => guildRepository.CanChangeNameAsync(x.Id, x.Name, ct))
                    .WithMessage(x => $"Record already exists for guild with given name {x.Name}.")
                    .WithName(x => nameof(x.Name))
                    .WithErrorCode(nameof(HttpStatusCode.Conflict))

                    .MustAsync((x, ct) => memberRepository.IsGuildMemberAsync(x.MasterId, x.Id, ct))
                    .WithMessage("Member chosen for Guild Master must be member of target Guild.")
                    .WithName(x => nameof(x.MasterId))
                    .WithErrorCode(nameof(HttpStatusCode.UnprocessableEntity));
            });
        }
    }
}