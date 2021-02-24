using Application.Members.Queries.GetMember;
using Bogus;
using System;

namespace Tests.Application.Members.Queries.GetMember
{
    public static class GetMemberCommandFake
    {
        public static Faker<GetMemberCommand> Valid(Guid? id = null)
        {
            return new Faker<GetMemberCommand>().CustomInstantiator(_ => new GetMemberCommand { Id = id ?? Guid.NewGuid() });
        }

        public static Faker<GetMemberCommand> InvalidByEmptyId()
        {
            return new Faker<GetMemberCommand>().CustomInstantiator(_ => new GetMemberCommand { Id = Guid.Empty });
        }
    }
}
