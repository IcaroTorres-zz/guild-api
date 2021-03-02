using Application.Common.Responses;
using Application.Common.Results;
using Application.Guilds.Commands.CreateGuild;
using Application.Guilds.Commands.UpdateGuild;
using Application.Guilds.Queries.GetGuild;
using Application.Guilds.Queries.ListGuild;
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
    public class GuildsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuildsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(Output<GuildResponse>), StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpPost(Name = "create-guild")]
        public async Task<IActionResult> PostAsync([FromBody] CreateGuildCommand command, CancellationToken cancellationToken)
        {
            command.SetupForCreation(Url, "get-guild", x => new { id = x.Id });

            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Output<GuildResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpGet("{id}", Name = "get-guild")]
        [UseCache(10)]
        public async Task<IActionResult> GetAsync([FromRoute] GetGuildCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Output<PagedResponse<GuildResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpGet(Name = "get-guilds")]
        [UseCache(20)]
        public async Task<IActionResult> ListAsync([FromQuery] ListGuildCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Output<GuildResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpPut("{id}", Name = "update-guild")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] UpdateGuildCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
