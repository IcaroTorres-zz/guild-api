using Application.ActionFilters;
using Business.Dtos;
using Business.Usecases.Invites.AcceptInvite;
using Business.Usecases.Invites.CancelInvite;
using Business.Usecases.Invites.DenyInvite;
using Business.Usecases.Invites.GetInvite;
using Business.Usecases.Invites.InviteMember;
using Business.Usecases.Invites.ListInvite;
using Domain.Models;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Controllers
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

        [ProducesResponseType(typeof(Result<InviteDto>), StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPost(Name = "invite-member")]
        public async Task<IActionResult> InviteMember([FromBody] InviteMemberCommand command, CancellationToken cancellationToken)
        {
            command.SetupForCreation(Url, "get-invite", x => new { id = x.Id });

            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<InviteDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpGet("{id}", Name = "get-invite")]
        [UseCache(20)]
        public async Task<IActionResult> GetAsync([FromRoute] GetInviteCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<Pagination<InviteDto>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpGet(Name = "get-invites")]
        [UseCache(30)]
        public async Task<IActionResult> ListAsync([FromQuery] ListInviteCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<InviteDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPatch("{id}/accept", Name = "accept-invite")]
        public async Task<IActionResult> AcceptAsync([FromRoute] AcceptInviteCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<InviteDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPatch("{id}/deny", Name = "deny-invite")]
        public async Task<IActionResult> DenyAsync([FromRoute] DenyInviteCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<InviteDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPatch("{id}/cancel", Name = "cancel-invite")]
        public async Task<IActionResult> CancelAsync([FromRoute] CancelInviteCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
