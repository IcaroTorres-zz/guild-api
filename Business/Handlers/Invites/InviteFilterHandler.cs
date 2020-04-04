using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Invites;
using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Business.Handlers.Invites
{
	public class InviteFilterHandler : IPipelineBehavior<InviteFilterCommand, ApiResponse<Pagination<Invite>>>
	{
		private readonly IInviteRepository _repository;

		public InviteFilterHandler(IInviteRepository repository)
		{
			_repository = repository;
		}

		public async Task<ApiResponse<Pagination<Invite>>> Handle(InviteFilterCommand request,
			CancellationToken cancellationToken, RequestHandlerDelegate<ApiResponse<Pagination<Invite>>> next)
		{
			var query = _repository.Query(x => (request.MemberId == Guid.Empty || x.MemberId == request.MemberId) &&
			                                   (request.GuildId == Guid.Empty || x.GuildId == request.GuildId), true);

			var count = query.Count();

			var invites = await query.Take(request.Count).ToListAsync();

			return new ApiResponse<Pagination<Invite>>(new Pagination<Invite>(invites, count, request.Count));
		}
	}
}