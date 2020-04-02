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
    public async System.Threading.Tasks.Task<IActionResult> GetAsync([FromServices] IRepository<Membership> repository)
    {
      return Ok(await repository.GetAllAsync(readOnly: true));
    }
  }
}