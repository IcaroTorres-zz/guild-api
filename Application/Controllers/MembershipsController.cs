using Application.ActionFilters;
using Business.Dtos;
using Business.Usecases.Memberships.ListMemberships;
using Domain.Models;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
	[Authorize]
	[Route("api/[controller]/v1")]
	public class MembershipsController : ControllerBase
	{
		[ProducesResponseType(typeof(Result<Pagination<MembershipDto>>), StatusCodes.Status200OK)]
		[ProducesErrorResponseType(typeof(Result))]
		[HttpGet(Name = "get-memberships")]
		[UseCache(10)]
		public async Task<IActionResult> ListAsync([FromQuery] ListMembershipCommand command, [FromServices] IMediator mediator, CancellationToken cancellationToken)
		{
			return await mediator.Send(command, cancellationToken);
		}
	}
}