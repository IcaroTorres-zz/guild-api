using Application.ActionFilters;
using Domain.DTOs;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Application.Controllers
{
    [ApiController, Route("api/[controller]/v1")]
    public class MembersController : ControllerBase
    {
        [HttpGet("{id}", Name = "get-member"), UseCache(10)]
        public ActionResult Get(Guid id, [FromServices] IMemberService service)
        {
            return Ok(service.Get(id, readOnly: true));
        }

        [HttpGet(Name = "get-members"), UseCache(10)]
        public ActionResult GetAll([FromServices] IMemberService service, [FromQuery] MemberFilterDto payload)
        {
            return Ok(service.List(payload));
        }

        [HttpPost(Name = "create-member"), UseUnitOfWork]
        public ActionResult Create([FromBody] MemberDto payload, [FromServices] IMemberService service)
        {
            var member = service.Create(payload);

            return CreatedAtRoute("get-member", new { id = member.Entity.Id }, member);
        }

        [HttpPut("{id}", Name = "update-member"), UseUnitOfWork]
        public ActionResult Update(Guid id, [FromBody] MemberDto payload, [FromServices] IMemberService service)
        {
            return Ok(service.Update(payload, id));
        }

        [HttpPatch("{id}/promote", Name = "promote-member"), UseUnitOfWork]
        public ActionResult Promote(Guid id, [FromServices] IMemberService service)
        {
            return Ok(service.Promote(id));
        }

        [HttpPatch("{id}/demote", Name = "demote-member"), UseUnitOfWork]
        public ActionResult Demote(Guid id, [FromServices] IMemberService service)
        {
            return Ok(service.Demote(id));
        }

        [HttpPatch("{id}/leave", Name = "leave-guild"), UseUnitOfWork]
        public ActionResult LeaveGuild(Guid id, [FromServices] IMemberService service)
        {
            return Ok(service.LeaveGuild(id));
        }

        [HttpDelete("{id}", Name = "delete-member"), UseUnitOfWork]
        public ActionResult Delete(Guid id, [FromServices] IMemberService service)
        {
            service.Delete(id);

            return NoContent();
        }
    }
}
