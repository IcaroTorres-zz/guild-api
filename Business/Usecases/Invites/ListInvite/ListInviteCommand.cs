using Business.Commands;
using Business.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Usecases.Invites.ListInvite
{
    [Serializable]
    public class ListInviteCommand : QueryListCommand<Pagination<Invite>, Pagination<InviteDto>>
    {
        [FromQuery(Name = "guildId")] public Guid? GuildId { get; set; }
        [FromQuery(Name = "memberId")] public Guid? MemberId { get; set; }
    }
}