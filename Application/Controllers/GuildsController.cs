using Application.ActionFilters;
using Business.Commands.Guilds;
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
  public class GuildsController : ControllerBase
  {
    private readonly IGuildRepository _guildRepository;
    private readonly IMediator _mediator;

    public GuildsController(IGuildRepository guildRepository, IMediator mediator)
    {
      _guildRepository = guildRepository;
      _mediator = mediator;
    }

    [HttpGet("{id}", Name = "get-guild"), UseCache(10)]
    public async Task<IActionResult> GetAsync(Guid id)
    {
      var result = await _guildRepository.GetByIdAsync(id, readOnly: true);

      return result is NullGuild ? (IActionResult)NotFound() : Ok(result);
    }

    [HttpGet(Name = "get-guilds"), UseCache(20)]
    public async Task<IActionResult> GetAsync([FromQuery] GuildFilterCommand command)
    {
      var result = await _mediator.Send(command);

      return result.Errors.Any() ? (IActionResult)BadRequest(result.AsErrorOutput()) : Ok(result.Value);
    }

    [HttpPost(Name = "create-guild"), UseUnitOfWork]
    public async Task<IActionResult> PostAsync([FromBody] UpdateGuildCommand command)
    {
      var result = await _mediator.Send(command);

      return result.Errors.Any()
          ? (IActionResult)BadRequest(result.AsErrorOutput())
          : CreatedAtAction(nameof(GetAsync), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id}", Name = "update-guild"), UseUnitOfWork]
    public async Task<IActionResult> PutAsync([FromBody] UpdateGuildCommand command)
    {
      var result = await _mediator.Send(command);

      return result.Errors.Any() ? (IActionResult)BadRequest(result.AsErrorOutput()) : Ok(result.Value);
    }
  }
}
