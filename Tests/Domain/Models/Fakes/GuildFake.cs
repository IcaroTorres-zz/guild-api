using Bogus;
using Domain.Models;
using System;
using Tests.Domain.Models.Proxies;
using Tests.Helpers;

namespace Tests.Domain.Models.Fakes
{
    public static class GuildFake
    {
        public static Faker<Guild> NoMembers(Guid? id = null)
        {
            return new Faker<Guild>().CustomInstantiator(_ => new GuildTestProxy())
                .RuleFor(x => x.Name, x => x.Company.CatchPhrase())
                .RuleFor(x => x.Id, _ => id ?? Guid.NewGuid());
        }

        public static Faker<Guild> LeaderOnly(Guid? id = null)
        {
            var guild = NoMembers(id).Generate();
            var leader = MemberFake.WithoutGuild().Generate();
            var invite = guild.InviteMember(leader, TestModelFactoryHelper.Factory)
                              .BeAccepted(TestModelFactoryHelper.Factory);
            return new Faker<Guild>().CustomInstantiator(_ => guild);
        }

        public static Faker<Guild> Complete(Guid? id = null, int membersCount = 2)
        {
            membersCount = Math.Max(2, membersCount);
            return new Faker<Guild>().CustomInstantiator(_ =>
            {
                var guild = NoMembers().Generate();
                foreach (var member in MemberFake.WithoutGuild().Generate(membersCount))
                {
                    var invite = guild.InviteMember(member, TestModelFactoryHelper.Factory);
                    invite.BeAccepted(TestModelFactoryHelper.Factory);
                }
                return guild;
            });
        }
    }
}
