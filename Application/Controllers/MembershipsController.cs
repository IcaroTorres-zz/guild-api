using Application.ActionFilters;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Application.Controllers
{
    [ApiController, Route("api/[controller]/v1")]
    public class MembershipsController : ControllerBase
    {
        [HttpGet(Name = "get-memberships"), UseCache(10)]
        public ActionResult Get([FromServices] IMemberService service)
        {
            return Ok((service as MemberService).GetAll<Membership>(readOnly: true));
        }
    }
}