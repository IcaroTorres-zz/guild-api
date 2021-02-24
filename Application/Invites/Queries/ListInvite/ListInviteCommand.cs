using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Application.Invites.Queries.ListInvite
{
    [Serializable]
    public class ListInviteCommand : QueryListCommand<PagedResponse<Invite>, PagedResponse<InviteResponse>>
    {
        [FromQuery(Name = "guildId")] public Guid? GuildId { get; set; }
        [FromQuery(Name = "memberId")] public Guid? MemberId { get; set; }
    }
}