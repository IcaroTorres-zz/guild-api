using System;
using Business.ResponseOutputs;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Business.Commands.Members
{
	public class MemberFilterCommand : IRequest<ApiResponse<Pagination<Member>>>
	{
		[FromQuery(Name = "name")] public string Name { get; set; } = string.Empty;
		[FromQuery(Name = "guildId")] public Guid GuildId { get; set; } = Guid.Empty;
		[FromQuery(Name = "count")] public int Count { get; set; } = 20;
	}
}