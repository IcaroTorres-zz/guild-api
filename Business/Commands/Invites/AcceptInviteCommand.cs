using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Commands.Invites
{
  public class AcceptInviteCommand : IRequest<ApiResponse<Invite>>
  {
    public Guid Id { get; set; }
    public Invite Invite { get; private set; }

    public AcceptInviteCommand([FromRoute(Name = "id")] Guid id, [FromServices] IInviteRepository repository)
    {
      Id = id;
      Invite = repository.GetForAcceptOperation(id).Result;
    }
  }
}
