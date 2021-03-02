using Bogus;
using Domain.Models;
using System;

namespace Tests.Domain.Models.Fakes
{
    public static class GuildFake
    {
        /// <summary>
        /// Generates a valid <see cref="Guild"/> with at least the leader and a vice-leader for testing purposes.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="membersCount">The members count.</param>
        /// <param name="leader">The leader.</param>
        /// <returns></returns>
        public static Faker<Guild> Valid(Guid? id = null, int membersCount = 2, Member leader = null)
        {
            membersCount = Math.Max(2, membersCount);
            return new Faker<Guild>().CustomInstantiator(x =>
            {
                var guild = new Guild(x.Company.CatchPhrase(), Member.Null) { Id = id ?? Guid.NewGuid() };
                if (leader is Member)
                {
                    guild.InviteMember(leader);
                    guild.GetLatestInvite().BeAccepted();
                }
                foreach (var member in MemberFake.WithoutGuild().Generate(membersCount - guild.Members.Count))
                {
                    guild.InviteMember(member);
                    guild.GetLatestInvite().BeAccepted();
                }
                return guild;
            });
        }
    }
}
