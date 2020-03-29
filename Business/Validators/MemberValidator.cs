using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using System;
using System.Linq;

namespace Business.Validators
{
    public class MemberValidator : BaseValidator<Member>
    {
        private readonly IMemberRepository MemberRepository;

        public MemberValidator(IMemberRepository repository)
        {
            MemberRepository = repository;

            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.GuildId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .Unless(x => x.Guild == null);

            RuleFor(x => x.Guild.Id)
                .Equal(x => x.GuildId.Value)
                .Unless(x => x.Guild == null);

            RuleFor(x => x.Memberships)
                .Must(x => x.Any(ms
                    => ms.MemberId == Entity.Id
                    && ms.GuildId == Entity.GuildId
                    && ms.Until == null
                    && !ms.Disabled))
                .When(x => x.Memberships.Any() && Entity.GuildId != null);
        }
    }
}
