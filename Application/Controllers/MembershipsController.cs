using Application.ActionFilters;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController, Route("api/[controller]/v1")]
    public class MembershipsController : ControllerBase
    {
        [HttpGet(Name = "get-memberships"), UseCache(10)]
        public ActionResult Get([FromServices] IRepository<Membership> repository)
        {
            return Ok(repository.GetAll(readOnly: true));
        }
    }
}