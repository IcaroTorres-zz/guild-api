using Domain.Commands;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Usecases.Guilds.GetGuild
{
    public class GetGuildCommand : IRequest<IApiResult>, IQueryItemCommand
    {
        [FromRoute] public Guid Id { get; set; }
    }
}