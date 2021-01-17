using Bogus;
using Business.Usecases.Invites.ListInvite;

namespace Tests.Business.Usecases.Invites.ListInvite
{
    public static class ListInviteCommandFake
    {
        public static Faker<ListInviteCommand> Valid(int? pageSize = null, int? page = null)
        {
            return new Faker<ListInviteCommand>().CustomInstantiator(x => new ListInviteCommand
            {
                Page = page ?? x.Random.Int(min: 1, 10),
                PageSize = pageSize ?? x.Random.Int(min: 1, 20),
            });
        }

        public static Faker<ListInviteCommand> InvalidByPage()
        {
            return new Faker<ListInviteCommand>().CustomInstantiator(x => new ListInviteCommand
            {
                Page = x.Random.Int(max: 0),
                PageSize = x.Random.Int(min: 1),
            });
        }

        public static Faker<ListInviteCommand> InvalidByPageSize()
        {
            return new Faker<ListInviteCommand>().CustomInstantiator(x => new ListInviteCommand
            {
                Page = x.Random.Int(min: 1),
                PageSize = x.Random.Int(max: 0),
            });
        }
    }
}
