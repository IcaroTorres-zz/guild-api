using Bogus;
using Domain.Models;

namespace Tests.Domain.Models.Fakes
{
    public static class MembershipFake
    {
        public static Faker<Membership> Active()
        {
            return new Faker<Membership>().CustomInstantiator(_ =>
            {
                return MemberFake.GuildMember().Generate().GetActiveMembership();
            });
        }

        public static Faker<Membership> Finished()
        {
            return new Faker<Membership>().CustomInstantiator(_ =>
            {
                var member = MemberFake.GuildMember().Generate();
                member.State.Leave();
                return member.GetLastFinishedMembership();
            });
        }
    }
}
