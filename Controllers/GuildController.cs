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
    [Route("lumen.api/[controller]")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        // injected unit of work from startup.cs configure services
        public GuildController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpPost("{guildname}/{mastername}")]
        public bool CreateGuild(string guildname, string mastername)
        {
            try {  return _unitOfWork.Guilds.CreateGuild(guildname,mastername); }
            catch(Exception e) {
                _unitOfWork.Rollback();
                return false;
            }
        }

        [HttpGet]
        public IEnumerable<string> GetGuilds() => _unitOfWork.Guilds.GetNthGuilds();

        [HttpGet("{count}")]
        public IEnumerable<string> GetGuilds(int count) => _unitOfWork.Guilds.GetNthGuilds(count);
        
        [HttpGet("{guildname}")]
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
            }
            return result;
        }

        [HttpPut("{guildname}/{username}")]
        public bool EnterTheGuild(string guildname, string username)
        {
            bool result = false;
            try
            {   
                if (_unitOfWork.Guilds.AddMember(guildname, username))
                    result = _unitOfWork.Users.EnterTheGuild(guildname, username);

                _unitOfWork.Complete();
            } catch (Exception e)
            {
                _unitOfWork.Rollback();
            }
            return result;
        }

        [HttpDelete("{username}/{guildname}")]
        public bool LeaveTheGuild(string username, string guildname)
        {
            bool result = false;
            try
            {   
                if (_unitOfWork.Guilds.RemoveMember(username, guildname))
                    result = _unitOfWork.Users.LeaveTheGuild(username, guildname);

                _unitOfWork.Complete();
            } catch (Exception e)
            {
                _unitOfWork.Rollback();
            }
            return result;
        }

        [HttpPut("{guildname}/{username}")]
        public bool TransferOwnership(string guildname, string username)
        {
            var result = false;
            try
            {   
                result = _unitOfWork.Guilds.TransferOwnership(guildname, username);
                _unitOfWork.Complete();
            } catch (Exception e)
            {
                _unitOfWork.Rollback();
            }
            return result;
        }
    }
}
