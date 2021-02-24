using Application.Members.Queries.ListMember;
using Bogus;

namespace Tests.Application.Members.Queries.ListMember
{
    public static class ListMemberCommandFake
    {
        public static Faker<ListMemberCommand> Valid(int? pageSize = null, int? page = null)
        {
            return new Faker<ListMemberCommand>().CustomInstantiator(x => new ListMemberCommand
            {
                Page = page ?? x.Random.Int(min: 1),
                PageSize = pageSize ?? x.Random.Int(min: 1),
            });
        }

        public static Faker<ListMemberCommand> InvalidByPage()
        {
            return new Faker<ListMemberCommand>().CustomInstantiator(x => new ListMemberCommand
            {
                Page = x.Random.Int(max: 0),
                PageSize = x.Random.Int(min: 1),
            });
        }

        public static Faker<ListMemberCommand> InvalidByPageSize()
        {
            return new Faker<ListMemberCommand>().CustomInstantiator(x => new ListMemberCommand
            {
                Page = x.Random.Int(min: 1),
                PageSize = x.Random.Int(max: 0),
            });
        }
    }
}
