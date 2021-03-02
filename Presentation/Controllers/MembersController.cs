using Application.Common.Abstractions;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Members.Commands.ChangeMemberName;
using Application.Members.Commands.CreateMember;
using Application.Members.Commands.DemoteMember;
using Application.Members.Commands.LeaveGuild;
using Application.Members.Commands.PromoteMember;
using Application.Members.Queries.GetMember;
using Application.Members.Queries.ListMember;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Controllers
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

        [ProducesResponseType(typeof(Output<MemberResponse>), StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpPost(Name = "create-member")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateMemberCommand command, CancellationToken cancellationToken)
        {
            command.SetupForCreation(Url, "get-member", x => new { id = x.Id });

            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Output<MemberResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpGet("{id}", Name = "get-member")]
        [UseCache(10)]
        public async Task<IActionResult> GetAsync([FromRoute] GetMemberCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Output<PagedResponse<MemberResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpGet(Name = "get-members")]
        [UseCache(10)]
        public async Task<IActionResult> ListAsync([FromQuery] ListMemberCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Output<MemberResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpPatch("{id}/name", Name = "change-member-name")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] ChangeMemberNameCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Output<MemberResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpPatch("{id}/promote", Name = "promote-member")]
        public async Task<IActionResult> PromoteAsync([FromRoute] PromoteMemberCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Output<MemberResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpPatch("{id}/demote", Name = "demote-member")]
        public async Task<IActionResult> DemoteAsync([FromRoute] DemoteMemberCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Output<MemberResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpPatch("{id}/leave", Name = "leave-guild")]
        public async Task<IActionResult> LeaveGuildAsync([FromRoute] LeaveGuildCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
