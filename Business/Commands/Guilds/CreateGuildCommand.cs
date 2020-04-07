using System;
using Business.Responses;
using Domain.Entities;
using MediatR;

namespace Business.Commands.Guilds
{
	public class CreateGuildCommand : IRequest<ApiResponse<Guild>>, ITransactionalCommand
	{
		public string Name { get; set; }
		public Guid MasterId { get; set; }
	}
}