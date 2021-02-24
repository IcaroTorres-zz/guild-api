using Application.Guilds.Commands.CreateGuild;
using Bogus;
using System;
using Tests.Helpers.Builders;

namespace Tests.Application.Guilds.Commands.CreateGuild
{
    public static class CreateGuildCommandFake
    {
        public static Faker<CreateGuildCommand> Valid(Guid? leaderId = null)
        {
            return new Faker<CreateGuildCommand>().CustomInstantiator(x =>
            {
                var command = new CreateGuildCommand
                {
                    Name = x.Company.CatchPhrase(),
                    LeaderId = leaderId ?? Guid.NewGuid()
                };
                const string routename = "get-guild";
                var urlHelper = UrlHelperMockBuilder.Create().SetupLink(routename).Build();
                command.SetupForCreation(urlHelper, routename, x => new { x.Id });
                return command;
            });
        }

        public static Faker<CreateGuildCommand> InvalidByEmptyName(Guid? leaderId = null)
        {
            return new Faker<CreateGuildCommand>().CustomInstantiator(_ => new CreateGuildCommand
            {
                Name = string.Empty,
                LeaderId = leaderId ?? Guid.NewGuid()
            });
        }

        public static Faker<CreateGuildCommand> InvalidByEmptyMasterId()
        {
            return new Faker<CreateGuildCommand>().CustomInstantiator(x => new CreateGuildCommand
            {
                Name = x.Company.CatchPhrase(),
                LeaderId = Guid.Empty
            });
        }
    }
}
