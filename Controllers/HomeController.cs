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
        private readonly IUnitOfWork _unitOfWork;
        // injected unit of work from startup.cs configure services
        public HomeController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpPost("[action]")]
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
                
        [HttpGet("[action]/{count=20}")]
        public IEnumerable<string> Guilds(int count) => _unitOfWork.Guilds.GetNthGuilds(count);

        [HttpGet("[action]/{username}")]
        public Dictionary<string, dynamic> UserInfo(string username)
        {
            Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
            try {
                var user =_unitOfWork.Users.Get(username);
                if (user != null)
                    return new Dictionary<string, dynamic>{ { "user found", user } };

                user = new User(){ Id = username };
                _unitOfWork.Users.Add(user);
                _unitOfWork.Complete();
                return new Dictionary<string, dynamic>{ { "user created", user } };
            } catch (Exception) {
                _unitOfWork.Rollback();
                return new Dictionary<string, dynamic>{ { "error", $"Fails on user [{username}]." } };
            }
        }

        [HttpGet("[action]/{guildname}")]
        public Dictionary<string, dynamic> GuildInfo(string guildname)
        {
            Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
            try
            {
                result = _unitOfWork.Guilds.GuildInfo(guildname);
                _unitOfWork.Complete();
            } catch (Exception e)
            {
                _unitOfWork.Rollback();
                return new Dictionary<string, dynamic>() { { "error", e.Message } };
            }
            return result;
        }

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

        [HttpGet("[action]/{username}/{guildname}")]
        public bool LeaveGuild(string username, string guildname)
        {
            try
            {   
                if (_unitOfWork.Guilds.RemoveMember(username, guildname))
                    _unitOfWork.Complete();
                else 
                    throw new Exception($"error: fails removing user [{username}] from members of [{guildname}] guild.");
                
                return true;
            } catch (Exception)
            {
                _unitOfWork.Rollback();
                return false;
            }
        }

        [HttpGet("[action]/{guildname}/{username}")]
        public bool Transfer(string guildname, string username)
        {
            try
            {   
                if (_unitOfWork.Guilds.TransferOwnership(guildname, username))
                    _unitOfWork.Complete();
                else 
                    throw new Exception($"error: fails transfering position of GuildMaster of [{guildname}] guild to user [{username}].");

                return true;
            } catch (Exception)
            {
                _unitOfWork.Rollback();
                return false;
            }
        }
    }
}
