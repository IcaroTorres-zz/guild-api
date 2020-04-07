using System;
using Business.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Business.Commands.Members
{
	public class UpdateMemberCommand : IRequest<ApiResponse<Member>>, ITransactionalCommand
	{
		[FromRoute(Name = "id")] public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid GuildId { get; set; }
	}
}