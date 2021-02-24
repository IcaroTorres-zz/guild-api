using Application.Guilds.Queries.GetGuild;
using Bogus;
using System;

namespace Tests.Application.Guilds.Queries.GetGuild
{
    public static class GetGuildCommandFake
    {
        public static Faker<GetGuildCommand> Valid(Guid? id = null)
        {
            return new Faker<GetGuildCommand>().CustomInstantiator(_ => new GetGuildCommand { Id = id ?? Guid.NewGuid() });
        }

        public static Faker<GetGuildCommand> InvalidByEmptyId()
        {
            return new Faker<GetGuildCommand>().CustomInstantiator(_ => new GetGuildCommand { Id = Guid.Empty });
        }
    }
}
