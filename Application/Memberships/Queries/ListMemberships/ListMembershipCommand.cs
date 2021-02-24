using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Application.Memberships.Queries.ListMemberships
{
    [Serializable]
    public class ListMembershipCommand : QueryListCommand<PagedResponse<Membership>, PagedResponse<MembershipResponse>>
    {
        [FromQuery(Name = "guildId")] public Guid? GuildId { get; set; }
        [FromQuery(Name = "memberId")] public Guid? MemberId { get; set; }
    }
}
