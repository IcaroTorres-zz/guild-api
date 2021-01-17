using Bogus;
using Business.Usecases.Guilds.UpdateGuild;
using System;

namespace Tests.Business.Usecases.Guilds.UpdateGuild
{
    public static class UpdateGuildCommandFake
    {
        public static Faker<UpdateGuildCommand> Valid(Guid? id = null, Guid? masterId = null, string name = null)
        {
            return new Faker<UpdateGuildCommand>().CustomInstantiator(x =>
            {
                return new UpdateGuildCommand
                {
                    Id = id ?? Guid.NewGuid(),
                    Name = name ?? x.Company.CatchPhrase(),
                    MasterId = masterId ?? Guid.NewGuid()
                };
            });
        }

        public static Faker<UpdateGuildCommand> InvalidByEmptyId()
        {
            return new Faker<UpdateGuildCommand>().CustomInstantiator(x =>
            {
                return new UpdateGuildCommand
                {
                    Id = Guid.Empty,
                    Name = x.Company.CatchPhrase(),
                    MasterId = Guid.NewGuid()
                };
            });
        }

        public static Faker<UpdateGuildCommand> InvalidByEmptyName()
        {
            return new Faker<UpdateGuildCommand>().CustomInstantiator(x =>
            {
                return new UpdateGuildCommand
                {
                    Id = Guid.NewGuid(),
                    Name = string.Empty,
                    MasterId = Guid.NewGuid()
                };
            });
        }

        public static Faker<UpdateGuildCommand> InvalidByEmptyMasterId()
        {
            return new Faker<UpdateGuildCommand>().CustomInstantiator(x =>
            {
                return new UpdateGuildCommand
                {
                    Id = Guid.NewGuid(),
                    Name = x.Company.CatchPhrase(),
                    MasterId = Guid.Empty
                };
            });
        }
    }
}
