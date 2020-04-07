using System.Threading;
using System.Threading.Tasks;
using Application.ActionFilters;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	[ApiController]
	[Route("api/[controller]/v1")]
	public class MembershipsController : ControllerBase
	{
		[HttpGet(Name = "get-memberships")]
		[UseCache(10)]
		public async Task<IActionResult> GetAsync([FromServices] IRepository<Membership> repository,
			CancellationToken cancellationToken)
		{
			return Ok(await repository.GetAllAsync(true, cancellationToken));
		}
	}
}