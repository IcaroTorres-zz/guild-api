using System;
using Business.ResponseOutputs;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Business.Commands.Members
{
	public class UpdateMemberCommand : IRequest<ApiResponse<Member>>
	{
		[FromRoute(Name = "id")] public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid GuildId { get; set; }
	}
}