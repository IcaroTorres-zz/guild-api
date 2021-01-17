using Bogus;
using Business.Usecases.Invites.AcceptInvite;
using Business.Usecases.Invites.CancelInvite;
using Business.Usecases.Invites.DenyInvite;
using System;

namespace Tests.Business.Usecases.Invites
{
    public static class PatchInviteCommandFake
    {
        public static Faker<AcceptInviteCommand> AcceptValid(Guid? id = null)
        {
            return new Faker<AcceptInviteCommand>().CustomInstantiator(_ => new AcceptInviteCommand { Id = id ?? Guid.NewGuid() });
        }

        public static Faker<AcceptInviteCommand> AcceptInvalidByEmptyId()
        {
            return new Faker<AcceptInviteCommand>().CustomInstantiator(_ => new AcceptInviteCommand { Id = Guid.Empty });
        }

        public static Faker<DenyInviteCommand> DenyValid(Guid? id = null)
        {
            return new Faker<DenyInviteCommand>().CustomInstantiator(_ => new DenyInviteCommand { Id = id ?? Guid.NewGuid() });
        }

        public static Faker<DenyInviteCommand> DenyInvalidByEmptyId()
        {
            return new Faker<DenyInviteCommand>().CustomInstantiator(_ => new DenyInviteCommand { Id = Guid.Empty });
        }

        public static Faker<CancelInviteCommand> CancelValid(Guid? id = null)
        {
            return new Faker<CancelInviteCommand>().CustomInstantiator(_ => new CancelInviteCommand { Id = id ?? Guid.NewGuid() });
        }

        public static Faker<CancelInviteCommand> CancelInvalidByEmptyId()
        {
            return new Faker<CancelInviteCommand>().CustomInstantiator(_ => new CancelInviteCommand { Id = Guid.Empty });
        }
    }
}
