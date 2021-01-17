using Bogus;
using Business.Usecases.Members.ChangeMemberName;
using Business.Usecases.Members.DemoteMember;
using Business.Usecases.Members.LeaveGuild;
using Business.Usecases.Members.PromoteMember;
using System;

namespace Tests.Business.Usecases.Members
{
    public static class PatchMemberCommandFake
    {
        public static Faker<PromoteMemberCommand> PromoteMemberValid(Guid? id = null)
        {
            return new Faker<PromoteMemberCommand>().CustomInstantiator(_ => new PromoteMemberCommand { Id = id ?? Guid.NewGuid() });
        }

        public static Faker<PromoteMemberCommand> PromoteMemberInvalidByEmptyId()
        {
            return new Faker<PromoteMemberCommand>().CustomInstantiator(_ => new PromoteMemberCommand { Id = Guid.Empty });
        }

        public static Faker<DemoteMemberCommand> DemoteMemberValid(Guid? id = null)
        {
            return new Faker<DemoteMemberCommand>().CustomInstantiator(_ => new DemoteMemberCommand { Id = id ?? Guid.NewGuid() });
        }

        public static Faker<DemoteMemberCommand> DemoteMemberInvalidByEmptyId()
        {
            return new Faker<DemoteMemberCommand>().CustomInstantiator(_ => new DemoteMemberCommand { Id = Guid.Empty });
        }

        public static Faker<LeaveGuildCommand> LeaveGuildValid(Guid? id = null)
        {
            return new Faker<LeaveGuildCommand>().CustomInstantiator(_ => new LeaveGuildCommand { Id = id ?? Guid.NewGuid() });
        }

        public static Faker<LeaveGuildCommand> LeaveGuildInvalidByEmptyId()
        {
            return new Faker<LeaveGuildCommand>().CustomInstantiator(_ => new LeaveGuildCommand { Id = Guid.Empty });
        }

        public static Faker<ChangeMemberNameCommand> ChangeMemberNameValid(Guid? id = null, string name = null)
        {
            return new Faker<ChangeMemberNameCommand>().CustomInstantiator(x => new ChangeMemberNameCommand
            {
                Id = id ?? Guid.NewGuid(),
                Name = name ?? x.Person.UserName
            });
        }

        public static Faker<ChangeMemberNameCommand> ChangeMemberNameInvalidByEmptyId()
        {
            return new Faker<ChangeMemberNameCommand>().CustomInstantiator(x => new ChangeMemberNameCommand
            {
                Id = Guid.Empty,
                Name = x.Person.UserName
            });
        }

        public static Faker<ChangeMemberNameCommand> ChangeMemberNameInvalidByEmptyName(Guid? id = null)
        {
            return new Faker<ChangeMemberNameCommand>().CustomInstantiator(_ => new ChangeMemberNameCommand
            {
                Id = id ?? Guid.NewGuid(),
                Name = string.Empty
            });
        }
    }
}
