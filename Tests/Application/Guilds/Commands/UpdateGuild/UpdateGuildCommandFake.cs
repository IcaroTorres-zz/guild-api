using Application.Guilds.Commands.UpdateGuild;
using Bogus;
using System;

namespace Tests.Application.Guilds.Commands.UpdateGuild
{
    public static class UpdateGuildCommandFake
    {
        public static Faker<UpdateGuildCommand> Valid(Guid? id = null, Guid? leaderId = null, string name = null)
        {
            return new Faker<UpdateGuildCommand>().CustomInstantiator(x =>
            {
                return new UpdateGuildCommand
                {
                    Id = id ?? Guid.NewGuid(),
                    Name = name ?? x.Company.CatchPhrase(),
                    LeaderId = leaderId ?? Guid.NewGuid()
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
                    LeaderId = Guid.NewGuid()
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
                    LeaderId = Guid.NewGuid()
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
                    LeaderId = Guid.Empty
                };
            });
        }
    }
}
