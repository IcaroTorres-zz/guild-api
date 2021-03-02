using Application.Common.Responses;
using Application.Common.Results;
using Application.Memberships.Queries.ListMemberships;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/v1")]
    public class MembershipsController : ControllerBase
    {
        [ProducesResponseType(typeof(Output<PagedResponse<MembershipResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpGet(Name = "get-memberships")]
        [UseCache(10)]
        public async Task<IActionResult> ListAsync([FromQuery] ListMembershipCommand command, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            return await mediator.Send(command, cancellationToken);
        }
    }
}