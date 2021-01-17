using Bogus;
using Business.Usecases.Members.CreateMember;
using Tests.Business.Usecases.Guilds.CreateGuild;

namespace Tests.Business.Usecases.Members.CreateMember
{
    public static class CreateMemberCommandFake
    {
        public static Faker<CreateMemberCommand> Valid()
        {
            return new Faker<CreateMemberCommand>().CustomInstantiator(x =>
            {
                var command = new CreateMemberCommand { Name = x.Person.UserName };

                const string routename = "get-member";
                var urlHelper = UrlHelperMockBuilder.Create().SetupLink(routename).Build();
                command.SetupForCreation(urlHelper, routename, x => new { x.Id });
                return command;
            });
        }

        public static Faker<CreateMemberCommand> InvalidByEmptyName()
        {
            return new Faker<CreateMemberCommand>().CustomInstantiator(_ => new CreateMemberCommand { Name = string.Empty });
        }
    }
}
