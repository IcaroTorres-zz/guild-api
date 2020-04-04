using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Members;
using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Business.Handlers.Members
{
	public class MemberFilterHandler : IPipelineBehavior<MemberFilterCommand, ApiResponse<Pagination<Member>>>
	{
		private readonly IMemberRepository _memberRepository;

		public MemberFilterHandler(IMemberRepository memberRepository)
		{
			_memberRepository = memberRepository;
		}

		public async Task<ApiResponse<Pagination<Member>>> Handle(MemberFilterCommand request,
			CancellationToken cancellationToken, RequestHandlerDelegate<ApiResponse<Pagination<Member>>> next)
		{
			var query = _memberRepository.Query(x =>
				x.Name.Contains(request.Name) &&
				(request.GuildId == Guid.Empty || x.GuildId == request.GuildId), true);
			var totalCount = query.Count();
			var members = await query.Take(request.Count).ToListAsync();

			return new ApiResponse<Pagination<Member>>(new Pagination<Member>(members, totalCount, request.Count));
		}
	}
}