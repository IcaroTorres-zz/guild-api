using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lumen.api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace lumen.api.Controllers
{
    [Route("lumen.api/[controller]")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        // injected unit of work from startup.cs configure services
        public GuildController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
