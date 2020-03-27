using Application.ActionFilters;
using Business.Services;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

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