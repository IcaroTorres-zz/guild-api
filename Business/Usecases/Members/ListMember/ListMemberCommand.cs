using Business.Commands;
using Business.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Usecases.Members.ListMember
{
    [Serializable]
    public class ListMemberCommand : QueryListCommand<Pagination<Member>, Pagination<MemberDto>>
    {
        [FromQuery(Name = "name")] public string Name { get; set; } = string.Empty;
        [FromQuery(Name = "guildId")] public Guid? GuildId { get; set; }
    }
}