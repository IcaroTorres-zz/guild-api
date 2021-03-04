using Application.Common.Responses;
using Application.Members.Queries.ListMember;
using Bogus;
using Domain.Enums;
using Domain.Models;
using System.Collections.Generic;

namespace Tests.Domain.Models.Fakes
{
    public static class PagedResponseFake
    {
        public static Faker<PagedResponse<Guild>> PaginateGuilds(int pageSize = 3, int page = 1, int totalItems = 20)
        {
            List<Guild> items = new List<Guild>(GuildFake.Valid().Generate(pageSize));
            var pagedResult = new PagedResponse<Guild>(items, totalItems, pageSize, page);
            return new Faker<PagedResponse<Guild>>().CustomInstantiator(_ => pagedResult);
        }

        public static Faker<PagedResponse<Member>> PaginateMembers(ListMemberCommand command, int totalItems = 20)
        {
            return new Faker<PagedResponse<Member>>().CustomInstantiator(x =>
            {
                var items = new List<Member>();
                var guild = GuildFake.Valid(id: command.GuildId, membersCount: command.PageSize).Generate();
                foreach (var member in guild.Members)
                {
                    items.Add(member.ChangeName($"{x.Name.FullName()} {command.Name}"));
                }

                return new PagedResponse<Member>(items, totalItems, command.PageSize, command.Page);
            });
        }

        public static Faker<PagedResponse<Invite>> PaginateInvites(int pageSize = 3, int page = 1, int totalItems = 20)
        {
            var items = new List<Invite>();
            for (int i = 0; i < pageSize; i++)
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

            return new Faker<PagedResponse<Invite>>().CustomInstantiator(_ => new PagedResponse<Invite>(items, totalItems, pageSize, page));
        }

        public static Faker<PagedResponse<Membership>> PaginateMemberships(int pageSize = 3, int page = 1, int totalItems = 20)
        {
            var items = new List<Membership>();
            for (int i = 0; i < pageSize; i++)
            {
                if (i % 3 == 0) items.Add(MembershipFake.Finished().Generate());
                else items.Add(MembershipFake.Active().Generate());
            }

            return new Faker<PagedResponse<Membership>>().CustomInstantiator(_ => new PagedResponse<Membership>(items, totalItems, pageSize, page));
        }
    }
}
