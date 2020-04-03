using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System;

namespace Business.Commands.Invites
{
  public class DeclineInviteCommand : IRequest<ApiResponse<Invite>>
  {
    public Guid Id { get; set; }
    public Invite Invite { get; private set; }

    public DeclineInviteCommand(Guid id, IInviteRepository repository)
    {
      Id = id;
      Invite = repository.GetByIdAsync(id).Result;
    }
  }
}
