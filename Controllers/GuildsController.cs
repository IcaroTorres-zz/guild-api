using ActionFIlters;
using DTOs;
using Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;

namespace Controllers
{
    [Route("api/[controller]/v1"), ApiController]
    public class GuildsController : ControllerBase
    {
        [HttpGet("{id}", Name = "get-guild")]
        public ActionResult Get(Guid id, [FromServices] IGuildService service)
        {
            return Ok(service.Get(id));
        }

        [HttpGet(Name = "get-guilds")]
        public ActionResult GetAll([FromServices] IGuildService service, [FromQuery(Name = "count")] int count = 20)
        {
            return Ok(service.List(count));
        }

        [HttpPost(Name = "create-guild"), UseUnitOfWork]
        public ActionResult Create([FromBody] GuildDto payload, [FromServices] IGuildService service)
        {
            var guild = service.CreateOrUpdate(payload);

            return Created($"{Request.Path.ToUriComponent()}/{guild.Id}", guild);
        }

        [HttpPut("{id}", Name = "update-guild"), UseUnitOfWork]
        public ActionResult Update(Guid id, [FromBody] GuildDto payload, [FromServices] IGuildService service)
        {
            return Ok(service.CreateOrUpdate(payload, id));
        }

        [HttpPatch("{id}", Name = "patch-guild"), UseUnitOfWork]
        public ActionResult Patch(Guid id, [FromBody] JsonPatchDocument<Guild> patchPayload, [FromServices] IGuildService service)
        {
            return Ok(service.Update(id, patchPayload));
        }

        [HttpDelete("{id}", Name = "delete-guild"), UseUnitOfWork]
        public ActionResult Delete(Guid id, [FromServices] IGuildService service)
        {
            service.Delete(id);

            return NoContent();
        }
    }
}
