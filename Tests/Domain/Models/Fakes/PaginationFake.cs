using Application.Common.Responses;
using Bogus;
using Domain.Enums;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Domain.Models.Fakes
{
    public static class PagedResponseFake
    {
        public static Faker<PagedResponse<Guild>> PaginateGuilds(int pageSize = 10, int page = 1, int totalItems = 20)
        {
            var items = GuildFake.WithGuildLeaderAndMembers(otherMembersCount:2).Generate(totalItems);

            return new Faker<PagedResponse<Guild>>().CustomInstantiator(_ => Paginate(items, pageSize, page));
        }

        public static Faker<PagedResponse<Member>> PaginateMembers(int pageSize = 10, int page = 1, int totalItems = 20)
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

            return new Faker<PagedResponse<Member>>().CustomInstantiator(_ => Paginate(items, pageSize, page));
        }

        public static Faker<PagedResponse<Invite>> PaginateInvites(int pageSize = 10, int page = 1, int totalItems = 20)
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

            return new Faker<PagedResponse<Invite>>().CustomInstantiator(_ => Paginate(items, pageSize, page));
        }

        public static Faker<PagedResponse<Membership>> PaginateMemberships(int pageSize = 10, int page = 1, int totalItems = 20)
        {
            var items = new List<Membership>();
            for (int i = 0; i < totalItems; i++)
            {
                if (i % 3 == 0) items.Add(MembershipFake.Finished().Generate());
                else items.Add(MembershipFake.Active().Generate());
            }

            return new Faker<PagedResponse<Membership>>().CustomInstantiator(_ => Paginate(items, pageSize, page));
        }

        private static Faker<PagedResponse<T>> Paginate<T>(List<T> items, int pageSize = 10, int page = 1) where T : class
        {
            var PagedResponseItems = items.Take(pageSize).ToList();

            return new Faker<PagedResponse<T>>().CustomInstantiator(_ => new PagedResponse<T>(PagedResponseItems, items.Count, pageSize, page));
        }
    }
}
