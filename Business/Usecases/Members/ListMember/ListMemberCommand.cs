using Domain.Commands;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Usecases.Members.ListMember
{
    [Serializable]
    public class ListMemberCommand : IRequest<IApiResult>, IQueryListCommand
    {
        [FromQuery(Name = "name")] public string Name { get; set; } = string.Empty;
        [FromQuery(Name = "guildId")] public Guid? GuildId { get; set; }
        [FromQuery(Name = "pageSize")] public int PageSize { get; set; } = 20;
        [FromQuery(Name = "page")] public int Page { get; set; } = 1;
    }
}