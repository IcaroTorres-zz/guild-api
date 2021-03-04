using Bogus;
using Domain.Models;
using System;
using Tests.Domain.Models.TestModels;
using Tests.Helpers;

namespace Tests.Domain.Models.Fakes
{
    public static class GuildFake
    {
        public static Faker<Guild> Valid(Guid? id = null, int membersCount = 2)
        {
            membersCount = Math.Max(2, membersCount);
            return new Faker<Guild>().CustomInstantiator(x =>
            {
                var guild = new TestGuild { Name = x.Company.CatchPhrase(), Id = id ?? Guid.NewGuid() };
                foreach (var member in MemberFake.WithoutGuild().Generate(membersCount - guild.Members.Count))
                {
                    var invite = guild.InviteMember(member, TestModelFactoryHelper.Factory);
                    invite.BeAccepted(TestModelFactoryHelper.Factory);
                }
                return guild;
            });
        }
    }
}
