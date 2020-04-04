using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.ActionFilters;
using Business.Commands.Guilds;
using Domain.Entities;
using Domain.Entities.Nulls;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	[ApiController]
	[Route("api/[controller]/v1")]
	public class GuildsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IGuildRepository _repository;

		public GuildsController(IGuildRepository guildRepository, IMediator mediator)
		{
			_repository = guildRepository;
			_mediator = mediator;
		}

		[HttpGet("{id}", Name = "get-guild")]
		[UseCache(10)]
		public async Task<IActionResult> GetAsync(Guid id, CancellationToken token)
		{
			var result = await _repository.GetByIdAsync(id, true);

			return result is NullGuild ? (IActionResult) NotFound() : Ok(result);
		}

		[HttpGet(Name = "get-guilds")]
		[UseCache(20)]
		public async Task<IActionResult> GetAsync([FromQuery] GuildFilterCommand command, CancellationToken token)
		{
			var result = await _mediator.Send(command, token);

			return result.Errors.Any() ? (IActionResult) BadRequest(result.AsErrorOutput()) : Ok(result.Value);
		}

		[HttpPost(Name = "create-guild")]
		[UseUnitOfWork]
		public async Task<IActionResult> PostAsync([FromBody] CreateGuildCommand command, CancellationToken token)
		{
			var result = await _mediator.Send(command, token);

			return result.Errors.Any()
				? (IActionResult) BadRequest(result.AsErrorOutput())
				: CreatedAtAction(nameof(GetAsync), new {id = result.Value.Id}, result.Value);
		}

		[HttpPut("{id}", Name = "update-guild")]
		[UseUnitOfWork]
		public async Task<IActionResult> PutAsync([FromBody] UpdateGuildCommand command, CancellationToken token)
		{
			var result = await _mediator.Send(command, token);

			return result.Errors.Any() ? (IActionResult) BadRequest(result.AsErrorOutput()) : Ok(result.Value);
		}
	}
}