using Bogus;
using Domain.Models;

namespace Tests.Domain.Models.Fakes
{
    public static class MemberFake
    {
        public static Faker<Member> WithoutGuild(string name = null)
        {
            return new Faker<Member>().CustomInstantiator(x => new Member(name ?? x.Name.FullName()));
        }

        public static Faker<Member> GuildMember(Guild guild = null, string name = null)
        {
            return new Faker<Member>().CustomInstantiator(_ =>
            {
                var member = WithoutGuild(name).Generate();
                guild ??= GuildFake.Valid().Generate();
                guild.InviteMember(member);
                guild.GetLatestInvite().BeAccepted();
                return member;
            });
        }

        public static Faker<Member> GuildLeader()
        {
            return new Faker<Member>().CustomInstantiator(_ => GuildFake.Valid().Generate().GetLeader());
        }
    }
}
