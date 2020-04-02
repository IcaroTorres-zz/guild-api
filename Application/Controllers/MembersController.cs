using Application.ActionFilters;
using Business.Commands.Members;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController, Route("api/[controller]/v1")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMediator _mediator;

        public MembersController(IMemberRepository memberRepository, IMediator mediator)
        {
            _memberRepository = memberRepository;
            _mediator = mediator;
        }

        [HttpGet("{id}", Name = "get-member"), UseCache(10)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _memberRepository.GetByIdAsync(id, readOnly: true);

            return result is NullMember ? (IActionResult)NotFound() : Ok(result);
        }

        [HttpGet(Name = "get-members"), UseCache(10)]
        public async Task<IActionResult> GetAllAsync([FromQuery] MemberFilterCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Errors.Any() ? (IActionResult)BadRequest(result.Errors) : Ok(result.Value);
        }

        [HttpPost(Name = "create-member"), UseUnitOfWork]
        public async Task<IActionResult> CreateAsync([FromBody] CreateMemberCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Errors.Any()
                ? (IActionResult)BadRequest(result.Errors)
                : CreatedAtAction(nameof(GetAsync), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}", Name = "update-member"), UseUnitOfWork]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateMemberCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Errors.Any() ? (IActionResult)BadRequest(result.Errors) : Ok(result.Value);
        }

        [HttpPatch("{id}/promote", Name = "promote-member"), UseUnitOfWork]
        public async Task<IActionResult> PromoteAsync([FromRoute] PromoteMemberCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Errors.Any() ? (IActionResult)BadRequest(result.Errors) : Ok(result.Value);
        }

        [HttpPatch("{id}/demote", Name = "demote-member"), UseUnitOfWork]
        public async Task<IActionResult> DemoteAsync([FromRoute] DemoteMemberCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Errors.Any() ? (IActionResult)BadRequest(result.Errors) : Ok(result.Value);
        }

        [HttpPatch("{id}/leave", Name = "leave-guild"), UseUnitOfWork]
        public async Task<IActionResult> LeaveGuildAsync([FromRoute] LeaveGuildCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Errors.Any() ? (IActionResult)BadRequest(result.Errors) : Ok(result.Value);
        }
    }
}
