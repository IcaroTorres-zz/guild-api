using Application.Members.Commands.CreateMember;
using Bogus;
using Tests.Helpers.Builders;

namespace Tests.Application.Members.Commands.CreateMember
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
