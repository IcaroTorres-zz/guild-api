using System;
using System.Collections.Generic;
using Business.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Business.Commands.Guilds
{
	public class UpdateGuildCommand : IRequest<ApiResponse<Guild>>, ITransactionalCommand
	{
		[FromRoute(Name = "id")] public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid MasterId { get; set; }
		public HashSet<Guid> MemberIds { get; set; } = new HashSet<Guid>();
	}
}