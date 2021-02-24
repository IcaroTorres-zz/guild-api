using Application.Invites.Commands.InviteMember;
using Bogus;
using System;
using Tests.Helpers.Builders;

namespace Tests.Application.Invites.Commands.InviteMember
{
    public static class InviteMemberCommandFake
    {
        public static Faker<InviteMemberCommand> Valid(Guid? guildId = null, Guid? memberId = null)
        {
            return new Faker<InviteMemberCommand>().CustomInstantiator(_ =>
            {
                var command = new InviteMemberCommand
                {
                    GuildId = guildId ?? Guid.NewGuid(),
                    MemberId = memberId ?? Guid.NewGuid()
                };

                const string routename = "get-invite";
                var urlHelper = UrlHelperMockBuilder.Create().SetupLink(routename).Build();
                command.SetupForCreation(urlHelper, routename, x => new { x.Id });
                return command;
            });
        }

        public static Faker<InviteMemberCommand> InvalidByEmptyGuildId()
        {
            return new Faker<InviteMemberCommand>().CustomInstantiator(_ => new InviteMemberCommand
            {
                GuildId = Guid.Empty,
                MemberId = Guid.NewGuid()
            });
        }

        public static Faker<InviteMemberCommand> InvalidByEmptyMemberId()
        {
            return new Faker<InviteMemberCommand>().CustomInstantiator(_ => new InviteMemberCommand
            {
                GuildId = Guid.NewGuid(),
                MemberId = Guid.Empty
            });
        }
    }
}
