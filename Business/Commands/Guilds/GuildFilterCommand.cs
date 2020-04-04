using Business.ResponseOutputs;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Business.Commands.Guilds
{
	public class GuildFilterCommand : IRequest<ApiResponse<Pagination<Guild>>>
	{
		[FromQuery(Name = "count")] public long Count { get; set; }
	}
}