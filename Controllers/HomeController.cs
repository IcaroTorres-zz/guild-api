using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lumen.api.Models;
using lumen.api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;

namespace lumen.api.Controllers
{
    [Route("lumen.api")]
    [Produces("application/json")]
    [ApiController]
    public class HomeController : ControllerBase
    {        
        // injected unit of work from startup.cs configure services
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpPost("users")]
        public ActionResult CreateUser([FromBody] string username)
        {
            try {
                var user =_unitOfWork.Users.Get(username);
                if (user != null)
                    throw new ArgumentException($"{username} already exists.");

                user = new User(){ Id = username };
                _unitOfWork.Users.Add(user);
                _unitOfWork.Complete();
                return Created("lumen.api/users", user);
            } catch (Exception e) {
                _unitOfWork.Rollback();
                return BadRequest($"error: Fails on user [{username}]. Exception found: {e.Message}.");
            }
        }
        [HttpGet("users/{username:string}")]
        public ActionResult GetUser(string username)
        {
            try {
                var user =_unitOfWork.Users.Get(username);
                if (user != null) return Ok(user);
                else return NotFound(username);
            } catch (Exception e) {
                _unitOfWork.Rollback();
                return BadRequest($"error: Fails on user [{username}]. Exception found: {e.Message}.");
            }
        }
        [HttpPut("users/{username:string}")]
        public ActionResult UpdateUser(string username, [FromBody] User updatedUser)
        {
            if (!username.Equals(updatedUser.Id)) return BadRequest("Ids not matching on Request URL and Body.");
            try {
                var user =_unitOfWork.Users.Get(username);
                if (user == null) return NotFound(username);
                else
                {
                    user = updatedUser;
                    _unitOfWork.Users.Update(user);
                    _unitOfWork.Complete();
                    return Ok(user);
                }
            } catch (Exception e) {
                _unitOfWork.Rollback();
                return BadRequest($"error: Fails on user [{username}]. Exception found: {e.Message}.");
            }
        }
        [HttpGet("users/{count=20}")]
        public ActionResult GetUsers(int count)
        {
            try {
                var users = _unitOfWork.Users.GetNthUsers(count);
                if (users.Any()) return Ok(users);
                else return NoContent();
            } catch (Exception e) {
                _unitOfWork.Rollback();
                return BadRequest($"error: Fails on retrieving users. Exception found: {e.Message}.");
            }
        }
        [HttpPost("guilds")]
        public ActionResult CreateGuild([FromBody] Guild guildInfo)
        {
            try { 
                var createdGuild = _unitOfWork.Guilds.CreateGuild(guildInfo.Id, guildInfo.MasterId);
                if (createdGuild != null)
                    _unitOfWork.Complete();
                else 
                    throw new Exception($"error: fails creating [{guildInfo.Id}] as a new guild.");
                return Created("lumen.api/createguild", createdGuild);
            }
            catch(Exception e) {
                _unitOfWork.Rollback();
                return BadRequest(e.Message);
            }
        }
        [HttpGet("[action]/{guildname}")]
        public ActionResult GuildInfo(string guildname)
        {
            try {
                var guild =_unitOfWork.Guilds.Get(guildname);
                if (guild != null) return Ok(guild);
                else return NotFound(guildname);
            } catch (Exception e) {
                _unitOfWork.Rollback();
                return BadRequest($"error: Fails on guild [{guildname}]. Exception found: {e.Message}.");
            }
        }
        [HttpGet("[action]/{count=20}")]
        public IEnumerable<string> Guilds(int count) => _unitOfWork.Guilds.GetNthGuilds(count);

        [HttpGet("[action]/{guildname}/{username}")]
        public bool EnterGuild(string guildname, string username)
        {
            try
            {   
                if (_unitOfWork.Guilds.AddMember(guildname, username))
                    _unitOfWork.Complete();
                else 
                    throw new Exception($"error: fails adding user [{username}] as member of [{guildname}] guild.");
                
                return true;
            } catch (Exception)
            {
                _unitOfWork.Rollback();
                return false;
            }
        }

        [HttpDelete("[action]/{guildname:string}")]
        public ActionResult LeaveGuild([FromBody] string username, string guildname)
        {
            try
            {
                var guild =_unitOfWork.Guilds.Get(guildname);
                if (guild == null) return NotFound("Guild");
                var user =_unitOfWork.Users.Get(username);
                if (user == null) return NotFound("User");

                if (_unitOfWork.Guilds.RemoveMember(username, guildname))
                {
                    _unitOfWork.Complete();
                    return Ok(_unitOfWork.Guilds.Get(updatedGuild.Id));
                }
                else
                {
                    Response.StatusCode = 304;
                    return NoContent();
                }
            } catch (Exception e)
            {
                _unitOfWork.Rollback();
                return BadRequest($"error: fails removing user [{username}] from members of [{guildname}] guild. Exception found: {e.Message}." );
            }
        }

        [HttpPatch("[action]/{guildname:string}")]
        public ActionResult Transfer(string guildname, [FromBody] Guild updatedGuild)
        {
            if (guildname != updatedGuild.Id) return BadRequest("Route and Body Guild Id parameters not matching.");
            try
            {
                var guild =_unitOfWork.Guilds.Get(guildname);
                if (guild == null) return NotFound("Guild");
                var user =_unitOfWork.Users.Get(updatedGuild.MasterId);
                if (user == null) return NotFound("User");

                if (_unitOfWork.Guilds.TransferOwnership(updatedGuild.Id, updatedGuild.MasterId))
                {
                    _unitOfWork.Complete();
                    return Ok(_unitOfWork.Guilds.Get(updatedGuild.Id));
                }
                else
                {
                    Response.StatusCode = 304;
                    return NoContent();
                }
            } catch (Exception e)
            {
                _unitOfWork.Rollback();
                return BadRequest($"error: fails transfering [{updatedGuild.Id}'s] GuildMaster to [{updatedGuild.MasterId}]. Exception found: {e.Message}.");
            }
        }
    }
}
