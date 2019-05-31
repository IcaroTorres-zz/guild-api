using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;

namespace api.Controllers
{
    [Route("api")]
    [Produces("application/json")]
    [ApiController]
    public class GuildsController : ControllerBase
    {        
        // injected unit of work from startup.cs configure services
        private readonly IUnitOfWork _unitOfWork;
        public GuildsController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpPost("guilds")] //DONE
        public ActionResult CreateGuild([FromBody]  GuildForm payload)
        {
            try
            {
                var guild = _unitOfWork.Guilds.CreateGuild(payload.Name, payload.MasterName);
                _unitOfWork.Complete();
                return Created($"api/guilds/{guild.Name}", guild);
            }
            catch(Exception e)
            {
                _unitOfWork.Rollback();
                var exceptionParts = e.Message.Split("||");
                if(exceptionParts.Length == 2 && exceptionParts[0].Equals("409"))
                    return Conflict(ExceptionMessageBuilder("POST", "api/guilds", exceptionParts[1]));

                return BadRequest(ExceptionMessageBuilder("POST", "api/guilds", e.Message));
            }
        }
        
        [HttpGet("[controller]/{name}")] //DONE
        public ActionResult GuildInfo(string name)
        {
            try
            {
                var guild =_unitOfWork.Guilds.Get(name);
                if (guild != null) return Ok(guild);
                else return NotFound(ExceptionMessageBuilder("GET", $"api/guilds/{name}", $"Guild {name} not found"));
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionMessageBuilder("GET", $"api/guilds/{name}", e.Message));
            }
        }
        
        [HttpGet("[controller]/list/{count=20}")] //DONE
        public ActionResult Guilds(int count)
        {
            try { return Ok(_unitOfWork.Guilds.GetNthGuilds(count)); }
            catch (Exception e) { return BadRequest(ExceptionMessageBuilder("GET", $"api/guilds/{count}", e.Message)); }
        }

        [HttpPut("[controller]/{name}")] //DONE
        public ActionResult UpdateGuild(string name, [FromBody]  GuildForm payload)
        {
            // Guild predata = new Guild();
            // Guild posdata = new Guild();
            try
            {
                if (!ModelState.IsValid)
                    BadRequest(ExceptionMessageBuilder("PUT", $"api/guilds/{name}", "Ivalid Formated payload Data"));

                var guild = _unitOfWork.Guilds.Get(name);
                if (guild != null)
                {
                    // predata = guild;
                    _unitOfWork.Guilds.Remove(guild);
                    _unitOfWork.Complete();

                    // pre-conditions
                    if (string.IsNullOrWhiteSpace(payload.MasterName))
                        return BadRequest(ExceptionMessageBuilder("PUT", $"api/guilds/{name}", "MasterName can not be Null or whitespaces"));

                    if (!payload.Members.Contains(payload.MasterName))
                        return BadRequest(ExceptionMessageBuilder("PUT",
                                                                  $"api/guilds/{name}",
                                                                  $"Members must contain given MasterName value {payload.MasterName}"));

                    // re-mounting enity with new values
                    var updatedGuild = new Guild
                    {
                        Name = name,
                        MasterName = payload.MasterName,
                        Members = payload.Members?
                                         .Select(memberName => _unitOfWork.Users.Get(memberName)
                                                               ?? new User { Name = memberName }).ToList()
                                                               ?? new List<User>()
                    };

                    _unitOfWork.Guilds.Add(updatedGuild);
                    _unitOfWork.Complete();
                    // posdata = updatedGuild;
                    return Ok(updatedGuild);
                }
                else return NotFound(ExceptionMessageBuilder("PUT", $"api/guilds/{name}", $"Guild {name} not found"));
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                // posdata = predata;
                // _unitOfWork.Guilds.Update(posdata);
                // _unitOfWork.Complete();
                return BadRequest(ExceptionMessageBuilder("PUT", $"api/guilds/{name}", e.Message));
            }
        }
        
        [HttpPatch("[controller]/{name}")] //DONE
        public ActionResult PatchGuild(string name, [FromBody] Dictionary<string, string> payload)
        {
            if (payload.TryGetValue("addedMember", out string addedMemberName))
                return UpdateMembers(name, addedMemberName);

            if (payload.TryGetValue("removedMember", out string removedMemberName))
                return UpdateMembers(name, removedMemberName, 1);

            if (payload.TryGetValue("newMasterName", out string newMasterName))
                return Transfer(name, newMasterName);

            return BadRequest($"Fails on PATCH 'api/guilds/{name}'.");
        }

        //DONE
        private ActionResult UpdateMembers(string name, string memberName, int mode = 0)
        {
            var stringMode = mode == 0 ? "add" : "remove";
            try
            {
                if (mode == 0) _unitOfWork.Guilds.AddMember(name, memberName);
                else _unitOfWork.Guilds.RemoveMember(name, memberName);
                
                _unitOfWork.Complete();
                return Ok(true);
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                var exceptionParts = e.Message.Split("||");
                if(exceptionParts.Length == 2)
                {
                    var errorMessage = ExceptionMessageBuilder("PATCH",
                                                                $"api/guilds/{name}",
                                                                $"{exceptionParts[1]} to {stringMode} member {memberName}"); 

                    if (exceptionParts[0].Equals("404")) return NotFound(errorMessage);
                    if (exceptionParts[0].Equals("409")) return Conflict(errorMessage);
                }
                return BadRequest(false);
            }
        }
        
        //DONE
        private ActionResult Transfer(string name, string masterName)
        {
            try
            {
                if (_unitOfWork.Guilds.TransferOwnership(name, masterName)) _unitOfWork.Complete();
                return Ok(true);
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                var exceptionParts = e.Message.Split("||");
                if(exceptionParts.Length == 2)
                {
                    var errorMessage = ExceptionMessageBuilder("PATCH",
                                                            $"api/guilds/{name}",
                                                            $"{exceptionParts[1]} while transfering guild ownership to member {masterName}"); 
                    if (exceptionParts[0].Equals("404")) return NotFound(errorMessage);
                    if (exceptionParts[0].Equals("409")) return Conflict(errorMessage);
                }
                return BadRequest(false);
            }
        }
        
        [HttpDelete("[controller]/{name}")] // DONE
        public ActionResult DeleteGuild(string name)
        {
            try
            {
                var guild = _unitOfWork.Guilds.Get(name);
                if (guild != null)
                {
                    _unitOfWork.Guilds.Remove(guild);
                    _unitOfWork.Complete();
                    return Ok(true);
                }
                else return NotFound(ExceptionMessageBuilder("DELETE", $"api/guilds/{name}", $"Guild {name} not Found"));
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                return BadRequest(ExceptionMessageBuilder("DELETE", $"api/guilds/{name}", e.Message));
            }
        }

        private string ExceptionMessageBuilder(string Method, string Uri, string Message) =>
            $"Fails on {Method} to '{Uri}'. Exception found: {Message}.";
    }
}
