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
    [Route("lumen.api/")]
    [Produces("application/json")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        // injected unit of work from startup.cs configure services
        public GuildController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        
        public ActionResult<object> Index() => new {
            users = _unitOfWork.Users.GetAll(),
            guilds = _unitOfWork.Guilds.GetAll()
        };

        [HttpGet("create/{guildname}/{mastername}")]
        public ActionResult<bool> Create(string guildname, string mastername)
        {
            bool result = false;
            try { 
                result = _unitOfWork.Guilds.CreateGuild(guildname,mastername); 
                _unitOfWork.Complete();
            }
            catch(Exception) {
                _unitOfWork.Rollback();
            }
            return result;
        }
        [HttpGet("user/{username}")]
        public ActionResult<string> User(string username){
            var user  =_unitOfWork.Users.Get(username);
            if (user == null) {
                try {
                    user = new User { Name = username };
                    _unitOfWork.Users.Add(user);
                    _unitOfWork.Complete();
                    return $"Created {user.Name}";
                } catch (Exception) {
                    _unitOfWork.Rollback();
                    return "Error: Failure getting user.";
                }
            } else return $"Got {user.Name}";
        }

        [HttpGet("guilds")]
        public IEnumerable<string> Guilds() => _unitOfWork.Guilds.GetNthGuilds();

        [HttpGet("guilds/{count}")]
        public IEnumerable<string> Guilds(int count) => _unitOfWork.Guilds.GetNthGuilds(count);
        
        [HttpGet("info/{guildname}")]
        public Dictionary<string, dynamic> Info(string guildname)
        {
            Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
            try
            {
                result = _unitOfWork.Guilds.GuildInfo(guildname);
                _unitOfWork.Complete();
            } catch (Exception)
            {
                _unitOfWork.Rollback();
            }
            return result;
        }

        [HttpGet("enter/{guildname}/{username}")]
        public ActionResult<bool>  Enter(string guildname, string username)
        {
            bool result = false;
            try
            {   
                if (_unitOfWork.Guilds.AddMember(guildname, username))
                    result = _unitOfWork.Users.EnterTheGuild(guildname, username);

                _unitOfWork.Complete();
            } catch (Exception)
            {
                _unitOfWork.Rollback();
            }
            return result;
        }

        [HttpGet("leave/{username}/{guildname}")]
        public ActionResult<bool>  Leave(string username, string guildname)
        {
            bool result = false;
            try
            {   
                if (_unitOfWork.Guilds.RemoveMember(username, guildname))
                    result = _unitOfWork.Users.LeaveTheGuild(username, guildname);

                _unitOfWork.Complete();
            } catch (Exception)
            {
                _unitOfWork.Rollback();
            }
            return result;
        }

        [HttpGet("transfer/{guildname}/{username}")]
        public ActionResult<bool> Transfer(string guildname, string username)
        {
            var result = false;
            try
            {   
                result = _unitOfWork.Guilds.TransferOwnership(guildname, username);
                _unitOfWork.Complete();
            } catch (Exception)
            {
                _unitOfWork.Rollback();
            }
            return result;
        }
    }
}
