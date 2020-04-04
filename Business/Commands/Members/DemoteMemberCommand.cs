using System;
using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Commands.Members
{
	public class DemoteMemberCommand : IRequest<ApiResponse<Member>>
	{
		public DemoteMemberCommand(Guid id, IMemberRepository repository)
		{
			Id = id;
			Member = repository.GetForGuildOperationsAsync(id).Result;
		}

		public Guid Id { get; set; }
		public Member Member { get; }
	}
}