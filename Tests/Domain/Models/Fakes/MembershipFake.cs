using Bogus;
using Domain.Models;

namespace Tests.Domain.Models.Fakes
{
    public static class MembershipFake
    {
        public static Faker<Membership> NullObject()
        {
            return new Faker<Membership>().CustomInstantiator(_ => Membership.Null);
        }

        public static Faker<Membership> Active()
        {
            return new Faker<Membership>().CustomInstantiator(_ =>
            {
                return MemberFake.GuildMember().Generate().ActiveMembership;
            });
        }

        public static Faker<Membership> Finished()
        {
            return new Faker<Membership>().CustomInstantiator(_ =>
            {
                var member = MemberFake.GuildMember().Generate();
                member.LeaveGuild();
                return member.LastFinishedMembership;
            });
        }
    }
}
