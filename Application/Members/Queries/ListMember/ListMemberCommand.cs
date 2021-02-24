using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Application.Members.Queries.ListMember
{
    [Serializable]
    public class ListMemberCommand : QueryListCommand<PagedResponse<Member>, PagedResponse<MemberResponse>>
    {
        [FromQuery(Name = "name")] public string Name { get; set; } = string.Empty;
        [FromQuery(Name = "guildId")] public Guid? GuildId { get; set; }
    }
}