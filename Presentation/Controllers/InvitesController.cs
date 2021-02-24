using Application.Common.Responses;
using Application.Common.Results;
using Application.Invites.Commands.AcceptInvite;
using Application.Invites.Commands.CancelInvite;
using Application.Invites.Commands.DenyInvite;
using Application.Invites.Commands.InviteMember;
using Application.Invites.Queries.GetInvite;
using Application.Invites.Queries.ListInvite;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/v1")]
    public class InvitesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvitesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(Result<InviteResponse>), StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPost(Name = "invite-member")]
        public async Task<IActionResult> InviteMember([FromBody] InviteMemberCommand command, CancellationToken cancellationToken)
        {
            command.SetupForCreation(Url, "get-invite", x => new { id = x.Id });

            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<InviteResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpGet("{id}", Name = "get-invite")]
        [UseCache(20)]
        public async Task<IActionResult> GetAsync([FromRoute] GetInviteCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<PagedResponse<InviteResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpGet(Name = "get-invites")]
        [UseCache(30)]
        public async Task<IActionResult> ListAsync([FromQuery] ListInviteCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<InviteResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPatch("{id}/accept", Name = "accept-invite")]
        public async Task<IActionResult> AcceptAsync([FromRoute] AcceptInviteCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<InviteResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPatch("{id}/deny", Name = "deny-invite")]
        public async Task<IActionResult> DenyAsync([FromRoute] DenyInviteCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<InviteResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPatch("{id}/cancel", Name = "cancel-invite")]
        public async Task<IActionResult> CancelAsync([FromRoute] CancelInviteCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
