using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.ActionFilters;
using Application.Extensions;
using Business.Commands.Members;
using Domain.Entities;
using Domain.Entities.Nulls;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	[ApiController]
	[Route("api/[controller]/v1")]
	public class MembersController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IMemberRepository _repository;

		public MembersController(IMemberRepository repository, IMediator mediator)
		{
			_repository = repository;
			_mediator = mediator;
		}

		[HttpGet("{id}", Name = "get-member")]
		[UseCache(10)]
		public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
		{
			var result = await _repository.GetByIdAsync(id, true, cancellationToken);

			return result is NullMember ? this.NotFoundFor<Member>(id) : Ok(result);
		}

		[HttpGet(Name = "get-members")]
		[UseCache(10)]
		public async Task<IActionResult> GetAllAsync([FromQuery] MemberFilterCommand command,
			CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken);

			return result.Failures.Any() ? BadRequest(result.GenerateFailuresOutput()) as IActionResult : Ok(result.Data);
		}

		[HttpPost(Name = "create-member")]
		public async Task<IActionResult> CreateAsync([FromBody] CreateMemberCommand command,
			CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken);

			return result.Failures.Any()
				? BadRequest(result.GenerateFailuresOutput()) as IActionResult
				: CreatedAtRoute("get-member", new {id = result.Data.Id}, result.Data);
		}

		[HttpPut("{id}", Name = "update-member")]
		public async Task<IActionResult> UpdateAsync([FromBody] UpdateMemberCommand command,
			CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken);

			return result.Failures.Any() ? BadRequest(result.GenerateFailuresOutput()) as IActionResult : Ok(result.Data);
		}

		[HttpPatch("{id}/promote", Name = "promote-member")]
		public async Task<IActionResult> PromoteAsync(Guid id, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new PromoteMemberCommand(id, _repository), cancellationToken);

			return result.Failures.Any() ? BadRequest(result.GenerateFailuresOutput()) as IActionResult : Ok(result.Data);
		}

		[HttpPatch("{id}/demote", Name = "demote-member")]
		public async Task<IActionResult> DemoteAsync(Guid id, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new DemoteMemberCommand(id, _repository), cancellationToken);

			return result.Failures.Any() ? BadRequest(result.GenerateFailuresOutput()) as IActionResult : Ok(result.Data);
		}

		[HttpPatch("{id}/leave", Name = "leave-guild")]
		public async Task<IActionResult> LeaveGuildAsync(Guid id, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new LeaveGuildCommand(id, _repository), cancellationToken);

			return result.Failures.Any() ? BadRequest(result.GenerateFailuresOutput()) as IActionResult : Ok(result.Data);
		}
	}
}