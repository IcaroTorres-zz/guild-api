using Application.ActionFilters;
using Business.Dtos;
using Business.Usecases.Members.ChangeMemberName;
using Business.Usecases.Members.CreateMember;
using Business.Usecases.Members.DemoteMember;
using Business.Usecases.Members.GetMember;
using Business.Usecases.Members.LeaveGuild;
using Business.Usecases.Members.ListMember;
using Business.Usecases.Members.PromoteMember;
using Domain.Hateoas;
using Domain.Models;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/v1")]
    public class MembersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IApiHateoasFactory _hateoasFactory;

        public MembersController(IMediator mediator, IApiHateoasFactory hateoasFactory)
        {
            _mediator = mediator;
            _hateoasFactory = hateoasFactory;
        }

        [ProducesResponseType(typeof(Result<MemberDto>), StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPost(Name = "create-member")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateMemberCommand command, CancellationToken cancellationToken)
        {
            command.SetupForCreation(Url, "get-member", x => new { id = x.Id });

            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<MemberDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpGet("{id}", Name = "get-member")]
        [UseCache(10)]
        public async Task<IActionResult> GetAsync([FromRoute] GetMemberCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<Pagination<MemberDto>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpGet(Name = "get-members")]
        [UseCache(10)]
        public async Task<IActionResult> ListAsync([FromQuery] ListMemberCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<MemberDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPatch("{id}/name", Name = "change-member-name")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] ChangeMemberNameCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<MemberDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPatch("{id}/promote", Name = "promote-member")]
        public async Task<IActionResult> PromoteAsync([FromRoute] PromoteMemberCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<MemberDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPatch("{id}/demote", Name = "demote-member")]
        public async Task<IActionResult> DemoteAsync([FromRoute] DemoteMemberCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<MemberDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPatch("{id}/leave", Name = "leave-guild")]
        public async Task<IActionResult> LeaveGuildAsync([FromRoute] LeaveGuildCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
