using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System;

namespace Business.Commands.Members
{
  public class DemoteMemberCommand : IRequest<ApiResponse<Member>>
  {
    public Guid Id { get; set; }
    public Member Member { get; private set; }

    public DemoteMemberCommand(Guid id, IMemberRepository repository)
    {
      Id = id;
      Member = repository.GetForGuildOperationsAsync(id).Result;
    }
  }
}
