using Business.Commands;
using Business.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Usecases.Memberships.ListMemberships
{
    [Serializable]
    public class ListMembershipCommand : QueryListCommand<Pagination<Membership>, Pagination<MembershipDto>>
    {
        [FromQuery(Name = "guildId")] public Guid? GuildId { get; set; }
        [FromQuery(Name = "memberId")] public Guid? MemberId { get; set; }
    }
}
