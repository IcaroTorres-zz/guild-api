using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Commands.Members
{
    public class DemoteMemberCommand : IRequest<ApiResponse<Member>>
    {
        public Guid Id { get; set; }
        public Member Member { get; private set; }

        public DemoteMemberCommand([FromRoute(Name = "id")] Guid id, [FromServices] IMemberRepository repository)
        {
            Id = id;
            Member = repository.GetForGuildOperationsAsync(id).Result;
        }
    }
}
