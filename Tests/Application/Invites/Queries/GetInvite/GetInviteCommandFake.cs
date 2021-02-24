using Application.Invites.Queries.GetInvite;
using Bogus;
using System;

namespace Tests.Application.Invites.Queries.GetInvite
{
    public static class GetInviteCommandFake
    {
        public static Faker<GetInviteCommand> Valid(Guid? id = null)
        {
            return new Faker<GetInviteCommand>().CustomInstantiator(_ => new GetInviteCommand { Id = id ?? Guid.NewGuid() });
        }

        public static Faker<GetInviteCommand> InvalidByEmptyId()
        {
            return new Faker<GetInviteCommand>().CustomInstantiator(_ => new GetInviteCommand { Id = Guid.Empty });
        }
    }
}
