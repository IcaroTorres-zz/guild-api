using Bogus;
using Business.Usecases.Invites.GetInvite;
using System;

namespace Tests.Business.Usecases.Invites.GetInvite
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
