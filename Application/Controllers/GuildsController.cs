using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.ActionFilters;
using Application.Extensions;
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
		public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
		{
			var result = await _repository.GetByIdAsync(id, true, cancellationToken);

			return result is NullGuild ? this.NotFoundFor<Guild>(id) : Ok(result);
		}

		[HttpGet(Name = "get-guilds")]
		[UseCache(20)]
		public async Task<IActionResult> GetAsync([FromQuery] GuildFilterCommand command,
			CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken);

			return result.Failures.Any() ? BadRequest(result.GenerateFailuresOutput()) as IActionResult : Ok(result.Data);
		}

		[HttpPost(Name = "create-guild")]
		public async Task<IActionResult> PostAsync([FromBody] CreateGuildCommand command,
			CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken);

			return result.Failures.Any()
				? BadRequest(result.GenerateFailuresOutput()) as IActionResult
				: CreatedAtRoute("get-guild", new {id = result.Data.Id}, result.Data);
		}

		[HttpPut("{id}", Name = "update-guild")]
		public async Task<IActionResult> PutAsync([FromBody] UpdateGuildCommand command,
			CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken);

			return result.Failures.Any() ? BadRequest(result.GenerateFailuresOutput()) as IActionResult : Ok(result.Data);
		}
	}
}