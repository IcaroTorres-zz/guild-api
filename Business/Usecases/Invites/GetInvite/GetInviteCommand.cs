using Domain.Commands;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Usecases.Invites.GetInvite
{
    public class GetInviteCommand : IRequest<IApiResult>, IQueryItemCommand
    {
        [FromRoute] public Guid Id { get; set; }
    }
}