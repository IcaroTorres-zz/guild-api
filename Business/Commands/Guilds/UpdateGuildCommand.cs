using System;
using System.Collections.Generic;
using Business.ResponseOutputs;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Business.Commands.Guilds
{
	public class UpdateGuildCommand : IRequest<ApiResponse<Guild>>
	{
		public UpdateGuildCommand([FromRoute(Name = "id")] Guid id, string name, Guid masterId, HashSet<Guid> memberIds)
		{
			Id = id;
			Name = name;
			MasterId = masterId;
			MemberIds = memberIds;
		}

		public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid MasterId { get; set; }
		public HashSet<Guid> MemberIds { get; set; } = new HashSet<Guid>();
	}
}