using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Members;
using Business.Responses;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Business.Handlers.Members
{
	public class MemberFilterHandler : IRequestHandler<MemberFilterCommand, ApiResponse<Pagination<Member>>>
	{
		private readonly IMemberRepository _memberRepository;

		public MemberFilterHandler(IMemberRepository memberRepository)
		{
			_memberRepository = memberRepository;
		}

		public async Task<ApiResponse<Pagination<Member>>> Handle(MemberFilterCommand request,
			CancellationToken cancellationToken)
		{
			var query = _memberRepository.Query(x =>
				x.Name.Contains(request.Name) &&
				(request.GuildId == Guid.Empty || x.GuildId == request.GuildId), true);
			var totalCount = query.Count();
			var members = await query.Take(request.Count).ToListAsync(cancellationToken);
			var membersPaginated = new Pagination<Member>(members, totalCount, request.Count);
			return new ApiResponse<Pagination<Member>>(membersPaginated);
		}
	}
}