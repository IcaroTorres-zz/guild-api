using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.DTOs;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class v2GuildsController : ControllerBase
    {        
        // injected unit of work from startup.cs configure services
        private readonly IGuildService _guildService;
        public v2GuildsController(IGuildService service) => _guildService = service;

        [HttpPost] 
        public ActionResult CreateGuild([FromBody] GuildDto payload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessageBuilder(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors)
                                                                                          .Select(e => e.ErrorMessage))));
            try
            {
                return Created($"{Request.Path.ToUriComponent()}/{payload.Id}", _guildService.CreateGuild(payload));
            }
            catch (InvalidOperationException e) { return RollbackAndResult409(e); }
            catch (Exception e) { return RollbackAndResult500(e); }
        }
        
        [HttpGet("{id}")] 
        public ActionResult GuildInfo(string id)
        {
            try
            {
                var guild =_guildService.Get<Guild, string>(id);
                if (guild != null) return Ok(guild);
                else return NotFound(ErrorMessageBuilder($"Guild '{id}' not found"));
            }
            catch (Exception e) { return RollbackAndResult500(e); }
        }
        
        [HttpGet("list/{count:int=20}")] 
        public ActionResult Guilds(int count)
        {
            try { return Ok(_guildService.GetNthGuilds(count)); }
            catch (Exception e) { return RollbackAndResult500(e); }
        }

        [HttpPut("{id}")] 
        public ActionResult UpdateGuild(string id, [FromBody] GuildDto payload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessageBuilder(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors)
                                                                                          .Select(e => e.ErrorMessage))));
            if (payload.Members != null && !payload.Members.Contains(payload.MasterId))
                return BadRequest(ErrorMessageBuilder($"Members must contain given MasterId value {payload.MasterId}"));

            try
            {
                var guild = _guildService.Get<Guild, string>(id);
                if (guild != null)
                {
                    _guildService.Remove(guild);
                    _guildService.Complete();

                    // return Ok(_guildService.CreateGuild(payload));
                    return Ok(_guildService.UpdateGuild(payload));
                }
                else return NotFound(ErrorMessageBuilder($"Guild '{id}' not found"));
            }
            catch (Exception e) { return RollbackAndResult500(e); }
        }
        
        [HttpPatch("{id}")] 
        public ActionResult PatchGuild(string id, [FromBody] PatchDto payload)
        {
            var messageSuffixs = new Dictionary<PatchAction, string> ()
            {
                { PatchAction.Add, $"to add member '{payload.userId}' in guild '{id}'" },
                { PatchAction.Remove, $"to remove member '{payload.userId}' in guild '{id}'" },
                { PatchAction.Transfer, $"to transfer '{id}' to member {payload.userId}" },
            };

            try
            {
                if (payload.Action == PatchAction.Add)
                    _guildService.AddMember(id, payload.userId);
                else if (payload.Action == PatchAction.Remove)
                    _guildService.RemoveMember(id, payload.userId);
                else
                    _guildService.Transfer(id, payload.userId);

                _guildService.Complete();
                return Ok(true);
            }
            catch (ArgumentNullException e) { return RollbackAndResult404(e); }
            catch (InvalidOperationException e) { return RollbackAndResult409(e); }
            catch (Exception e) { return RollbackAndResult500(e); }
        }
        
        [HttpDelete("{id}")] // DONE
        public ActionResult DeleteGuild(string id)
        {
            try
            {
                var guild = _guildService.Get<Guild, string>(id);
                if (guild != null)
                {
                    _guildService.Remove(guild);
                    _guildService.Complete();
                    return NoContent();
                }
                else return NotFound(ErrorMessageBuilder($"Guild {id}' not Found"));
            }
            catch (Exception e) { return RollbackAndResult500(e); }
        }

        private string ErrorMessageBuilder(string Message = "") =>
            $"Fails on {Request.Method} " +
            $"to '{Request.Path.ToUriComponent()}'. " +
            $"Exception found: {Message}.";

        private ObjectResult RollbackAndResult404(Exception e)
        {
            _guildService.Rollback();
            return NotFound(ErrorMessageBuilder(e.Message));
        }
        private ObjectResult RollbackAndResult409(Exception e)
        {
            _guildService.Rollback();
            return Conflict(ErrorMessageBuilder(e.Message));
        }
        private ObjectResult RollbackAndResult500(Exception e)
        {
            _guildService.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageBuilder(e.Message));
        }
    }
}
