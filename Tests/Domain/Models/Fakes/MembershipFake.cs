using Bogus;
using Domain.Models;
using Tests.Helpers;

namespace Tests.Domain.Models.Fakes
{
    public static class MembershipFake
    {
        public static Faker<Membership> Active(Guild guild = null, Member member = null)
        {
            member ??= MemberFake.GuildMember().Generate();
            guild ??= member.Guild;
            var membership = TestModelFactoryHelper.Factory.CreateMembership(guild, member);
            return new Faker<Membership>().CustomInstantiator(_ => membership);
        }

        public static Faker<Membership> Finished()
        {
            return new Faker<Membership>().CustomInstantiator(_ =>
            {
                var member = MemberFake.GuildMember().Generate();
                return member.State.Leave();
            });
        }
    }
}
