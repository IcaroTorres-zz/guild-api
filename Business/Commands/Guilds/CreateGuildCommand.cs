using Business.ResponseOutputs;
using Domain.Entities;
using MediatR;
using System;

namespace Business.Commands.Guilds
{
  public class CreateGuildCommand : IRequest<ApiResponse<Guild>>
  {
    public string Name { get; set; }
    public Guid MasterId { get; set; }
  }
}