using Domain.Commands;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Usecases.Memberships.ListMemberships
{
    [Serializable]
    public class ListMembershipCommand : IRequest<IApiResult>, IQueryListCommand
    {
        [FromQuery(Name = "guildId")] public Guid? GuildId { get; set; }
        [FromQuery(Name = "memberId")] public Guid? MemberId { get; set; }
        [FromQuery(Name = "pageSize")] public int PageSize { get; set; } = 20;
        [FromQuery(Name = "page")] public int Page { get; set; } = 1;
    }
}
