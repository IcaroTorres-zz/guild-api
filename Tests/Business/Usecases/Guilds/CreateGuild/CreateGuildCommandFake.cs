using Bogus;
using Business.Usecases.Guilds.CreateGuild;
using System;

namespace Tests.Business.Usecases.Guilds.CreateGuild
{
    public static class CreateGuildCommandFake
    {
        public static Faker<CreateGuildCommand> Valid(Guid? masterId = null)
        {
            return new Faker<CreateGuildCommand>().CustomInstantiator(x =>
            {
                var command = new CreateGuildCommand
                {
                    Name = x.Company.CatchPhrase(),
                    MasterId = masterId ?? Guid.NewGuid()
                };
                const string routename = "get-guild";
                var urlHelper = UrlHelperMockBuilder.Create().SetupLink(routename).Build();
                command.SetupForCreation(urlHelper, routename, x => new { x.Id });
                return command;
            });
        }

        public static Faker<CreateGuildCommand> InvalidByEmptyName(Guid? masterId = null)
        {
            return new Faker<CreateGuildCommand>().CustomInstantiator(_ => new CreateGuildCommand
            {
                Name = string.Empty,
                MasterId = masterId ?? Guid.NewGuid()
            });
        }

        public static Faker<CreateGuildCommand> InvalidByEmptyMasterId()
        {
            return new Faker<CreateGuildCommand>().CustomInstantiator(x => new CreateGuildCommand
            {
                Name = x.Company.CatchPhrase(),
                MasterId = Guid.Empty
            });
        }
    }
}
