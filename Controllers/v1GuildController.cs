using DTOs;
using Services;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;

namespace Controllers
{
    [Route("api/guilds/v1")]
    [Produces("application/json")]
    [ApiController]
    public class V1GuildsController : ControllerBase
    {
        // injected unit of work from startup.cs configure services
        private readonly IGuildService _service;
        public V1GuildsController(IGuildService service) => _service = service;

        [HttpPost("")]
        public ActionResult CreateGuild([FromBody] GuildDto payload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessageBuilder(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors)
                                                                                          .Select(e => e.ErrorMessage))));
            try
            {
                var guild = _service.CreateGuild(payload);
                _service.Commit();
                return Created($"{Request.Path.ToUriComponent()}/{guild.Id}", ResponseWithLinks(guild));
            }
            catch (InvalidOperationException e) { return RollbackAndResult409(e); }
            catch (Exception e) { return RollbackAndResult500(e); }
        }

        [HttpGet("{id}")]
        public ActionResult GuildInfo(Guid id)
        {
            try
            {
                var guild = _service.Get<Guild>(id);
                if (guild != null) return Ok(ResponseWithLinks(guild));
                else return NotFound(ErrorMessageBuilder($"Guild '{id}' not found"));
            }
            catch (Exception e) { return RollbackAndResult500(e); }
        }

        [HttpGet("list/{count:int=20}")]
        public ActionResult Guilds(int count)
        {
            try { return Ok(ResponseWithLinks(_service.List(count))); }
            catch (Exception e) { return RollbackAndResult500(e); }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateGuild(Guid id, [FromBody] GuildDto payload)
        {
            if (!ModelState.IsValid || payload.Id != id)
                return BadRequest(ErrorMessageBuilder(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors)
                                                                                          .Select(e => e.ErrorMessage))));
            if (payload.Members != null && !payload.Members.Contains(payload.MasterId))
                return BadRequest(ErrorMessageBuilder($"Members must contain given MasterId value {payload.MasterId}"));

            try
            {
                var guild = _service.UpdateGuild(payload);
                if (guild.Id == id)
                {
                    _service.Commit();
                    return Ok(ResponseWithLinks(guild));
                }
                else
                    throw new InvalidOperationException($"Conflict in keys given in Put Route and Data. [{id} != {guild.Id}]");
            }
            catch (ArgumentNullException e) { return RollbackAndResult404(e); }
            catch (InvalidOperationException e) { return RollbackAndResult409(e); }
            catch (Exception e) { return RollbackAndResult500(e); }
        }

        [HttpPatch("{id}")]
        public ActionResult PatchGuild(Guid id, [FromBody] JsonPatchDocument<Guild> patchPayload)
        {
            try
            {
                var guild = _service.Get<Guild>(id);
                patchPayload.ApplyTo(guild);

                return Ok(ResponseWithLinks(guild));
            }
            catch (Exception e) { return RollbackAndResult500(e); }
        }
        //    var messageSuffixs = new Dictionary<PatchAction>()
        //    {
        //        { PatchAction.Add, $"to add member '{payload.userId}' in guild '{id}'" },
        //        { PatchAction.Remove, $"to remove member '{payload.userId}' in guild '{id}'" },
        //        { PatchAction.Transfer, $"to transfer '{id}' to member {payload.userId}" },
        //    };

        //    try
        //    {
        //        if (payload.Action == PatchAction.Add)
        //            _service.AddMember(id, payload.userId);
        //        else if (payload.Action == PatchAction.Remove)
        //            _service.RemoveMember(id, payload.userId);
        //        else
        //            _service.Transfer(id, payload.userId);

        //        _service.Complete();
        //        return Ok(true);
        //    }
        //    catch (ArgumentNullException e) { return RollbackAndResult404(e); }
        //    catch (InvalidOperationException e) { return RollbackAndResult409(e); }
        //    catch (Exception e) { return RollbackAndResult500(e); }
        //}

        [HttpDelete("{id}")]
        public ActionResult DeleteGuild(Guid id)
        {
            try
            {
                var guild = _service.Get<Guild>(id);
                if (guild != null)
                {
                    _service.Remove(guild);
                    _service.Commit();
                    return StatusCode(StatusCodes.Status204NoContent, ResponseWithLinks(guild));
                }
                else throw new ArgumentNullException(nameof(Guild), $"Guild with id {id}' not Found");
            }
            catch (ArgumentNullException e) { return RollbackAndResult404(e); }
            catch (Exception e) { return RollbackAndResult500(e); }
        }

        private string ErrorMessageBuilder(string Message = "") =>
            $"Fails on {Request.Method} " +
            $"to '{Request.Path.ToUriComponent()}'. " +
            $"Exception found: {Message}.";

        private ObjectResult RollbackAndResult404(Exception e)
        {
            _service.Rollback();
            return NotFound(ErrorMessageBuilder(e.Message));
        }
        private ObjectResult RollbackAndResult409(Exception e)
        {
            _service.Rollback();
            return Conflict(ErrorMessageBuilder(e.Message));
        }
        private ObjectResult RollbackAndResult500(Exception e)
        {
            _service.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageBuilder(e.Message));
        }

        private object ResponseWithLinks(IEnumerable<Entity<Guid>> entities)
        {
            var uri = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/api/guilds/v1";
            var Data = entities;
            var entityType = entities.First().GetType().Name.Replace("Proxy", "");
            return new
            {
                Data,
                Links = new List<object>{
                    new
                    {
                        Href = $"{uri}",
                        Method = "Post",
                        Intent = $"Create {entityType}",
                        Type = entityType
                    }
                }
            };
        }
        private object ResponseWithLinks(Entity<Guid> entity)
        {
            var uri = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/api/guilds/v1";
            var Data = entity;
            var entityType = entity.GetType().Name.Replace("Proxy", "");
            var Links = new List<object>();
            var post = new
            {
                Href = $"{uri}",
                Method = "Post",
                Intent = $"Create {entityType}",
                Type = entityType
            };
            var get = new
            {
                Href = $"{uri}/{entity.Id}",
                Method = "Get",
                Intent = $"{entityType} info",
                Type = entityType
            };
            var put = new
            {
                Href = $"{uri}/{entity.Id}",
                Method = "Put",
                Intent = $"Rewrite {entityType}",
                Type = entityType
            };
            var patch = new
            {
                Href = $"{uri}/{entity.Id}",
                Method = "Patch",
                Intent = $"Partially Update {entityType}",
                Type = entityType
            };
            var delete = new
            {
                Href = $"{uri}/{entity.Id}",
                Method = "Delete",
                Intent = $"Delete {entityType}",
                Type = entityType
            };
            var list = new
            {
                Href = uri + "/{count:int?}",
                Method = "Get",
                Intent = $"List of {entityType} items",
                Type = entityType
            };

            switch (Request.Method.ToLower())
            {
                case "post":
                    Links.AddRange(new[] { get, list, put, patch, delete });
                    break;
                case "get":
                    Links.AddRange(new[] { put, patch, delete });
                    break;
                case "put":
                    Links.AddRange(new[] { get, patch, delete });
                    break;
                case "patch":
                    Links.AddRange(new[] { get, put, delete });
                    break;
                case "delete":
                    Links.AddRange(new[] { post, list });
                    break;
                default:
                    break;
            }
            if (Request.Method.ToLower() == "delete")
                return new { Links, Data = new { } };
            else
                return new { Data, Links };
        }
    }
}
