using Business.ResponseOutputs;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Business.Commands.Guilds
{
  public class UpdateGuildCommand : IRequest<ApiResponse<Guild>>
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid MasterId { get; set; }
    public HashSet<Guid> MemberIds { get; set; } = new HashSet<Guid>();

    public UpdateGuildCommand(string name, Guid masterId, HashSet<Guid> memberIds, [FromRoute(Name = "id")] Guid id)
    {
      Id = id;
      Name = name;
      MasterId = masterId;
      MemberIds = memberIds;
    }
  }
}