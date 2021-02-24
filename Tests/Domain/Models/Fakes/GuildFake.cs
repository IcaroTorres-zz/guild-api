using Bogus;
using Domain.Models;

namespace Tests.Domain.Models.Fakes
{
    public static class GuildFake
    {
        public static Faker<Guild> NullObject()
        {
            return new Faker<Guild>().CustomInstantiator(_ => Guild.Null);
        }

        public static Faker<Guild> WithGuildLeader(Member leader = null)
        {
            return new Faker<Guild>().CustomInstantiator(x =>
            {
                leader ??= MemberFake.WithoutGuild().Generate();
                return new Guild(x.Company.CatchPhrase(), leader);
            });
        }

        public static Faker<Guild> WithGuildLeaderAndMembers(Member leader = null, int otherMembersCount = 5)
        {
            return new Faker<Guild>().CustomInstantiator(_ =>
            {
                var guild = WithGuildLeader(leader).Generate();
                foreach (var member in MemberFake.WithoutGuild().Generate(otherMembersCount))
                {
                    guild.InviteMember(member);
                    guild.GetLatestInvite().BeAccepted();
                }
                return guild;
            });
        }
    }
}
