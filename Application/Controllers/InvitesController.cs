using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.ActionFilters;
using Application.Extensions;
using Business.Commands.Invites;
using Domain.Entities;
using Domain.Entities.Nulls;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	[ApiController]
	[Route("api/[controller]/v1")]
	public class InvitesController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IInviteRepository _repository;

		public InvitesController(IInviteRepository repository, IMediator mediator)
		{
			_repository = repository;
			_mediator = mediator;
		}

		[HttpGet("{id}", Name = "get-invite")]
		[UseCache(20)]
		public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
		{
			var result = await _repository.GetByIdAsync(id, true, cancellationToken);

			return result is NullInvite ? this.NotFoundFor<Invite>(id) : Ok(result);
		}

		[HttpGet(Name = "get-invites")]
		[UseCache(30)]
		public async Task<IActionResult> GetAllAsync([FromQuery] InviteFilterCommand command,
			CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken);

			return result.Failures.Any() ? BadRequest(result.GenerateFailuresOutput()) as IActionResult : Ok(result.Data);
		}

		[HttpPost(Name = "invite-member")]
		public async Task<IActionResult> InviteMember([FromBody] InviteMemberCommand command,
			CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken);

			return result.Failures.Any()
				? BadRequest(result.GenerateFailuresOutput()) as IActionResult
				: CreatedAtRoute("get-invite", new {id = result.Data.Id}, result.Data);
		}

		[HttpPatch("{id}/accept", Name = "accept-invite")]
		public async Task<IActionResult> AcceptAsync(Guid id, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new AcceptInviteCommand(id, _repository), cancellationToken);

			return result.Failures.Any() ? BadRequest(result.GenerateFailuresOutput()) as IActionResult : Ok(result.Data);
		}

		[HttpPatch("{id}/decline", Name = "decline-invite")]
		public async Task<IActionResult> DeclineAsync(Guid id, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new DeclineInviteCommand(id, _repository), cancellationToken);

			return result.Failures.Any() ? BadRequest(result.GenerateFailuresOutput()) as IActionResult : Ok(result.Data);
		}

		[HttpPatch("{id}/cancel", Name = "cancel-invite")]
		public async Task<IActionResult> CancelAsync(Guid id, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CancelInviteCommand(id, _repository), cancellationToken);

			return result.Failures.Any() ? BadRequest(result.GenerateFailuresOutput()) as IActionResult : Ok(result.Data);
		}
	}
}