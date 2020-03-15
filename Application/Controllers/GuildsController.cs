using Domain.Services;
using Application.ActionFilters;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Application.Controllers
{
    [Route("api/[controller]/v1"), ApiController]
    public class GuildsController : ControllerBase
    {
        [HttpGet("{id}", Name = "get-guild"), ValidateResult,  CacheResponse(30)]
        public IActionResult Get(Guid id, [FromServices] IGuildService service)
        {
            return Ok(service.Get(id));
        }

        [HttpGet(Name = "get-guilds"), ValidateResult, CacheResponse(30)]
        public IActionResult Get([FromServices] IGuildService service, [FromQuery(Name = "count")] int count = 20)
        {
            return Ok(service.List(count));
        }

        [HttpPost(Name = "create-guild"), UseUnitOfWork]
        public IActionResult Post([FromBody] GuildDto payload, [FromServices] IGuildService service)
        {
            var guild = service.Create(payload);

            return CreatedAtRoute("get-guild", new { id = guild.Id } , guild);
        }

        [HttpPut("{id}", Name = "update-guild"), UseUnitOfWork]
        public IActionResult Put(Guid id, [FromBody] GuildDto payload, [FromServices] IGuildService service)
        {
            return Ok(service.Update(payload, id));
        }

        [HttpDelete("{id}", Name = "delete-guild"), UseUnitOfWork]
        public IActionResult Delete(Guid id, [FromServices] IGuildService service)
        {
            service.Delete(id);
            return NoContent();
        }
    }
}
