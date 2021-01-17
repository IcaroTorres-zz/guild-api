using Bogus;
using Domain.Models;

namespace Tests.Domain.Models.Fakes
{
    public static class MemberFake
    {
        public static Faker<Member> NullObject()
        {
            return new Faker<Member>().CustomInstantiator(_ => Member.Null);
        }

        public static Faker<Member> WithoutGuild()
        {
            return new Faker<Member>().CustomInstantiator(x => new Member(x.Person.FullName));
        }

        public static Faker<Member> GuildMember(Guild guild = null)
        {
            return new Faker<Member>().CustomInstantiator(_ =>
            {
                var member = WithoutGuild().Generate();
                guild ??= GuildFake.WithGuildLeader().Generate();
                guild.InviteMember(member);
                guild.LatestInvite.BeAccepted();
                return member;
            });
        }

        public static Faker<Member> GuildLeader(Guild guild = null)
        {
            return new Faker<Member>().CustomInstantiator(_ =>
            {
                guild ??= GuildFake.WithGuildLeaderAndMembers(otherMembersCount: 1).Generate();
                return guild.Leader;
            });
        }
    }
}
