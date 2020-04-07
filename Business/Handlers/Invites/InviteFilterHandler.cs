using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Invites;
using Business.Responses;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Business.Handlers.Invites
{
	public class InviteFilterHandler : IRequestHandler<InviteFilterCommand, ApiResponse<Pagination<Invite>>>
	{
		private readonly IInviteRepository _repository;

		public InviteFilterHandler(IInviteRepository repository)
		{
			_repository = repository;
		}

		public async Task<ApiResponse<Pagination<Invite>>> Handle(InviteFilterCommand request,
			CancellationToken cancellationToken)
		{
			var query = _repository.Query(x =>
				(request.MemberId == Guid.Empty || x.MemberId == request.MemberId) &&
				(request.GuildId == Guid.Empty || x.GuildId == request.GuildId), true);
			var count = query.Count();
			var invites = await query.Take(request.Count).ToListAsync(cancellationToken);
			var invitesPaginated = new Pagination<Invite>(invites, count, request.Count);
			return new ApiResponse<Pagination<Invite>>(invitesPaginated);
		}
	}
}