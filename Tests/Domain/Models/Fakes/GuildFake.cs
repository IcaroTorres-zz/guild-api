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

        public static Faker<Guild> WithGuildLeader(Member master = null)
        {
            return new Faker<Guild>().CustomInstantiator(x =>
            {
                master ??= MemberFake.WithoutGuild().Generate();
                return new Guild(x.Company.CatchPhrase(), master);
            });
        }

        public static Faker<Guild> WithGuildLeaderAndMembers(Member master = null, int otherMembersCount = 5)
        {
            return new Faker<Guild>().CustomInstantiator(_ =>
            {
                var guild = WithGuildLeader(master).Generate();
                foreach (var member in MemberFake.WithoutGuild().Generate(otherMembersCount))
                {
                    guild.InviteMember(member);
                    guild.LatestInvite.BeAccepted();
                }
                return guild;
            });
        }
    }
}
