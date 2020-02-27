using ActionFIlters;
using DTOs;
using Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;

namespace Controllers
{
    [Route("api/guilds/v1"), Produces("application/json"), ApiController, UseUnitOfWork]
    public class V1GuildsController : ControllerBase
    {
        // injected unit of work from startup.cs configure services
        private readonly IGuildService _service;
        public V1GuildsController(IGuildService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult CreateGuild([FromBody] GuildDto payload)
        {
            var guild = _service.CreateOrUpdate(payload);

            return Created($"{Request.Path.ToUriComponent()}/{guild.Id}", guild);
        }

        [HttpGet("{id}")]
        public ActionResult GuildInfo(Guid id)
        {
            return Ok(_service.Get(id));
        }

        [HttpGet]
        public ActionResult Guilds([FromQuery(Name = "count")] int count)
        {
            return Ok(_service.List(count));
        }

        [HttpPut("{id}")]
        public ActionResult UpdateGuild(Guid id, [FromBody] GuildDto payload)
        {
            return Ok(_service.CreateOrUpdate(payload, id));
        }

        [HttpPatch("{id}")]
        public ActionResult PatchGuild(Guid id, [FromBody] JsonPatchDocument<Guild> patchPayload)
        {
            return Ok(_service.Update(id, patchPayload));
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteGuild(Guid id)
        {
            _service.Delete(id);

            return NoContent();
        }
    }
}
