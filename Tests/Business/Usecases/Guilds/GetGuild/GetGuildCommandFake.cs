using Bogus;
using Business.Usecases.Guilds.GetGuild;
using System;

namespace Tests.Business.Usecases.Guilds.GetGuild
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
