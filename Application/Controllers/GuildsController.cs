using Domain.Services;
using Application.ActionFilters;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using DataAccess.Entities;
using Domain.Validations;

namespace Application.Controllers
{
    [Route("api/[controller]/v1"), ApiController]
    public class GuildsController : ControllerBase
    {
        [HttpGet("{id}", Name = "get-guild"), CacheResponse(30)]
        public IActionResult Get(Guid id, [FromServices] IGuildService service)
        {
            IValidationResult result = new NotFoundValidationResult($"A {nameof(Guild)} with given {nameof(Guild.Id)} '{id}' do not exists.");

            if (service.GetGuild(id) is Guild guild)
            {
                return guild.ValidationResult.AsActionResult();
            }

            return result.AsActionResult();
        }

        [HttpGet(Name = "get-guilds"), CacheResponse(30)]
        public IActionResult GetAll([FromServices] IGuildService service, [FromQuery(Name = "count")] int count = 20)
        {
            return Ok(service.List(count));
        }

        [HttpPost(Name = "create-guild"), UseUnitOfWork]
        public IActionResult Create([FromBody] GuildDto payload, [FromServices] IGuildService service)
        {
            var result = service.Create(payload);

            if (result is CreatedValidationResult createdResult && createdResult.Data is Guild guild)
                return Created($"{Request.Path.ToUriComponent()}{guild.Id}", guild);
                
            return new ContentResult
            {
                Content = result.AsSerializedError(),
                StatusCode = (int)result.Status
            };
        }

        [HttpPut("{id}", Name = "update-guild"), UseUnitOfWork]
        public IActionResult Update(Guid id, [FromBody] GuildDto payload, [FromServices] IGuildService service)
        {
            var result = service.Update(payload, id);

            if (result is OkValidationResult)
                return Ok(result.Data);

            return new ContentResult
            {
                Content = result.AsSerializedError(),
                StatusCode = (int)result.Status
            };
        }

        [HttpDelete("{id}", Name = "delete-guild"), UseUnitOfWork]
        public IActionResult Delete(Guid id, [FromServices] IGuildService service)
        {
            var result = service.Delete(id);

            if (result is NoContentValidationResult)
                return NoContent();

            return new ContentResult
            {
                Content = result.AsSerializedError(),
                StatusCode = (int)result.Status
            };
        }
    }
}
