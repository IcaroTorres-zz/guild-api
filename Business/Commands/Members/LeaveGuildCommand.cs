using System;
using Business.Responses;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Commands.Members
{
	public class LeaveGuildCommand : IRequest<ApiResponse<Member>>, ITransactionalCommand
	{
		public LeaveGuildCommand(Guid id, IMemberRepository repository)
		{
			Id = id;
			Member = repository.GetForGuildOperationsAsync(id).Result;
		}

		public Guid Id { get; private set; }
		public Member Member { get; }
	}
}