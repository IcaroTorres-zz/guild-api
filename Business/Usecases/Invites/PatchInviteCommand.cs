using Domain.Commands;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Usecases.Invites
{
    public abstract class PatchInviteCommand : IRequest<IApiResult>, ITransactionalCommand
    {
        [FromRoute(Name = "id")] public Guid Id { get; set; }
    }
}