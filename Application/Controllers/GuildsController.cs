using Application.ActionFilters;
using Business.Dtos;
using Business.Usecases.Guilds.CreateGuild;
using Business.Usecases.Guilds.GetGuild;
using Business.Usecases.Guilds.ListGuild;
using Business.Usecases.Guilds.UpdateGuild;
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
    public class GuildsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuildsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(Result<GuildDto>), StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPost(Name = "create-guild")]
        public async Task<IActionResult> PostAsync([FromBody] CreateGuildCommand command, CancellationToken cancellationToken)
        {
            command.SetupForCreation(Url, "get-guild", x => new { id = x.Id });

            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<GuildDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpGet("{id}", Name = "get-guild")]
        [UseCache(10)]
        public async Task<IActionResult> GetAsync([FromRoute] GetGuildCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<Pagination<GuildDto>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpGet(Name = "get-guilds")]
        [UseCache(20)]
        public async Task<IActionResult> ListAsync([FromQuery] ListGuildCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(typeof(Result<GuildDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPut("{id}", Name = "update-guild")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] UpdateGuildCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            return await _mediator.Send(command, cancellationToken);
        }
	}
}
