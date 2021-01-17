using Bogus;
using Domain.Enums;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Domain.Models.Fakes
{
    public static class PaginationFake
    {
        public static Faker<Pagination<Guild>> PaginateGuilds(int pageSize = 10, int page = 1, int totalItems = 20)
        {
            var items = GuildFake.WithGuildLeaderAndMembers(otherMembersCount:2).Generate(totalItems);

            return new Faker<Pagination<Guild>>().CustomInstantiator(_ => Paginate(items, pageSize, page));
        }

        public static Faker<Pagination<Member>> PaginateMembers(int pageSize = 10, int page = 1, int totalItems = 20)
        {
            var items = new List<Member>();
            var guild = GuildFake.WithGuildLeader().Generate();
            for (int i = 0; i < totalItems; i++)
            {
                var member = i switch
                {
                    int count when count % 3 == 0 => MemberFake.WithoutGuild().Generate(),
                    int count when count == totalItems -1 => MemberFake.GuildLeader(guild: guild).Generate(),
                    _ => MemberFake.GuildMember(guild: guild).Generate(),
                };
                items.Add(member);
            }

            return new Faker<Pagination<Member>>().CustomInstantiator(_ => Paginate(items, pageSize, page));
        }

        public static Faker<Pagination<Invite>> PaginateInvites(int pageSize = 10, int page = 1, int totalItems = 20)
        {
            var items = new List<Invite>();
            for (int i = 0; i < totalItems; i++)
            {
                var status = i switch
                {
                    int count when count % 4 == 1 => InviteStatuses.Accepted,
                    int count when count % 4 == 2 => InviteStatuses.Denied,
                    int count when count % 4 == 3 => InviteStatuses.Canceled,
                    _ => InviteStatuses.Pending,
                };
                items.Add(InviteFake.ValidWithStatus(status).Generate());
            }

            return new Faker<Pagination<Invite>>().CustomInstantiator(_ => Paginate(items, pageSize, page));
        }

        public static Faker<Pagination<Membership>> PaginateMemberships(int pageSize = 10, int page = 1, int totalItems = 20)
        {
            var items = new List<Membership>();
            for (int i = 0; i < totalItems; i++)
            {
                if (i % 3 == 0) items.Add(MembershipFake.Finished().Generate());
                else items.Add(MembershipFake.Active().Generate());
            }

            return new Faker<Pagination<Membership>>().CustomInstantiator(_ => Paginate(items, pageSize, page));
        }

        private static Faker<Pagination<T>> Paginate<T>(List<T> items, int pageSize = 10, int page = 1) where T : class
        {
            var paginationItems = items.Take(pageSize).ToList();

            return new Faker<Pagination<T>>().CustomInstantiator(_ => new Pagination<T>(paginationItems, items.Count, pageSize, page));
        }
    }
}
