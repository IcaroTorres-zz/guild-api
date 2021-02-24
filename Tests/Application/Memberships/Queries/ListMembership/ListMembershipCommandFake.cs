using Application.Memberships.Queries.ListMemberships;
using Bogus;

namespace Tests.Application.Memberships.Queries.ListMembership
{
    public static class ListMembershipCommandFake
    {
        public static Faker<ListMembershipCommand> Valid(int? pageSize = null, int? page = null)
        {
            return new Faker<ListMembershipCommand>().CustomInstantiator(x => new ListMembershipCommand
            {
                Page = page ?? x.Random.Int(min: 1),
                PageSize = pageSize ?? x.Random.Int(min: 1),
            });
        }

        public static Faker<ListMembershipCommand> InvalidByPage()
        {
            return new Faker<ListMembershipCommand>().CustomInstantiator(x => new ListMembershipCommand
            {
                Page = x.Random.Int(max: 0),
                PageSize = x.Random.Int(min: 1),
            });
        }

        public static Faker<ListMembershipCommand> InvalidByPageSize()
        {
            return new Faker<ListMembershipCommand>().CustomInstantiator(x => new ListMembershipCommand
            {
                Page = x.Random.Int(min: 1),
                PageSize = x.Random.Int(max: 0),
            });
        }
    }
}
