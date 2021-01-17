using Domain.Commands;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Usecases.Guilds.ListGuild
{
    [Serializable]
    public class ListGuildCommand : IRequest<IApiResult>, IQueryListCommand
    {
        [FromQuery(Name = "pageSize")] public int PageSize { get; set; } = 20;
        [FromQuery(Name = "page")] public int Page { get; set; } = 1;
    }
}