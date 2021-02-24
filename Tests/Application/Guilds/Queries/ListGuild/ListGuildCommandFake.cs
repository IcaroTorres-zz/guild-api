using Application.Guilds.Queries.ListGuild;
using Bogus;

namespace Tests.Application.Guilds.Queries.ListGuild
{
    public static class ListGuildCommandFake
    {
        public static Faker<ListGuildCommand> Valid(int? pageSize = null, int? page = null)
        {
            return new Faker<ListGuildCommand>().CustomInstantiator(x => new ListGuildCommand
            {
                Page = page ?? x.Random.Int(min: 1),
                PageSize = pageSize ?? x.Random.Int(min: 1),
            });
        }

        public static Faker<ListGuildCommand> InvalidByPage()
        {
            return new Faker<ListGuildCommand>().CustomInstantiator(x => new ListGuildCommand
            {
                Page = x.Random.Int(max: 0),
                PageSize = x.Random.Int(min: 1),
            });
        }

        public static Faker<ListGuildCommand> InvalidByPageSize()
        {
            return new Faker<ListGuildCommand>().CustomInstantiator(x => new ListGuildCommand
            {
                Page = x.Random.Int(min: 1),
                PageSize = x.Random.Int(max: 0),
            });
        }
    }
}
