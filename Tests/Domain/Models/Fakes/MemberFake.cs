using Bogus;
using Domain.Models;
using System;
using Tests.Domain.Models.TestModels;

namespace Tests.Domain.Models.Fakes
{
    public static class MemberFake
    {
        public static Faker<Member> WithoutGuild(string name = null)
        {
            return new Faker<Member>().CustomInstantiator(x => new TestMember
            {
                Name = name ?? x.Name.FullName(),
                Id = Guid.NewGuid()
            });
        }

        public static Faker<Member> GuildMember()
        {
            return new Faker<Member>().CustomInstantiator(_ => GuildFake.Complete().Generate().GetVice());
        }

        public static Faker<Member> GuildLeader()
        {
            return new Faker<Member>().CustomInstantiator(_ => GuildFake.Complete().Generate().GetLeader());
        }
    }
}
